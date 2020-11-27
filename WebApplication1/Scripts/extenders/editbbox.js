/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.editbox = function () {
    var service = this.data('service');
    var field = this.data('field');
    var select = this.find('select');
    if (select.length == 0) select = undefined;

    var cvalue = select?select.val():this.text().trim();
    var ctext = select?select.find('option[selected]').text():cvalue;

    this.html('');
    var textspan = $('<span class="value" />').text(ctext).appendTo(this);
    var editbutton = $('<a href="#" class="fa fa-edit" />').appendTo(this);

    var input = (select||$('<input type="text" />')).appendTo(this).hide();
    var okbutton = $('<a href="#" class="fa fa-check" />').appendTo(this).hide();
    var cancelbutton = $('<a href="#" class="fa fa-times" />').appendTo(this).hide();

    editbutton.click($.createHandler(this, function (sender, e) {
        e.preventDefault();

        textspan.hide();
        editbutton.hide();

        input.show().val(cvalue);

        okbutton.show();
        cancelbutton.show();
    }));

    okbutton.click($.createHandler(this, function (sender, e) {
        e.preventDefault();

        var value = input.val();
        var text = select?input.find('option:selected').text():value;
        $.sendRequest(service, { field: field, value: value }, $.createDelegate(this, function (data) {
            data = data.d || data;

            if (data.success) {
                input.hide();
                okbutton.hide();
                cancelbutton.hide();

                cvalue = value;
                ctext = text;

                textspan.text(text).show();
                editbutton.show();
            }
            else
                alert(data.error);
        }));
    }));

    cancelbutton.click($.createHandler(this, function (sender, e) {
        e.preventDefault();

        input.hide();
        okbutton.hide();
        cancelbutton.hide();

        textspan.show();
        editbutton.show();
    }));

};