
window.onload = function () {


    var chk = true;
    var ifSide2 = false;
    initCanvas('canvas');
    initCanvas('canvas_s2');
    var canvas = document.getElementById('canvas').fabric;
    var canvass2 = document.getElementById('canvas_s2').fabric;
 
  
    $('#chkBox').change(function () {     
        if ($(this).is(':not(:checked)')) {
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
            }
        }
        else {
            addSafety(canvas);
            var sb;
            var tf;
            canvas.forEachObject(function (obj) {
                var id = obj.id;
                if (id == 'safetyBorders') {
                    sb = obj;
                }
                if (id == 'textField') {
                    tf = obj;
                }
            });
            canvas.add(sb);
            canvas.add(tf);
            canvass2.add(sb);
            canvass2.add(tf);
            canvas.renderAll();
        }
    });



    $('#clear').click(function () {
        canvas.clear();
        canvas.backgroundImage = 0;
        canvas.renderAll();
    });

    document.onkeydown = function(e)
    {
        if (46 === e.keyCode)
        {
            if (canvas.getActiveObject != null)
                {
            canvas.remove(canvas.getActiveObject());
            //var arr = [];
            var arr = canvas.getActiveGroup();
            arr.objects.forEach(function (object, key) {
                canvas.remove(object);
                arr.removeWithUpdate(object);
            });
            canvas.discardActiveGroup();
            canvas.renderAll();
            }
            else if (canvass2.getActiveObject != null)
            {
                canvass2.remove(canvas.getActiveObject());
                //var arr = [];
                var arr = canvass2.getActiveGroup();
                arr.objects.forEach(function (object, key) {
                    canvass2.remove(object);
                    arr.removeWithUpdate(object);
                });
                canvass2.discardActiveGroup();
                canvass2.renderAll();
            }
        }
    }

   

 
       
    var arr = document.getElementsByClassName('canvas-container');
    var arr2 = document.getElementsByClassName('upper-canvas');
    arr2[0] = 'ufside';
    arr2[1] = 'usside';
    arr2[1].classList.add('hidden');
    arr[0].id = 'fside';
    arr[1].id = 'sside';
    arr[1].classList.add('hidden');
  

    //UploadSVG(canvas);
    if (document.getElementById('canvasToImage') != null)
    {
        var imageSender = document.getElementById('canvasToImage').addEventListener('click', canvasToImage, false);
    }

    //var imageLoader = document.getElementById('imageLoader').addEventListener('change', function () { handleImage(this, canvas); }, false);
    $('#imageLoader').change(function () { handleImage(this, canvas); });


    if (document.getElementById('side2') != null) {
        $('#imageLoader_s2').change(function () { handleImage(this, canvass2); });
    }

    $('#color').change(function () {
        var obj = canvas.getActiveObject();
        if (obj.type == 'text' || obj.type == 'i-text') {
            var selection = $('#color').val();
            canvas.getActiveObject().setSelectionStyles({ 'fill': selection })
            canvas.renderAll();
        }
    });

  


    $('#styleddl').change(function () {        
        var url = "https://fonts.googleapis.com/css?family={family}&subset=latin,cyrillic";        
        var link = document.createElement('link');        
        link.rel = 'stylesheet';        
        document.getElementsByTagName('head')[0].appendChild(link);
        var obj = canvas.getActiveObject();
        if (obj.type == 'i-text')
        {
            var selection = $('#styleddl option:selected').text();
            link.href = url.replace('{family}', encodeURIComponent(selection));
            canvas.getActiveObject().setSelectionStyles({ 'fontFamily': selection });
            canvas.renderAll();
        }     
    });

        $('#textddl').change(function () {
            var obj = canvas.getActiveObject();
            if (obj.type == 'text' || obj.type == 'i-text')
            {
                var selection = $('#textddl option:selected').text();
                canvas.getActiveObject().setSelectionStyles({'fontSize' : selection})
                canvas.renderAll();
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
            //currentCanvas = document.getElementById('canvas_s2').fabric;
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
            //currentCanvas = document.getElementById('canvas').fabric;
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
            //currentCanvas = document.getElementById('canvas_s2').fabric;
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
            //currentCanvas = document.getElementById('canvas').fabric;
            ifSide2 = false;
        });


        var input = document.getElementById("imageLoader");
        var input2 = document.getElementById("imageLoader_s2");
        input2.onclick = function () {
            this.value = null;
        };
        input.onclick = function () {
            this.value = null;
        };

    //var UploadSVG = function (canvas) {    
        function handleImage(e, canvas) {
            var sb;
            var tf;
            canvas.forEachObject(function (obj) {
                var id = obj.id;
                if (id == 'safetyBorders') {
                    sb = obj;
                }
                if (id == 'textField')
                {
                    tf = obj;
                }
            });
            var reader = new FileReader();
            reader.onload = function (event) {
                var img = new Image();
                img.src = event.target.result;
                img.onload = function () {
                    fabric.loadSVGFromURL(img.src, function (objects, options) {
                        var tmpobj = fabric.util.groupSVGElements(objects, options);
                        canvas.clear();
                        var arr = new Array();
                        for (var i = 0; i < objects.length; i++) {
                            //var obj = new fabric.Object({ padding: 0 });
                            var obj = objects[i];
                            if (obj.get('type') == 'text') {
                                var text = new fabric.IText('Tap and Type', {
                                    fontFamily: obj.get('fontFamily'),
                                    left: obj.get('left'),
                                    top: obj.get('top'),
                                    text: obj.get('text'),
                                    oCoords: obj.get('oCoords'),                                  
                                    fontSize: obj.get('fontSize'),
                                    height: obj.get('height'),
                                    width: obj.get('width'),
                                    fill: obj.get('fill'),
                                    originX: 0,
                                    originY: 0                                   
                                });
                                obj = text;                             
                                //canvas.bringToFront(obj);
                                canvas.add(obj);
                            }
                            else {
                                //arr.push(obj);
                                canvas.add(obj);
                            }                                                    
                        }
                        //var gr = new fabric.Group(arr);
                        //var tmpCanv = new fabric.Canvas();
                        //tmpCanv.add(gr);
                        //tmpCanv.renderAll();
                        //tmpCanv.setWidth(canvas.width);
                        //tmpCanv.setHeight(canvas.height);
                        //var imgurl = tmpCanv.toDataURL();
                        //console.log(imgurl);
                        //var bimg = new fabric.Image.fromURL(imgurl);
                        //canvas.setBackgroundImage(imgurl, canvas.renderAll.bind(canvas), function (){backgroundStretch:true});
                        canvas.add(sb);
                        canvas.add(tf);
                        canvas.renderAll();
                        var objs = canvas.getObjects().map(function (o) {
                            return o.set('active', true);
                        });
                        var group = new fabric.Group(objs, {
                            originX: 'center',
                            originY: 'center'
                        });
                        if (canvas.item('logo') != null) {
                            canvas.bringToFront(canvas.item('logo'));
                        }
                        canvas._activeObject = null;
                        canvas.setActiveGroup(group.setCoords()).renderAll();
                        canvas.deactivateAll().renderAll();
                        
                    }, function (item, object) {
                        object.set('id', item.getAttribute('id'));
                    });
                }
            }
            console.log(e);
            var res = reader.readAsDataURL(e.files[0]);
        }
    

        $('#ddl1').change(function ()
        {
            var selection = jQuery("#ddl1 option:selected").text();          
            //var dimensions = selection.val();
            var ret = selection.split(/\s+/);
            var width = ret[0];
            var height = ret[1];
            //canvass2.setWidth(width);
            //canvass2.setHeight(height);
            //canvas.setWidth(width);
            //canvas.setHeight(height);
            setDimensions('canvas', width, height);
            setDimensions('canvas_s2', width, height);
            addSafety(canvas);
            addSafety(canvass2);
        })
           

        document.getElementById('logoLoader').onchange = function handleLogo(e) {
            var reader = new FileReader();
            reader.onload = function (event) {
                console.log('fdsf');
                var imgObj = document.createElement('img');
                imgObj.src = event.target.result;
                imgObj.onload = function () {
                    // start fabricJS stuff
                    var image = new fabric.NamedImage(imgObj, { name: 'logo' });
                    image.set({
                        id: 'logo',
                        left: 25,
                        top: 25,
                        padding: 10,
                        cornersize: 10,
                        height: 50,
                        width: 50
                    });
                    canvas.bringToFront(image);
                    //image.scale(getRandomNum(0.1, 0.25)).setCoords();
                    canvas.add(image);
                    canvas.renderAll();
                    console.log(canvas.item(0).name);
                    // end fabricJS stuff
                }
            }
            reader.readAsDataURL(e.target.files[0]);
        }


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
            var preview = canvas.toDataURL({
                format: 'png',
                quality: 1.0
            });
            //var image = canvas.toSVG();
   
        if (ifSide2)
            {
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
            //var images2 = JSON.stringify(canvass2.toDatalessJSON());      
            var images2 = JSON.stringify(canvass2.toDatalessJSON());
            var preview_2 = canvass2.toDataURL({
                format: 'png',
                quality: 1.0
            });
            $('#preview_2').val(null);
            $('#preview_2').val(preview_2);
        $('#hiddenImage_s2').val(null);
        $('#hiddenImage_s2').val(images2);
        }
        $('#preview').val(null);
        $('#preview').val(preview);
        $('#hiddenImage').val(null);
        $('#hiddenImage').val(image);
        }

        
    
        canvas.observe('object:scaling', ObserveScaling);
        canvass2.observe('object:scaling', ObserveScaling);
        canvas.observe('object:moving', ObserveMoving);
        canvass2.observe('object:moving', ObserveMoving);
        canvas.observe('object:changed', ObserveChanged);
        canvass2.observe('object:changed', ObserveChanged);
        //canvas.observe('canvas:cleared', ObserveCleared);
        //canvass2.observe('canvas:cleared', ObserveChanged);
        function ObserveScaling (e) {
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

        function ObserveMoving (e) {
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

        canvas.observe('canvas:cleared', function (e) {
            addSafety(canvas);
        });
        canvass2.observe('canvas:cleared', function (e) {
            addSafety(canvass2);
        });
       
}


function setDimensions(id, width, height)
{
    var canvas = document.getElementById(id).fabric;
    //canvas.upperCanvasEl.style.width = width;
    //canvas.upperCanvasEl.style.height = height;
    //canvas.lowerCanvasEl.style.width = width;
    //canvas.lowerCanvasEl.style.height = height;
    if (!Number.isInteger(width))
    {
        width = parseInt(width, 10);
    }
    if (!Number.isInteger(height))
    {
        height = parseInt(height, 10);
    }
    var tmpWidth = 600;
    var tmpHeight = 600;
    if (width > height) {
        actWidth = width * 30 / 2.54;
        actHeight = height * 30 / 2.54;   
        var factor = width / tmpWidth;
        tmpHeight = height / factor;
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
        canvas.renderAll();
        canvas = document.getElementById(id);
        canvas.width = actWidth;
        canvas.height = actHeight;
    }
    else if (height > width)
    {
        actWidth = width * 30 / 2.54;
        actHeight = height * 30 / 2.54;
        var factor = height / tmpHeight;
        tmpWidth = width / factor;
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
        canvas.renderAll();
        canvas = document.getElementById(id);
        canvas.width = actWidth;
        canvas.height = actHeight;
    }
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

function initCanvas(id)
{
    var canvas = new fabric.Canvas(id, {
        backgroundColor: 'rgb(255,255,255)',
    });
    document.getElementById(id).fabric = canvas;
    setDimensions(id, 400, 400);
    addSafety(canvas);
    initAligningGuidelines(canvas);
    initCenteringGuidelines(canvas);
}

function uploadImage(e, canvas) {
    var reader = new FileReader();
    reader.onload = function (event) {
        var img = new Image();
        img.onload = function () {
            var imgInstance = new fabric.Image(img, {
                scaleX: 1,
                scaleY: 1
            })
            canvas.add(imgInstance);
        }
        img.src = event.target.result;
    }
    reader.readAsDataURL(e.target.files[0]);
}