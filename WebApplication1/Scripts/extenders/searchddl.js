/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.searchddl = function () {
    // 1. read the sittings.
    var placeholder = this.find('option[value=]').text(); // this.data('placeholder');
    var searchplaceholder = this.data('searchplaceholder');
    var newtext = this.data('newtext');
    var addService = this.data('addservice');
    var emptytext = this.data('emptytext');

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
        // determine selected item
        var item = list.find('li>a[data-value=' + value + ']');

        // unselect all items
        list.find('li>a').removeClass('selected');

        // select selected item
        item.addClass('selected');

        // set the value of original select
        if (this.val() != value)
            this.val(value);

        // change the caption text
        if (value != '') {
            caption.text(item.text());
            caption.addClass("findRes");
        }
        else {
            caption.text(placeholder);
            caption.remove("findRes");
        }

        close();
        this.change();
    });

    var addCallback = $.createDelegate(this, function (data) {
        if (data.d)
            data = data.d;

        if (data.success) {
            var options = this.prop('options');
            options[options.length] = new Option(data.data.text, data.data.value);

            newItem.before($('<li />')
                .append($('<a href="#"  />')
                    .attr('data-value', data.data.value)
                    .text(data.data.text)
                ));

            selectItem(data.data.value);
        }
        else
            $.msgbox(data.error, '', 'ok', 'error');

    });

    $.registerEvents(this, ['addstart', 'addend']);

    // 3. hide the original select
    this.hide();

    // 4. add the user interface
    var containerdiv = $('<div class="searchddl" />').insertBefore(this);
    var caption = $('<a href="#" class="caption" />').appendTo(containerdiv);
    var dropdown = $('<div class="dropdown" />').appendTo(containerdiv);
    var searchdiv = $('<div class="searchdiv" />').appendTo(dropdown);
    var searchbox = $('<input type="text" class="searchbox" />').attr('placeholder', searchplaceholder).appendTo(searchdiv);

    var listdiv = $('<div class="listdiv" />').appendTo(dropdown);
    var list = $('<ul />').appendTo(listdiv);

    var options = this.find("option");

    options.each(function (i, option) {
        option = $(option);

        if (option.val() != '') {
            $('<li />').appendTo(list)
                .append($('<a href="#"  />')
                    .attr('data-value', option.val())
                    .text(option.text()));
        }

    });

    var emptyItem = $('<li style="display:none;" />').text(emptytext).appendTo(list);

    var newItem = $('<li class="newitem" />')
        .append($('<a href="#"  />')
            .attr('data-value', 'new')
            .text(newtext)
        ).appendTo(list);

    // 5. load the initial value.
    selectItem(this.val());

    // 6. handle the events
    caption.click($.createHandler(this, function (sender, e) {
        e.preventDefault();

        dropDownToggle();
    }));

    list.find("a").click($.createHandler(this, function (sender, e) {
        e.preventDefault();

        sender = $(sender);
        if (this.data('parent')) {
            var parent = $(this.data('parent'));
            var parentId = parent.val();
            if (parentId == '') return;
        }
        var value = sender.data('value');

        if (value != 'new')
            selectItem(value);
        else {
            caption.hide();
            var parentId = $(this.data('parent')).val();
            var textbox = $('<input type="text" />').appendTo(containerdiv);
            textbox.change(
                function (e) {
                    var data = { text: this.value }
                    if (parentId != undefined) data.id = parentId;

                    $.sendRequest(
                        addService,
                        data,
                        function (data) {
                            addCallback(data);
                            textbox.remove();
                            caption.show();
                        });
                }).keyup(function (e) {
                    if (e.keyCode == 27)   // esc
                    {
                        textbox.remove();
                        caption.show();
                    }
                    else if (e.keyCode == 13)   // enter
                        textbox.change();
                }).blur(function (e) {
                    textbox.remove();
                    caption.show();
                }).select();

            close();
        }
    }));

    searchbox.keyup($.createHandler(this, function (sender, e) {
        var text = $(searchbox).val();
        var textLen = text.length;
        var listItems = list.find("li");

        var found = false;
        listItems.each(function (i, item) {
            item = $(item);

            if ((item.text().slice(0, textLen)) != text) {
                item.hide();
            } else {
                item.show();
                found = true;
            }
        });

        if (!found) {
            emptyItem.show();
        }
        else
            emptyItem.hide();
    }));

    var containerClickOut = $.createHandler(this, function (sender, e) {

        var rootElement = containerdiv.get(0);
        if (e.target == rootElement)
            return;

        var $target = $(e.target);

        var root = $target.closest(".searchddl");
        if (root.length == 0 || root.get(0) != rootElement)
            close();

    });

    $(document).on("click", containerClickOut);

    // register dispose script
    $.registerDispose(this, $.createDelegate(this, function () {
        $(document).off("click", containerClickOut);
        containerdiv.remove();
        this.show();
    }));
    return this;
};
