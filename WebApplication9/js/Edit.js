$(document).ready(function () {
    var canvasLoaded = false;
    var qrcode;
    var currentCanvas;
    var ifSide2 = true;
    var save = document.getElementById('save').addEventListener('click', canvasToImage, false);
    var saveTo = document.getElementById('saveTo').addEventListener('click', canvasToImage, false);
    initCanvas('canvas');
    var canvas = document.getElementById('canvas').fabric;
    currentCanvas = document.getElementById('canvas').fabric;
    var image = jQuery("#hiddenImage").val();
    parseHiddenJSON(canvas, image);
    var image2 = jQuery("#hiddenImage_s2").val();
    if (image2 != "") {
        initCanvas('canvas_s2');
        var canvass2 = document.getElementById('canvas_s2').fabric;
        $('#side2').removeClass('hidden');
        $('#side1').removeClass('hidden');
        parseHiddenJSON(canvass2, image2);
        var arr = document.getElementsByClassName('canvas-container');
        var arr2 = document.getElementsByClassName('upper-canvas');
        arr2[0] = 'ufside';
        arr2[1] = 'usside';
        arr2[1].classList.add('hidden');
        arr[0].id = 'fside';
        arr[1].id = 'sside';
        arr[1].classList.add('hidden');
    }

    var state = [];
    var mods = 0;
 
    currentCanvas.on(
        'object:modified', function () {
            updateModifications(true);
        },
        'object:added', function () {
            updateModifications(true);
        },
         'object:removed', function () {
             updateModifications(true);
         });


    function updateModifications(savehistory) {
        removeSafety(canvas);
        if (savehistory === true) {
            myjson = JSON.stringify(canvas);
            state.push(myjson);
        }
        addSafety(canvas);
    }
  

    $('#qrcodeev').click(function () {      
        var text = $('#qrcodein').val();
        var myNode = document.getElementById("qrcode");
        if (myNode.firstChild)
        {
            clearQr();
        }
        if (text != "") {
            var element = document.createElement('div');
            var src;
            qrcode = new QRCode(document.getElementById('qrcode'), {
                text: text,
                width: 256,
                height: 256,
                colorDark: "#000000",
                colorLight: "#ffffff",
                correctLevel: QRCode.CorrectLevel.H
            });
        }
        else {
            var div = document.getElementById("qrcode");
            var oNewP = document.createElement("p");
            var oText = document.createTextNode("Введите текст");
            oNewP.color = 'red';
            oNewP.appendChild(oText);
            div.appendChild(oNewP);
        }
        
    });
    

    $('#modalshow').click(function () {
        var text = $('#qrcodein').val("");
        clearQr();
    });
    
    $('#addQrCode').click(function () {
        var imgSrc = document.getElementById('image');
        var qrcode = imgSrc.src;
        var image = new Image();
        image.src = qrcode;
        resImg = new fabric.Image(image);
        //var resImg = new fabric.Image(image, { width: canvas.width, height: canvas.height });
        currentCanvas.add(resImg);
        currentCanvas.renderAll();
    });

    function clearQr()
    {     
        var myNode = document.getElementById("qrcode");
        while (myNode.firstChild) {
            myNode.removeChild(myNode.firstChild);
        }
    }

    $('#undo').click(function undo() {
        if (mods < state.length) {
            canvas.clear().renderAll();
            canvas.loadFromJSON(state[state.length - 1 - mods - 1]);
            canvas.renderAll();
            //console.log("geladen " + (state.length-1-mods-1));
            //console.log("state " + state.length);
            mods += 1;
            //console.log("mods " + mods);
        }
    });

    $('#redo').click(function redo() {
        if (mods > 0) {
            canvas.clear().renderAll();
            canvas.loadFromJSON(state[state.length - 1 - mods + 1]);
            canvas.renderAll();
            //console.log("geladen " + (state.length-1-mods+1));
            mods -= 1;
            //console.log("state " + state.length);
            //console.log("mods " + mods);
        }
    });

    $('#reset').click(function () {
        canvas.clear();
        parseHiddenJSON(canvas, image);
        var image2 = jQuery("#hiddenImage_s2").val();
        if (image2 != "") {
            canvass2.clear();
            parseHiddenJSON(canvass2, image2);
        }
    });

    $('#uploadImage').change(function uploadImage(e) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var imgObj = new Image();
            imgObj.src = event.target.result;
            imgObj.onload = function () {
                var image = new fabric.Image(imgObj);
                currentCanvas.centerObject(image);
                currentCanvas.add(image);
                currentCanvas.renderAll();
            }
        }
        var files = e.target.files || (e.dataTransfer && e.dataTransfer.files);
        reader.readAsDataURL(files[0]);
    });

    $('#clear').click(function () {
        canvas.clear();
        canvas.backgroundImage = 0;
        canvas.renderAll();
        var image2 = jQuery("#hiddenImage_s2").val();
        if (ifSide2) {
            canvass2.clear();
            canvass2.backgroundImage = 0;
            canvass2.renderAll();
        }
    });

    $('#chkBox').change(function () {
        if ($(this).is(':not(:checked)')) {
            removeSafety(canvas);
            removeSafety(canvass2);
        }
        else {
            addSafety(canvas);
            addSafety(canvass2);
        }
    });

    $('#SaveTo').click(function () {
        canvas.deactivateAll().renderAll();
        var image = JSON.stringify(canvas.toDatalessJSON());
        var model = {
            Id: jQuery("#hiddenId").val(),
            ITEM_TYPE_ID: jQuery("#hiddenItemType").val(),
            layout_id: jQuery("#hiddenLayout").val(),
            text: jQuery("#hiddenText").val(),
            image: image
        }
        jQuery.ajax({
            type: 'POST',
            url: "../../cards/SaveTo",
            data: { model: model },
            success: function (data, status) {
                window.location.href = "../../addCards/UserCards";
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    function canvasToImage() {
        canvas.deactivateAll();
        canvas.forEachObject(function (obj) {
            var id = obj.id;
            if (id == 'safetyBorders') {
                canvas.remove(obj);
            }
            if (id == 'textField') {
                canvas.remove(obj);
            }
        });
        canvass2.forEachObject(function (obj) {
            var id = obj.id;
            if (id == 'safetyBorders') {
                canvass2.remove(obj);
            }
            if (id == 'textField') {
                canvass2.remove(obj);
            }
        });
        var image = JSON.stringify(canvas.toDatalessJSON());

        if ($("#hiddenImage_s2").val() != null) {
            canvass2.deactivateAll();
            canvass2.forEachObject(function (obj) {
                var id = obj.id;
                if (id == 'safetyBorders') {
                    canvas.remove(obj);
                }
                if (id == 'textField') {
                    canvas.remove(obj);
                }
            });
            var images2 = JSON.stringify(canvass2.toDatalessJSON());
            $('#hiddenImage_s2').val(null);
            $('#hiddenImage_s2').val(images2);
        }
        $('#hiddenImage').val(null);
        $('#hiddenImage').val(image);
    }

    function parseHiddenJSON(canvas, image) {
        $(document).ready(
        function handleImage() {
            $.each(JSON.parse(image), function (index, item) {
                var objct = JSON.parse(image);
                canvas.loadFromJSON(image, function (obj) {
                    if (objct.backgroundImage) {

                    }
                    addSafety(canvas);
                    canvas.renderAll();
                    console.log(' this is a callback. invoked when canvas is loaded!xxx ');
                });
            });
        });
        canvasLoaded = true;
    }

  


    function canvasToImageNew() {
        canvas.deactivateAll().renderAll();
        var image = JSON.stringify(canvas.toDatalessJSON());
        var EditCardId = jQuery("#cardId").val();
        $('#hiddenImage').val(null);
        $('#hiddenImage').val(image);
    }

document.onkeydown = function (e) {
    if (46 === e.keyCode) {
        if (currentCanvas.getActiveObject() != null) {
            currentCanvas.remove(currentCanvas.getActiveObject());
            currentCanvas.renderAll();
        }
        else if (currentCanvas.getActiveGroup() != null) {
            var arr = currentCanvas.getActiveGroup();
            arr.objects.forEach(function (object, key) {
                currentCanvas.remove(object);
                arr.removeWithUpdate(object);
            });
            currentCanvas.discardActiveGroup();
            currentCanvas.renderAll();
        }

    }
}


$('input:radio[name=options]').change(function () {
    switch (this.id)
    {
        case 'left':
            var obj = currentCanvas.getActiveObject();
            //if (obj.type == "text" || obj.type == "i-text") {
            //    width = obj.width / 2;
            //    obj.left = width;
            //}
            //else {
            obj.left = 0;
            obj.setCoords();
            //}
            //if (obj.type == 'text' || obj.type == 'i-text') {
            //    currentCanvas.bringToFront(obj);
            //}
            currentCanvas.renderAll();
            break;
        case 'center':
            var obj = currentCanvas.getActiveObject();
            //if (obj.type == "text" || obj.type == "i-text") {
            //    obj.left = currentCanvas.width / 2;
            //}
            //else {
            obj.left = currentCanvas.width / 2 - obj.width * obj.scaleX / 2;
            obj.setCoords();
            //}
            //if (obj.type == 'text' || obj.type == 'i-text') {
            
            //}
            currentCanvas.renderAll();
            break;
        case 'right':
            var obj = currentCanvas.getActiveObject();
            //if (obj.type == "text" || obj.type == "i-text") {
            //    var left = obj.left - (obj.width / 2);
            //    var right = currentCanvas.width - left - obj.width;
            //    obj.left = left + right + obj.width / 2;
            //}
            //else {
                obj.left = 0;
                var right = currentCanvas.width - obj.width * obj.scaleX;
                obj.left = right;
                obj.setCoords();
            //}
            //if (obj.type == 'text' || obj.type == 'i-text')
            //    {
               
            //}
            currentCanvas.renderAll();
            break;

    }
});

$('#bold').click(function () {
    var text = currentCanvas.getActiveObject();
    if (text.fontWeight == "normal")
    {
        //currentCanvas.getActiveObject().setSelectionStyles({ 'fontWeight': "bold" });
        text.fontWeight = "bold";
        $('#bold').button('toggle');
    }
    else if (text.fontWeight == "bold")
    {
        //currentCanvas.getActiveObject().setSelectionStyles({ 'fontWeight': "normal" });
        text.fontWeight = "normal";
        $('#bold').button('toggle');
    }
    currentCanvas.bringToFront(text);
    canvas.renderAll();
});

$('#italic').click(function () {
    var text = currentCanvas.getActiveObject();
    if (text.fontStyle == "normal" || text.fontStyle == "") {
        //currentCanvas.getActiveObject().setSelectionStyles({ 'fontStyle': "italic" });
        text.fontStyle = "italic";
        $('#italic').button('toggle');
    }
    else if (text.fontStyle == "italic") {
        //currentCanvas.getActiveObject().setSelectionStyles({ 'fontStyle': "normal" });
        text.fontStyle = "normal";
        $('#italic').button('toggle');
    }
    currentCanvas.bringToFront(text);
    canvas.renderAll();
});

$('#addText').click(function () {
    currentCanvas.add(new fabric.IText('Нажмите, чтобы печатать', {
        fontFamily: 'arial black',
        fontSize: 22,
        left: 100,
        top: 100,
    }));
    currentCanvas.renderAll();
});

$('#color').change(function () {
    var obj = currentCanvas.getActiveObject();
    var selection = $('#color').val();
    if (obj != null) {
        if (obj.type == 'text' || obj.type == 'i-text') {
            
            currentCanvas.getActiveObject().setSelectionStyles({ 'fill': selection })
            currentCanvas.renderAll();
        }
        else
        {
            
            currentCanvas.getActiveObject().set({ 'fill': selection })
            currentCanvas.renderAll();
        }
    }
    else
    {
        currentCanvas.backgroundColor = selection;
        currentCanvas.renderAll();
    }
});

$('#styleddl').change(function () {
    var url = "https://fonts.googleapis.com/css?family={family}&subset=latin,cyrillic";
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    document.getElementsByTagName('head')[0].appendChild(link);
    var obj = currentCanvas.getActiveObject();
    if (obj.type == 'i-text') {
        var selection = $('#styleddl option:selected').text();
        link.href = url.replace('{family}', encodeURIComponent(selection));
        //currentCanvas.getActiveObject().setSelectionStyles({ 'fontFamily': selection });
        obj.fontFamily = selection;
        currentCanvas.renderAll();
        currentCanvas.calcOffset();
    }
});

$('#textddl').change(function () {
    var obj = currentCanvas.getActiveObject();
    if (obj.type == 'text' || obj.type == 'i-text') {
        var selection = $('#textddl option:selected').text();
        //currentCanvas.getActiveObject().setSelectionStyles({ 'fontSize': selection });
        obj.fontSize = selection;
        
        currentCanvas.renderAll();
    }
});

$('#addRect').click(function () {
    addShape(currentCanvas, "rect");
});
$('#addCircle').click(function(){
    addShape(currentCanvas, "circle");
});
$('#addTriangle').click(function () {
    addShape(currentCanvas, "triangle");
});
$('#addLine').click(function () {
    addShape(currentCanvas, "line");
});
$('#addElipse').click(function(){
    addShape(currentCanvas, "elipse");
})
$('#addStar').click(function () {
    addShape(currentCanvas, "rect");
});
$('#addPentagon').click(function () {
    addShape(currentCanvas, "pentagon");
});
$('#addHexagon').click(function () {
    addShape(currentCanvas, "hexagon");
});

$('.addImg').click(function (e) {
    var url = e.target.src;
    var img = e.target;
    addImg(url, currentCanvas);
});

$('.addBg').click(function (e) {
    var url = e.target.src;
    var img = e.target;
    addBg(url, currentCanvas);
});

function addImg(myImg, canvas) {
    var img = toDataUrl(myImg, function (base64Img) {
        console.log(base64Img);
        var image = new Image();
        image.src = base64Img;
        resImg = new fabric.Image(image);
        //var resImg = new fabric.Image(image, { width: canvas.width, height: canvas.height });
        canvas.add(resImg);
    });
   
}

function addBg(myImg, canvas) {
    var img = toDataUrl(myImg, function (base64Img) {
        console.log(base64Img);
        var image = new Image();
        image.src = base64Img;
        resImg = new fabric.Image(image, { width: canvas.width, height: canvas.height });
        //var resImg = new fabric.Image(image, { width: canvas.width, height: canvas.height });
        canvas.setBackgroundImage(resImg);
        canvas.renderAll();
    });
}

$("#side2").click(function () {
    $('#side1').toggleClass('disabled');
    $('#side2').toggleClass('disabled');
    $('#side1').toggleClass('btn-primary');
    $('#side2').toggleClass('btn-primary');
    $("#canvas_s2").toggleClass('hidden');
    $("#imageLoader_s2").toggleClass('hidden');
    $("#logoLoader").toggleClass('hidden');
    $("#canvas").toggleClass('hidden');
    $("#imageLoader").toggleClass('hidden');
    $("#side2logo").toggleClass('hidden');
    $("#fside").toggleClass('hidden');
    $("#sside").toggleClass('hidden');
    $("#ufside").toggleClass('hidden');
    $("#usside").toggleClass('hidden');
    $(".upper-canvas").toggleClass('hidden');
    currentCanvas = document.getElementById('canvas_s2').fabric;
});

$("#side1").click(function () {
    $('#side1').toggleClass('disabled');
    $('#side2').toggleClass('disabled');
    $('#side1').toggleClass('btn-primary');
    $('#side2').toggleClass('btn-primary');
    $("#canvas_s2").toggleClass('hidden');
    $("#imageLoader_s2").toggleClass('hidden');
    $("#canvas").toggleClass('hidden');
    $("#imageLoader").toggleClass('hidden');
    $("#logoLoader").toggleClass('hidden');
    $("#side2logo").toggleClass('hidden');
    $("#fside").toggleClass('hidden');
    $("#sside").toggleClass('hidden');
    $("#ufside").toggleClass('hidden');
    $("#usside").toggleClass('hidden');
    $(".upper-canvas").toggleClass('hidden');
    currentCanvas = document.getElementById('canvas').fabric;
});

$("#addside2").click(function () {
    $('#side1').toggleClass('disabled');
    $('#side2').toggleClass('disabled');
    $('#side1').toggleClass('btn-primary');
    $('#side2').toggleClass('btn-primary');
    $("#side2").toggleClass('hidden');
    $("#addside2").toggleClass('hidden');
    $("#deleteside2").toggleClass('hidden');
    $("#canvas_s2").toggleClass('hidden');
    $("#imageLoader_s2").toggleClass('hidden');
    $("#logoLoader").toggleClass('hidden');
    $("#canvas").toggleClass('hidden');
    $("#imageLoader").toggleClass('hidden');
    $("#side2logo").toggleClass('hidden');
    $("#fside").toggleClass('hidden');
    $("#sside").toggleClass('hidden');
    $("#ufside").toggleClass('hidden');
    $("#usside").toggleClass('hidden');
    $(".upper-canvas").toggleClass('hidden');
    var hwc = document.getElementById("canvas").fabric;
    var canvas = document.getElementById("canvas_s2").fabric;
    canvas.width = hwc.width;
    canvas.height = hwc.height;
    currentCanvas = document.getElementById('canvas_s2').fabric;
    ifSide2 = true;
});

$("#deleteside2").click(function () {
    $('#side1').removeClass();
    $('#side2').removeClass();
    $('#addside2').removeClass('hidden');
    $('#deleteside2').addClass('hidden');
    $('#canvas_s2').addClass('hidden');
    $('#imageLoader_s2').addClass('hidden');
    $('#canvas').removeClass('hidden');
    $("#fside").removeClass('hidden');
    $("#sside").addClass('hidden');
    $("#ufside").removeClass('hidden');
    $("#usside").addClass('hidden');
    $(".upper-canvas").toggleClass('hidden');
    $("#imageLoader").toggleClass('hidden');
    $("#side2logo").toggleClass('hidden');
    $('#side1').addClass('btn btn-default btn-primary disabled');
    $('#side2').addClass('btn btn-default hidden');
    $(".upper-canvas").toggleClass('hidden');
    var canvass2 = document.getElementById("canvas_s2").fabric;
    canvass2.clear();
    canvass2.backgroundImage = 0;
    canvass2.renderAll();
    $("#hiddenImage_s2").val = null;
    currentCanvas = document.getElementById('canvas').fabric;
    ifSide2 = false;
});

function addShape(canvas, shape)
{
    switch (shape)
    {
        case "rect":
            var rectShape = new fabric.Rect({
                originX: "left",
                originY: "top",
                left: 40,
                top: 40,
                fill: 'green',
                opacity: 1,
                width: 40,
                height: 40,
                cornerSize: 6,
            });
            canvas.add(rectShape);
            break;
        case "circle":
            var circleShape = new fabric.Circle({
                radius: 20, fill: 'green', left: 40, top: 40
            });
            canvas.add(circleShape);
            break;
        case "triangle":
            var triangle = new fabric.Triangle({
                width: 20, height: 30, fill: 'blue', left: 40, top: 40
            });
            canvas.add(triangle);
            break;
        case "line":
            var line = new fabric.Line([10, 10, 10, 100], {
                fill: 'green',
                stroke: 'green'
            });
            canvas.add(line);
            break;
        case "elipse":
            var ellipse = new fabric.Ellipse({
                rx: 45,
                ry: 80,
                fill: 'yellow',
                stroke: 'red',
                strokeWidth: 3,
                angle: 30,
                left: 100,
                top: 100
            });
            canvas.add(ellipse);
            break;
        case "star":
            var star = new fabric.Polygon([
                { x: 50, y: 50 },
                { x: 75, y: 150 },
                { x: 15, y: 100 },
                { x: 85, y: 100 },
                { x: 25, y: 150 }], {
                left: 0,
                top: 0,
                angle: 0,
                fill: 'green'
            });
            canvas.add(star);
            break;
        case "pentagon":
            var pent = new fabric.Polygon([
                { x: 100, y: 0 },
                { x: 5, y: 69 },
                { x: 41, y: 181 },
                { x: 159, y: 181 },
                { x: 195, y: 69 }], {
                left: 0,
                top: 0,
                angle: 0,
                fill: 'green'
            });
            canvas.add(pent);
            break;
        case "hexagon":
            var pent = new fabric.Polygon([
                { x: 150, y: 13 },
                { x: 50, y: 13 },
                { x: 0, y: 100 },
                { x: 50, y: 187 },
                { x: 150, y: 187 },
                { x: 200, y: 100 }], {
                left: 0,
                top: 0,
                angle: 0,
                fill: 'green'
            });
            canvas.add(pent);
            break;
    }
    canvas.renderAll();
}

function toDataUrl(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.responseType = 'blob';
    xhr.onload = function () {
        var reader = new FileReader();
        reader.onloadend = function () {
            callback(reader.result);
        }
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.send();
}

function setDimensions(id, width, height) {
    var canvas = document.getElementById(id).fabric;
    if (!Number.isInteger(width)) {
        width = parseInt(width, 10);
    }
    if (!Number.isInteger(height)) {
        height = parseInt(height, 10);
    }
    if (width > height) {
        actWidth = width * 30 / 2.54;
        actHeight = height * 30 / 2.54;
        var tmpWidth = 600;
        var factor = width / tmpWidth;
        var tmpHeight = height / factor;
        canvas.upperCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.upperCanvasEl.style.height = tmpHeight + "px";//actHeight * 0.7 + "px";
        canvas.lowerCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.lowerCanvasEl.style.height = tmpHeight + "px";//actHeight * 0.7 + "px";
        canvas.wrapperEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.wrapperEl.style.height = tmpHeight + "px";//actHeight * 0.7 + "px";
        canvas.upperCanvasEl.width = actWidth;
        canvas.upperCanvasEl.height = actHeight;
        canvas.lowerCanvasEl.width = actWidth;
        canvas.lowerCanvasEl.height = actHeight;
        canvas.width = actWidth;
        canvas.height = actHeight;
        canvas = document.getElementById(id);
        canvas.width = actWidth;
        canvas.height = actHeight;
    } else {
        actWidth = width * 30 / 2.54;
        actHeight = height * 30 / 2.54;
        var tmpHeight = 900;
        var factor = height / tmpHeight;
        var tmpWidth = width / factor;
        canvas.upperCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.upperCanvasEl.style.height = tmpHeight + "px";//actHeight * 0.7 + "px";
        canvas.lowerCanvasEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.lowerCanvasEl.style.height = tmpHeight + "px";//actHeight * 0.7 + "px";
        canvas.wrapperEl.style.width = tmpWidth + "px"; //actWidth * 0.7 + "px";
        canvas.wrapperEl.style.height = tmpHeight + "px";//actHeight * 0.7 + "px";
        canvas.upperCanvasEl.width = actWidth;
        canvas.upperCanvasEl.height = actHeight;
        canvas.lowerCanvasEl.width = actWidth;
        canvas.lowerCanvasEl.height = actHeight;
        canvas.width = actWidth;
        canvas.height = actHeight;
        canvas = document.getElementById(id);
        canvas.width = actWidth;
        canvas.height = actHeight;
    }
}

function addSafety(canvas) {
    var safetyFactor = 2 * 30 / 2.54;
    var textPlaceFactor = 5 * 30 / 2.54;
    var width, height;
    width = canvas.width;
    height = canvas.height;
    var rect = new fabric.Rect({
        originX: "left",
        originY: "top",
        stroke: "rgb(245,12,12)",
        strokeWidth: 1,
        strokeDashArray: [5, 5],
        fill: 'transparent',
        opacity: 1,
        cornerSize: 6,
        evented: false,
        selectable: false,
        id: 'safetyBorders'
    });
    rect.left = safetyFactor;
    rect.top = safetyFactor;
    rect.width = width - safetyFactor * 2;
    rect.height = height - safetyFactor * 2;
    canvas.add(rect);

    //var textField = new fabric.Rect(
    //{
    //    originX: "left",
    //    originY: "top",
    //    stroke: "rgb(140,207,255)",
    //    strokeWidth: 1,
    //    fill: 'transparent',
    //    opacity: 1,
    //    cornerSize: 6,
    //    evented: false,
    //    selectable: false,
    //    id: 'textField'
    //});
    //textField.left = safetyFactor + textPlaceFactor;
    //textField.top = safetyFactor + textPlaceFactor;
    //textField.width = width - safetyFactor * 2 - textPlaceFactor * 2;
    //textField.height = height - safetyFactor * 2 - textPlaceFactor * 2;
    //canvas.add(textField);
    //canvas.renderAll();
};

function removeSafety(canvas) {
    canvas.forEachObject(function (obj) {
        var id = obj.id;
        if (id == 'safetyBorders') {
            canvas.remove(obj);
        }
        if (id == 'textField') {
            canvas.remove(obj);
        }
    });
    canvas.renderAll();
}

function initCanvas(id) {
    var canvas = new fabric.Canvas(id, {
        backgroundColor: 'rgb(255,255,255)',
    });
    document.getElementById(id).fabric = canvas;
    var width = $('#hiddenWidth').val();
    var height = $('#hiddenHeight').val();
    setDimensions(id, width, height);
    //addSafety(canvas);
    initAligningGuidelines(canvas);
    initCenteringGuidelines(canvas);
}

//canvas.observe('object:scaling', ObserveScaling);
//canvass2.observe('object:scaling', ObserveScaling);
//canvas.observe('object:moving', ObserveMoving);
//canvass2.observe('object:moving', ObserveMoving);
//canvas.observe('object:changed', ObserveChanged);
//canvass2.observe('object:changed', ObserveChanged);
//canvas.observe('canvas:cleared', ObserveCleared);
//canvass2.observe('canvas:cleared', ObserveChanged);
function ObserveScaling(e) {
    var obj = e.target;
    if (obj.getHeight() > obj.canvas.height || obj.getWidth() > obj.canvas.width) {
        obj.setScaleY(obj.originalState.scaleY);
        obj.setScaleX(obj.originalState.scaleX);
    }
    obj.setCoords();
    if (obj.getBoundingRect().top - (obj.cornerSize / 2) < 0 ||
       obj.getBoundingRect().left - (obj.cornerSize / 2) < 0) {
        obj.top = Math.max(obj.top, obj.top - obj.getBoundingRect().top + (obj.cornerSize / 2));
        obj.left = Math.max(obj.left, obj.left - obj.getBoundingRect().left + (obj.cornerSize / 2));
    }
    if (obj.getBoundingRect().top + obj.getBoundingRect().height + obj.cornerSize > obj.canvas.height || obj.getBoundingRect().left + obj.getBoundingRect().width + obj.cornerSize > obj.canvas.width) {

        obj.top = Math.min(obj.top, obj.canvas.height - obj.getBoundingRect().height + obj.top - obj.getBoundingRect().top - obj.cornerSize / 2);
        obj.left = Math.min(obj.left, obj.canvas.width - obj.getBoundingRect().width + obj.left - obj.getBoundingRect().left - obj.cornerSize / 2);
    }
};

function ObserveMoving(e) {
    var obj = e.target;
    if (obj.getHeight() > obj.canvas.height || obj.getWidth() > obj.canvas.width) {
        obj.setScaleY(obj.originalState.scaleY);
        obj.setScaleX(obj.originalState.scaleX);
    }
    obj.setCoords();
    if (obj.getBoundingRect().top - (obj.cornerSize / 2) < 0 ||
       obj.getBoundingRect().left - (obj.cornerSize / 2) < 0) {
        obj.top = Math.max(obj.top, obj.top - obj.getBoundingRect().top + (obj.cornerSize / 2));
        obj.left = Math.max(obj.left, obj.left - obj.getBoundingRect().left + (obj.cornerSize / 2));
    }
    if (obj.getBoundingRect().top + obj.getBoundingRect().height + obj.cornerSize > obj.canvas.height || obj.getBoundingRect().left + obj.getBoundingRect().width + obj.cornerSize > obj.canvas.width) {

        obj.top = Math.min(obj.top, obj.canvas.height - obj.getBoundingRect().height + obj.top - obj.getBoundingRect().top - obj.cornerSize / 2);
        obj.left = Math.min(obj.left, obj.canvas.width - obj.getBoundingRect().width + obj.left - obj.getBoundingRect().left - obj.cornerSize / 2);
    }
};

function ObserveChanged(e) {
    var obj = e.target;
    if (obj.getHeight() > obj.canvas.height || obj.getWidth() > obj.canvas.width) {
        obj.setScaleY(obj.originalState.scaleY);
        obj.setScaleX(obj.originalState.scaleX);
    }
    obj.setCoords();
    if (obj.getBoundingRect().top - (obj.cornerSize / 2) < 0 ||
       obj.getBoundingRect().left - (obj.cornerSize / 2) < 0) {
        obj.top = Math.max(obj.top, obj.top - obj.getBoundingRect().top + (obj.cornerSize / 2));
        obj.left = Math.max(obj.left, obj.left - obj.getBoundingRect().left + (obj.cornerSize / 2));
    }
    if (obj.getBoundingRect().top + obj.getBoundingRect().height + obj.cornerSize > obj.canvas.height || obj.getBoundingRect().left + obj.getBoundingRect().width + obj.cornerSize > obj.canvas.width) {

        obj.top = Math.min(obj.top, obj.canvas.height - obj.getBoundingRect().height + obj.top - obj.getBoundingRect().top - obj.cornerSize / 2);
        obj.left = Math.min(obj.left, obj.canvas.width - obj.getBoundingRect().width + obj.left - obj.getBoundingRect().left - obj.cornerSize / 2);
    }
};

canvas.observe('object:selected', function () {
    var obj = currentCanvas.getActiveObject();
    if (obj.type == 'i-text' || obj.type == 'text')
    {
        $('#bold').removeClass('disabled');
        $('#italic').removeClass('disabled');
        $('#styleddl').prop('disabled', false);
        $('#textddl').prop('disabled', false);
        if (obj.fontWeight == 'bold')
        {
            $('#bold').addClass('active');
        }
        else
        {
             $('#bold').removeClass('active');
        }
        if (obj.fontStyle == 'italic')
        {
            $('#italic').addClass('active');
        }
        else
        {
            $('#italic').removeClass('active');
        }
    var sizeselect = document.getElementById('textddl');
    var tmp = parseFloat(obj.fontSize);
    tmp = Math.round(tmp);
    tmp = "" + tmp;
    var contains = false;
    for (var i = 0; i < sizeselect.options.length; i++)
    {
        if (sizeselect.options[i].text == tmp)
        {
            contains = true;
            break;         
        }
    }
    if (contains)
    {
        $('#textddl').val(tmp);
    }
    else
    {
        var option = document.createElement('option');
        option.text = option.value = tmp;
        sizeselect.add(option, 999);
        $('#textddl').val(tmp);
        contains = false;
    }

    var styleselect = document.getElementById('styleddl');
    var tmp = obj.fontFamily;
    for (var i = 0; i < styleselect.options.length; i++) {
        if (sizeselect.options[i].text == tmp) {
            contains = true;
            break;
        }
    }
    if (contains) {
        $('#styleddl').val(tmp);
    }
    else {
        var option = document.createElement('option');
        option.text = option.value = tmp;
        styleselect.add(option, 999);
        $('#styleddl').val(tmp);
        contains = false;
    }

    }
    else
    {
        
        $('#bold').removeClass('active');
        $('#italic').removeClass('active');
        $('#bold').addClass('disabled');
        $('#italic').addClass('disabled');
        $('#styleddl').prop('disabled', true);
        $('#textddl').prop('disabled', true);
    }
   
});

canvas.observe('canvas:cleared', function (e) {
    addSafety(canvas);
});
canvass2.observe('canvas:cleared', function (e) {
    addSafety(canvass2);
});

    ymaps.ready(init);
    var myMap,
        myPlacemark;

    function init() {
        myMap = new ymaps.Map("map", {
            center: [55.76, 37.64],
            zoom: 7,
            controls: ["zoomControl", "fullscreenControl"]
        });
        // Создание элемента управления «Поиск по карте».
        var searchControl = new ymaps.control.SearchControl({
            options: {
                // Будет производиться поиск и по топонимам и по организациям.
                noPlacemark: true,
                provider: 'yandex#search'
            }
        });

        myMap.controls.add(searchControl);
        searchControl.events.add("resultselect", function (result) {
            myMap.geoObjects.removeAll();
        });

        myMap.events.add('click', function (e, img, width, height) {
            myMap.geoObjects.removeAll();
            // Получение координат щелчка
            var coords = e.get('coords');
            myPlacemark = new ymaps.Placemark(coords);
            myPlacemark.events.add('click', function (e) {
                myMap.geoObjects.remove(myPlacemark);
            });
            myMap.geoObjects.add(myPlacemark);
 
            var width = 450;
            var height = 450;
            var url = "https://static-maps.yandex.ru/1.x/?ll=" + coords[1] + "," + coords[0] + "&size=" + width + "," + height + "&z=" + myMap._zoom + "&l=map&pt=" + coords[1] + "," + coords[0] + ",comma";
            var img = document.getElementById('staticMap');
            img.width = 450;
            img.height = 450;
            $('#staticMap').removeClass('hidden');
            img.src = url;
        });

        //$('input:radio[name=siderlt]').change(function () {
        //    var img = document.getElementById('staticMap');
        //    var width = img.width;
        //    var height = img.height;
        //    switch (this.id) {
        //        case '11':
        //            img.width = 450;
        //            img.height = 450;
        //            break;
        //        case '43':
        //            var tmp = width / 4;
        //            height = tmp * 3;
        //            img.width = width;
        //            img.height = height;
        //            break;
        //        case '69':
        //            var tmp = width / 6;
        //            height = tmp * 9;
        //            img.width = width;
        //            img.height = height;
        //            break;
        //        default:
        //            img.width = width;
        //            img.height = height;
        //            break;

        //    }
        //});

        // Добавляем элемент управления на карту.
    
    }

   


    $('#addMap').click(function () {
        var url = document.getElementById('staticMap').src;
        fabric.Image.fromURL(url, function (oImg) {
            //oImg.set('left', PosX).set('top',PosY);
            currentCanvas.add(oImg);
            currentCanvas.renderAll();
        });
    });

    

    //$('#sbtn').click(function () {
    //    var address = $('#sfld').val();
    //    var myGeocoder = ymaps.geocode(address);
    //    myGeocoder.then(
    //        function (res) {
    //            alert('Координаты объекта :' + res.geoObjects.get(0).geometry.getCoordinates());
    //            myMap.setCenter(res.geoObjects.get(0).geometry.getCoordinates(), 14);
    //        },
    //        function (err) {
    //            alert('Ошибка');
    //        }
    //    );
    //});
});

