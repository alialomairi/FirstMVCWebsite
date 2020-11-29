/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.searchlist = function () {
    var container = $('<div class="searchlist" />').insertBefore(this);
    var list = $('<ul />').appendTo(container);
    var addButton = $('<a />').text('Add').appendTo(container);
    this.hide();


    addButton.click($.createHandler(this, function (sender, e) {
        var select = $('<select data-extender="searchddl" />').attr({
            'data-emptytext': this.data('emptytext'),
            'data-newtext': this.data('newtext'),
            'data-searchplaceholder': this.data('searchplaceholder'),
            'data-addservice': this.data('addservice')
        });

        var options = this.prop('options');
        var selectoptions = select.prop('options');

        $.each(options, function (i, option) {
            if(!option.selected)
                selectoptions[selectoptions.length] = new Option(option.text,option.value);
        });
        select.insertAfter(list).execteExtenders();

    }));
    return this;
};
