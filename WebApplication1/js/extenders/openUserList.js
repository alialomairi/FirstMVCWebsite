$.fn.openUserList = function () {
    this.click(function (e) {
        e.preventDefault();
        $('#userlist').toggleClass("opened");
    });

    return this;
};