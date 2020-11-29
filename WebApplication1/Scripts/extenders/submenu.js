$.fn.submenu = function () {
    this.children('.dropdown-toggle').on("click", function (e) {
        e.stopPropagation();
        e.preventDefault();
        $(this).next('.dropdown-menu').toggle();
    });
}