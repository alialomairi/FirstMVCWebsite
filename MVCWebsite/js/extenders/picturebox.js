/// <reference path="../jquery-1.7.1.min.js" />
/// <reference path="../extend-1.0.min.js" />

$.fn.picturebox = function () {

    var service = this.data('service');
    var data = { id: this.data('id') };
    var datainputs = {};

    var source = this.data('picture');
    var boxStyle = this.data('boxstyle');
    var size = this.data('size');
    var extentions = this.attr('accept').split(',');
    var dimintions = size.split(',');
    var width = dimintions[0].trim();
    var height = dimintions[1].trim();
    var frameSize = {
        width: parseInt(dimintions[0].trim()),
        height: parseInt(dimintions[1].trim())
    };

    if (typeof (service) !== "string") {
        datainputs.size = $('<input type="hidden" name="size" value="' + size + '" />').insertAfter(this);
        datainputs.position = $('<input type="hidden" name="position" value="0, 0" />').insertAfter(this);
        datainputs.zoom = $('<input type="hidden" name="zoom" value="1" />').insertAfter(this);
    }

    this.css({
        'width': width + 'px',
        'height': height + 'px',
        'opacity': '0'
    });

    var picture = $('<img style="position:absolute" />').attr('src', source);
    var box = $('<div style="position:relative;overflow:hidden;" />').insertBefore(this).append(picture).append(this);

    if (boxStyle)
        box.addClass(boxStyle);

    this.change(function (e) {
        var input = this;

        // 1. validate file extention.
        var ext = this.value.substr(this.value.indexOf('.'));

        var valid = false;

        $.each(extentions, function (i, extention) {
            if (ext.toLowerCase() == extention.trim().toLowerCase()) {
                valid = true;
                return;
            }
        });

        if (!valid) {
            e.preventDefault();
            alert('هذا الملف غير صحيح.');
            return;
        }

        // 2. display the selected image.
        var reader = new FileReader();

        reader.onload = function (e) {
            picture.attr('src', e.target.result);


            //if (originalSize.width < frameSize.width || originalSize.height < frameSize.height) {
            //}
            //else {
            //}
            picture.get(0).onload = function (e) {
                // create the clipping dialog
                var dialog = $('<div style="padding: 40px 100px;" />').appendTo(document.body);
                // create a frame with the picture in it.
                var frame = $('<div />').css({
                    width: frameSize.width,
                    height: frameSize.height,
                    margin: '0 auto',
                    border: 'solid 2px #000',
                    overflow: 'hidden',
                    cursor: 'move',
                    position: 'relative'
                }).appendTo(dialog).append(picture.css('width', '').css('height', ''));

                //add the zoom controls.
                var ctrldiv = $('<div style="text-align:center;" />').appendTo(dialog);
                var zoomin = $('<input type="button" value="+" />').appendTo(ctrldiv);
                var zoomper = $('<span />').text('100%').appendTo(ctrldiv);
                var zoomout = $('<input type="button" value="-" />').appendTo(ctrldiv);

                // centerize the picture in the frame.
                var currpos = {
                    x: frameSize.width / 2,
                    y: frameSize.height / 2
                };

                function locatePicture() {
                    var w = picture.width();
                    var h = picture.height();

                    if (currpos.x < frameSize.width - w / 2)
                        currpos.x = frameSize.width - w / 2;
                    if (currpos.y < frameSize.height - h / 2)
                        currpos.y = frameSize.height - h / 2;
                    if (currpos.x > w / 2)
                        currpos.x = w / 2;
                    if (currpos.y > h / 2)
                        currpos.y = h / 2;


                    picture.css({
                        left: (currpos.x - w / 2) + 'px',
                        top: (currpos.y - h / 2) + 'px'
                    });
                }
                // zoom picture in and out.
                function zoomPicture() {
                    var zoom = zooms[zoomindex];

                    picture.css({
                        width: zoom * originalSize.width,
                        height: zoom * originalSize.height
                    });

                    zoomper.text((zoom * 100).toFixed(2) + '%');

                    locatePicture();
                }

                zoomin.click(function (e) {
                    if (zoomindex == 0)
                        return;

                    zoomPicture(--zoomindex);
                });

                zoomout.click(function (e) {
                    if (zoomindex == 5)
                        return;

                    zoomPicture(++zoomindex);
                });

                // move the picture
                var attached = false;
                var pos1, pos2;
                frame.mousedown(function (e) {
                    attached = true;
                    pos1 = {
                        x: e.clientX,
                        y: e.clientY
                    };
                });

                frame.mousemove(function (e) {
                    if (attached) {
                        pos2 = {
                            x: e.clientX,
                            y: e.clientY
                        };

                        currpos.x += pos2.x - pos1.x;
                        currpos.y += pos2.y - pos1.y;


                        locatePicture();

                        pos1 = pos2;
                    }
                });

                dialog.mouseup(function () {
                    attached = false;

                });

                picture.get(0).ondragstart = function (e) {
                    e.preventDefault();
                    return false;
                }

                dialog.modal({
                    title: 'تعديل الصورة',
                    buttons: 'ok',
                    callback: function (result) {
                        if (result == 'ok') {
                            data.zoom = zooms[zoomindex];
                            data.left = picture.position().left;
                            data.top = picture.position().top;

                            if (typeof (service) === "string")
                                $.sendFiles(service, data, $(input), function (data) {

                                    if (!data.success) {
                                        picture.attr('src', source);
                                        $.msgbox(data.error, '', 'ok', 'error');
                                    }
                                });
                            else {
                                datainputs.position.val(picture.position().left + ',' + picture.position().top);
                                datainputs.zoom.val(zooms[zoomindex])

                            }
                        }
                        else {
                            picture.attr('src', source);
                        }
                        picture.insertBefore(box.children().first());
                    }
                });

                // calcalate possible zooms
                var zooms = [];
                var zoomindex = 0;
                function calculateZooms() {
                    var xzoom = frameSize.width / originalSize.width;
                    var yzoom = frameSize.height / originalSize.height;
                    var minzoom = Math.max(xzoom, yzoom);

                    var increment = (1.00 - minzoom) / 5;
                    for (var i = 0; i <= 5; i++) {
                        zooms[zooms.length] = 1 - increment * i;
                    }
                }
                var originalSize = {
                    width: picture.width(),
                    height: picture.height()
                };

                calculateZooms();
                locatePicture();

            };
        }

        reader.readAsDataURL(this.files[0]);

    });

    return this;
};