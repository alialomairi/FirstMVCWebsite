/// <reference path="../jquery-1.7.1.js" />
/// <reference path="../extend-1.0.js" />

$.fn.addTextBox = function () {
    var target = $(this.data('target'));
    var addService = this.attr('href');
    var item = this.closest('li');
    var parentId = item.data('id');
    var me = this;

    var MessageType = 
    {
        General:0,
        Question:1,
        Answer:2,
        comment:3
    }

    if (item.length > 0 && target.length == 0) 
        target = $('<ul />').appendTo(item);

    function submit() {
        var container = this;
        var child = this.closest('li');
        var messageText = this.find('textarea').val();

        var data = {
            Subject: 'New Message',
            Text: messageText,
            Type: 0,
            parentId: parentId
        };

        if (me.data("data") != undefined) {
            $.extend(data, me.data("data"));
        }

        $.sendRequest(addService, data, function (data) {
            if (typeof data.d === "object")
                data = data.d;

            if (data.success) {
                var newrecord = $(data.html);
                newrecord.insertAfter(child).execteExtenders();
                child.remove();

                if (newrecord.closest('[data-extender=contentlist]').length > 0) {
                    var list = newrecord.closest('[data-extender=contentlist]');
                    list.data('component').routeClicks.call(newrecord);

                }

            }
            else
                $.msgbox(data.error, '', 'ok', 'error');
        });
    }
    function remove(issubmit) {
        if (issubmit)
            submit.call(this);

        this.remove();
    }

    this.click(function (e) {
        e.preventDefault();

        var child = $('<li />').appendTo(target);
        var container = $('<div />').appendTo(child);
        var toolbar = $('<div />').appendTo(container);
        var txtSubject = $('<input type="text" />').appendTo(toolbar);
        var ddlType = $('<select />').appendTo(toolbar);
        var options = ddlType.prop("options")
        for (var type in MessageType)
            options[options.length] = new Option(type, MessageType[type]);

        var sendbutton = $('<a href="#" />').text('إرسال').appendTo(toolbar);
        var cancelbutton = $('<a href="#" />').text('إلغاء').appendTo(toolbar);
        var textbox = $('<textarea />').appendTo(container);

        textbox.keyup(function (e) {
            if (e.keyCode == 27)
                container.remove();   // esc
            else if (e.keyCode == 13 && e.ctrlKey) {
                submit.call(container);   // ctrl + enter

            }
        });

        sendbutton.click(function (e) {
            e.preventDefault();
            submit.call(container);
        });

        cancelbutton.click(function (e) {
            e.preventDefault();
            container.remove();
        });
    });
};