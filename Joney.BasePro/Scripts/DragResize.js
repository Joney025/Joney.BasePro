/**Jquery Dom Move Or Resize**/
if (typeof addEvent != 'function') { var addEvent = function (o, t, f, l) { var d = 'addEventListener', n = 'on' + t, rO = o, rT = t, rF = f, rL = l; if (o[d] && !l) return o[d](t, f, false); if (!o._evts) o._evts = {}; if (!o._evts[t]) { o._evts[t] = o[n] ? { b: o[n] } : {}; o[n] = new Function('e', 'var r=true,o=this,a=o._evts["' + t + '"],i;for(i in a){o._f=a[i];r=o._f(e||window.event)!=false&&r;o._f=null}return r'); if (t != 'unload') addEvent(window, 'unload', function () { removeEvent(rO, rT, rF, rL) }) } if (!f._i) f._i = addEvent._i++; o._evts[t][f._i] = f }; addEvent._i = 1; var removeEvent = function (o, t, f, l) { var d = 'removeEventListener'; if (o[d] && !l) return o[d](t, f, false); if (o._evts && o._evts[t] && f._i) delete o._evts[t][f._i] } } function cancelEvent(e, c) { e.returnValue = false; if (e.preventDefault) e.preventDefault(); if (c) { e.cancelBubble = true; if (e.stopPropagation) e.stopPropagation() } }; function DragResize(myName, config) { var props = { myName: myName, enabled: true, handles: ['tl', 'tm', 'tr', 'ml', 'mr', 'bl', 'bm', 'br'], isElement: null, isHandle: null, element: null, handle: null, minWidth: 10, minHeight: 10, minLeft: 0, maxLeft: 9999, minTop: 0, maxTop: 9999, zIndex: 1, mouseX: 0, mouseY: 0, lastMouseX: 0, lastMouseY: 0, mOffX: 0, mOffY: 0, elmX: 0, elmY: 0, elmW: 0, elmH: 0, allowBlur: true, ondragfocus: null, ondragstart: null, ondragmove: null, ondragend: null, ondragblur: null }; for (var p in props) this[p] = (typeof config[p] == 'undefined') ? props[p] : config[p] }; DragResize.prototype.apply = function (node) { var obj = this; addEvent(node, 'mousedown', function (e) { obj.mouseDown(e) }); addEvent(node, 'mousemove', function (e) { obj.mouseMove(e) }); addEvent(node, 'mouseup', function (e) { obj.mouseUp(e) }) }; DragResize.prototype.select = function (newElement) { with (this) { if (!document.getElementById || !enabled) return; if (newElement && (newElement != element) && enabled) { element = newElement; element.style.zIndex = ++zIndex; if (this.resizeHandleSet) this.resizeHandleSet(element, true); elmX = parseInt(element.style.left); elmY = parseInt(element.style.top); elmW = element.offsetWidth; elmH = element.offsetHeight; if (ondragfocus) this.ondragfocus() } } }; DragResize.prototype.deselect = function (delHandles) { with (this) { if (!document.getElementById || !enabled) return; if (delHandles) { if (ondragblur) this.ondragblur(); if (this.resizeHandleSet) this.resizeHandleSet(element, false); element = null } handle = null; mOffX = 0; mOffY = 0 } }; DragResize.prototype.mouseDown = function (e) { with (this) { if (!document.getElementById || !enabled) return true; var elm = e.target || e.srcElement, newElement = null, newHandle = null, hRE = new RegExp(myName + '-([trmbl]{2})', ''); while (elm) { if (elm.className) { if (!newHandle && (hRE.test(elm.className) || isHandle(elm))) newHandle = elm; if (isElement(elm)) { newElement = elm; break } } elm = elm.parentNode } if (element && (element != newElement) && allowBlur) deselect(true); if (newElement && (!element || (newElement == element))) { if (newHandle) cancelEvent(e); select(newElement, newHandle); handle = newHandle; if (handle && ondragstart) this.ondragstart(hRE.test(handle.className)) } } }; DragResize.prototype.mouseMove = function (e) { with (this) { if (!document.getElementById || !enabled) return true; mouseX = e.pageX || e.clientX + document.documentElement.scrollLeft; mouseY = e.pageY || e.clientY + document.documentElement.scrollTop; var diffX = mouseX - lastMouseX + mOffX; var diffY = mouseY - lastMouseY + mOffY; mOffX = mOffY = 0; lastMouseX = mouseX; lastMouseY = mouseY; if (!handle) return true; var isResize = false; if (this.resizeHandleDrag && this.resizeHandleDrag(diffX, diffY)) { isResize = true } else { var dX = diffX, dY = diffY; if (elmX + dX < minLeft) mOffX = (dX - (diffX = minLeft - elmX)); else if (elmX + elmW + dX > maxLeft) mOffX = (dX - (diffX = maxLeft - elmX - elmW)); if (elmY + dY < minTop) mOffY = (dY - (diffY = minTop - elmY)); else if (elmY + elmH + dY > maxTop) mOffY = (dY - (diffY = maxTop - elmY - elmH)); elmX += diffX; elmY += diffY } with (element.style) { left = elmX + 'px'; width = elmW + 'px'; top = elmY + 'px'; height = elmH + 'px' } if (window.opera && document.documentElement) { var oDF = document.getElementById('op-drag-fix'); if (!oDF) { var oDF = document.createElement('input'); oDF.id = 'op-drag-fix'; oDF.style.display = 'none'; document.body.appendChild(oDF) } oDF.focus() } if (ondragmove) this.ondragmove(isResize); cancelEvent(e) } }; DragResize.prototype.mouseUp = function (e) { with (this) { if (!document.getElementById || !enabled) return; var hRE = new RegExp(myName + '-([trmbl]{2})', ''); if (handle && ondragend) this.ondragend(hRE.test(handle.className)); deselect(false) } }; DragResize.prototype.resizeHandleSet = function (elm, show) { with (this) { if (!elm._handle_tr) { for (var h = 0; h < handles.length; h++) { var hDiv = document.createElement('div'); hDiv.className = myName + ' ' + myName + '-' + handles[h]; elm['_handle_' + handles[h]] = elm.appendChild(hDiv) } } for (var h = 0; h < handles.length; h++) { elm['_handle_' + handles[h]].style.visibility = show ? 'inherit' : 'hidden' } } }; DragResize.prototype.resizeHandleDrag = function (diffX, diffY) { with (this) { var hClass = handle && handle.className && handle.className.match(new RegExp(myName + '-([tmblr]{2})')) ? RegExp.$1 : ''; var dY = diffY, dX = diffX, processed = false; if (hClass.indexOf('t') >= 0) { rs = 1; if (elmH - dY < minHeight) mOffY = (dY - (diffY = elmH - minHeight)); else if (elmY + dY < minTop) mOffY = (dY - (diffY = minTop - elmY)); elmY += diffY; elmH -= diffY; processed = true } if (hClass.indexOf('b') >= 0) { rs = 1; if (elmH + dY < minHeight) mOffY = (dY - (diffY = minHeight - elmH)); else if (elmY + elmH + dY > maxTop) mOffY = (dY - (diffY = maxTop - elmY - elmH)); elmH += diffY; processed = true } if (hClass.indexOf('l') >= 0) { rs = 1; if (elmW - dX < minWidth) mOffX = (dX - (diffX = elmW - minWidth)); else if (elmX + dX < minLeft) mOffX = (dX - (diffX = minLeft - elmX)); elmX += diffX; elmW -= diffX; processed = true } if (hClass.indexOf('r') >= 0) { rs = 1; if (elmW + dX < minWidth) mOffX = (dX - (diffX = minWidth - elmW)); else if (elmX + elmW + dX > maxLeft) mOffX = (dX - (diffX = maxLeft - elmX - elmW)); elmW += diffX; processed = true } return processed } };

