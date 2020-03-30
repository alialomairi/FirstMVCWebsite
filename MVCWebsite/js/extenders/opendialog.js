/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../extend-1.0.js" />
/// <reference path="postdata.js" />
/// <reference path="../message.js" />

$.fn.opendialog = function () {

    var me = this;
    var dialog = typeof (this.data("dialog")) === "string" ? $.findComponent(this.data("dialog")) : null;
    var dialogservice = this.data("dialogservice");
    var getservice = this.data('getservice');
    var updateservice = this.data('updateservice');
    var updatedialog = this.data('updatedialog');
    var updatedialogservice = this.data('updatedialogservice');
    var updateextender = this.data('updateextender');
    var title = this.text();

    $.fn.postdata.call(this);

    var clickHandler = $.createHandler(this, function (sender, e) {
        e.preventDefault();

        if (typeof (dialogservice) === "string")
        {
            dialog = null;

            var data = this.data('data') || {};
            var record = $(this).closest('li');
            if (record.length > 0) {
                var id =  record.data('id');
                if(typeof id === "number") data.id = id;
            }
            var cdata = data;
            $.sendRequest(dialogservice, data, function (data) {
                if (typeof data.d == "object")
                    data = data.d;

                dialog = $('<ul class="form" data-extender="inputform" />');

                for (var i in data) {
                    var field = $('<li data-name="' + i + '" />').appendTo(dialog);
                    $('<span />').appendTo(field).text(data[i].label);
                    var input = $('<input type="text" />').appendTo(field).val(data[i].value);

                    if (updateservice) {
                        input.attr('data-service', updateservice)
                        cdata.qtr = i;
                        input.attr('data-data', JSON.stringify( cdata));
                    }
                    if(updatedialog)
                        input.attr('data-dialog', updatedialog)
                    if(updatedialogservice)
                        input.attr('data-dialogservice', updatedialogservice)
                    if (updateextender)
                        input.attr('data-extender', updateextender)
                }

                dialog.execteExtenders();
                dialog = dialog.data('component');

                showdialog();
            });
        }
        if (typeof (getservice) === "string")
        {
            var data = this.data('data') || {};
            var record = $(this).closest('li');
            if (record.length > 0) {
                data.id = record.data('id');
            }

            $.sendRequest(getservice, data, function (data) {
                if (typeof data.d == "object")
                    data = data.d;

                if (data.success) {
                    dialog.setData(data.data);
                }
            });
        }
        else
        if(dialog && dialog.reset) dialog.reset();

        if(dialog && dialog.modal) showdialog();

    });
        function showdialog() {
            dialog.modal({
                title: title,
                vldMetod: $.createDelegate(dialog, dialog.validate),
                buttons: 'okCancel',
                onaccept: $.createDelegate(me, function (callback) {
                    var data = dialog.getData();
                     //$.extend(data, this.data('data'));
                    for (prop in this.data('data')) {
                        if (!data.hasOwnProperty(prop)) {
                            data[prop] = this.data('data')[prop];
                        }
                    }
                    this.data('data', data);
                    this.sendRequest(callback);
                })
            });
        }

    $._data(this.get(0), 'events')['click'][0].handler = clickHandler;

    return this;
}