$.fn.codesample = function () {
    function togglepage() {
        codetab.toggleClass('active');
        prevtab.toggleClass('active');
        codepage.toggle();
        prevpage.toggle();
    }

    var title = this.text();

    var container = $('<div class="codesample"></div>').insertAfter(this);
    container.append('<span class="title">' + title + '</span>');
    var tabbar = $('<div class="tabbar"></div>').appendTo(container);
    var codetab = $('<span class="active">Code</span>').appendTo(tabbar).click(togglepage);
    var prevtab = $('<span>Previw</span>').appendTo(tabbar).click(togglepage);

    var source = this.attr('href');
    var pagecont = $('<div class="pagecont" />').appendTo(container);
    var codepage = $('<pre />').appendTo(pagecont);
    var codeblock = $('<code class="lang-html" />').appendTo(codepage);
    var prevpage = $('<iframe src="' + source + '" style="display:none;"  />').appendTo(pagecont);

    $.get(source, function (code) {
        codeblock.text(code);
        hljs.highlightBlock(codepage.find('code')[0]);
    });

    this.hide();
};