/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.treeddl = function () {
    // 1. read the sittings.
    var placeholder = this.find('option[value=""]').text();
    var multiple = (this.attr('multiple') == 'multiple');
   
    // 2. define members.
    var isOpened = false;
    var open = $.createDelegate(this, function () {
        dropdown.show();
        caption.addClass("clicked");
        isOpened = true;
    });

    var close = $.createDelegate(this, function () {
        dropdown.hide();
        caption.removeClass("clicked");
        isOpened = false;
    });

    var dropDownToggle = $.createDelegate(this, function () {
        isOpened ? close() : open();
    });

    var selectItem = $.createDelegate(this, function (value) {

        if (multiple) {
            // uncheck all
            list.find('li>input[checked=checked]').removeAttr('checked').siblings('a').removeClass('selected');
            this.find('option:selected').removeAttr('selected');

            // check selected values
            if(value)
            $.each(value, $.createDelegate(this, function (i, val) {
                list.find('li>input[value="' + val + '"]').attr('checked', 'checked').siblings('a').addClass('selected');
                this.find('option[value="' + val + '"]').attr('selected', 'selected');
            }));

            // change the caption text
            if (value && value.length) {
                var items = this.find('option:selected');

                var text = '';
                items.each(function (i, item) {
                    item = $(item);

                    text += item.text() + ' ';
                });
                caption.text(text);
                caption.addClass("findRes");
            }
            else {
                caption.text(placeholder);
                caption.remove("findRes");
            }

            
        }
        else {
            // unselect all items
            list.find('li>a').removeClass('selected');

            // determine selected item
            list.find('li>a[data-value="' + value + '"]').addClass('selected');

        // set the value of original select
        if (this.val() != value) {
            this.val(value);
            this.change();
        }

        // change the caption text
        if (value != '') {
            var item = this.find('option:selected');


            caption.text(item.text());
            caption.addClass("findRes");
        }
        else {
            caption.text(placeholder);
            caption.remove("findRes");
        }
        }

        close();
    });

    $.registerEvents(this, ['addstart', 'addend']);

    // 3. hide the original select
    this.hide();

    // 4. add the user interface
    var containerdiv = $('<div class="treeddl" />').insertBefore(this);
    var caption = $('<a href="#" class="caption" />').appendTo(containerdiv);
    var dropdown = $('<div class="dropdown" />').appendTo(containerdiv);

    var listdiv = $('<div class="listdiv" />').appendTo(dropdown);
    var list = $('<ul />').appendTo(listdiv);
    var filterList;
    var parentNodes = [];

    var loadList = $.createDelegate(this, function (parent) {
        var parentid = '';
        if (parent) parentid = parent.find('a').data('value');
        var thelist = list;

        var options = this.find('option' + (parent ? '[data-parent=' + parentid + ']' : '[data-parent=""],option:not([data-parent])'));
        if (parent && options.length > 0) {
            thelist = $('<ul style="display:none;" />').appendTo(parent);
            parent.find('span').addClass('treeNode expand');
        }

        options.each( $.createDelegate(this, function (i, option) {
            option = $(option);

            if (option.val() != '') {
               
                var item = $('<li />').appendTo(thelist);
                var icon = $('<span  />').html("&nbsp;").appendTo(item);
                var lable = $('<a href="#" />').attr('data-value', option.val()).text(option.text()).appendTo(item);
                if (multiple) {
                    var checkbox = $('<input type="checkbox" />')
                        .val(option.val())
                        .click($.createHandler(this, function (sender, e) {
                            sender = $(sender);

                            var checked = sender.prop('checked');
                            var val = sender.val();

                            var option = this.find('option[value=' + val + ']');

                            if (checked) 
                                option.attr('selected', 'selected');
                            else
                                option.removeAttr('selected', 'selected');

                            var selectedvalue = [];
                            this.find('option:selected').each(function (i, option) {
                                option = $(option);
                                selectedvalue[selectedvalue.length] = option.val();
                            });

                            selectItem(selectedvalue);
                            open();
                        }))
                        .insertBefore(lable);
                }
                if (i == options.length - 1) item.addClass("last");
                loadList(item); // recursive call.

            }

        }));



    });
    loadList();

    // 5. load the initial value.
    selectItem(this.val());


    this.change($.createHandler(this, function (sender, e) {
        selectItem(this.val());
    }));

    // 6. handle the events
    caption.click($.createHandler(this, function (sender, e) {
        e.preventDefault();

        dropDownToggle();
    }));

    list.find("a").click($.createHandler(this, function (sender, e) {


        sender = $(sender);

        var value = sender.data('value');
        selectItem(value);
    }));

    list.find(".treeNode").click($.createHandler(this, function (sender, e) {

        e.stopPropagation();

        $(sender).toggleClass('expand');
        $(sender).siblings('ul').toggle();


    }));

    var containerClickOut = $.createHandler(this, function (sender, e) {

        var rootElement = containerdiv.get(0);
        if (e.target == rootElement)
            return;

        var $target = $(e.target);

        var root = $target.closest(".treeddl");
        if (root.length == 0 || root.get(0) != rootElement) {
              close();
        }
    });

    $(document).on("click", containerClickOut);

    // register dispose script
    $.registerDispose(this, function () {

        $(document).off("click", containerClickOut);
    });
    return this;
};
