import { LitElement as Ui, html as G, css as qi, property as Vi, state as q, customElement as ji, nothing as Dt } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as Gi } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as Fi } from "@umbraco-cms/backoffice/property-editor";
/*!
 * Cropper.js v1.4.0
 * https://fengyuanchen.github.io/cropperjs
 *
 * Copyright 2015-present Chen Fengyuan
 * Released under the MIT license
 *
 * Date: 2018-06-01T15:18:18.692Z
 */
var bi = typeof window < "u", X = bi ? window : {}, M = "cropper", Zt = "all", yi = "crop", Di = "move", Ci = "zoom", et = "e", at = "w", ht = "s", Q = "n", wt = "ne", xt = "nw", bt = "se", yt = "sw", Xt = M + "-crop", ni = M + "-disabled", I = M + "-hidden", hi = M + "-hide", Qi = M + "-invisible", Lt = M + "-modal", Yt = M + "-move", Et = M + "Action", Ot = M + "Preview", Kt = "crop", Mi = "move", Ei = "none", $t = "crop", Ut = "cropend", qt = "cropmove", Vt = "cropstart", li = "dblclick", ci = X.PointerEvent ? "pointerdown" : "touchstart mousedown", di = X.PointerEvent ? "pointermove" : "touchmove mousemove", pi = X.PointerEvent ? "pointerup pointercancel" : "touchend touchcancel mouseup", fi = "ready", ui = "resize", gi = "wheel mousewheel DOMMouseScroll", jt = "zoom", Zi = /^(?:e|w|s|n|se|sw|ne|nw|all|crop|move|zoom)$/, Ki = /^data:/, Ji = /^data:image\/jpeg;base64,/, te = /^(?:img|canvas)$/i, vi = {
  // Define the view mode of the cropper
  viewMode: 0,
  // 0, 1, 2, 3
  // Define the dragging mode of the cropper
  dragMode: Kt,
  // 'crop', 'move' or 'none'
  // Define the initial aspect ratio of the crop box
  initialAspectRatio: NaN,
  // Define the aspect ratio of the crop box
  aspectRatio: NaN,
  // An object with the previous cropping result data
  data: null,
  // A selector for adding extra containers to preview
  preview: "",
  // Re-render the cropper when resize the window
  responsive: !0,
  // Restore the cropped area after resize the window
  restore: !0,
  // Check if the current image is a cross-origin image
  checkCrossOrigin: !0,
  // Check the current image's Exif Orientation information
  checkOrientation: !0,
  // Show the black modal
  modal: !0,
  // Show the dashed lines for guiding
  guides: !0,
  // Show the center indicator for guiding
  center: !0,
  // Show the white modal to highlight the crop box
  highlight: !0,
  // Show the grid background
  background: !0,
  // Enable to crop the image automatically when initialize
  autoCrop: !0,
  // Define the percentage of automatic cropping area when initializes
  autoCropArea: 0.8,
  // Enable to move the image
  movable: !0,
  // Enable to rotate the image
  rotatable: !0,
  // Enable to scale the image
  scalable: !0,
  // Enable to zoom the image
  zoomable: !0,
  // Enable to zoom the image by dragging touch
  zoomOnTouch: !0,
  // Enable to zoom the image by wheeling mouse
  zoomOnWheel: !0,
  // Define zoom ratio when zoom the image by wheeling mouse
  wheelZoomRatio: 0.1,
  // Enable to move the crop box
  cropBoxMovable: !0,
  // Enable to resize the crop box
  cropBoxResizable: !0,
  // Toggle drag mode between "crop" and "move" when click twice on the cropper
  toggleDragModeOnDblclick: !0,
  // Size limitation
  minCanvasWidth: 0,
  minCanvasHeight: 0,
  minCropBoxWidth: 0,
  minCropBoxHeight: 0,
  minContainerWidth: 200,
  minContainerHeight: 100,
  // Shortcuts of events
  ready: null,
  cropstart: null,
  cropmove: null,
  cropend: null,
  crop: null,
  zoom: null
}, ie = '<div class="cropper-container" touch-action="none"><div class="cropper-wrap-box"><div class="cropper-canvas"></div></div><div class="cropper-drag-box"></div><div class="cropper-crop-box"><span class="cropper-view-box"></span><span class="cropper-dashed dashed-h"></span><span class="cropper-dashed dashed-v"></span><span class="cropper-center"></span><span class="cropper-face"></span><span class="cropper-line line-e" data-cropper-action="e"></span><span class="cropper-line line-n" data-cropper-action="n"></span><span class="cropper-line line-w" data-cropper-action="w"></span><span class="cropper-line line-s" data-cropper-action="s"></span><span class="cropper-point point-e" data-cropper-action="e"></span><span class="cropper-point point-n" data-cropper-action="n"></span><span class="cropper-point point-w" data-cropper-action="w"></span><span class="cropper-point point-s" data-cropper-action="s"></span><span class="cropper-point point-ne" data-cropper-action="ne"></span><span class="cropper-point point-nw" data-cropper-action="nw"></span><span class="cropper-point point-sw" data-cropper-action="sw"></span><span class="cropper-point point-se" data-cropper-action="se"></span></div></div>', ee = typeof Symbol == "function" && typeof Symbol.iterator == "symbol" ? function(a) {
  return typeof a;
} : function(a) {
  return a && typeof Symbol == "function" && a.constructor === Symbol && a !== Symbol.prototype ? "symbol" : typeof a;
}, ae = function(a, t) {
  if (!(a instanceof t))
    throw new TypeError("Cannot call a class as a function");
}, re = /* @__PURE__ */ (function() {
  function a(t, e) {
    for (var i = 0; i < e.length; i++) {
      var s = e[i];
      s.enumerable = s.enumerable || !1, s.configurable = !0, "value" in s && (s.writable = !0), Object.defineProperty(t, s.key, s);
    }
  }
  return function(t, e, i) {
    return e && a(t.prototype, e), i && a(t, i), t;
  };
})(), _i = function(a) {
  if (Array.isArray(a)) {
    for (var t = 0, e = Array(a.length); t < a.length; t++) e[t] = a[t];
    return e;
  } else
    return Array.from(a);
}, se = Number.isNaN || X.isNaN;
function x(a) {
  return typeof a == "number" && !se(a);
}
function Wt(a) {
  return typeof a > "u";
}
function ot(a) {
  return (typeof a > "u" ? "undefined" : ee(a)) === "object" && a !== null;
}
var oe = Object.prototype.hasOwnProperty;
function ct(a) {
  if (!ot(a))
    return !1;
  try {
    var t = a.constructor, e = t.prototype;
    return t && e && oe.call(e, "isPrototypeOf");
  } catch {
    return !1;
  }
}
function k(a) {
  return typeof a == "function";
}
function _(a, t) {
  if (a && k(t))
    if (Array.isArray(a) || x(a.length)) {
      var e = a.length, i = void 0;
      for (i = 0; i < e && t.call(a, a[i], i, a) !== !1; i += 1)
        ;
    } else ot(a) && Object.keys(a).forEach(function(s) {
      t.call(a, a[s], s, a);
    });
  return a;
}
var C = Object.assign || function(t) {
  for (var e = arguments.length, i = Array(e > 1 ? e - 1 : 0), s = 1; s < e; s++)
    i[s - 1] = arguments[s];
  return ot(t) && i.length > 0 && i.forEach(function(r) {
    ot(r) && Object.keys(r).forEach(function(o) {
      t[o] = r[o];
    });
  }), t;
}, ne = /\.\d*(?:0|9){12}\d*$/i;
function ut(a) {
  var t = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : 1e11;
  return ne.test(a) ? Math.round(a * t) / t : a;
}
var he = /^(?:width|height|left|top|marginLeft|marginTop)$/;
function Z(a, t) {
  var e = a.style;
  _(t, function(i, s) {
    he.test(s) && x(i) && (i += "px"), e[s] = i;
  });
}
function le(a, t) {
  return a.classList ? a.classList.contains(t) : a.className.indexOf(t) > -1;
}
function O(a, t) {
  if (t) {
    if (x(a.length)) {
      _(a, function(i) {
        O(i, t);
      });
      return;
    }
    if (a.classList) {
      a.classList.add(t);
      return;
    }
    var e = a.className.trim();
    e ? e.indexOf(t) < 0 && (a.className = e + " " + t) : a.className = t;
  }
}
function U(a, t) {
  if (t) {
    if (x(a.length)) {
      _(a, function(e) {
        U(e, t);
      });
      return;
    }
    if (a.classList) {
      a.classList.remove(t);
      return;
    }
    a.className.indexOf(t) >= 0 && (a.className = a.className.replace(t, ""));
  }
}
function dt(a, t, e) {
  if (t) {
    if (x(a.length)) {
      _(a, function(i) {
        dt(i, t, e);
      });
      return;
    }
    e ? O(a, t) : U(a, t);
  }
}
var ce = /([a-z\d])([A-Z])/g;
function Jt(a) {
  return a.replace(ce, "$1-$2").toLowerCase();
}
function _t(a, t) {
  return ot(a[t]) ? a[t] : a.dataset ? a.dataset[t] : a.getAttribute("data-" + Jt(t));
}
function mt(a, t, e) {
  ot(e) ? a[t] = e : a.dataset ? a.dataset[t] = e : a.setAttribute("data-" + Jt(t), e);
}
function Ti(a, t) {
  if (ot(a[t]))
    try {
      delete a[t];
    } catch {
      a[t] = void 0;
    }
  else if (a.dataset)
    try {
      delete a.dataset[t];
    } catch {
      a.dataset[t] = void 0;
    }
  else
    a.removeAttribute("data-" + Jt(t));
}
var Ni = /\s\s*/, Si = (function() {
  var a = !1;
  if (bi) {
    var t = !1, e = function() {
    }, i = Object.defineProperty({}, "once", {
      get: function() {
        return a = !0, t;
      },
      /**
       * This setter can fix a `TypeError` in strict mode
       * {@link https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Getter_only}
       * @param {boolean} value - The value to set
       */
      set: function(r) {
        t = r;
      }
    });
    X.addEventListener("test", e, i), X.removeEventListener("test", e, i);
  }
  return a;
})();
function H(a, t, e) {
  var i = arguments.length > 3 && arguments[3] !== void 0 ? arguments[3] : {}, s = e;
  t.trim().split(Ni).forEach(function(r) {
    if (!Si) {
      var o = a.listeners;
      o && o[r] && o[r][e] && (s = o[r][e], delete o[r][e], Object.keys(o[r]).length === 0 && delete o[r], Object.keys(o).length === 0 && delete a.listeners);
    }
    a.removeEventListener(r, s, i);
  });
}
function z(a, t, e) {
  var i = arguments.length > 3 && arguments[3] !== void 0 ? arguments[3] : {}, s = e;
  t.trim().split(Ni).forEach(function(r) {
    if (i.once && !Si) {
      var o = a.listeners, n = o === void 0 ? {} : o;
      s = function() {
        for (var c = arguments.length, l = Array(c), h = 0; h < c; h++)
          l[h] = arguments[h];
        delete n[r][e], a.removeEventListener(r, s, i), e.apply(a, l);
      }, n[r] || (n[r] = {}), n[r][e] && a.removeEventListener(r, n[r][e], i), n[r][e] = s, a.listeners = n;
    }
    a.addEventListener(r, s, i);
  });
}
function gt(a, t, e) {
  var i = void 0;
  return k(Event) && k(CustomEvent) ? i = new CustomEvent(t, {
    detail: e,
    bubbles: !0,
    cancelable: !0
  }) : (i = document.createEvent("CustomEvent"), i.initCustomEvent(t, !0, !0, e)), a.dispatchEvent(i);
}
function Ai(a) {
  var t = a.getBoundingClientRect();
  return {
    left: t.left + (window.pageXOffset - document.documentElement.clientLeft),
    top: t.top + (window.pageYOffset - document.documentElement.clientTop)
  };
}
var Ht = X.location, de = /^(https?:)\/\/([^:/?#]+):?(\d*)/i;
function mi(a) {
  var t = a.match(de);
  return t && (t[1] !== Ht.protocol || t[2] !== Ht.hostname || t[3] !== Ht.port);
}
function wi(a) {
  var t = "timestamp=" + (/* @__PURE__ */ new Date()).getTime();
  return a + (a.indexOf("?") === -1 ? "?" : "&") + t;
}
function Ct(a) {
  var t = a.rotate, e = a.scaleX, i = a.scaleY, s = a.translateX, r = a.translateY, o = [];
  x(s) && s !== 0 && o.push("translateX(" + s + "px)"), x(r) && r !== 0 && o.push("translateY(" + r + "px)"), x(t) && t !== 0 && o.push("rotate(" + t + "deg)"), x(e) && e !== 1 && o.push("scaleX(" + e + ")"), x(i) && i !== 1 && o.push("scaleY(" + i + ")");
  var n = o.length ? o.join(" ") : "none";
  return {
    WebkitTransform: n,
    msTransform: n,
    transform: n
  };
}
function pe(a) {
  var t = C({}, a), e = [];
  return _(a, function(i, s) {
    delete t[s], _(t, function(r) {
      var o = Math.abs(i.startX - r.startX), n = Math.abs(i.startY - r.startY), d = Math.abs(i.endX - r.endX), c = Math.abs(i.endY - r.endY), l = Math.sqrt(o * o + n * n), h = Math.sqrt(d * d + c * c), p = (h - l) / l;
      e.push(p);
    });
  }), e.sort(function(i, s) {
    return Math.abs(i) < Math.abs(s);
  }), e[0];
}
function Bt(a, t) {
  var e = a.pageX, i = a.pageY, s = {
    endX: e,
    endY: i
  };
  return t ? s : C({
    startX: e,
    startY: i
  }, s);
}
function fe(a) {
  var t = 0, e = 0, i = 0;
  return _(a, function(s) {
    var r = s.startX, o = s.startY;
    t += r, e += o, i += 1;
  }), t /= i, e /= i, {
    pageX: t,
    pageY: e
  };
}
var ue = Number.isFinite || X.isFinite;
function K(a) {
  var t = a.aspectRatio, e = a.height, i = a.width, s = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : "contain", r = function(d) {
    return ue(d) && d > 0;
  };
  if (r(i) && r(e)) {
    var o = e * t;
    s === "contain" && o > i || s === "cover" && o < i ? e = i / t : i = e * t;
  } else r(i) ? e = i / t : r(e) && (i = e * t);
  return {
    width: i,
    height: e
  };
}
function ge(a) {
  var t = a.width, e = a.height, i = a.degree;
  if (i = Math.abs(i) % 180, i === 90)
    return {
      width: e,
      height: t
    };
  var s = i % 90 * Math.PI / 180, r = Math.sin(s), o = Math.cos(s), n = t * o + e * r, d = t * r + e * o;
  return i > 90 ? {
    width: d,
    height: n
  } : {
    width: n,
    height: d
  };
}
function ve(a, t, e, i) {
  var s = t.aspectRatio, r = t.naturalWidth, o = t.naturalHeight, n = t.rotate, d = n === void 0 ? 0 : n, c = t.scaleX, l = c === void 0 ? 1 : c, h = t.scaleY, p = h === void 0 ? 1 : h, w = e.aspectRatio, m = e.naturalWidth, E = e.naturalHeight, y = i.fillColor, N = y === void 0 ? "transparent" : y, A = i.imageSmoothingEnabled, T = A === void 0 ? !0 : A, V = i.imageSmoothingQuality, L = V === void 0 ? "low" : V, f = i.maxWidth, D = f === void 0 ? 1 / 0 : f, S = i.maxHeight, P = S === void 0 ? 1 / 0 : S, j = i.minWidth, J = j === void 0 ? 0 : j, tt = i.minHeight, F = tt === void 0 ? 0 : tt, Y = document.createElement("canvas"), R = Y.getContext("2d"), it = K({
    aspectRatio: w,
    width: D,
    height: P
  }), At = K({
    aspectRatio: w,
    width: J,
    height: F
  }, "cover"), Pt = Math.min(it.width, Math.max(At.width, m)), zt = Math.min(it.height, Math.max(At.height, E)), ai = K({
    aspectRatio: s,
    width: D,
    height: P
  }), ri = K({
    aspectRatio: s,
    width: J,
    height: F
  }, "cover"), si = Math.min(ai.width, Math.max(ri.width, r)), oi = Math.min(ai.height, Math.max(ri.height, o)), Yi = [-si / 2, -oi / 2, si, oi];
  return Y.width = ut(Pt), Y.height = ut(zt), R.fillStyle = N, R.fillRect(0, 0, Pt, zt), R.save(), R.translate(Pt / 2, zt / 2), R.rotate(d * Math.PI / 180), R.scale(l, p), R.imageSmoothingEnabled = T, R.imageSmoothingQuality = L, R.drawImage.apply(R, [a].concat(_i(Yi.map(function($i) {
    return Math.floor(ut($i));
  })))), R.restore(), Y;
}
var Oi = String.fromCharCode;
function me(a, t, e) {
  var i = "", s = void 0;
  for (e += t, s = t; s < e; s += 1)
    i += Oi(a.getUint8(s));
  return i;
}
var we = /^data:.*,/;
function xe(a) {
  var t = a.replace(we, ""), e = atob(t), i = new ArrayBuffer(e.length), s = new Uint8Array(i);
  return _(s, function(r, o) {
    s[o] = e.charCodeAt(o);
  }), i;
}
function be(a, t) {
  var e = new Uint8Array(a), i = "";
  return _(e, function(s) {
    i += Oi(s);
  }), "data:" + t + ";base64," + btoa(i);
}
function ye(a) {
  var t = new DataView(a), e = void 0, i = void 0, s = void 0, r = void 0;
  if (t.getUint8(0) === 255 && t.getUint8(1) === 216)
    for (var o = t.byteLength, n = 2; n < o; ) {
      if (t.getUint8(n) === 255 && t.getUint8(n + 1) === 225) {
        s = n;
        break;
      }
      n += 1;
    }
  if (s) {
    var d = s + 4, c = s + 10;
    if (me(t, d, 4) === "Exif") {
      var l = t.getUint16(c);
      if (i = l === 18761, (i || l === 19789) && t.getUint16(c + 2, i) === 42) {
        var h = t.getUint32(c + 4, i);
        h >= 8 && (r = c + h);
      }
    }
  }
  if (r) {
    var p = t.getUint16(r, i), w = void 0, m = void 0;
    for (m = 0; m < p; m += 1)
      if (w = r + m * 12 + 2, t.getUint16(w, i) === 274) {
        w += 8, e = t.getUint16(w, i), t.setUint16(w, 1, i);
        break;
      }
  }
  return e;
}
function De(a) {
  var t = 0, e = 1, i = 1;
  switch (a) {
    // Flip horizontal
    case 2:
      e = -1;
      break;
    // Rotate left 180°
    case 3:
      t = -180;
      break;
    // Flip vertical
    case 4:
      i = -1;
      break;
    // Flip vertical and rotate right 90°
    case 5:
      t = 90, i = -1;
      break;
    // Rotate right 90°
    case 6:
      t = 90;
      break;
    // Flip horizontal and rotate right 90°
    case 7:
      t = 90, e = -1;
      break;
    // Rotate left 90°
    case 8:
      t = -90;
      break;
  }
  return {
    rotate: t,
    scaleX: e,
    scaleY: i
  };
}
var Ce = {
  render: function() {
    this.initContainer(), this.initCanvas(), this.initCropBox(), this.renderCanvas(), this.cropped && this.renderCropBox();
  },
  initContainer: function() {
    var t = this.element, e = this.options, i = this.container, s = this.cropper;
    O(s, I), U(t, I);
    var r = {
      width: Math.max(i.offsetWidth, Number(e.minContainerWidth) || 200),
      height: Math.max(i.offsetHeight, Number(e.minContainerHeight) || 100)
    };
    this.containerData = r, Z(s, {
      width: r.width,
      height: r.height
    }), O(t, I), U(s, I);
  },
  // Canvas (image wrapper)
  initCanvas: function() {
    var t = this.containerData, e = this.imageData, i = this.options.viewMode, s = Math.abs(e.rotate) % 180 === 90, r = s ? e.naturalHeight : e.naturalWidth, o = s ? e.naturalWidth : e.naturalHeight, n = r / o, d = t.width, c = t.height;
    t.height * n > t.width ? i === 3 ? d = t.height * n : c = t.width / n : i === 3 ? c = t.width / n : d = t.height * n;
    var l = {
      aspectRatio: n,
      naturalWidth: r,
      naturalHeight: o,
      width: d,
      height: c
    };
    l.left = (t.width - d) / 2, l.top = (t.height - c) / 2, l.oldLeft = l.left, l.oldTop = l.top, this.canvasData = l, this.limited = i === 1 || i === 2, this.limitCanvas(!0, !0), this.initialImageData = C({}, e), this.initialCanvasData = C({}, l);
  },
  limitCanvas: function(t, e) {
    var i = this.options, s = this.containerData, r = this.canvasData, o = this.cropBoxData, n = i.viewMode, d = r.aspectRatio, c = this.cropped && o;
    if (t) {
      var l = Number(i.minCanvasWidth) || 0, h = Number(i.minCanvasHeight) || 0;
      n > 1 ? (l = Math.max(l, s.width), h = Math.max(h, s.height), n === 3 && (h * d > l ? l = h * d : h = l / d)) : n > 0 && (l ? l = Math.max(l, c ? o.width : 0) : h ? h = Math.max(h, c ? o.height : 0) : c && (l = o.width, h = o.height, h * d > l ? l = h * d : h = l / d));
      var p = K({
        aspectRatio: d,
        width: l,
        height: h
      });
      l = p.width, h = p.height, r.minWidth = l, r.minHeight = h, r.maxWidth = 1 / 0, r.maxHeight = 1 / 0;
    }
    if (e)
      if (n) {
        var w = s.width - r.width, m = s.height - r.height;
        r.minLeft = Math.min(0, w), r.minTop = Math.min(0, m), r.maxLeft = Math.max(0, w), r.maxTop = Math.max(0, m), c && this.limited && (r.minLeft = Math.min(o.left, o.left + (o.width - r.width)), r.minTop = Math.min(o.top, o.top + (o.height - r.height)), r.maxLeft = o.left, r.maxTop = o.top, n === 2 && (r.width >= s.width && (r.minLeft = Math.min(0, w), r.maxLeft = Math.max(0, w)), r.height >= s.height && (r.minTop = Math.min(0, m), r.maxTop = Math.max(0, m))));
      } else
        r.minLeft = -r.width, r.minTop = -r.height, r.maxLeft = s.width, r.maxTop = s.height;
  },
  renderCanvas: function(t, e) {
    var i = this.canvasData, s = this.imageData;
    if (e) {
      var r = ge({
        width: s.naturalWidth * Math.abs(s.scaleX || 1),
        height: s.naturalHeight * Math.abs(s.scaleY || 1),
        degree: s.rotate || 0
      }), o = r.width, n = r.height, d = i.width * (o / i.naturalWidth), c = i.height * (n / i.naturalHeight);
      i.left -= (d - i.width) / 2, i.top -= (c - i.height) / 2, i.width = d, i.height = c, i.aspectRatio = o / n, i.naturalWidth = o, i.naturalHeight = n, this.limitCanvas(!0, !1);
    }
    (i.width > i.maxWidth || i.width < i.minWidth) && (i.left = i.oldLeft), (i.height > i.maxHeight || i.height < i.minHeight) && (i.top = i.oldTop), i.width = Math.min(Math.max(i.width, i.minWidth), i.maxWidth), i.height = Math.min(Math.max(i.height, i.minHeight), i.maxHeight), this.limitCanvas(!1, !0), i.left = Math.min(Math.max(i.left, i.minLeft), i.maxLeft), i.top = Math.min(Math.max(i.top, i.minTop), i.maxTop), i.oldLeft = i.left, i.oldTop = i.top, Z(this.canvas, C({
      width: i.width,
      height: i.height
    }, Ct({
      translateX: i.left,
      translateY: i.top
    }))), this.renderImage(t), this.cropped && this.limited && this.limitCropBox(!0, !0);
  },
  renderImage: function(t) {
    var e = this.canvasData, i = this.imageData, s = i.naturalWidth * (e.width / e.naturalWidth), r = i.naturalHeight * (e.height / e.naturalHeight);
    C(i, {
      width: s,
      height: r,
      left: (e.width - s) / 2,
      top: (e.height - r) / 2
    }), Z(this.image, C({
      width: i.width,
      height: i.height
    }, Ct(C({
      translateX: i.left,
      translateY: i.top
    }, i)))), t && this.output();
  },
  initCropBox: function() {
    var t = this.options, e = this.canvasData, i = t.aspectRatio || t.initialAspectRatio, s = Number(t.autoCropArea) || 0.8, r = {
      width: e.width,
      height: e.height
    };
    i && (e.height * i > e.width ? r.height = r.width / i : r.width = r.height * i), this.cropBoxData = r, this.limitCropBox(!0, !0), r.width = Math.min(Math.max(r.width, r.minWidth), r.maxWidth), r.height = Math.min(Math.max(r.height, r.minHeight), r.maxHeight), r.width = Math.max(r.minWidth, r.width * s), r.height = Math.max(r.minHeight, r.height * s), r.left = e.left + (e.width - r.width) / 2, r.top = e.top + (e.height - r.height) / 2, r.oldLeft = r.left, r.oldTop = r.top, this.initialCropBoxData = C({}, r);
  },
  limitCropBox: function(t, e) {
    var i = this.options, s = this.containerData, r = this.canvasData, o = this.cropBoxData, n = this.limited, d = i.aspectRatio;
    if (t) {
      var c = Number(i.minCropBoxWidth) || 0, l = Number(i.minCropBoxHeight) || 0, h = Math.min(s.width, n ? r.width : s.width), p = Math.min(s.height, n ? r.height : s.height);
      c = Math.min(c, s.width), l = Math.min(l, s.height), d && (c && l ? l * d > c ? l = c / d : c = l * d : c ? l = c / d : l && (c = l * d), p * d > h ? p = h / d : h = p * d), o.minWidth = Math.min(c, h), o.minHeight = Math.min(l, p), o.maxWidth = h, o.maxHeight = p;
    }
    e && (n ? (o.minLeft = Math.max(0, r.left), o.minTop = Math.max(0, r.top), o.maxLeft = Math.min(s.width, r.left + r.width) - o.width, o.maxTop = Math.min(s.height, r.top + r.height) - o.height) : (o.minLeft = 0, o.minTop = 0, o.maxLeft = s.width - o.width, o.maxTop = s.height - o.height));
  },
  renderCropBox: function() {
    var t = this.options, e = this.containerData, i = this.cropBoxData;
    (i.width > i.maxWidth || i.width < i.minWidth) && (i.left = i.oldLeft), (i.height > i.maxHeight || i.height < i.minHeight) && (i.top = i.oldTop), i.width = Math.min(Math.max(i.width, i.minWidth), i.maxWidth), i.height = Math.min(Math.max(i.height, i.minHeight), i.maxHeight), this.limitCropBox(!1, !0), i.left = Math.min(Math.max(i.left, i.minLeft), i.maxLeft), i.top = Math.min(Math.max(i.top, i.minTop), i.maxTop), i.oldLeft = i.left, i.oldTop = i.top, t.movable && t.cropBoxMovable && mt(this.face, Et, i.width >= e.width && i.height >= e.height ? Di : Zt), Z(this.cropBox, C({
      width: i.width,
      height: i.height
    }, Ct({
      translateX: i.left,
      translateY: i.top
    }))), this.cropped && this.limited && this.limitCanvas(!0, !0), this.disabled || this.output();
  },
  output: function() {
    this.preview(), gt(this.element, $t, this.getData());
  }
}, Me = {
  initPreview: function() {
    var t = this.crossOrigin, e = this.options.preview, i = t ? this.crossOriginUrl : this.url, s = document.createElement("img");
    if (t && (s.crossOrigin = t), s.src = i, this.viewBox.appendChild(s), this.viewBoxImage = s, !!e) {
      var r = e;
      typeof e == "string" ? r = this.element.ownerDocument.querySelectorAll(e) : e.querySelector && (r = [e]), this.previews = r, _(r, function(o) {
        var n = document.createElement("img");
        mt(o, Ot, {
          width: o.offsetWidth,
          height: o.offsetHeight,
          html: o.innerHTML
        }), t && (n.crossOrigin = t), n.src = i, n.style.cssText = 'display:block;width:100%;height:auto;min-width:0!important;min-height:0!important;max-width:none!important;max-height:none!important;image-orientation:0deg!important;"', o.innerHTML = "", o.appendChild(n);
      });
    }
  },
  resetPreview: function() {
    _(this.previews, function(t) {
      var e = _t(t, Ot);
      Z(t, {
        width: e.width,
        height: e.height
      }), t.innerHTML = e.html, Ti(t, Ot);
    });
  },
  preview: function() {
    var t = this.imageData, e = this.canvasData, i = this.cropBoxData, s = i.width, r = i.height, o = t.width, n = t.height, d = i.left - e.left - t.left, c = i.top - e.top - t.top;
    !this.cropped || this.disabled || (Z(this.viewBoxImage, C({
      width: o,
      height: n
    }, Ct(C({
      translateX: -d,
      translateY: -c
    }, t)))), _(this.previews, function(l) {
      var h = _t(l, Ot), p = h.width, w = h.height, m = p, E = w, y = 1;
      s && (y = p / s, E = r * y), r && E > w && (y = w / r, m = s * y, E = w), Z(l, {
        width: m,
        height: E
      }), Z(l.getElementsByTagName("img")[0], C({
        width: o * y,
        height: n * y
      }, Ct(C({
        translateX: -d * y,
        translateY: -c * y
      }, t))));
    }));
  }
}, Ee = {
  bind: function() {
    var t = this.element, e = this.options, i = this.cropper;
    k(e.cropstart) && z(t, Vt, e.cropstart), k(e.cropmove) && z(t, qt, e.cropmove), k(e.cropend) && z(t, Ut, e.cropend), k(e.crop) && z(t, $t, e.crop), k(e.zoom) && z(t, jt, e.zoom), z(i, ci, this.onCropStart = this.cropStart.bind(this)), e.zoomable && e.zoomOnWheel && z(i, gi, this.onWheel = this.wheel.bind(this)), e.toggleDragModeOnDblclick && z(i, li, this.onDblclick = this.dblclick.bind(this)), z(t.ownerDocument, di, this.onCropMove = this.cropMove.bind(this)), z(t.ownerDocument, pi, this.onCropEnd = this.cropEnd.bind(this)), e.responsive && z(window, ui, this.onResize = this.resize.bind(this));
  },
  unbind: function() {
    var t = this.element, e = this.options, i = this.cropper;
    k(e.cropstart) && H(t, Vt, e.cropstart), k(e.cropmove) && H(t, qt, e.cropmove), k(e.cropend) && H(t, Ut, e.cropend), k(e.crop) && H(t, $t, e.crop), k(e.zoom) && H(t, jt, e.zoom), H(i, ci, this.onCropStart), e.zoomable && e.zoomOnWheel && H(i, gi, this.onWheel), e.toggleDragModeOnDblclick && H(i, li, this.onDblclick), H(t.ownerDocument, di, this.onCropMove), H(t.ownerDocument, pi, this.onCropEnd), e.responsive && H(window, ui, this.onResize);
  }
}, _e = {
  resize: function() {
    var t = this.options, e = this.container, i = this.containerData, s = Number(t.minContainerWidth) || 200, r = Number(t.minContainerHeight) || 100;
    if (!(this.disabled || i.width <= s || i.height <= r)) {
      var o = e.offsetWidth / i.width;
      if (o !== 1 || e.offsetHeight !== i.height) {
        var n = void 0, d = void 0;
        t.restore && (n = this.getCanvasData(), d = this.getCropBoxData()), this.render(), t.restore && (this.setCanvasData(_(n, function(c, l) {
          n[l] = c * o;
        })), this.setCropBoxData(_(d, function(c, l) {
          d[l] = c * o;
        })));
      }
    }
  },
  dblclick: function() {
    this.disabled || this.options.dragMode === Ei || this.setDragMode(le(this.dragBox, Xt) ? Mi : Kt);
  },
  wheel: function(t) {
    var e = this, i = Number(this.options.wheelZoomRatio) || 0.1, s = 1;
    this.disabled || (t.preventDefault(), !this.wheeling && (this.wheeling = !0, setTimeout(function() {
      e.wheeling = !1;
    }, 50), t.deltaY ? s = t.deltaY > 0 ? 1 : -1 : t.wheelDelta ? s = -t.wheelDelta / 120 : t.detail && (s = t.detail > 0 ? 1 : -1), this.zoom(-s * i, t)));
  },
  cropStart: function(t) {
    if (!this.disabled) {
      var e = this.options, i = this.pointers, s = void 0;
      t.changedTouches ? _(t.changedTouches, function(r) {
        i[r.identifier] = Bt(r);
      }) : i[t.pointerId || 0] = Bt(t), Object.keys(i).length > 1 && e.zoomable && e.zoomOnTouch ? s = Ci : s = _t(t.target, Et), Zi.test(s) && gt(this.element, Vt, {
        originalEvent: t,
        action: s
      }) !== !1 && (t.preventDefault(), this.action = s, this.cropping = !1, s === yi && (this.cropping = !0, O(this.dragBox, Lt)));
    }
  },
  cropMove: function(t) {
    var e = this.action;
    if (!(this.disabled || !e)) {
      var i = this.pointers;
      t.preventDefault(), gt(this.element, qt, {
        originalEvent: t,
        action: e
      }) !== !1 && (t.changedTouches ? _(t.changedTouches, function(s) {
        C(i[s.identifier], Bt(s, !0));
      }) : C(i[t.pointerId || 0], Bt(t, !0)), this.change(t));
    }
  },
  cropEnd: function(t) {
    if (!this.disabled) {
      var e = this.action, i = this.pointers;
      t.changedTouches ? _(t.changedTouches, function(s) {
        delete i[s.identifier];
      }) : delete i[t.pointerId || 0], e && (t.preventDefault(), Object.keys(i).length || (this.action = ""), this.cropping && (this.cropping = !1, dt(this.dragBox, Lt, this.cropped && this.options.modal)), gt(this.element, Ut, {
        originalEvent: t,
        action: e
      }));
    }
  }
}, Te = {
  change: function(t) {
    var e = this.options, i = this.canvasData, s = this.containerData, r = this.cropBoxData, o = this.pointers, n = this.action, d = e.aspectRatio, c = r.left, l = r.top, h = r.width, p = r.height, w = c + h, m = l + p, E = 0, y = 0, N = s.width, A = s.height, T = !0, V = void 0;
    !d && t.shiftKey && (d = h && p ? h / p : 1), this.limited && (E = r.minLeft, y = r.minTop, N = E + Math.min(s.width, i.width, i.left + i.width), A = y + Math.min(s.height, i.height, i.top + i.height));
    var L = o[Object.keys(o)[0]], f = {
      x: L.endX - L.startX,
      y: L.endY - L.startY
    }, D = function(P) {
      switch (P) {
        case et:
          w + f.x > N && (f.x = N - w);
          break;
        case at:
          c + f.x < E && (f.x = E - c);
          break;
        case Q:
          l + f.y < y && (f.y = y - l);
          break;
        case ht:
          m + f.y > A && (f.y = A - m);
          break;
      }
    };
    switch (n) {
      // Move crop box
      case Zt:
        c += f.x, l += f.y;
        break;
      // Resize crop box
      case et:
        if (f.x >= 0 && (w >= N || d && (l <= y || m >= A))) {
          T = !1;
          break;
        }
        D(et), h += f.x, h < 0 && (n = at, h = -h, c -= h), d && (p = h / d, l += (r.height - p) / 2);
        break;
      case Q:
        if (f.y <= 0 && (l <= y || d && (c <= E || w >= N))) {
          T = !1;
          break;
        }
        D(Q), p -= f.y, l += f.y, p < 0 && (n = ht, p = -p, l -= p), d && (h = p * d, c += (r.width - h) / 2);
        break;
      case at:
        if (f.x <= 0 && (c <= E || d && (l <= y || m >= A))) {
          T = !1;
          break;
        }
        D(at), h -= f.x, c += f.x, h < 0 && (n = et, h = -h, c -= h), d && (p = h / d, l += (r.height - p) / 2);
        break;
      case ht:
        if (f.y >= 0 && (m >= A || d && (c <= E || w >= N))) {
          T = !1;
          break;
        }
        D(ht), p += f.y, p < 0 && (n = Q, p = -p, l -= p), d && (h = p * d, c += (r.width - h) / 2);
        break;
      case wt:
        if (d) {
          if (f.y <= 0 && (l <= y || w >= N)) {
            T = !1;
            break;
          }
          D(Q), p -= f.y, l += f.y, h = p * d;
        } else
          D(Q), D(et), f.x >= 0 ? w < N ? h += f.x : f.y <= 0 && l <= y && (T = !1) : h += f.x, f.y <= 0 ? l > y && (p -= f.y, l += f.y) : (p -= f.y, l += f.y);
        h < 0 && p < 0 ? (n = yt, p = -p, h = -h, l -= p, c -= h) : h < 0 ? (n = xt, h = -h, c -= h) : p < 0 && (n = bt, p = -p, l -= p);
        break;
      case xt:
        if (d) {
          if (f.y <= 0 && (l <= y || c <= E)) {
            T = !1;
            break;
          }
          D(Q), p -= f.y, l += f.y, h = p * d, c += r.width - h;
        } else
          D(Q), D(at), f.x <= 0 ? c > E ? (h -= f.x, c += f.x) : f.y <= 0 && l <= y && (T = !1) : (h -= f.x, c += f.x), f.y <= 0 ? l > y && (p -= f.y, l += f.y) : (p -= f.y, l += f.y);
        h < 0 && p < 0 ? (n = bt, p = -p, h = -h, l -= p, c -= h) : h < 0 ? (n = wt, h = -h, c -= h) : p < 0 && (n = yt, p = -p, l -= p);
        break;
      case yt:
        if (d) {
          if (f.x <= 0 && (c <= E || m >= A)) {
            T = !1;
            break;
          }
          D(at), h -= f.x, c += f.x, p = h / d;
        } else
          D(ht), D(at), f.x <= 0 ? c > E ? (h -= f.x, c += f.x) : f.y >= 0 && m >= A && (T = !1) : (h -= f.x, c += f.x), f.y >= 0 ? m < A && (p += f.y) : p += f.y;
        h < 0 && p < 0 ? (n = wt, p = -p, h = -h, l -= p, c -= h) : h < 0 ? (n = bt, h = -h, c -= h) : p < 0 && (n = xt, p = -p, l -= p);
        break;
      case bt:
        if (d) {
          if (f.x >= 0 && (w >= N || m >= A)) {
            T = !1;
            break;
          }
          D(et), h += f.x, p = h / d;
        } else
          D(ht), D(et), f.x >= 0 ? w < N ? h += f.x : f.y >= 0 && m >= A && (T = !1) : h += f.x, f.y >= 0 ? m < A && (p += f.y) : p += f.y;
        h < 0 && p < 0 ? (n = xt, p = -p, h = -h, l -= p, c -= h) : h < 0 ? (n = yt, h = -h, c -= h) : p < 0 && (n = wt, p = -p, l -= p);
        break;
      // Move canvas
      case Di:
        this.move(f.x, f.y), T = !1;
        break;
      // Zoom canvas
      case Ci:
        this.zoom(pe(o), t), T = !1;
        break;
      // Create crop box
      case yi:
        if (!f.x || !f.y) {
          T = !1;
          break;
        }
        V = Ai(this.cropper), c = L.startX - V.left, l = L.startY - V.top, h = r.minWidth, p = r.minHeight, f.x > 0 ? n = f.y > 0 ? bt : wt : f.x < 0 && (c -= h, n = f.y > 0 ? yt : xt), f.y < 0 && (l -= p), this.cropped || (U(this.cropBox, I), this.cropped = !0, this.limited && this.limitCropBox(!0, !0));
        break;
    }
    T && (r.width = h, r.height = p, r.left = c, r.top = l, this.action = n, this.renderCropBox()), _(o, function(S) {
      S.startX = S.endX, S.startY = S.endY;
    });
  }
}, Ne = {
  // Show the crop box manually
  crop: function() {
    return this.ready && !this.cropped && !this.disabled && (this.cropped = !0, this.limitCropBox(!0, !0), this.options.modal && O(this.dragBox, Lt), U(this.cropBox, I), this.setCropBoxData(this.initialCropBoxData)), this;
  },
  // Reset the image and crop box to their initial states
  reset: function() {
    return this.ready && !this.disabled && (this.imageData = C({}, this.initialImageData), this.canvasData = C({}, this.initialCanvasData), this.cropBoxData = C({}, this.initialCropBoxData), this.renderCanvas(), this.cropped && this.renderCropBox()), this;
  },
  // Clear the crop box
  clear: function() {
    return this.cropped && !this.disabled && (C(this.cropBoxData, {
      left: 0,
      top: 0,
      width: 0,
      height: 0
    }), this.cropped = !1, this.renderCropBox(), this.limitCanvas(!0, !0), this.renderCanvas(), U(this.dragBox, Lt), O(this.cropBox, I)), this;
  },
  /**
   * Replace the image's src and rebuild the cropper
   * @param {string} url - The new URL.
   * @param {boolean} [hasSameSize] - Indicate if the new image has the same size as the old one.
   * @returns {Cropper} this
   */
  replace: function(t) {
    var e = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : !1;
    return !this.disabled && t && (this.isImg && (this.element.src = t), e ? (this.url = t, this.image.src = t, this.ready && (this.viewBoxImage.src = t, _(this.previews, function(i) {
      i.getElementsByTagName("img")[0].src = t;
    }))) : (this.isImg && (this.replaced = !0), this.options.data = null, this.uncreate(), this.load(t))), this;
  },
  // Enable (unfreeze) the cropper
  enable: function() {
    return this.ready && this.disabled && (this.disabled = !1, U(this.cropper, ni)), this;
  },
  // Disable (freeze) the cropper
  disable: function() {
    return this.ready && !this.disabled && (this.disabled = !0, O(this.cropper, ni)), this;
  },
  /**
   * Destroy the cropper and remove the instance from the image
   * @returns {Cropper} this
   */
  destroy: function() {
    var t = this.element;
    return _t(t, M) ? (this.isImg && this.replaced && (t.src = this.originalUrl), this.uncreate(), Ti(t, M), this) : this;
  },
  /**
   * Move the canvas with relative offsets
   * @param {number} offsetX - The relative offset distance on the x-axis.
   * @param {number} [offsetY=offsetX] - The relative offset distance on the y-axis.
   * @returns {Cropper} this
   */
  move: function(t) {
    var e = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : t, i = this.canvasData, s = i.left, r = i.top;
    return this.moveTo(Wt(t) ? t : s + Number(t), Wt(e) ? e : r + Number(e));
  },
  /**
   * Move the canvas to an absolute point
   * @param {number} x - The x-axis coordinate.
   * @param {number} [y=x] - The y-axis coordinate.
   * @returns {Cropper} this
   */
  moveTo: function(t) {
    var e = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : t, i = this.canvasData, s = !1;
    return t = Number(t), e = Number(e), this.ready && !this.disabled && this.options.movable && (x(t) && (i.left = t, s = !0), x(e) && (i.top = e, s = !0), s && this.renderCanvas(!0)), this;
  },
  /**
   * Zoom the canvas with a relative ratio
   * @param {number} ratio - The target ratio.
   * @param {Event} _originalEvent - The original event if any.
   * @returns {Cropper} this
   */
  zoom: function(t, e) {
    var i = this.canvasData;
    return t = Number(t), t < 0 ? t = 1 / (1 - t) : t = 1 + t, this.zoomTo(i.width * t / i.naturalWidth, null, e);
  },
  /**
   * Zoom the canvas to an absolute ratio
   * @param {number} ratio - The target ratio.
   * @param {Object} pivot - The zoom pivot point coordinate.
   * @param {Event} _originalEvent - The original event if any.
   * @returns {Cropper} this
   */
  zoomTo: function(t, e, i) {
    var s = this.options, r = this.canvasData, o = r.width, n = r.height, d = r.naturalWidth, c = r.naturalHeight;
    if (t = Number(t), t >= 0 && this.ready && !this.disabled && s.zoomable) {
      var l = d * t, h = c * t;
      if (gt(this.element, jt, {
        ratio: t,
        oldRatio: o / d,
        originalEvent: i
      }) === !1)
        return this;
      if (i) {
        var p = this.pointers, w = Ai(this.cropper), m = p && Object.keys(p).length ? fe(p) : {
          pageX: i.pageX,
          pageY: i.pageY
        };
        r.left -= (l - o) * ((m.pageX - w.left - r.left) / o), r.top -= (h - n) * ((m.pageY - w.top - r.top) / n);
      } else ct(e) && x(e.x) && x(e.y) ? (r.left -= (l - o) * ((e.x - r.left) / o), r.top -= (h - n) * ((e.y - r.top) / n)) : (r.left -= (l - o) / 2, r.top -= (h - n) / 2);
      r.width = l, r.height = h, this.renderCanvas(!0);
    }
    return this;
  },
  /**
   * Rotate the canvas with a relative degree
   * @param {number} degree - The rotate degree.
   * @returns {Cropper} this
   */
  rotate: function(t) {
    return this.rotateTo((this.imageData.rotate || 0) + Number(t));
  },
  /**
   * Rotate the canvas to an absolute degree
   * @param {number} degree - The rotate degree.
   * @returns {Cropper} this
   */
  rotateTo: function(t) {
    return t = Number(t), x(t) && this.ready && !this.disabled && this.options.rotatable && (this.imageData.rotate = t % 360, this.renderCanvas(!0, !0)), this;
  },
  /**
   * Scale the image on the x-axis.
   * @param {number} scaleX - The scale ratio on the x-axis.
   * @returns {Cropper} this
   */
  scaleX: function(t) {
    var e = this.imageData.scaleY;
    return this.scale(t, x(e) ? e : 1);
  },
  /**
   * Scale the image on the y-axis.
   * @param {number} scaleY - The scale ratio on the y-axis.
   * @returns {Cropper} this
   */
  scaleY: function(t) {
    var e = this.imageData.scaleX;
    return this.scale(x(e) ? e : 1, t);
  },
  /**
   * Scale the image
   * @param {number} scaleX - The scale ratio on the x-axis.
   * @param {number} [scaleY=scaleX] - The scale ratio on the y-axis.
   * @returns {Cropper} this
   */
  scale: function(t) {
    var e = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : t, i = this.imageData, s = !1;
    return t = Number(t), e = Number(e), this.ready && !this.disabled && this.options.scalable && (x(t) && (i.scaleX = t, s = !0), x(e) && (i.scaleY = e, s = !0), s && this.renderCanvas(!0, !0)), this;
  },
  /**
   * Get the cropped area position and size data (base on the original image)
   * @param {boolean} [rounded=false] - Indicate if round the data values or not.
   * @returns {Object} The result cropped data.
   */
  getData: function() {
    var t = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : !1, e = this.options, i = this.imageData, s = this.canvasData, r = this.cropBoxData, o = void 0;
    if (this.ready && this.cropped) {
      o = {
        x: r.left - s.left,
        y: r.top - s.top,
        width: r.width,
        height: r.height
      };
      var n = i.width / i.naturalWidth;
      if (_(o, function(l, h) {
        o[h] = l / n;
      }), t) {
        var d = Math.round(o.y + o.height), c = Math.round(o.x + o.width);
        o.x = Math.round(o.x), o.y = Math.round(o.y), o.width = c - o.x, o.height = d - o.y;
      }
    } else
      o = {
        x: 0,
        y: 0,
        width: 0,
        height: 0
      };
    return e.rotatable && (o.rotate = i.rotate || 0), e.scalable && (o.scaleX = i.scaleX || 1, o.scaleY = i.scaleY || 1), o;
  },
  /**
   * Set the cropped area position and size with new data
   * @param {Object} data - The new data.
   * @returns {Cropper} this
   */
  setData: function(t) {
    var e = this.options, i = this.imageData, s = this.canvasData, r = {};
    if (this.ready && !this.disabled && ct(t)) {
      var o = !1;
      e.rotatable && x(t.rotate) && t.rotate !== i.rotate && (i.rotate = t.rotate, o = !0), e.scalable && (x(t.scaleX) && t.scaleX !== i.scaleX && (i.scaleX = t.scaleX, o = !0), x(t.scaleY) && t.scaleY !== i.scaleY && (i.scaleY = t.scaleY, o = !0)), o && this.renderCanvas(!0, !0);
      var n = i.width / i.naturalWidth;
      x(t.x) && (r.left = t.x * n + s.left), x(t.y) && (r.top = t.y * n + s.top), x(t.width) && (r.width = t.width * n), x(t.height) && (r.height = t.height * n), this.setCropBoxData(r);
    }
    return this;
  },
  /**
   * Get the container size data.
   * @returns {Object} The result container data.
   */
  getContainerData: function() {
    return this.ready ? C({}, this.containerData) : {};
  },
  /**
   * Get the image position and size data.
   * @returns {Object} The result image data.
   */
  getImageData: function() {
    return this.sized ? C({}, this.imageData) : {};
  },
  /**
   * Get the canvas position and size data.
   * @returns {Object} The result canvas data.
   */
  getCanvasData: function() {
    var t = this.canvasData, e = {};
    return this.ready && _(["left", "top", "width", "height", "naturalWidth", "naturalHeight"], function(i) {
      e[i] = t[i];
    }), e;
  },
  /**
   * Set the canvas position and size with new data.
   * @param {Object} data - The new canvas data.
   * @returns {Cropper} this
   */
  setCanvasData: function(t) {
    var e = this.canvasData, i = e.aspectRatio;
    return this.ready && !this.disabled && ct(t) && (x(t.left) && (e.left = t.left), x(t.top) && (e.top = t.top), x(t.width) ? (e.width = t.width, e.height = t.width / i) : x(t.height) && (e.height = t.height, e.width = t.height * i), this.renderCanvas(!0)), this;
  },
  /**
   * Get the crop box position and size data.
   * @returns {Object} The result crop box data.
   */
  getCropBoxData: function() {
    var t = this.cropBoxData, e = void 0;
    return this.ready && this.cropped && (e = {
      left: t.left,
      top: t.top,
      width: t.width,
      height: t.height
    }), e || {};
  },
  /**
   * Set the crop box position and size with new data.
   * @param {Object} data - The new crop box data.
   * @returns {Cropper} this
   */
  setCropBoxData: function(t) {
    var e = this.cropBoxData, i = this.options.aspectRatio, s = void 0, r = void 0;
    return this.ready && this.cropped && !this.disabled && ct(t) && (x(t.left) && (e.left = t.left), x(t.top) && (e.top = t.top), x(t.width) && t.width !== e.width && (s = !0, e.width = t.width), x(t.height) && t.height !== e.height && (r = !0, e.height = t.height), i && (s ? e.height = e.width / i : r && (e.width = e.height * i)), this.renderCropBox()), this;
  },
  /**
   * Get a canvas drawn the cropped image.
   * @param {Object} [options={}] - The config options.
   * @returns {HTMLCanvasElement} - The result canvas.
   */
  getCroppedCanvas: function() {
    var t = arguments.length > 0 && arguments[0] !== void 0 ? arguments[0] : {};
    if (!this.ready || !window.HTMLCanvasElement)
      return null;
    var e = this.canvasData, i = ve(this.image, this.imageData, e, t);
    if (!this.cropped)
      return i;
    var s = this.getData(), r = s.x, o = s.y, n = s.width, d = s.height, c = i.width / Math.floor(e.naturalWidth);
    c !== 1 && (r *= c, o *= c, n *= c, d *= c);
    var l = n / d, h = K({
      aspectRatio: l,
      width: t.maxWidth || 1 / 0,
      height: t.maxHeight || 1 / 0
    }), p = K({
      aspectRatio: l,
      width: t.minWidth || 0,
      height: t.minHeight || 0
    }, "cover"), w = K({
      aspectRatio: l,
      width: t.width || (c !== 1 ? i.width : n),
      height: t.height || (c !== 1 ? i.height : d)
    }), m = w.width, E = w.height;
    m = Math.min(h.width, Math.max(p.width, m)), E = Math.min(h.height, Math.max(p.height, E));
    var y = document.createElement("canvas"), N = y.getContext("2d");
    y.width = ut(m), y.height = ut(E), N.fillStyle = t.fillColor || "transparent", N.fillRect(0, 0, m, E);
    var A = t.imageSmoothingEnabled, T = A === void 0 ? !0 : A, V = t.imageSmoothingQuality;
    N.imageSmoothingEnabled = T, V && (N.imageSmoothingQuality = V);
    var L = i.width, f = i.height, D = r, S = o, P = void 0, j = void 0, J = void 0, tt = void 0, F = void 0, Y = void 0;
    D <= -n || D > L ? (D = 0, P = 0, J = 0, F = 0) : D <= 0 ? (J = -D, D = 0, P = Math.min(L, n + D), F = P) : D <= L && (J = 0, P = Math.min(n, L - D), F = P), P <= 0 || S <= -d || S > f ? (S = 0, j = 0, tt = 0, Y = 0) : S <= 0 ? (tt = -S, S = 0, j = Math.min(f, d + S), Y = j) : S <= f && (tt = 0, j = Math.min(d, f - S), Y = j);
    var R = [D, S, P, j];
    if (F > 0 && Y > 0) {
      var it = m / n;
      R.push(J * it, tt * it, F * it, Y * it);
    }
    return N.drawImage.apply(N, [i].concat(_i(R.map(function(At) {
      return Math.floor(ut(At));
    })))), y;
  },
  /**
   * Change the aspect ratio of the crop box.
   * @param {number} aspectRatio - The new aspect ratio.
   * @returns {Cropper} this
   */
  setAspectRatio: function(t) {
    var e = this.options;
    return !this.disabled && !Wt(t) && (e.aspectRatio = Math.max(0, t) || NaN, this.ready && (this.initCropBox(), this.cropped && this.renderCropBox())), this;
  },
  /**
   * Change the drag mode.
   * @param {string} mode - The new drag mode.
   * @returns {Cropper} this
   */
  setDragMode: function(t) {
    var e = this.options, i = this.dragBox, s = this.face;
    if (this.ready && !this.disabled) {
      var r = t === Kt, o = e.movable && t === Mi;
      t = r || o ? t : Ei, e.dragMode = t, mt(i, Et, t), dt(i, Xt, r), dt(i, Yt, o), e.cropBoxMovable || (mt(s, Et, t), dt(s, Xt, r), dt(s, Yt, o));
    }
    return this;
  }
}, Se = X.Cropper, Gt = (function() {
  function a(t) {
    var e = arguments.length > 1 && arguments[1] !== void 0 ? arguments[1] : {};
    if (ae(this, a), !t || !te.test(t.tagName))
      throw new Error("The first argument is required and must be an <img> or <canvas> element.");
    this.element = t, this.options = C({}, vi, ct(e) && e), this.cropped = !1, this.disabled = !1, this.pointers = {}, this.ready = !1, this.reloading = !1, this.replaced = !1, this.sized = !1, this.sizing = !1, this.init();
  }
  return re(a, [{
    key: "init",
    value: function() {
      var e = this.element, i = e.tagName.toLowerCase(), s = void 0;
      if (!_t(e, M)) {
        if (mt(e, M, this), i === "img") {
          if (this.isImg = !0, s = e.getAttribute("src") || "", this.originalUrl = s, !s)
            return;
          s = e.src;
        } else i === "canvas" && window.HTMLCanvasElement && (s = e.toDataURL());
        this.load(s);
      }
    }
  }, {
    key: "load",
    value: function(e) {
      var i = this;
      if (e) {
        this.url = e, this.imageData = {};
        var s = this.element, r = this.options;
        if (!r.rotatable && !r.scalable && (r.checkOrientation = !1), !r.checkOrientation || !window.ArrayBuffer) {
          this.clone();
          return;
        }
        if (Ki.test(e)) {
          Ji.test(e) ? this.read(xe(e)) : this.clone();
          return;
        }
        var o = new XMLHttpRequest();
        this.reloading = !0, this.xhr = o;
        var n = function() {
          i.reloading = !1, i.xhr = null;
        };
        o.ontimeout = n, o.onabort = n, o.onerror = function() {
          n(), i.clone();
        }, o.onload = function() {
          n(), i.read(o.response);
        }, r.checkCrossOrigin && mi(e) && s.crossOrigin && (e = wi(e)), o.open("get", e), o.responseType = "arraybuffer", o.withCredentials = s.crossOrigin === "use-credentials", o.send();
      }
    }
  }, {
    key: "read",
    value: function(e) {
      var i = this.options, s = this.imageData, r = ye(e), o = 0, n = 1, d = 1;
      if (r > 1) {
        this.url = be(e, "image/jpeg");
        var c = De(r);
        o = c.rotate, n = c.scaleX, d = c.scaleY;
      }
      i.rotatable && (s.rotate = o), i.scalable && (s.scaleX = n, s.scaleY = d), this.clone();
    }
  }, {
    key: "clone",
    value: function() {
      var e = this.element, i = this.url, s = void 0, r = void 0;
      this.options.checkCrossOrigin && mi(i) && (s = e.crossOrigin, s ? r = i : (s = "anonymous", r = wi(i))), this.crossOrigin = s, this.crossOriginUrl = r;
      var o = document.createElement("img");
      s && (o.crossOrigin = s), o.src = r || i, this.image = o, o.onload = this.start.bind(this), o.onerror = this.stop.bind(this), O(o, hi), e.parentNode.insertBefore(o, e.nextSibling);
    }
  }, {
    key: "start",
    value: function() {
      var e = this, i = this.isImg ? this.element : this.image;
      i.onload = null, i.onerror = null, this.sizing = !0;
      var s = X.navigator && /(Macintosh|iPhone|iPod|iPad).*AppleWebKit/i.test(X.navigator.userAgent), r = function(c, l) {
        C(e.imageData, {
          naturalWidth: c,
          naturalHeight: l,
          aspectRatio: c / l
        }), e.sizing = !1, e.sized = !0, e.build();
      };
      if (i.naturalWidth && !s) {
        r(i.naturalWidth, i.naturalHeight);
        return;
      }
      var o = document.createElement("img"), n = document.body || document.documentElement;
      this.sizingImage = o, o.onload = function() {
        r(o.width, o.height), s || n.removeChild(o);
      }, o.src = i.src, s || (o.style.cssText = "left:0;max-height:none!important;max-width:none!important;min-height:0!important;min-width:0!important;opacity:0;position:absolute;top:0;z-index:-1;", n.appendChild(o));
    }
  }, {
    key: "stop",
    value: function() {
      var e = this.image;
      e.onload = null, e.onerror = null, e.parentNode.removeChild(e), this.image = null;
    }
  }, {
    key: "build",
    value: function() {
      if (!(!this.sized || this.ready)) {
        var e = this.element, i = this.options, s = this.image, r = e.parentNode, o = document.createElement("div");
        o.innerHTML = ie;
        var n = o.querySelector("." + M + "-container"), d = n.querySelector("." + M + "-canvas"), c = n.querySelector("." + M + "-drag-box"), l = n.querySelector("." + M + "-crop-box"), h = l.querySelector("." + M + "-face");
        this.container = r, this.cropper = n, this.canvas = d, this.dragBox = c, this.cropBox = l, this.viewBox = n.querySelector("." + M + "-view-box"), this.face = h, d.appendChild(s), O(e, I), r.insertBefore(n, e.nextSibling), this.isImg || U(s, hi), this.initPreview(), this.bind(), i.initialAspectRatio = Math.max(0, i.initialAspectRatio) || NaN, i.aspectRatio = Math.max(0, i.aspectRatio) || NaN, i.viewMode = Math.max(0, Math.min(3, Math.round(i.viewMode))) || 0, O(l, I), i.guides || O(l.getElementsByClassName(M + "-dashed"), I), i.center || O(l.getElementsByClassName(M + "-center"), I), i.background && O(n, M + "-bg"), i.highlight || O(h, Qi), i.cropBoxMovable && (O(h, Yt), mt(h, Et, Zt)), i.cropBoxResizable || (O(l.getElementsByClassName(M + "-line"), I), O(l.getElementsByClassName(M + "-point"), I)), this.render(), this.ready = !0, this.setDragMode(i.dragMode), i.autoCrop && this.crop(), this.setData(i.data), k(i.ready) && z(e, fi, i.ready, {
          once: !0
        }), gt(e, fi);
      }
    }
  }, {
    key: "unbuild",
    value: function() {
      this.ready && (this.ready = !1, this.unbind(), this.resetPreview(), this.cropper.parentNode.removeChild(this.cropper), U(this.element, I));
    }
  }, {
    key: "uncreate",
    value: function() {
      this.ready ? (this.unbuild(), this.ready = !1, this.cropped = !1) : this.sizing ? (this.sizingImage.onload = null, this.sizing = !1, this.sized = !1) : this.reloading ? this.xhr.abort() : this.image && this.stop();
    }
    /**
     * Get the no conflict cropper class.
     * @returns {Cropper} The cropper class.
     */
  }], [{
    key: "noConflict",
    value: function() {
      return window.Cropper = Se, a;
    }
    /**
     * Change the default options.
     * @param {Object} options - The new default options.
     */
  }, {
    key: "setDefaults",
    value: function(e) {
      C(vi, ct(e) && e);
    }
  }]), a;
})();
C(Gt.prototype, Ce, Me, Ee, _e, Te, Ne);
var Ae = Object.defineProperty, Oe = Object.getOwnPropertyDescriptor, Bi = (a) => {
  throw TypeError(a);
}, W = (a, t, e, i) => {
  for (var s = i > 1 ? void 0 : i ? Oe(t, e) : t, r = a.length - 1, o; r >= 0; r--)
    (o = a[r]) && (s = (i ? o(t, e, s) : o(s)) || s);
  return i && s && Ae(t, e, s), s;
}, ti = (a, t, e) => t.has(a) || Bi("Cannot " + e), u = (a, t, e) => (ti(a, t, "read from private field"), e ? e.call(a) : t.get(a)), lt = (a, t, e) => t.has(a) ? Bi("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(a) : t.set(a, e), $ = (a, t, e, i) => (ti(a, t, "write to private field"), t.set(a, e), e), b = (a, t, e) => (ti(a, t, "access private method"), e), g, Tt, pt, rt, st, v, nt, Ri, Ft, ki, St, Nt, Mt, Ii, Li, vt, Pi, ii, zi, ei, Wi, Qt, Hi, Xi;
const Rt = "/App_Plugins/N3O.Umbraco.Cropper", ft = 500, kt = 5, It = {};
function xi(a) {
  return a in It || (It[a] = new Promise((t, e) => {
    const i = document.createElement("script");
    i.src = a, i.onload = () => t(), i.onerror = () => e(new Error("Failed to load " + a)), document.head.appendChild(i);
  })), It[a];
}
const Be = "n3o-cropper";
let B = class extends Gi(Ui) {
  constructor() {
    super(...arguments), lt(this, v), lt(this, g, null), lt(this, Tt), lt(this, pt, !1), lt(this, rt, 0), lt(this, st, 0), this._uploadInProgress = !1, this._uploadPercent = 0, this._errorMessage = null, this._cropIndex = null, this._showCropSize = !1, this._currentCropWidth = 0, this._currentCropHeight = 0, this._requiredCropWidth = 0, this._requiredCropHeight = 0, this._mediaId = "";
  }
  get value() {
    return u(this, g);
  }
  set value(a) {
    const t = u(this, g);
    $(this, g, a ?? null), this.requestUpdate("value", t);
  }
  // Configuration (prevalues) arrives as an UmbPropertyEditorConfigCollection.
  set config(a) {
    $(this, Tt, a), $(this, rt, 0), $(this, st, 0);
    for (const t of u(this, v, nt))
      t.width && $(this, rt, Math.max(t.width, u(this, rt))), t.height && $(this, st, Math.max(t.height, u(this, st)));
  }
  async firstUpdated() {
    b(this, v, Ft).call(this, `${Rt}/cropperjs/cropper.min.css`), b(this, v, Ft).call(this, `${Rt}/formstone/upload.css`), await xi(`${Rt}/formstone/core.js`), await xi(`${Rt}/formstone/upload.js`), u(this, g) && b(this, v, ki).call(this), b(this, v, ii).call(this);
  }
  render() {
    return G`
            <div class="n3o-umbraco-cropper">
                ${u(this, g) ? b(this, v, Xi).call(this) : b(this, v, Hi).call(this)}
            </div>
        `;
  }
};
g = /* @__PURE__ */ new WeakMap();
Tt = /* @__PURE__ */ new WeakMap();
pt = /* @__PURE__ */ new WeakMap();
rt = /* @__PURE__ */ new WeakMap();
st = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakSet();
nt = function() {
  var a;
  return ((a = u(this, Tt)) == null ? void 0 : a.getValueByAlias("cropDefinitions")) ?? [];
};
Ri = function() {
  var t;
  const a = (t = u(this, Tt)) == null ? void 0 : t.getValueByAlias("altText");
  return a === !0 || a === "1" || a === 1;
};
Ft = function(a) {
  const t = document.createElement("link");
  t.rel = "stylesheet", t.href = a, this.renderRoot.appendChild(t);
};
ki = function() {
  var t, e, i, s, r, o;
  const a = u(this, v, nt).length;
  if (u(this, g) !== null)
    if (((t = u(this, g).crops) == null ? void 0 : t.length) === a && ((e = u(this, g).cropBoxes) == null ? void 0 : e.length) !== void 0)
      b(this, v, Mt).call(this, 0);
    else if (((i = u(this, g).crops) == null ? void 0 : i.length) === a && ((s = u(this, g).cropBoxes) == null ? void 0 : s.length) === void 0) {
      u(this, g).cropBoxes = new Array(a);
      for (let n = 0; n < u(this, g).crops.length; n++)
        u(this, g).cropBoxes[n] = null;
      for (let n = 0; n < u(this, g).crops.length; n++) {
        const d = u(this, g).crops[n];
        if (d) {
          const c = d.x / kt, l = d.y / kt, h = ft - d.width / kt / ft, p = ft - d.height / kt / ft;
          u(this, g).cropBoxes[n] = { left: c, top: l, width: h, height: p };
        }
      }
      b(this, v, Mt).call(this, 0);
    } else {
      const n = ((r = u(this, g).crops) == null ? void 0 : r.length) === a && ((o = u(this, g).cropBoxes) == null ? void 0 : o.length) === void 0;
      u(this, g).crops = new Array(a), u(this, g).cropBoxes = new Array(a);
      for (let d = 0; d < u(this, g).crops.length; d++)
        u(this, g).crops[d] = null, u(this, g).cropBoxes[d] = null;
      n ? b(this, v, Nt).call(this, u(this, g).crops.length - 1, !1, !1) : b(this, v, Mt).call(this, 0);
    }
};
St = function() {
  this.dispatchEvent(new Fi());
};
Nt = function(a, t, e) {
  var d;
  const i = u(this, v, nt)[a];
  $(this, pt, !0), this._cropIndex = null;
  const s = this.renderRoot.querySelector(".crop-tool-wrapper");
  if (!s)
    return;
  s.querySelectorAll(".crop-tool").forEach((c) => c.remove());
  const r = document.createElement("div");
  r.className = "crop-tool " + (t ? "hidden" : ""), s.prepend(r);
  const o = document.createElement("img");
  o.src = ((d = u(this, g)) == null ? void 0 : d.src) ?? "", r.appendChild(o);
  const n = this;
  new Gt(o, {
    aspectRatio: Number((i.width / i.height).toFixed(3)),
    autoCrop: !0,
    guides: !1,
    highlight: !0,
    // dragCrop was renamed to dragMode in cropperjs v1 types; resizable → cropBoxResizable.
    dragMode: Gt.DragMode.Crop,
    movable: !0,
    cropBoxResizable: !0,
    zoomable: !1,
    viewMode: 2,
    minContainerWidth: ft,
    minContainerHeight: ft,
    crop: function() {
      var h, p;
      var c;
      if (u(n, pt) && e || !u(n, g))
        return;
      u(n, g).crops[a] = this.cropper.getData(!0), u(n, g).cropBoxes[a] = this.cropper.getCropBoxData();
      const l = u(n, v, nt)[a];
      n._requiredCropWidth = l.width, n._requiredCropHeight = l.height, n._currentCropHeight = ((h = u(n, g).crops[a]) == null ? void 0 : h.height) ?? 0, n._currentCropWidth = ((p = u(n, g).crops[a]) == null ? void 0 : p.width) ?? 0, n._showCropSize = n._currentCropHeight < n._requiredCropHeight || n._currentCropWidth < n._requiredCropWidth, u(n, pt) || b(c = n, v, St).call(c);
    },
    ready: function() {
      var l, h, p;
      var c;
      $(n, pt, !1), n._cropIndex = a, e && ((l = u(n, g)) != null && l.crops[a]) && this.cropper.setCropBoxData(((h = u(n, g).cropBoxes) == null ? void 0 : h[a]) ?? {}), a - 1 >= 0 && ((p = u(n, g)) == null ? void 0 : p.crops[a - 1]) === null && b(c = n, v, Nt).call(c, a - 1, a - 1 !== 0, !1);
    }
  });
};
Mt = function(a) {
  b(this, v, Nt).call(this, a, !1, !0);
};
Ii = function(a) {
  return "button cursor " + (this._cropIndex !== null && this._cropIndex === a ? "selected" : "not-selected");
};
Li = function(a) {
  return u(this, v, nt)[a].label;
};
vt = function(a, t) {
  if (a === null) {
    let e = t;
    typeof e == "string" && (e = JSON.parse(e));
    const i = {
      src: e.urlPath,
      mediaId: e.mediaId,
      filename: e.filename,
      width: e.width,
      height: e.height,
      crops: new Array(u(this, v, nt).length)
    };
    for (let s = 0; s < i.crops.length; s++)
      i.crops[s] = null;
    $(this, g, i), this.requestUpdate(), b(this, v, St).call(this), this.updateComplete.then(() => {
      b(this, v, Nt).call(this, u(this, g).crops.length - 1, !1, !1);
    });
  } else
    this._errorMessage = a;
  this._uploadInProgress = !1;
};
Pi = async function() {
  if (!(!this._mediaId || this._mediaId.length !== 17))
    try {
      const a = await fetch(`/umbraco/backoffice/api/cropper/media/${this._mediaId}`);
      if (a.ok) {
        const t = await a.json();
        b(this, v, vt).call(this, null, t);
      } else
        b(this, v, vt).call(this, "No media found with the specified ID");
    } catch {
      b(this, v, vt).call(this, "No media found with the specified ID");
    }
};
ii = function() {
  const a = window.jQuery ?? window.$, t = this.renderRoot.querySelector(".upload");
  if (!a || !t)
    return;
  const e = this;
  a(t).upload({
    action: "/umbraco/backoffice/api/cropper/upload",
    label: "Drop an image, or click to select. Min. size " + u(this, rt) + " x " + u(this, st) + ".",
    maxSize: 104857600,
    maxQueue: 1,
    postData: {
      minWidth: u(this, rt),
      minHeight: u(this, st)
    }
  }).on("filestart.upload", function() {
    e._uploadPercent = 0, e._uploadInProgress = !0;
  }).on("fileprogress.upload", function(i, s, r) {
    e._uploadPercent = r;
  }).on("filecomplete.upload", function(i, s, r) {
    var o;
    b(o = e, v, vt).call(o, null, r);
  }).on("fileerror.upload", function() {
    var i;
    b(i = e, v, vt).call(i, "The specified file is either not a valid image, exceeds the maximum allowed image size, or does not meet dimension constraints");
  });
};
zi = function(a) {
  var t;
  (t = navigator.clipboard) == null || t.writeText(a);
};
ei = function(a) {
  if (a && !confirm("Are you sure?"))
    return;
  const t = this.renderRoot.querySelector(".crop-tool-wrapper");
  t == null || t.querySelectorAll(".crop-tool").forEach((e) => e.remove()), $(this, g, null), this._errorMessage = null, this._cropIndex = null, this.requestUpdate(), b(this, v, St).call(this), this.updateComplete.then(() => b(this, v, ii).call(this));
};
Wi = function(a) {
  u(this, g) && (u(this, g).altText = a.target.value, b(this, v, St).call(this));
};
Qt = function(a) {
  this._mediaId = a.target.value, b(this, v, Pi).call(this);
};
Hi = function() {
  return G`
            ${this._uploadInProgress ? G`<div class="radial-progress" data-progress=${this._uploadPercent}>
                      <div class="circle">
                          <div class="mask full"><div class="fill"></div></div>
                          <div class="mask half"><div class="fill"></div><div class="fill fix"></div></div>
                          <div class="shadow"></div>
                      </div>
                      <div class="inset"><div class="percentage">${this._uploadPercent}%</div></div>
                  </div>` : Dt}

            ${!this._errorMessage && !this._uploadInProgress ? G`<div class="upload"></div>
                      <input
                          class="textBox media-id"
                          type="text"
                          placeholder="Load media by ID"
                          .value=${this._mediaId}
                          @input=${b(this, v, Qt)}
                          @paste=${b(this, v, Qt)} />` : Dt}

            ${this._errorMessage ? G`<p class="error">
                          Uploading of the file failed with the error:<br /><br />
                          ${this._errorMessage}
                      </p>
                      <p class="start-over">
                          <a @click=${() => b(this, v, ei).call(this, !1)} class="cursor reset">Try again</a>
                      </p>` : Dt}
        `;
};
Xi = function() {
  return G`
            <div class="crop-tool-wrapper"></div>

            <ul class="crops">
                ${(u(this, g).crops ?? []).map(
    (a, t) => G`<li>
                        <a @click=${() => b(this, v, Mt).call(this, t)} class=${b(this, v, Ii).call(this, t)}>
                            ${b(this, v, Li).call(this, t)} </a
                        >&nbsp;&nbsp;
                    </li>`
  )}
            </ul>

            ${u(this, v, Ri) ? G`<p>
                      <input
                          class="textBox"
                          type="text"
                          placeholder="Alt text"
                          .value=${u(this, g).altText ?? ""}
                          required
                          @input=${b(this, v, Wi)} />
                  </p>` : Dt}

            ${this._showCropSize ? G`<p style="text-align: center;">
                      <span style="color: #ff0000; font-weight: bold;"
                          >${this._currentCropWidth} x ${this._currentCropHeight}</span
                      >
                      is less than the required
                      <span style="font-weight: bold;">${this._requiredCropWidth} x ${this._requiredCropHeight}</span>
                  </p>` : Dt}

            <div class="start-over">
                <div style="float: left;">
                    <a @click=${() => b(this, v, zi).call(this, u(this, g).mediaId)}>${u(this, g).mediaId}</a>
                    |
                    <a href=${u(this, g).src} target="_blank">Download</a>
                </div>
                <div style="float: right;">
                    <a @click=${() => b(this, v, ei).call(this, !0)} class="reset cursor">Delete image</a>
                </div>
            </div>
        `;
};
B.styles = qi`
        :host {
            display: block;
        }

        .n3o-umbraco-cropper {
            max-width: 500px;
        }

        .hidden {
            display: none;
        }

        .upload {
            padding: 10px;
            border: 1px dashed #666;
            border-radius: 5px;
            text-align: center;
        }

        .error {
            background: red;
            color: white;
            padding: 10px;
        }

        .textBox {
            margin-top: 5px;
            margin-bottom: 5px;
            height: 30px;
            width: 100%;
            font-size: 15px;
            font-family: Verdana;
            line-height: 30px;
            display: inline-block;
            vertical-align: middle;
        }

        .cursor {
            cursor: pointer;
            text-decoration: none;
        }

        .reset {
            color: red;
            font-size: 120%;
            font-weight: bold;
            text-decoration: none;
            text-align: right;
        }

        .crops {
            width: 100%;
            overflow-x: auto;
            overflow-y: hidden;
            padding: 0;
            margin: 0;
        }

        .crops li {
            display: inline-block;
            margin: 0.3em 0.3em 0.3em 0;
            vertical-align: middle;
            padding: 0;
        }

        .crops li img {
            height: 100%;
            width: auto;
        }

        .crops li .button {
            text-decoration: none;
            font: menu;
            display: inline-block;
            padding: 2px 8px;
            background: ButtonFace;
            color: ButtonText;
            border-style: solid;
            border-width: 2px;
            border-color: ButtonHighlight ButtonShadow ButtonShadow ButtonHighlight;
        }

        .crops li .button:active,
        .crops li .selected {
            border-color: ButtonShadow ButtonHighlight ButtonHighlight ButtonShadow;
        }

        .radial-progress {
            margin: 10px;
            width: 120px;
            height: 120px;
            background-color: #d6dadc;
            border-radius: 50%;
            position: relative;
        }

        .radial-progress .inset {
            width: 90px;
            height: 90px;
            position: absolute;
            margin-left: 15px;
            margin-top: 15px;
            background-color: #fbfbfb;
            border-radius: 50%;
            box-shadow: 6px 6px 10px rgba(0, 0, 0, 0.2);
        }

        .radial-progress .inset .percentage {
            position: absolute;
            top: 35px;
            width: 100%;
            text-align: center;
            font-weight: 800;
            font-size: 22px;
            color: #97a71d;
        }

        .start-over {
            overflow: hidden;
            margin-top: 10px;
        }
    `;
W([
  Vi({ type: Object })
], B.prototype, "value", 1);
W([
  q()
], B.prototype, "_uploadInProgress", 2);
W([
  q()
], B.prototype, "_uploadPercent", 2);
W([
  q()
], B.prototype, "_errorMessage", 2);
W([
  q()
], B.prototype, "_cropIndex", 2);
W([
  q()
], B.prototype, "_showCropSize", 2);
W([
  q()
], B.prototype, "_currentCropWidth", 2);
W([
  q()
], B.prototype, "_currentCropHeight", 2);
W([
  q()
], B.prototype, "_requiredCropWidth", 2);
W([
  q()
], B.prototype, "_requiredCropHeight", 2);
W([
  q()
], B.prototype, "_mediaId", 2);
B = W([
  ji(Be)
], B);
const Le = B;
export {
  B as N3oCropperElement,
  Le as default
};
//# sourceMappingURL=cropper.js.map
