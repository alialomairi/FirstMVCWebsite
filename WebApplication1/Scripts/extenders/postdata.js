/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.postdata = function (optiont) {
    var sittings = {
        loadService: this.attr('href'),
        action: 'replace',
        remove: true
    };

    $.registerEvent(this, "oncallback");

    var me = this;

    if (this.data("remove") != undefined)
        sittings.remove = this.data("remove");

    if (this.data("action") != undefined)
        sittings.action = this.data("action");

    if (this.data("loader") != undefined)
        sittings.loader = this.siblings(this.data("loader"));


    this.sendRequest = function(callback){
        var data = this.data('data') || {};
        var target;
        var oldrecord = $(this).closest('li') 
        if (oldrecord.length > 0 && oldrecord.data('id')) {
            data.id = oldrecord.data('id');
            target = oldrecord;
        }
        if (typeof this.data('target') === "string")
            target = $(this.data('target'));
        if (typeof this.data('closetarget') === "string")
            target = this.closest(this.data('closetarget'));

        if (sittings.loader)
            sittings.loader.show();

        $.sendRequest(sittings.loadService, data, function (data) {
                   
        if (sittings.loader)
            sittings.loader.hide();

            if (typeof data.d === "object")
                data = data.d;

            if(typeof callback === "function")
                    callback({
                        success: data.success,
                        error: data.error
                    });

            $.raiseEvent(me.events.oncallback, me, {data:data});

            if (data.success) {
                if (sittings.remove && data.html == null) {
                    
                    if(target) target.remove();

                } else if (sittings.action == "add") {//add

                    var newrecord = $(data.html);
                    newrecord.appendTo(target).execteExtenders();


                } else if (sittings.action == "callback") {//callback

                    var mcallback = me.data('callback');
                    if (mcallback && typeof mcallback === 'function')
                        mcallback(data);

                } else if (sittings.action == "content") {

                    target.html(data.html);
                    target.children().execteExtenders();

                    if (target.closest('[data-extender=contentlist]').length > 0) {
                        var list = target.closest('[data-extender=contentlist]');
                        list.data('component').routeClicks.call(target);

                    }

                } else { // replace

                    var newrecord = $(data.html);
                    newrecord.insertAfter(target).execteExtenders();
                    target.remove();

                    if (newrecord.closest('[data-extender=contentlist]').length > 0) {
                        var list = newrecord.closest('[data-extender=contentlist]');
                        list.data('component').routeClicks.call(newrecord);

                    }
                }

                
                $.msgbox('لقد تمت العملية بنجاح', "Success", 'ok', 'information');
        }
        else
            $.msgbox(data.error, "Error", 'ok', 'error');
        });
    };

    this.click($.createHandler(this,function (sender, e) {
        sender = $(sender);

        e.preventDefault();

        var confirmmsg = this.data('confirmmsg');
        if (confirmmsg) {
            $.msgbox(confirmmsg, sender.text(), 'yesNo', 'exclamation', $.createDelegate(this, function (result) {
                if (result == 'yes') {
                    this.sendRequest();
                }
            }));
        }
        else
            this.sendRequest();
    }));

    return this;
};