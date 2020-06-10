/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.contentlist = function (options) {
    var list = this;
    var infobar = $('<div class="infobar" />').insertBefore(this);
    var pager;

    var sittings = {
        filterForm: this.data('filterform'),
        reloadServive:  this.data('reloadservice'),
        pageSize: this.data('pagesize'),
        totalCount: this.data('totalcount')
    };

    $.registerEvent(this, 'onreload');

    var frmFilter = $.findComponent(sittings.filterForm);
    var pageNumber = 1;

    if (sittings.pageSize) {
        pager = $('<div class="pager" >المزيد</div>').click(function (e) {
            pageNumber++;
            reload();
        }).insertAfter(this).hide();
    }

    function reload() {
        var data = {};
        if (frmFilter)
            $.extend(data, frmFilter.getData());

        if (sittings.pageSize) {
            data.pageNumber = pageNumber;
            data.pageSize = sittings.pageSize;
        }

        var loading = $('<div class="loading" />').insertAfter(list);
        $.sendRequest(sittings.reloadServive, data, function (data) {
            loading.remove();

            if (typeof data.d === "object")
                data = data.d;

            if (data.success) {
                if (pageNumber == 1)
                    list.empty();

                var newlist = $('<ul />').append(data.html);
                while (newlist.children().length > 0) {
                    routeClicks.call($(newlist.children().get(0)).appendTo(list).execteExtenders());
                }
                
                //list.append(data.html);
                //routeClicks.call(list.execteExtenders());

                if (sittings.pageSize) {
                    infobar.show().text(list.find('li').length + ' نتيجة من أصل ' + data.totalCount);
                  //  infobar.data('clone').text(list.find('li').length + ' نتيجة من أصل ' + data.totalCount).height(infobar.height());

                    if (list.find('li').length < data.totalCount)
                        pager.show();
                    else
                        pager.hide();
                }

                $.raiseEvent(list.events.onreload, this, {});
            }
            else
                $.msgbox(data.error, '', 'ok', 'error');
        });

    }

    if (frmFilter) {
        $.addEventHandler(frmFilter, 'onsubmit',listData);
    }

    function listData(sender, e) {
        pageNumber = 1;
        reload();
    }

    if (sittings.totalCount) {
        infobar.show().text(list.find('li').length + ' نتيجة من أصل ' + sittings.totalCount);
        infobar.data('clone').text(list.find('li').length + ' نتيجة من أصل ' + sittings.totalCount).height(infobar.height());
        if (sittings.totalCount > list.find('li').length)
            pager.show();
    }

    function addItem(e) {
        var sender = $(this);
        e.preventDefault();

        dlgItem.reset();
        var categoryfields = dlgItem.categoryfields;
        for (var prop in categoryfields) {
            var field = categoryfields[prop];

            var filtervalue = $.findComponent(sittings.filterForm).categoryfields[prop].ctrl.val();

            if (filtervalue > 0)
                field.ctrl.val(filtervalue);
        }

        dlgItem.modal({
            title: sittings.addTitle,
            vldMetod: $.createDelegate(dlgItem, dlgItem.validate),
            buttons: 'okCancel',
            onaccept: function (e) {
                function callback(data) {
                    if (typeof data.d === "object")
                        data = data.d;

                    e({
                        success: data.success,
                        error: data.error
                    });

                    if (data.success) {
                        var newrecord = routeClicks.call($(data.html)).appendTo(list).execteExtenders();
                    }
                    else
                        $.msgbox(data.error, '', 'ok', 'error');
                }

                var data = dlgItem.getData();
                if (frmFilter) {
                    for (var propname in frmFilter.categoryfields) {
                        var field = frmFilter.categoryfields[propname];

                        data[propname] = field.ctrl.val();
                    }
                }
                for (var propname in dlgItem.categoryfields) {
                    var field = dlgItem.categoryfields[propname];

                    data[propname] = field.ctrl.val();
                }

                if (dlgItem.files.length > 0)
                    $.sendFiles(sittings.addService, data, dlgItem.files, callback);

                else
                    $.sendRequest(sittings.addService, data, callback);

            }
        });
    }
    function editItem(e) {
        var sender = $(this);
        e.preventDefault();

        var oldrecord = sender.closest('li');
        var itemId = oldrecord.data('id')
        $.sendRequest(sittings.getService, { id: itemId }, function (data) {
            if (typeof data.d == "object")
                data = data.d;

            data.id = itemId;
            dlgItem.setData(data);
            dlgItem.modal({
                title: sittings.editTitle,
                vldMetod: $.createDelegate(dlgItem, dlgItem.validate),
                buttons: 'okCancel',
                onaccept: function (e) {
                    function callback(data) {
                        if (typeof data.d === "object")
                            data = data.d;

                        e({
                            success: data.success,
                            error: data.error
                        });

                        if (data.success) {
                            var newrecord = routeClicks.call($(data.html));
                            newrecord.insertAfter(oldrecord).execteExtenders();
                            oldrecord.remove();
                        }
                    }

                    var data = dlgItem.getData(itemId);
                    if (frmFilter) {
                        for (var propname in frmFilter.categoryfields) {
                            var field = frmFilter.categoryfields[propname];

                            data[propname] = field.ctrl.val();
                        }
                    }
                    for (var propname in dlgItem.categoryfields) {
                        var field = dlgItem.categoryfields[propname];

                        data[propname] = field.ctrl.val();
                    }

                    if (dlgItem.files.length > 0)
                        $.sendFiles(sittings.editService, data, dlgItem.files, callback);

                    else
                        $.sendRequest(sittings.editService, data, callback);

                }
            });
        });
    }
    function deleteItem(e) {
        var sender = this;
        var item = $(sender).closest('li');
        var itemId = item.data('id');

        $.msgbox(sittings.deleteConfirm, sittings.deleteTitle, 'yesNo', 'information', function (result) {
            if (result == 'yes') {
                $.sendRequest(sittings.deleteService, { id: itemId }, function (data) {
                    if (typeof data.d === "object")
                        data = data.d;

                    if (data.success) {
                        item.remove();
                        $.msgbox('لقد تمت العملية بنجاح', sittings.deleteTitle, 'ok', 'information');
                    }
                    else
                        $.msgbox(data.error, sittings.deleteTitle, 'ok', 'error');
                });
            }
        });
    }

    if (typeof sittings.addService === "string" && sittings.addService != '') {
        var buttons = $('<div class="buttons" />').insertBefore(this);
        $('<a class="add" href="#" />')
            .text(sittings.addTitle)
            .on('click', addItem)
            .appendTo(buttons);
        buttons.sticky();
    }


    function routeClicks() {
        
        this.find('.edit').on('click', editItem);
        this.find('.delete').on('click', deleteItem);

        return this;
    }

    this.routeClicks = routeClicks;
    routeClicks.call(this);

    $.registerDispose(this, function () {
        $(".buttons .add,.infobar,.pager").remove();
        if ($(".loading").length > 0)
            $(".loading").remove();

        list.events = list.events || {};
        list.events['onreload'] = null;

        if(frmFilter)
             $.removeEventHandler(frmFilter, "onsubmit", listData);

    });

    return this;
};
