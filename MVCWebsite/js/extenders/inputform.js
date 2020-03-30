/// <reference path="../jquery-1.8.2.min.js" />
/// <reference path="../extend-1.0.js" />

$.fn.inputform = function (options) {
    var EditMode = {
        None: 0,
        Add: 1,
        Edit: 2
    };

    var getDefaultValue = $.createDelegate(this, function (field) {

        var value = '';
        if (field.ctrl.length > 0) {
            if (!this.hasClass('filterform') && $.findComponent('.filterform') && field.hasClass('category')) {
                var propname = field.data('name');
                value = $.findComponent('.filterform').categoryfields[propname].ctrl.val();
            }
            else if (field.ctrl.prop('tagName').toLowerCase() == 'select') {
                value = field.ctrl.find('option:first').val();
            }
            else if (field.ctrl.prop('tagName').toLowerCase() == 'input' && field.ctrl.attr('type').toLowerCase() == 'checkbox') {
                value = false;
            }
            else if (field.ctrl.prop('tagName').toLowerCase() == 'input' && field.ctrl.attr('type').toLowerCase() == 'hidden') {
                value = field.ctrl.val();
            }
        }

        return value;
    });

    function init(options) {
        this.editmode = EditMode.None;

        this.extend(methods);

        var form = this;
        this.files = this.find('input[type=file]');
        this.categoryfields = {};
        var fields = {};
        var getFields = {};
        this.find('li').each(function (i, field) {
            field = $(field);
            if (field.hasClass('group')) return;
            if (field.hasClass('commandbar')) {
                field.find('input[type=submit]').click(function (e) {
                    e.preventDefault();
                    if (form.validate())
                        $.raiseEvent(form.events.onsubmit, form, e);
                });
                field.find('input[type=reset]').click(function (e) {
                    e.preventDefault();

                    form.reset();
                });

                return;
            }

            var propname = field.data('name');
            var label = field.find('span').text();
            var ctrl = field.find('input, select, textarea');
            var confirm = field.data('confirm');

            fields[propname] = field.extend({
                label: label,
                ctrl: ctrl,
                confirm: confirm
            });

            if (field.hasClass('submit')) {
                ctrl.change(function (e) {
                    if (form.validate())
                        $.raiseEvent(form.events.onsubmit, form, e);
                });
            }

            if (field.hasClass('category')) {
                form.categoryfields[propname] = field;
            }
        });

        if (this.data('getfield') != undefined) {
            addedFields = this.data('getfield').split(',');
            $(addedFields).each(function (i, field) {

                field = $(field);

                var propname = field.data('name');
                var label = field.find('span').text();
                var ctrl = field.find('input, select, textarea');
                var confirm = field.data('confirm');

                getFields[propname] = field.extend({
                    label: label,
                    ctrl: ctrl,
                    confirm: confirm
                });

                form.getFields = getFields;
            });
        }

        this.fields = fields;
        $.registerEvents(this, [
            'onsubmit',
            'ongetdata',
            'onsetdata'
        ]);

        return this;
    }
    var methods = {
        removevalidators: function () {
            this.find('.validator').remove();
        },
        validate: function () {
            this.removevalidators();

            var isvalid = true;
            for (var propname in this.fields) {
                var field = this.fields[propname];
                if (field.ctrl.length > 0) {
                    if (field.ctrl.hasClass('required') && field.ctrl.val() == '') {
                        if (field.ctrl.hasClass('onadd') && this.editmode != EditMode.Add)
                            continue;

                        $(field.ctrl).before('<span class="validator">ضروري</span>');
                        isvalid = false;
                    }
                    else if (typeof field.confirm === "string") {
                        var checkvalue = this.fields[field.confirm].ctrl.val();
                        if (field.ctrl.val() != checkvalue) {
                            $(field.ctrl).before('<span class="validator">غير مطابق</span>');
                            isvalid = false;
                        }
                    }
                }
            }

            return isvalid;
        },
        reset: function () {
            var defaults = {};
            for (var propname in this.fields) {
                var field = this.fields[propname];

                var value = getDefaultValue(field);

                defaults[propname] = value;
            }

            this.setData(defaults);
        },
        resetField: function (name) {
            var field = this.fields[name];

            var defaults = {};
            defaults[name] = getDefaultValue(field);

            this.setData(defaults);
        },
        getData: function (id) {
            var data = {};
            var me = this;
            var allFields = {};
            if (this.getFields != undefined)
                // allFields = $.merge(this.fields, this.getFields);
                $.extend(allFields, this.fields, this.getFields);
            else
                allFields = this.fields;

            for (var propname in /*this.fields*/allFields) {
                var field = /*this.fields*/allFields[propname];

                if (field.ctrl.length > 0) {
                    if (typeof field.confirm === "undefined") {
                        if (field.ctrl.length == 1) {
                            if (field.ctrl.prop('tagName').toLowerCase() == 'input' && field.ctrl.attr('type').toLowerCase() == 'checkbox')
                                data[propname] = field.ctrl.prop('checked');
                            else
                                data[propname] = field.ctrl.val();
                        }
                        else {
                            data[propname] = [];
                            field.ctrl.each(function (i, ctrl) {
                                ctrl = $(ctrl);

                                if (ctrl.prop('tagName').toLowerCase() == 'input' && ctrl.attr('type').toLowerCase() == 'checkbox')
                                    data[propname][i] = ctrl.prop('checked');
                                else
                                    data[propname][i] = ctrl.val();
                            });
                        }
                    }
                }
            }

            if (id != undefined)
                data.id = id;

            $.raiseEvent(this.events.ongetdata, this, { data: data });
            return data;
        },
        setData: function (data) {
            this.removevalidators();

            if (typeof data.id === "undefined")
                this.editmode = EditMode.Add;
            else
                this.editmode = EditMode.Edit;

            var readOnlyData;
            if (data["readOnlyData"] != undefined)
                readOnlyData = data["readOnlyData"].split(",");

            var hideData;
            if (data["hideData"] != undefined)
                hideData = data["hideData"].split(",");

            var me = this;
            for (var key in this.fields) {
                var field = me.fields[key];

                if (data["readOnlyData"] != undefined)
                    if (readOnlyData.includes(key))
                        field.ctrl.prop('disabled', 'true');

                if (data["hideData"] != undefined)
                    if (hideData.includes(key))
                        field.css('display', 'none');

                if (field.ctrl.length > 0) {
                    if (key in data) {
                        if (field.ctrl.prop('tagName').toLowerCase() == 'input' && field.ctrl.attr('type').toLowerCase() == 'checkbox')
                            field.ctrl.prop('checked', data[key]);
                        else if (field.ctrl.data("component") && typeof field.ctrl.data("component").reset === "function")
                            field.ctrl.data("component").reset(data[key]);
                        else {
                            if (field.ctrl.data("component") && typeof field.ctrl.data("component").setTargetValue === "function") {
                                var targetValue = data[$(field.ctrl.data("target")).closest('li').data('name')];
                                field.ctrl.data("component").setTargetValue(targetValue);
                            }
                            field.ctrl.val(data[key]);
                        }
                        field.ctrl.change();
                    }
                }
            }
            $.raiseEvent(this.events.onsetdata, this, { data: data });
        }
    };

    if (typeof options === "undefined" || typeof options === "object")
        return init.call(this, options);
    if (typeof options === "string" || typeof methods[options] === "function")
        return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
};
