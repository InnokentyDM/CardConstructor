
//$(document).ready(function () {
//    var tmp = document.getElementsByClassName("preview");
//    var cnv = [];
//    var cnv_s2 = [];
//    for (var i = 0; i < tmp.length; i++)
//    {
//        cnv[i] = tmp[i];
//    }
//    var canv = new fabric.Canvas();
//    var hiddenId = document.getElementsByClassName("hiddenId");
//    var hiddenWidth = document.getElementsByClassName("hiddenWidth");
//    var hiddenHeight = document.getElementsByClassName("hiddenHeight");
//    var tmp_s2 = document.getElementsByClassName("preview_s2");
//    for (var i = 0; i < tmp.length; i++) {
//        cnv_s2[i] = tmp_s2[i];
//    }
//    var canv_s2 = new fabric.Canvas();
//    var hiddenId_s2 = document.getElementsByClassName("hiddenId_s2");
//    renderHiddenJson(cnv, canv, hiddenId, hiddenWidth, hiddenHeight);
//    renderHiddenJson(cnv_s2, canv_s2, hiddenId_s2, hiddenWidth, hiddenHeight);

    
//});

$(document).ready(function () {
    var imageArr = document.getElementsByClassName('images');
   
    setImageSize(imageArr);

    $(window).bind('resize', function (e) {
        window.resizeEvt;
        $(window).resize(function () {
            clearTimeout(window.resizeEvt);
            window.resizeEvt = setTimeout(function () {
                setImageSize(imageArr);
            }, 250);
        });
    });

});




var setImageSize = function(imageArr) 
{
    for (var i = 0; i < imageArr.length; i++) {
        var item = imageArr[i];
        var max_width = window.innerWidth / 7;
        var max_height = 143;
        var factor = 0;
        var new_card = document.getElementById('new-card');
        if (item.width > item.height) {
            var width = item.width;
            var height = item.height;
            factor = width / max_width;
            item.width = max_width;
            item.height = height / factor;
            if (new_card != null) {
                new_card.style.width = "" + max_width + "px";
                var th = height / factor;
                new_card.style.height = "" + th + "px";
            }
        }
        else if (item.width < item.height) {
            var width = item.width;
            var height = item.height;
            factor = height / max_height;
            item.height = max_height;
            item.width = width / factor;
            if (new_card != null) {
                var tw = height / factor;
                new_card.style.width = "" + tw + "px";
                new_card.style.height = "" + max_height + "px";
            }
        }

    }
}

        


//        function zoomIt(canvas, factor) {
//            canvas.setHeight(canvas.getHeight() * factor);
//            canvas.setWidth(canvas.getWidth() * factor);
//            if (canvas.backgroundImage) {
//                // Need to scale background images as well
//                var bi = canvas.backgroundImage;
//                bi.width = bi.width * factor; bi.height = bi.height * factor;
//            }
//            var objects = canvas.getObjects();
//            for (var i in objects) {
//                var scaleX = objects[i].scaleX;
//                var scaleY = objects[i].scaleY;
//                var left = objects[i].left;
//                var top = objects[i].top;

//                var tempScaleX = scaleX * factor;
//                var tempScaleY = scaleY * factor;
//                var tempLeft = left * factor;
//                var tempTop = top * factor;

//                objects[i].scaleX = tempScaleX;
//                objects[i].scaleY = tempScaleY;
//                objects[i].left = tempLeft;
//                objects[i].top = tempTop;

//                objects[i].setCoords();
//            }
//            canvas.renderAll();
//            canvas.calcOffset();
//        }


//function renderHiddenJson(cnv, canv, hiddenId, hiddenWidth, hiddenHeight)
//        {
//        var i, tmpId;
//        var canvas = [], imageQuery = [];
//    for (i = 0; i < cnv.length; i++) {
//        tmpId = cnv[i].id;
       
//        canvas[i] = new fabric.StaticCanvas(tmpId, { backgroundColor: 'rgb(255,255,255)' });
//        document.getElementById(tmpId).fabric = canvas[i];
//        setDimensions(tmpId, hiddenWidth[i].value, hiddenHeight[i].value);
//        tmpId = hiddenId[i].id;
//        imageQuery[i] = jQuery("#" + tmpId).val();
//    }
//    for (i = 0; i < canvas.length; i++) {
//        if (imageQuery[i] != "") {
//        var obj = JSON.parse(imageQuery[i]);
//        var factor = 0.3;
//        canvas[i].renderAll();
//        canv = canvas[i];
//        console.log('this is a callback. invoked when canvas is loaded!xxx ');
//        }
//        else
//        {
//            var idd = canvas[i].lowerCanvasEl.id;
//            var obj = document.getElementById("" + idd);
//            obj.className += ' hidden';
//        }
//    }
//}

//function setDimensions(id, width, height) {
//    var canvas = document.getElementById(id).fabric;
//    if (!Number.isInteger(width)) {
//        width = parseInt(width, 10);
//    }
//    if (!Number.isInteger(height)) {
//        height = parseInt(height, 10);
//    }
//    if (width > height) {
//        actWidth = width * 30 / 2.54;
//        actHeight = height * 30 / 2.54;
//        var tmpWidth = 150;
//        var factor = width / tmpWidth;
//        var tmpHeight = height / factor;
//        //canvas.upperCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
//        //canvas.upperCanvasEl.style.height = tmpHeight + "px"; //actHeight * 0.7 + "px";
//        canvas.lowerCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
//        canvas.lowerCanvasEl.style.height = tmpHeight + "px"; //actHeight * 0.7 + "px";
//        //canvas.wrapperEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
//        //canvas.wrapperEl.style.height = tmpHeight + "px"; //actHeight * 0.7 + "px";
//        //canvas.upperCanvasEl.width = actWidth;
//        //canvas.upperCanvasEl.height = actHeight;
//        canvas.lowerCanvasEl.width = actWidth;
//        canvas.lowerCanvasEl.height = actHeight;
//        canvas.width = actWidth;
//        canvas.height = actHeight;
//        canvas = document.getElementById(id);
//        canvas.width = actWidth;
//        canvas.height = actHeight;
//    }
//}