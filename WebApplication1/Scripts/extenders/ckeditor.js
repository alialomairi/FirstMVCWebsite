$.fn.ckeditor = function () {
    CKEDITOR.replace(this.attr('id'), {
        extraAllowedContent : 'iframe[!src];a(sample)[!href,data-extender]'
    });

    return this;
};