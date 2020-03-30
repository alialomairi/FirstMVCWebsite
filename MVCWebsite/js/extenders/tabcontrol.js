/// <reference path="../jquery-1.7.1.js" />
/// <reference path="../extend-1.0.js" />

$.fn.tabcontrol = function () {
    var tabPages = this.children('li');
    tabPages.first().addClass('selected');

    tabPages.children('a').click(function (e) {
        e.preventDefault();

        var tabPage = $(this).parent();
        tabPages.removeClass('selected');
        tabPage.addClass('selected');
    });

    return this;
};