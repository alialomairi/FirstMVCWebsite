/// <reference path="jquery-1.8.2.min.js" />

(function ($) {
    if (!String.prototype.trim) {
        (function () {
            // Make sure we trim BOM and NBSP
            var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
            String.prototype.trim = function () {
                return this.replace(rtrim, '');
            };
        })();
    }
    $.extend({
        sendFiles: (function () {
            var UId = 1;
            return function (service, data, files, callback, uselocalpath) {
                var path = service;

                var name = 'uploan' + (UId++);
                var iframe = $('<iframe name="' + name + '" style="display:none" />')
                    .appendTo(document.body);
                var form = $('<form method="post" action="' + path + '" target="' + name + '" enctype="multipart/form-data"  />')
                    .appendTo(document.body);


                 //add data to the form.
                for (var key in data) {
                    var value = data[key];
                    var input = $('<input type="hidden" name="' + key + '"/>')
                        .val(value)
                        .appendTo(form);
                }

                // add file inputs to the form.
                for (var i = 0; i < files.length; i++) {
                    var file = $(files[i]);

                    //clone the file input to place a clean version in the dialog.
                    var clone = file.clone();
                    clone.insertBefore(file);

                    // add the file input to the form.
                    form.append(file);
                }

                // add the form to the document body.
                iframe.load(function (e) {
                    //retrieve the response document
                    var doc = iframe.prop('contentDocument');

                    // get the response text
                    var strdata = '';
                    if (doc.body.firstChild && doc.body.firstChild.nodeName.toUpperCase() == 'PRE') {
                        strdata = doc.body.firstChild.firstChild.nodeValue;
                    } else strdata = doc.body.innerHTML;

                    // parse the response as json.
                    var data = $.parseJSON(strdata);
                    if (typeof callback === "function")
                        callback(data);

                    // remove both the form and the frame.
                    clone.before(file).remove();
                    form.remove();
                    iframe.remove();
                });

                form.submit();
            }
        })(),
        sendRequest: function (service, data, callback, uselocalpath) {
            $.ajax({
                type: 'POST',
                url: (uselocalpath ? location.pathname + '/' : '') + service,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: callback
            });
        },
        findComponent: function (selecter) {
            return $(selecter).data('component');
        },
        registerNamespace: function (namespace, parent) {
            if (!parent) parent = window;

            var parts = namespace.split('.');
            var child = parts[0];
            if (!parent[child]) parent[child] = {};
            if (parts.length > 1) {
                namespace = namespace.substr(namespace.indexOf('.') + 1)
                $.registerNamespace(namespace, parent[child]);
            }
        },
        registerEvents: function (instance, eventTypes) {
            for (var i = 0; i < eventTypes.length; i++)
                $.registerEvent(instance, eventTypes[i]);
        },
        registerEvent: function (instance, eventType) {
            instance.events = this.events || {};
            instance.events[eventType] = [];
        },
        addEventHandler: function (instance, eventType, handler) {
            if (instance.events == undefined) instance.events = {};

            var event = instance.events[eventType];
            if (event == null) instance.events[eventType] = [handler];
            else {
                var index = event.length;
                event[index] = handler;
            }
        },
        removeEventHandler: function (instance, eventType, handler) {
            var event = instance.events[eventType];
            if (event == null) return;

            for (var i = 0; i < event.length; i++) {
                if (event[i] == handler) {
                    event[i] = null;
                    for (var j = i; j < event.length; j++) {
                        event[j] = event[j + 1];
                    }
                    event[event.length - 1] = null;
                    event.length = event.length - 1;

                    break;
                }
            }
        },
        removeAllEventHandlers: function (instance, eventType) {

            var event = instance.events[eventType];
            if (event == null) return;

            delete instance.events[eventType];

            instance.events[eventType] = [];

        },
        raiseEvent: function (handlers, sender, e) {
            if (handlers == null || handlers.length === 0) return;
            for (var i = 0; i < handlers.length; i++) {
                var handler = handlers[i];
                handler(sender, e);
            }
        },
        createDelegate: function (context, method) {
            return function () {
                return method.apply(context, arguments);
            }
        },
        createHandler: function (context, method) {
            return function (e) {
                var sender = this;
                return method.call(context, sender, e);
            }
        },
        registerDispose: function (instance, callback) {
            var dispose = instance.dispose;

            instance.dispose = function () {
                if (typeof (dispose) === "function")
                    dispose();

                callback();
            }
        }
    });
    function execteExtenders() {
        var component = this;
        if (component.attr('data-executed'))
            return;

        var extendernames = component.data('extender').split(',');

        $.each(extendernames, function (i, name) {
            name = name.trim();

            if (typeof component[name] === "function") {
                component[name]();
            }
        });
        component.data('component', component);
        component.attr('data-executed', 'executed');
    }

    $.fn.execteExtenders = function () {
        this.each(function (i, element) {
            element = $(element);

            if (element.data('extender'))
                execteExtenders.call(element);

            element.find('[data-extender]').each(function (i, child) {
                child = $(child);

                execteExtenders.call(child);
            });
        });    

        return this;
    };
    function disposeExtenders() {

          var component = this.data('component');

          if (component) {
              if (component.dispose && typeof (component.dispose === "function")) {

                  component.dispose();

              }
              component.removeAttr("data-executed");
          }
    }
    $.fn.disposeExtenders = function () {
        this.each(function (i, element) {
            element = $(element);

            element.find('[data-extender]').each(function (i, child) {
                child = $(child);

                disposeExtenders.call(child);
            });

            if (element.data('extender'))
                disposeExtenders.call(element);
        });

        return this;
    };

    $(document).ready(function () {
        $(document.body).execteExtenders();
    });



})(window.jQuery);