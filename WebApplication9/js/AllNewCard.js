$(document).ready(function () {
    var currentCanvas;
    var ifSide2 = true;
    var save = document.getElementById('save').addEventListener('click', canvasToImage, false);
    var saveTo = document.getElementById('saveTo').addEventListener('click', canvasToImage, false);
    initCanvas('canvas');;
    initCanvas('canvas_s2');
    var canvas = document.getElementById('canvas').fabric;
    var canvass2 = document.getElementById('canvas_s2').fabric;
    document.getElementById("canvas").fabric = canvas;
    document.getElementById("canvas_s2").fabric = canvass2;
    currentCanvas = document.getElementById('canvas').fabric;
    $('#side2').removeClass('hidden');
    $('#side1').removeClass('hidden');
    var arr = document.getElementsByClassName('canvas-container');
    var arr2 = document.getElementsByClassName('upper-canvas');
    arr2[0] = 'ufside';
    arr2[1] = 'usside';
    arr2[1].classList.add('hidden');
    arr[0].id = 'fside';
    arr[1].id = 'sside';
    arr[1].classList.add('hidden');

    var state = [];
    var mods = 0;
    canvas.on(
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

    undo = function undo() {
        if (mods < state.length) {
            canvas.clear().renderAll();
            canvas.loadFromJSON(state[state.length - 1 - mods - 1]);           
            canvas.renderAll();
            //console.log("geladen " + (state.length-1-mods-1));
            //console.log("state " + state.length);
            mods += 1;
            //console.log("mods " + mods);
        }
    }

    redo = function redo() {
        if (mods > 0) {
            canvas.clear().renderAll();
            canvas.loadFromJSON(state[state.length - 1 - mods + 1]);
            canvas.renderAll();
            //console.log("geladen " + (state.length-1-mods+1));
            mods -= 1;
            //console.log("state " + state.length);
            //console.log("mods " + mods);
        }
    }
    
    canvas.observe('object:scaling', ObserveScaling);
    canvass2.observe('object:scaling', ObserveScaling);
    canvas.observe('object:moving', ObserveMoving);
    canvass2.observe('object:moving', ObserveMoving);
    canvas.observe('object:changed', ObserveChanged);
    canvass2.observe('object:changed', ObserveChanged);
    canvas.observe('canvas:cleared', function (e) {
        addSafety(canvas);
    });
    canvass2.observe('canvas:cleared', function (e) {
        addSafety(canvass2);
    });

    $('#reset').click(function () {
        currentCanvas.clear();
        var image2 = jQuery("#hiddenImage_s2").val();
        if (image2 != "") {
            canvass2.clear();
        }
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

    $('#ddl1').change(function () {
        var selection = jQuery("#ddl1 option:selected").text();
        var ret = selection.split(/\s+/);
        var str1 = ret[0];
        var str2 = ret[1];
        setDimensions('canvas', str1, str2);
        setDimensions('canvas_s2', str1, str2);
        addSafety(canvas);
        addSafety(canvass2);
    })

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
            url: "../../Home/SaveTo",
            data: { model: model },
            success: function (data, status) {
                window.location.href = "../../addCards/UserCards";
            },
            error: function (err) {
                console.log(err);
            }
        });
    });

    $('#addText').click(function () {
        currentCanvas.add(new fabric.IText('Нажмите, чтобы печатать', {
            fontFamily: 'arial black',
            fontSize: 22, 
            left: 100,
            top: 100,
        }));
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

        if (ifSide2) {
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


    $('#color').change(function () {
        var obj = currentCanvas.getActiveObject();
        if (obj.type == 'text' || obj.type == 'i-text') {
            var selection = $('#color').val();
            currentCanvas.getActiveObject().setSelectionStyles({ 'fill': selection })
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
            currentCanvas.getActiveObject().setSelectionStyles({ 'fontFamily': selection });
            currentCanvas.renderAll();
            currentCanvas.calcOffset();
        }
    });

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


    $('#textddl').change(function () {
        var obj = currentCanvas.getActiveObject();
        if (obj.type == 'text' || obj.type == 'i-text') {
            var selection = $('#textddl option:selected').text();
            currentCanvas.getActiveObject().setSelectionStyles({ 'fontSize': selection })
            currentCanvas.renderAll();
        }
    });

    function canvasToImageNew() {
        canvas.deactivateAll().renderAll();
        var image = JSON.stringify(canvas.toDatalessJSON());
        var EditCardId = jQuery("#cardId").val();
        $('#hiddenImage').val(null);
        $('#hiddenImage').val(image);
    }

  
    $('#addRect').click(function () {
        addRect(currentCanvas);
    });
    $('#addCircle').click(function(){
        addCircle(currentCanvas);
    });
    $('#addTriangle').click(function () {
        addTriangle(currentCanvas);
    });
    $('#addLine').click(function () {
        addLine(currentCanvas);
    });
    $('#addElipse').click(function()
    {
        addElipse(currentCanvas);
    })
    $('#addStar').click(function () {
        addStar(currentCanvas);
    });
    $('#addPentagon').click(function () {
        addPentagon(currentCanvas);
    });
    $('#addHexagon').click(function () {
        addHexagon(currentCanvas);
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

function addRect(canvas) {
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
    canvas.renderAll();
   
};

function addCircle(canvas) {
    var circleShape = new fabric.Circle({
        radius: 20, fill: 'green', left: 40, top: 40
    });
    canvas.add(circleShape);
    canvas.renderAll();
};
function addTriangle(canvas) {
    var triangle = new fabric.Triangle({
        width: 20, height: 30, fill: 'blue', left: 40, top: 40
    });
    canvas.add(triangle);
    canvas.renderAll();
};
function addLine(canvas)
{
    var line = new fabric.Line([10, 10, 10, 100], {
        fill: 'green',
        stroke: 'green'
    });
    canvas.add(line);
}
function addElipse(canvas)
{
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
}
function addStar(canvas)
{
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
}

function addPentagon(canvas)
{
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

}

function addHexagon(canvas) {
    var pent = new fabric.Polygon([
   { x: 150, y: 13 },
   { x: 50, y: 13 },
   { x: 0, y: 100 },
     { x: 50, y: 187 },
     { x: 150, y: 187 },
    {x: 200, y: 100}], {
         left: 0,
         top: 0,
         angle: 0,
         fill: 'green'
     });
    canvas.add(pent);

}

function addSafety(canvas) {
    canvas.forEachObject(function (obj) {
        var id = obj.id;
        if (id == 'safetyBorders') {
            canvas.remove(obj);
        }
        if (id == 'textField') {
            canvas.remove(obj);
        }
    });
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

    var textField = new fabric.Rect(
    {
        originX: "left",
        originY: "top",
        stroke: "rgb(140,207,255)",
        strokeWidth: 1,
        fill: 'transparent',
        opacity: 1,
        cornerSize: 6,
        evented: false,
        selectable: false,
        id: 'textField'
    });
    textField.left = safetyFactor + textPlaceFactor;
    textField.top = safetyFactor + textPlaceFactor;
    textField.width = width - safetyFactor * 2 - textPlaceFactor * 2;
    textField.height = height - safetyFactor * 2 - textPlaceFactor * 2;
    canvas.add(textField);
};

function removeSafety(canvas)
{
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
    }
}

function initCanvas(id) {
    var canvas = new fabric.Canvas(id, {
        backgroundColor: 'rgb(255,255,255)',
    });
    document.getElementById(id).fabric = canvas;
    setDimensions(id, 400, 400);
    addSafety(canvas);
    initAligningGuidelines(canvas);
    initCenteringGuidelines(canvas);
}