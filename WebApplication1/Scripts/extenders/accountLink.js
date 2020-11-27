/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../extend-1.0.min.js" />

$.fn.accountLink = function () {
    this.click($.createHandler(this,function (sender, e) {
        e.preventDefault();

        $(sender).siblings('ul').toggle();
    }));
};