/**Jquery Dom Resize**/
(function ($, h, c) { var a = $([]), e = $.resize = $.extend($.resize, {}), i, k = "setTimeout", j = "resize", d = j + "-special-event", b = "delay", f = "throttleWindow"; e[b] = 250; e[f] = true; $.event.special[j] = { setup: function () { if (!e[f] && this[k]) { return false } var l = $(this); a = a.add(l); $.data(this, d, { w: l.width(), h: l.height() }); if (a.length === 1) { g() } }, teardown: function () { if (!e[f] && this[k]) { return false } var l = $(this); a = a.not(l); l.removeData(d); if (!a.length) { clearTimeout(i) } }, add: function (l) { if (!e[f] && this[k]) { return false } var n; function m(s, o, p) { var q = $(this), r = $.data(this, d); r.w = o !== c ? o : q.width(); r.h = p !== c ? p : q.height(); n.apply(this, arguments) } if ($.isFunction(l)) { n = l; return m } else { n = l.handler; l.handler = m } } }; function g() { i = h[k](function () { a.each(function () { var n = $(this), m = n.width(), l = n.height(), o = $.data(this, d); if (m !== o.w || l !== o.h) { n.trigger(j, [o.w = m, o.h = l]) } }); g() }, e[b]) } })(jQuery, this);

