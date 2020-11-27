/// <reference path="jquery-1.8.2.min.js" />
/// <reference path="util.js" />

$.msgbox = function (text, title, buttons, icon, callback) {
    var body = $('<div class="dlgbody" style="display:block" />')
           .append($('<div class="icon" />')
                .addClass(icon)
               .append('<span />'))
           .append($('<div class="text" />')
               .text(text)
               .append('<span />'))
           .append('<div class="clear" />');

    body.modal({
        title: title,
        buttons: buttons,
        callback: callback
    });
};

(function () {
    var body = $(document.body);
    var buttonText = {
        ok: body.data('ok'),
        yes: body.data('yes'),
        no: body.data('no'),
        abort: body.data('abort'),
        retry: body.data('retry'),
        ignore: body.data('ignore'),
        cancel: body.data('cancel')
    }
    var dialogResult = {
        ok: 0x01,
        yes: 0x02,
        no: 0x04,
        abort: 0x08,
        retry: 0x10,
        ignore: 0x20,
        cancel: 0x40
    }
    var msgbuttons = {
        none: 0,
        ok: dialogResult.ok,
        okCancel: dialogResult.ok | dialogResult.cancel,
        abortRetryIgnore: dialogResult.abort | dialogResult.retry | dialogResult.cancel,
        yesNoCancel: dialogResult.yes | dialogResult.no | dialogResult.cancel,
        yesNo: dialogResult.yes | dialogResult.no,
        retryCancel: dialogResult.retry | dialogResult.cancel
    };

    var methods = {
        init: function (options) {
            var parent = this.parent();

            var box = $('<div class="dialogbox" />')
               .append($('<div class="titlebar" />')
                   .append($('<span class="title" />')
                        .text(options.title || document.title))
                   .append('<a class="close" />')
                   .append('<div class="clear" />')
               )
               .append('<div class="error" style="display:none;" />')
               .append(this.show())
               .append('<div class="buttons" />');

            var body = $(document.body);
            $.extend(buttonText, {
                ok: 'موافق',
                yes: 'نعم',
                no: 'لا',
                abort: 'إجهاض',
                retry: 'إعادة',
                ignore: 'تجاهل',
                cancel: 'إلغاء'
            }, options.userText);

            var buttonsbar = box.find('.buttons');
            buttonsbar.html('');
            var buttons = options.buttons || 'ok';
            for (var result in dialogResult) {
                if ((~msgbuttons[buttons] & dialogResult[result]) == 0) {
                    buttonsbar.append('<input type="button" value="' + buttonText[result] + '" data-result="' + result + '" />');
                }
            }

            var background = $('<div class="modalbg" />');

            var thebody = this;
            function closeDialog(e) {
                var sender = this;

                var result = $(this).data('result');
                if (!result) result = 'cancel';

                if (!e.handled) {
                    if (result != 'cancel' && typeof options.vldMetod == "function" && options.vldMetod(options.vldArgs) == false)
                        return;
                    if (result == 'ok' && typeof options.onaccept === "function") {
                        options.onaccept(function (data) {
                            if (data.success) {
                                e.handled = true;
                                e.data = data;
                                closeDialog.call(sender, e);
                            }
                            else {
                                var error = data.error;
                                box.find('.error').text(error).show();
                            }
                        });

                        return;
                    }
                }

                if (typeof options.callback === "function") {
                    options.callback(result, e.data);
                }

                parent.append(thebody.hide());
                box.remove();
                background.remove();
            }

            buttonsbar.find('input').click(closeDialog);
            box.find('.close').click(closeDialog);

            $(document.body)
                .append(background)
                .append(box);

            var left = (background.width() - box.width()) / 2;
            var top = (background.height() - box.height()) / 2;

            box.css('left', left + 'px')
                .css('top', top + 'px');

            if (options.onLoad && typeof options.onLoad === "function")
                options.onLoad();


            return this;
        }
    };

    $.fn.modal = function (options) {
        if (typeof options === "undefined" || typeof options === "object")
            return methods.init.call(this, options);
    }
})();