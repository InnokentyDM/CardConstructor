//DropDownList post request for aplication prica processing;
$(document).ready(function () {
    var canvas = new fabric.StaticCanvas('canvas', {
        backgroundColor: 'rgb(255,255,255)'
    });
    document.getElementById('canvas').fabric = canvas;
    var width = $('#hiddenWidth').val();
    var height = $('#hiddenHeight').val();
    setDimensions('canvas', width, height);
    var image = jQuery("#hiddenImage").val();
    parseHiddenJSON(canvas, image);
    var image2 = jQuery("#hiddenImage_s2").val();
    if (image2 != "") {
        $('#canvas_s2').removeClass('hidden');  
        var canvass2 = new fabric.StaticCanvas('canvas_s2', {
            backgroundColor: 'rgb(255,255,255)'
        });
        document.getElementById('canvas_s2').fabric = canvass2;
        setDimensions('canvas_s2', width, height);
        parseHiddenJSON(canvass2, image2);
        //var arr = document.getElementsByClassName('canvas-container');
        //var arr2 = document.getElementsByClassName('upper-canvas');
        //arr2[0] = 'ufside';
        //arr2[1] = 'usside';
        //arr2[1].classList.add('hidden');
        //arr[0].id = 'fside';
        //arr[1].id = 'sside';
        //arr[1].classList.add('hidden');
    }

    

    //    $.ajax({
    //        url: "../../Home/SendImage",
    //        type: 'GET',
    //        dataType: 'json',
    //        success: function (data) {
    //            $.each(JSON.parse(data), function (index, item) {
    //                canvas.loadFromJSON(data, function (obj) {
    //                    canvas.renderAll();
    //                    console.log(' this is a callback. invoked when canvas is loaded!xxx ');
    //                });
    //            });             
    //        },
    //    error: function (xhr, ajaxOptions, thrownError) {
    //        alert(xhr.status);
    //        alert(thrownError);
    //    }
    //});


    //$("#processPrice").click(function () {
    //    var count = jQuery("#ddl1 option:selected").val();
    //    //var materialId = jQuery("#ddl2 option:selected").val();    
    //    $.ajax({
    //        type: 'POST',
    //        url: "https://localhost:44303/Home/getApplyServiceId/",
    //        data: { materialId: materialId, count: count },
    //        success: function (resultPrice) {               
    //           $("#resultPrice").val(resultPrice);
    //        }
    //    }).error(function (err) {
    //        console.log(err);
    //    });
    //})
    $('#deliveryChk').change(function () {
        if ($(this).is(':checked')) {
            $('.delivery').removeClass('hidden');
        }
        else {
            $('.delivery').addClass('hidden');
        }
    });
});


function parseHiddenJSON(canvas, image) {
    $(document).ready(
    function () {
        $.each(JSON.parse(image), function (index, item) {
            var objct = JSON.parse(image);
            canvas.loadFromJSON(image, function (obj) {
                var objcts = canvas.getObjects();
                for (var i = 0; i < objcts.length; i++) {
                    if (objcts[i].type == 'i-text') {
                        objcts[i].lockMovementX = true;
                        objcts[i].lockMovementY = true;
                        objcts[i].lockScalingX = true;
                        objcts[i].lockScalingY = true;
                        objcts[i].setControlVisible('mb', false);
                        objcts[i].setControlVisible('ml', false);
                        objcts[i].setControlVisible('mr', false);
                        objcts[i].setControlVisible('mt', false);
                        objcts[i].setControlVisible('bl', false);
                        objcts[i].setControlVisible('br', false);
                        objcts[i].setControlVisible('tl', false);
                        objcts[i].setControlVisible('tr', false);
                        objcts[i].setControlVisible('mtr', false);

                    }
                }
                if (objct.backgroundImage) {
                    canvas.setWidth(objct.backgroundImage.width);
                    canvas.setHeight(objct.backgroundImage.height);
                }
                canvas.renderAll();
                console.log(' this is a callback. invoked when canvas is loaded!xxx ');
            });
        });
    });
}


function setDimensions(id, width, height) {
    var canvas = document.getElementById(id).fabric;
    //canvas.upperCanvasEl.style.width = width;
    //canvas.upperCanvasEl.style.height = height;
    //canvas.lowerCanvasEl.style.width = width;
    //canvas.lowerCanvasEl.style.height = height;
    if (!Number.isInteger(width)) {
        width = parseInt(width, 10);
    }
    if (!Number.isInteger(height)) {
        height = parseInt(height, 10);
    }
    if (width > height) {
        actWidth = width * 30 / 2.54;
        actHeight = height * 30 / 2.54;
        var tmpWidth = 500;
        var factor = width / tmpWidth;
        var tmpHeight = height / factor;
        //canvas.upperCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        //canvas.upperCanvasEl.style.height = tmpHeight + "px"; //actHeight * 0.7 + "px";
        canvas.lowerCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.lowerCanvasEl.style.height = tmpHeight + "px"; //actHeight * 0.7 + "px";
        //canvas.wrapperEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        //canvas.wrapperEl.style.height = tmpHeight + "px"; //actHeight * 0.7 + "px";
        //canvas.upperCanvasEl.width = actWidth;
        //canvas.upperCanvasEl.height = actHeight;
        canvas.lowerCanvasEl.width = actWidth;
        canvas.lowerCanvasEl.height = actHeight;
        canvas.width = actWidth;
        canvas.height = actHeight;
        canvas = document.getElementById(id);
        canvas.width = actWidth;
        canvas.height = actHeight;
    }
}