/**Demo**/
//$(function () {
//    $("#dragBox_tool").draggable().resizable();//依赖jquery-ui.js,jquery-ui.css
//});
/**End Demo**/
/**Demo1**/
var Dragging = function (validateHandler) { //参数为验证点击区域是否为可移动区域，如果是返回欲移动元素，负责返回null
    var draggingObj = null; //dragging Dialog
    var diffX = 0;
    var diffY = 0;
    function mouseHandler(e) {
        switch (e.type) {
            case 'mousedown':
                draggingObj = validateHandler(e);//验证是否为可点击移动区域
                if (draggingObj != null) {
                    diffX = e.clientX - draggingObj.offsetLeft;
                    diffY = e.clientY - draggingObj.offsetTop;
                }
                break;

            case 'mousemove':
                if (draggingObj) {
                    draggingObj.style.left = (e.clientX - diffX) + 'px';
                    draggingObj.style.top = (e.clientY - diffY) + 'px';
                }
                break;

            case 'mouseup':
                draggingObj = null;
                diffX = 0;
                diffY = 0;
                break;
        }
    };
    return {
        enable: function () {
            document.addEventListener('mousedown', mouseHandler);
            document.addEventListener('mousemove', mouseHandler);
            document.addEventListener('mouseup', mouseHandler);
        },
        disable: function () {
            document.removeEventListener('mousedown', mouseHandler);
            document.removeEventListener('mousemove', mouseHandler);
            document.removeEventListener('mouseup', mouseHandler);
        }
    }
}
function getDraggingDialog(e) {
    var target = e.target;
    if (target.nodeName!='ME') {
        return null;
    }
    while (target && target.className.indexOf('mxWindowTitle') == -1) {
        target = target.offsetParent;
    }
    if (target != null) {
        return target.offsetParent;
    } else {
        return null;
    }
}
Dragging(getDraggingDialog).enable();
/**End Demo1**/
/**Demo2**/
//(function (document) {
//    //Usage: $("#id").drag();//
//    $.fn.Drag = function (e) {
//        console.log(e);
//        var M = false;
//        var Rx, Ry;
//        var t = $(this);
//        t.mousedown(function (event) {
//            Rx = event.pageX - (parseInt(t.css("left")) || 0);
//            Ry = event.pageY - (parseInt(t.css("top")) || 0);
//            t.css("position", "absolute").css('cursor', 'move').fadeTo(20, 0.5);
//            M = true;
//        })
//        .mouseup(function (event) {
//            M = false; t.fadeTo(20, 1);
//        });
//        $(document).mousemove(function (event) {
//            if (M) {
//                t.css({ top: event.pageY - Ry, left: event.pageX - Rx });
//            }
//        });
//    }

//})(document);
//$(document).ready(function () {
//    $("#dragBox_tool").Drag(1);
//});
/**End Demo2**/

/**Demo3**/
/**窗口拖拽拉伸**/
//var dragresize = new DragResize('dragresize', { minWidth: 550, minHeight: 500, minLeft: 5, minTop: 5, maxLeft: '100%', maxTop: '100%' });
//dragresize.isElement = function (elm) {
//    if (elm.className && elm.className.indexOf('mxWindow') > -1) return true;
//};
//dragresize.isHandle = function (elm) {
//    if (elm.className && elm.className.indexOf('mxWindow') > -1) return true;
//};
//dragresize.ondragfocus = function () {
//    /*
//    $("div.dragTkbox").css("position", "static");
//    $(this).css("z-index", cIndex +1);
//    */
//    var cIndex = parseInt($(".mxWindow").css("z-index"));
//    if (cIndex <= 5) {
//        $(".mxWindow").css("z-index", cIndex + 5);
//    }
//};
//dragresize.ondragstart = function (isResize) { };
//dragresize.ondragmove = function (isResize) { };
//dragresize.ondragend = function (isResize) { };
//dragresize.ondragblur = function () { };
//dragresize.apply(document);
/**End Demo3**/
