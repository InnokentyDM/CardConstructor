$(document).ready(function () {
    initCanvas('canvas');
    var canvas = document.getElementById('canvas').fabric;
    initCanvas('canvas_s2');
    var canvass2 = document.getElementById('canvas_s2').fabric;
    var json_s1 = $("#hiddencanvas").val();
    var json_s2 = $("#hiddencanvas_s2").val();
    if (json_s2 != "")
    {
        loadCanvas(canvass2, json_s2);
        $("#canvas_s2").toggleClass('hidden');
    }
    loadCanvas(canvas, json_s1);

    $('#download').click({ param1: canvas, param2: canvass2 }, download);
    var arr = document.getElementsByClassName('canvas-container');
    var arr2 = document.getElementsByClassName('upper-canvas');
    arr2[0] = 'ufside';
    arr2[1] = 'usside';
    arr2[1].classList.add('hidden');
    arr[0].id = 'fside';
    arr[1].id = 'sside';
    arr[1].classList.add('hidden');

    $('#layout').click(function (e) {
        var json_s = $("#hiddencanvas").val();
        var htmlCanv = document.createElement('canvas');
        htmlCanv.setAttribute("id", "tmpCanvas");
        var cnv = new fabric.Canvas('tmpCanvas', {
            backgroundColor: 'rgb(255,255,255)',
        });
        loadCanvas(cnv, json_s);
        cnv.renderAll();
        var link = document.createElement("a");
        link.download = name;
        link.href = canvas.toDataURL({
            format: 'png',
            quality: 1.0
        });
        link.click();
    });
});





function loadCanvas(canvas, json)
{
    var obj = JSON.parse(json);  
    canvas.loadFromJSON(obj);
    //if (obj.backgroundImage) {
    //    canvas.setWidth(obj.backgroundImage.width);
    //    canvas.setHeight(obj.backgroundImage.height);
    //}
    canvas.renderAll();
    console.log(' this is a callback. invoked when canvas is loaded!xxx ');
}

function download(event) {
    var doc = new jsPDF();
    var width = event.data.param1.width * 0.5;
    var height = event.data.param1.height * 0.5;
    var imgData = event.data.param1.toDataURL("image/png");
    var imgData1 = event.data.param2.toDataURL("image/png");

    var insdate = $('#l-ins-date').html();
    var insdate2 = $('#i-ins-date').html();
    var findate = $('#l-fin-date').html();
    var findate2 = $('#i-fin-date').html();
    var stat = $('#l-status').html();
    var stat2 = $('#i-status').html();
    var summ = $('#l-summ').html();
    var summ2 = $('#i-summ').html();
    var count = $('#l-count').html();
    var count2 = $('#i-count').html();
    var desc = $('#l-desc').html();
    var desc2 = $('#i-desc').html();
    var user = $('#l-user').html();
    var user2 = $('#i-user').html();

    doc.text("profdesign.pro", 15, 15);
    var src = "<p>" + user2 + "</p>" +
         "<p>" + insdate + ":" + insdate2 + "</p>" +
        "<p>" + findate + ":" + findate2 + "</p>" +
        "<p>" + stat + ":" + stat2 + "</p>" +
        "<p>" + count + ":" + count2 + "</p>" +
        "<p>" + desc + ":" + desc2 + "</p>" +
        "<p>" + user + ":" + user2 + "</p>";
    doc.addImage(imgData, 'PNG', 15, 30, width, height);
    doc.addImage(imgData1, 'PNG', 15, 230, width, height);
    doc.fromHTML(src, 15, 115, {
        'width': 170,
        //'elementHandlers': specialElementHandlers
    });
    doc.save("profdesign_" + $('#UserName').text() + ".pdf");
    var svg = event.data.param1.toSVG({ encoding: 'UTF-16' });
    var svg1 = event.data.param2.toSVG({ encoding: 'UTF-16' });
    canvasToSVG(svg);
    canvasToSVG(svg1);
}

function canvasToSVG(svg)
{
    
    //var b64 = svg;
    var serializer = new XMLSerializer();
    var source = svg;
    //$("body").append($("<img src='data:image/svg+xml;base64,\n" + b64 + "' alt='file.svg'/>"));
    //$("body").append($("<a href-lang='image/svg+xml' href='data:image/svg+xml;base64,\n" + b64 + "' title='file.svg'>Download</a>"));
    //add name spaces.
    if (!source.match(/^<svg[^>]+xmlns="http\:\/\/www\.w3\.org\/2000\/svg"/)) {
        source = source.replace(/^<svg/, '<svg xmlns="http://www.w3.org/2000/svg"');
    }
    if (!source.match(/^<svg[^>]+"http\:\/\/www\.w3\.org\/1999\/xlink"/)) {
        source = source.replace(/^<svg/, '<svg xmlns:xlink="http://www.w3.org/1999/xlink"');
    }
    //convert svg source to URI data scheme.
    var url = "data:image/svg+xml;charset=utf-8," + encodeURIComponent(source);
    //set url value to a element's href attribute.
    //document.getElementById("link").href = url;
    var name = "profdesign_" + $('#UserName').text() + ".svg";
    var link = document.createElement("a");
    link.download = name;
    link.href = url;
    link.click();
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
    }
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