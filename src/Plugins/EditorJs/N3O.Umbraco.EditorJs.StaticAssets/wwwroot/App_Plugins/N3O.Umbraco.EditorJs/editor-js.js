import { customElement as Vi } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as Wi } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as qi } from "@umbraco-cms/backoffice/property-editor";
import { UMB_MODAL_MANAGER_CONTEXT as Ki } from "@umbraco-cms/backoffice/modal";
import { useRef as we, useEffect as Yi, createElement as Xi } from "react";
import { createRoot as Zi } from "react-dom/client";
import { jsxs as Hr, jsx as Ie } from "react/jsx-runtime";
import { UMB_MEDIA_PICKER_MODAL as Gi } from "@umbraco-cms/backoffice/media";
import { UMB_LINK_PICKER_MODAL as Ji } from "@umbraco-cms/backoffice/multi-url-picker";
const Qi = ".skriv-let{position:relative;background-color:#fff;max-width:920px;margin:0 auto}.skriv-let.cdx-search-field__input{width:auto}.ce-popover__container{width:250px}@media(min-width:651px){.ce-block__content{max-width:calc(100% - 120px)!important;margin:0 60px}}@media(min-width:651px){.ce-toolbar__content{width:0px!important;margin:0 50px}}.cdx-block{max-width:100%!important}@media(min-width:651px){.codex-editor--narrow .ce-toolbox .ce-popover{left:0;right:0}}@media(min-width:651px){.codex-editor--narrow .ce-settings .ce-popover{right:0;left:0}}.ce-popover{width:auto!important}.ce-popover--inline .ce-popover--nested .ce-popover__container{width:250px}.skriv-let-data{margin:0 auto;max-width:800px}.cdx-label{font-weight:700}.ce-paragraph,.cdx-list__item,.cdx-quote__text,.cdx-checklist__item-text,.embed-tool__caption{font-size:1.0675rem;line-height:1.5}.simple-image{padding:20px 0}.simple-image img{scroll-margin-top:20px;cursor:pointer}.simple-image input,.simple-image [contenteditable]{width:100%;padding:10px 12px;border:1px solid #e4e4e4;background:#fff;box-sizing:border-box;border-radius:3px;outline:none;font-size:1.125rem;height:auto}.simple-image input{margin-bottom:7px}.simple-image img{max-width:100%;margin-bottom:15px}.simple-image.withBorder img{border:1px solid #e8e8eb}.simple-image.withBackground{background:#eff2f5;padding:10px}.simple-image.withBackground img{display:block;max-width:60%;margin:0 auto 15px}.skriv-let__fullscreen-button{display:flex;align-items:center;justify-content:center;position:absolute;top:0;right:0;padding:0;height:50px;width:50px;background-color:transparent;color:#1d202b;border:none;-webkit-appearance:none;-moz-appearance:none;appearance:none;cursor:pointer;z-index:100;border-radius:7px}@media(max-width:650px){.skriv-let__fullscreen-button{background-color:#fff;border:1px solid #e8e8eb}}.skriv-let__fullscreen-button:hover{background-color:#eff2f5}.skriv-let__container:fullscreen{background-color:#fff;color:#242424;line-height:1.5;padding:20px;height:100dvh;overflow-y:scroll}.skriv-let__container:fullscreen .codex-editor{max-width:1080px;margin:0 auto}.skriv-let__container:fullscreen .skriv-let__add-image-button,.skriv-let__container:fullscreen .ce-popover-item[data-item-name=image],.skriv-let__container:fullscreen .ce-popover-item-html[data-item-name=link]{display:none}.skriv-let__container:fullscreen .ce-paragraph,.skriv-let__container:fullscreen .cdx-list__item,.skriv-let__container:fullscreen .cdx-quote__text,.skriv-let__container:fullscreen .cdx-checklist__item-text,.skriv-let__container:fullscreen .embed-tool__caption{font-size:1.25rem}.sr-only{position:absolute;width:1px;height:1px;padding:0;margin:-1px;overflow:hidden;clip:rect(0,0,0,0);white-space:nowrap;border:0}";
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".ce-hint--align-start{text-align:left}.ce-hint--align-center{text-align:center}.ce-hint__description{opacity:.6;margin-top:3px}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
var Ve = typeof globalThis < "u" ? globalThis : typeof window < "u" ? window : typeof global < "u" ? global : typeof self < "u" ? self : {};
function vt(o) {
  return o && o.__esModule && Object.prototype.hasOwnProperty.call(o, "default") ? o.default : o;
}
function es(o) {
  if (o.__esModule)
    return o;
  var e = o.default;
  if (typeof e == "function") {
    var t = function r() {
      return this instanceof r ? Reflect.construct(e, arguments, this.constructor) : e.apply(this, arguments);
    };
    t.prototype = e.prototype;
  } else
    t = {};
  return Object.defineProperty(t, "__esModule", { value: !0 }), Object.keys(o).forEach(function(r) {
    var n = Object.getOwnPropertyDescriptor(o, r);
    Object.defineProperty(t, r, n.get ? n : {
      enumerable: !0,
      get: function() {
        return o[r];
      }
    });
  }), t;
}
function Lt() {
}
Object.assign(Lt, {
  default: Lt,
  register: Lt,
  revert: function() {
  },
  __esModule: !0
});
Element.prototype.matches || (Element.prototype.matches = Element.prototype.matchesSelector || Element.prototype.mozMatchesSelector || Element.prototype.msMatchesSelector || Element.prototype.oMatchesSelector || Element.prototype.webkitMatchesSelector || function(o) {
  const e = (this.document || this.ownerDocument).querySelectorAll(o);
  let t = e.length;
  for (; --t >= 0 && e.item(t) !== this; )
    ;
  return t > -1;
});
Element.prototype.closest || (Element.prototype.closest = function(o) {
  let e = this;
  if (!document.documentElement.contains(e))
    return null;
  do {
    if (e.matches(o))
      return e;
    e = e.parentElement || e.parentNode;
  } while (e !== null);
  return null;
});
Element.prototype.prepend || (Element.prototype.prepend = function(o) {
  const e = document.createDocumentFragment();
  Array.isArray(o) || (o = [o]), o.forEach((t) => {
    const r = t instanceof Node;
    e.appendChild(r ? t : document.createTextNode(t));
  }), this.insertBefore(e, this.firstChild);
});
Element.prototype.scrollIntoViewIfNeeded || (Element.prototype.scrollIntoViewIfNeeded = function(o) {
  o = arguments.length === 0 ? !0 : !!o;
  const e = this.parentNode, t = window.getComputedStyle(e, null), r = parseInt(t.getPropertyValue("border-top-width")), n = parseInt(t.getPropertyValue("border-left-width")), i = this.offsetTop - e.offsetTop < e.scrollTop, s = this.offsetTop - e.offsetTop + this.clientHeight - r > e.scrollTop + e.clientHeight, a = this.offsetLeft - e.offsetLeft < e.scrollLeft, l = this.offsetLeft - e.offsetLeft + this.clientWidth - n > e.scrollLeft + e.clientWidth, c = i && !s;
  (i || s) && o && (e.scrollTop = this.offsetTop - e.offsetTop - e.clientHeight / 2 - r + this.clientHeight / 2), (a || l) && o && (e.scrollLeft = this.offsetLeft - e.offsetLeft - e.clientWidth / 2 - n + this.clientWidth / 2), (i || s || a || l) && !o && this.scrollIntoView(c);
});
window.requestIdleCallback = window.requestIdleCallback || function(o) {
  const e = Date.now();
  return setTimeout(function() {
    o({
      didTimeout: !1,
      timeRemaining: function() {
        return Math.max(0, 50 - (Date.now() - e));
      }
    });
  }, 1);
};
window.cancelIdleCallback = window.cancelIdleCallback || function(o) {
  clearTimeout(o);
};
let ts = (o = 21) => crypto.getRandomValues(new Uint8Array(o)).reduce((e, t) => (t &= 63, t < 36 ? e += t.toString(36) : t < 62 ? e += (t - 26).toString(36).toUpperCase() : t > 62 ? e += "-" : e += "_", e), "");
var ln = /* @__PURE__ */ ((o) => (o.VERBOSE = "VERBOSE", o.INFO = "INFO", o.WARN = "WARN", o.ERROR = "ERROR", o))(ln || {});
const A = {
  BACKSPACE: 8,
  TAB: 9,
  ENTER: 13,
  ESC: 27,
  LEFT: 37,
  UP: 38,
  DOWN: 40,
  RIGHT: 39,
  DELETE: 46
}, os = {
  LEFT: 0
};
function Ke(o, e, t = "log", r, n = "color: inherit") {
  if (!("console" in window) || !window.console[t])
    return;
  const i = ["info", "log", "warn", "error"].includes(t), s = [];
  switch (Ke.logLevel) {
    case "ERROR":
      if (t !== "error")
        return;
      break;
    case "WARN":
      if (!["error", "warn"].includes(t))
        return;
      break;
    case "INFO":
      if (!i || o)
        return;
      break;
  }
  r && s.push(r);
  const a = "Editor.js 2.31.6";
  o && (i ? (s.unshift(`line-height: 1em;
            color: #006FEA;
            display: inline-block;
            font-size: 11px;
            line-height: 1em;
            background-color: #fff;
            padding: 4px 9px;
            border-radius: 30px;
            border: 1px solid rgba(56, 138, 229, 0.16);
            margin: 4px 5px 4px 0;`, n), e = `%c${a}%c ${e}`) : e = `( ${a} )${e}`);
  try {
    i ? r ? console[t](`${e} %o`, ...s) : console[t](e, ...s) : console[t](e);
  } catch {
  }
}
Ke.logLevel = "VERBOSE";
function rs(o) {
  Ke.logLevel = o;
}
const j = Ke.bind(window, !1), Y = Ke.bind(window, !0);
function me(o) {
  return Object.prototype.toString.call(o).match(/\s([a-zA-Z]+)/)[1].toLowerCase();
}
function R(o) {
  return me(o) === "function" || me(o) === "asyncfunction";
}
function U(o) {
  return me(o) === "object";
}
function ie(o) {
  return me(o) === "string";
}
function ns(o) {
  return me(o) === "boolean";
}
function Fr(o) {
  return me(o) === "number";
}
function $r(o) {
  return me(o) === "undefined";
}
function X(o) {
  return o ? Object.keys(o).length === 0 && o.constructor === Object : !0;
}
function cn(o) {
  return o > 47 && o < 58 || // number keys
  o === 32 || o === 13 || // Space bar & return key(s)
  o === 229 || // processing key input for certain languages — Chinese, Japanese, etc.
  o > 64 && o < 91 || // letter keys
  o > 95 && o < 112 || // Numpad keys
  o > 185 && o < 193 || // ;=,-./` (in order)
  o > 218 && o < 223;
}
async function is(o, e = () => {
}, t = () => {
}) {
  async function r(n, i, s) {
    try {
      await n.function(n.data), await i($r(n.data) ? {} : n.data);
    } catch {
      s($r(n.data) ? {} : n.data);
    }
  }
  return o.reduce(async (n, i) => (await n, r(i, e, t)), Promise.resolve());
}
function dn(o) {
  return Array.prototype.slice.call(o);
}
function at(o, e) {
  return function() {
    const t = this, r = arguments;
    window.setTimeout(() => o.apply(t, r), e);
  };
}
function ss(o) {
  return o.name.split(".").pop();
}
function as(o) {
  return /^[-\w]+\/([-+\w]+|\*)$/.test(o);
}
function Ur(o, e, t) {
  let r;
  return (...n) => {
    const i = this, s = () => {
      r = null, o.apply(i, n);
    };
    window.clearTimeout(r), r = window.setTimeout(s, e);
  };
}
function $t(o, e, t = void 0) {
  let r, n, i, s = null, a = 0;
  t || (t = {});
  const l = function() {
    a = t.leading === !1 ? 0 : Date.now(), s = null, i = o.apply(r, n), s || (r = n = null);
  };
  return function() {
    const c = Date.now();
    !a && t.leading === !1 && (a = c);
    const d = e - (c - a);
    return r = this, n = arguments, d <= 0 || d > e ? (s && (clearTimeout(s), s = null), a = c, i = o.apply(r, n), s || (r = n = null)) : !s && t.trailing !== !1 && (s = setTimeout(l, d)), i;
  };
}
function ls() {
  const o = {
    win: !1,
    mac: !1,
    x11: !1,
    linux: !1
  }, e = Object.keys(o).find((t) => window.navigator.appVersion.toLowerCase().indexOf(t) !== -1);
  return e && (o[e] = !0), o;
}
function lt(o) {
  return o[0].toUpperCase() + o.slice(1);
}
function Ut(o, ...e) {
  if (!e.length)
    return o;
  const t = e.shift();
  if (U(o) && U(t))
    for (const r in t)
      U(t[r]) ? (o[r] || Object.assign(o, { [r]: {} }), Ut(o[r], t[r])) : Object.assign(o, { [r]: t[r] });
  return Ut(o, ...e);
}
function eo(o) {
  const e = ls();
  return o = o.replace(/shift/gi, "⇧").replace(/backspace/gi, "⌫").replace(/enter/gi, "⏎").replace(/up/gi, "↑").replace(/left/gi, "→").replace(/down/gi, "↓").replace(/right/gi, "←").replace(/escape/gi, "⎋").replace(/insert/gi, "Ins").replace(/delete/gi, "␡").replace(/\+/gi, " + "), e.mac ? o = o.replace(/ctrl|cmd/gi, "⌘").replace(/alt/gi, "⌥") : o = o.replace(/cmd/gi, "Ctrl").replace(/windows/gi, "WIN"), o;
}
function cs(o) {
  try {
    return new URL(o).href;
  } catch {
  }
  return o.substring(0, 2) === "//" ? window.location.protocol + o : window.location.origin + o;
}
function ds() {
  return ts(10);
}
function hs(o) {
  window.open(o, "_blank");
}
function us(o = "") {
  return `${o}${Math.floor(Math.random() * 1e8).toString(16)}`;
}
function zt(o, e, t) {
  const r = `«${e}» is deprecated and will be removed in the next major release. Please use the «${t}» instead.`;
  o && Y(r, "warn");
}
function Me(o, e, t) {
  const r = t.value ? "value" : "get", n = t[r], i = `#${e}Cache`;
  if (t[r] = function(...s) {
    return this[i] === void 0 && (this[i] = n.apply(this, ...s)), this[i];
  }, r === "get" && t.set) {
    const s = t.set;
    t.set = function(a) {
      delete o[i], s.apply(this, a);
    };
  }
  return t;
}
const hn = 650;
function _e() {
  return window.matchMedia(`(max-width: ${hn}px)`).matches;
}
const Vt = typeof window < "u" && window.navigator && window.navigator.platform && (/iP(ad|hone|od)/.test(window.navigator.platform) || window.navigator.platform === "MacIntel" && window.navigator.maxTouchPoints > 1);
function ps(o, e) {
  const t = Array.isArray(o) || U(o), r = Array.isArray(e) || U(e);
  return t || r ? JSON.stringify(o) === JSON.stringify(e) : o === e;
}
let g = class V {
  /**
   * Check if passed tag has no closed tag
   *
   * @param {HTMLElement} tag - element to check
   * @returns {boolean}
   */
  static isSingleTag(e) {
    return e.tagName && [
      "AREA",
      "BASE",
      "BR",
      "COL",
      "COMMAND",
      "EMBED",
      "HR",
      "IMG",
      "INPUT",
      "KEYGEN",
      "LINK",
      "META",
      "PARAM",
      "SOURCE",
      "TRACK",
      "WBR"
    ].includes(e.tagName);
  }
  /**
   * Check if element is BR or WBR
   *
   * @param {HTMLElement} element - element to check
   * @returns {boolean}
   */
  static isLineBreakTag(e) {
    return e && e.tagName && [
      "BR",
      "WBR"
    ].includes(e.tagName);
  }
  /**
   * Helper for making Elements with class name and attributes
   *
   * @param  {string} tagName - new Element tag name
   * @param  {string[]|string} [classNames] - list or name of CSS class name(s)
   * @param  {object} [attributes] - any attributes
   * @returns {HTMLElement}
   */
  static make(e, t = null, r = {}) {
    const n = document.createElement(e);
    if (Array.isArray(t)) {
      const i = t.filter((s) => s !== void 0);
      n.classList.add(...i);
    } else
      t && n.classList.add(t);
    for (const i in r)
      Object.prototype.hasOwnProperty.call(r, i) && (n[i] = r[i]);
    return n;
  }
  /**
   * Creates Text Node with the passed content
   *
   * @param {string} content - text content
   * @returns {Text}
   */
  static text(e) {
    return document.createTextNode(e);
  }
  /**
   * Append one or several elements to the parent
   *
   * @param  {Element|DocumentFragment} parent - where to append
   * @param  {Element|Element[]|DocumentFragment|Text|Text[]} elements - element or elements list
   */
  static append(e, t) {
    Array.isArray(t) ? t.forEach((r) => e.appendChild(r)) : e.appendChild(t);
  }
  /**
   * Append element or a couple to the beginning of the parent elements
   *
   * @param {Element} parent - where to append
   * @param {Element|Element[]} elements - element or elements list
   */
  static prepend(e, t) {
    Array.isArray(t) ? (t = t.reverse(), t.forEach((r) => e.prepend(r))) : e.prepend(t);
  }
  /**
   * Swap two elements in parent
   *
   * @param {HTMLElement} el1 - from
   * @param {HTMLElement} el2 - to
   * @deprecated
   */
  static swap(e, t) {
    const r = document.createElement("div"), n = e.parentNode;
    n.insertBefore(r, e), n.insertBefore(e, t), n.insertBefore(t, r), n.removeChild(r);
  }
  /**
   * Selector Decorator
   *
   * Returns first match
   *
   * @param {Element} el - element we searching inside. Default - DOM Document
   * @param {string} selector - searching string
   * @returns {Element}
   */
  static find(e = document, t) {
    return e.querySelector(t);
  }
  /**
   * Get Element by Id
   *
   * @param {string} id - id to find
   * @returns {HTMLElement | null}
   */
  static get(e) {
    return document.getElementById(e);
  }
  /**
   * Selector Decorator.
   *
   * Returns all matches
   *
   * @param {Element|Document} el - element we searching inside. Default - DOM Document
   * @param {string} selector - searching string
   * @returns {NodeList}
   */
  static findAll(e = document, t) {
    return e.querySelectorAll(t);
  }
  /**
   * Returns CSS selector for all text inputs
   */
  static get allInputsSelector() {
    return "[contenteditable=true], textarea, input:not([type]), " + ["text", "password", "email", "number", "search", "tel", "url"].map((e) => `input[type="${e}"]`).join(", ");
  }
  /**
   * Find all contenteditable, textarea and editable input elements passed holder contains
   *
   * @param holder - element where to find inputs
   */
  static findAllInputs(e) {
    return dn(e.querySelectorAll(V.allInputsSelector)).reduce((t, r) => V.isNativeInput(r) || V.containsOnlyInlineElements(r) ? [...t, r] : [...t, ...V.getDeepestBlockElements(r)], []);
  }
  /**
   * Search for deepest node which is Leaf.
   * Leaf is the vertex that doesn't have any child nodes
   *
   * @description Method recursively goes throw the all Node until it finds the Leaf
   * @param {Node} node - root Node. From this vertex we start Deep-first search
   *                      {@link https://en.wikipedia.org/wiki/Depth-first_search}
   * @param {boolean} [atLast] - find last text node
   * @returns - it can be text Node or Element Node, so that caret will able to work with it
   *            Can return null if node is Document or DocumentFragment, or node is not attached to the DOM
   */
  static getDeepestNode(e, t = !1) {
    const r = t ? "lastChild" : "firstChild", n = t ? "previousSibling" : "nextSibling";
    if (e && e.nodeType === Node.ELEMENT_NODE && e[r]) {
      let i = e[r];
      if (V.isSingleTag(i) && !V.isNativeInput(i) && !V.isLineBreakTag(i))
        if (i[n])
          i = i[n];
        else if (i.parentNode[n])
          i = i.parentNode[n];
        else
          return i.parentNode;
      return this.getDeepestNode(i, t);
    }
    return e;
  }
  /**
   * Check if object is DOM node
   *
   * @param {*} node - object to check
   * @returns {boolean}
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  static isElement(e) {
    return Fr(e) ? !1 : e && e.nodeType && e.nodeType === Node.ELEMENT_NODE;
  }
  /**
   * Check if object is DocumentFragment node
   *
   * @param {object} node - object to check
   * @returns {boolean}
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  static isFragment(e) {
    return Fr(e) ? !1 : e && e.nodeType && e.nodeType === Node.DOCUMENT_FRAGMENT_NODE;
  }
  /**
   * Check if passed element is contenteditable
   *
   * @param {HTMLElement} element - html element to check
   * @returns {boolean}
   */
  static isContentEditable(e) {
    return e.contentEditable === "true";
  }
  /**
   * Checks target if it is native input
   *
   * @param {*} target - HTML element or string
   * @returns {boolean}
   */
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  static isNativeInput(e) {
    const t = [
      "INPUT",
      "TEXTAREA"
    ];
    return e && e.tagName ? t.includes(e.tagName) : !1;
  }
  /**
   * Checks if we can set caret
   *
   * @param {HTMLElement} target - target to check
   * @returns {boolean}
   */
  static canSetCaret(e) {
    let t = !0;
    if (V.isNativeInput(e))
      switch (e.type) {
        case "file":
        case "checkbox":
        case "radio":
        case "hidden":
        case "submit":
        case "button":
        case "image":
        case "reset":
          t = !1;
          break;
      }
    else
      t = V.isContentEditable(e);
    return t;
  }
  /**
   * Checks node if it is empty
   *
   * @description Method checks simple Node without any childs for emptiness
   * If you have Node with 2 or more children id depth, you better use {@link Dom#isEmpty} method
   * @param {Node} node - node to check
   * @param {string} [ignoreChars] - char or substring to treat as empty
   * @returns {boolean} true if it is empty
   */
  static isNodeEmpty(e, t) {
    let r;
    return this.isSingleTag(e) && !this.isLineBreakTag(e) ? !1 : (this.isElement(e) && this.isNativeInput(e) ? r = e.value : r = e.textContent.replace("​", ""), t && (r = r.replace(new RegExp(t, "g"), "")), r.length === 0);
  }
  /**
   * checks node if it is doesn't have any child nodes
   *
   * @param {Node} node - node to check
   * @returns {boolean}
   */
  static isLeaf(e) {
    return e ? e.childNodes.length === 0 : !1;
  }
  /**
   * breadth-first search (BFS)
   * {@link https://en.wikipedia.org/wiki/Breadth-first_search}
   *
   * @description Pushes to stack all DOM leafs and checks for emptiness
   * @param {Node} node - node to check
   * @param {string} [ignoreChars] - char or substring to treat as empty
   * @returns {boolean}
   */
  static isEmpty(e, t) {
    const r = [e];
    for (; r.length > 0; )
      if (e = r.shift(), !!e) {
        if (this.isLeaf(e) && !this.isNodeEmpty(e, t))
          return !1;
        e.childNodes && r.push(...Array.from(e.childNodes));
      }
    return !0;
  }
  /**
   * Check if string contains html elements
   *
   * @param {string} str - string to check
   * @returns {boolean}
   */
  static isHTMLString(e) {
    const t = V.make("div");
    return t.innerHTML = e, t.childElementCount > 0;
  }
  /**
   * Return length of node`s text content
   *
   * @param {Node} node - node with content
   * @returns {number}
   */
  static getContentLength(e) {
    return V.isNativeInput(e) ? e.value.length : e.nodeType === Node.TEXT_NODE ? e.length : e.textContent.length;
  }
  /**
   * Return array of names of block html elements
   *
   * @returns {string[]}
   */
  static get blockElements() {
    return [
      "address",
      "article",
      "aside",
      "blockquote",
      "canvas",
      "div",
      "dl",
      "dt",
      "fieldset",
      "figcaption",
      "figure",
      "footer",
      "form",
      "h1",
      "h2",
      "h3",
      "h4",
      "h5",
      "h6",
      "header",
      "hgroup",
      "hr",
      "li",
      "main",
      "nav",
      "noscript",
      "ol",
      "output",
      "p",
      "pre",
      "ruby",
      "section",
      "table",
      "tbody",
      "thead",
      "tr",
      "tfoot",
      "ul",
      "video"
    ];
  }
  /**
   * Check if passed content includes only inline elements
   *
   * @param {string|HTMLElement} data - element or html string
   * @returns {boolean}
   */
  static containsOnlyInlineElements(e) {
    let t;
    ie(e) ? (t = document.createElement("div"), t.innerHTML = e) : t = e;
    const r = (n) => !V.blockElements.includes(n.tagName.toLowerCase()) && Array.from(n.children).every(r);
    return Array.from(t.children).every(r);
  }
  /**
   * Find and return all block elements in the passed parent (including subtree)
   *
   * @param {HTMLElement} parent - root element
   * @returns {HTMLElement[]}
   */
  static getDeepestBlockElements(e) {
    return V.containsOnlyInlineElements(e) ? [e] : Array.from(e.children).reduce((t, r) => [...t, ...V.getDeepestBlockElements(r)], []);
  }
  /**
   * Helper for get holder from {string} or return HTMLElement
   *
   * @param {string | HTMLElement} element - holder's id or holder's HTML Element
   * @returns {HTMLElement}
   */
  static getHolder(e) {
    return ie(e) ? document.getElementById(e) : e;
  }
  /**
   * Returns true if element is anchor (is A tag)
   *
   * @param {Element} element - element to check
   * @returns {boolean}
   */
  static isAnchor(e) {
    return e.tagName.toLowerCase() === "a";
  }
  /**
   * Returns the closest ancestor anchor (A tag) of the given element (including itself)
   * 
   * @param element - element to check
   * @returns {HTMLAnchorElement | null}
   */
  static getClosestAnchor(e) {
    return e.closest("a");
  }
  /**
   * Return element's offset related to the document
   *
   * @todo handle case when editor initialized in scrollable popup
   * @param el - element to compute offset
   */
  static offset(e) {
    const t = e.getBoundingClientRect(), r = window.pageXOffset || document.documentElement.scrollLeft, n = window.pageYOffset || document.documentElement.scrollTop, i = t.top + n, s = t.left + r;
    return {
      top: i,
      left: s,
      bottom: i + t.height,
      right: s + t.width
    };
  }
  /**
   * Find text node and offset by total content offset
   *
   * @param {Node} root - root node to start search from
   * @param {number} totalOffset - offset relative to the root node content
   * @returns {{node: Node | null, offset: number}} - node and offset inside node
   */
  static getNodeByOffset(e, t) {
    let r = 0, n = null;
    const i = document.createTreeWalker(
      e,
      NodeFilter.SHOW_TEXT,
      null
    );
    let s = i.nextNode();
    for (; s; ) {
      const c = s.textContent, d = c === null ? 0 : c.length;
      if (n = s, r + d >= t)
        break;
      r += d, s = i.nextNode();
    }
    if (!n)
      return {
        node: null,
        offset: 0
      };
    const a = n.textContent;
    if (a === null || a.length === 0)
      return {
        node: null,
        offset: 0
      };
    const l = Math.min(t - r, a.length);
    return {
      node: n,
      offset: l
    };
  }
};
function fs(o) {
  return !/[^\t\n\r ]/.test(o);
}
function gs(o) {
  const e = window.getComputedStyle(o), t = parseFloat(e.fontSize), r = parseFloat(e.lineHeight) || t * 1.2, n = parseFloat(e.paddingTop), i = parseFloat(e.borderTopWidth), s = parseFloat(e.marginTop), a = t * 0.8, l = (r - t) / 2;
  return s + i + n + l + a;
}
function un(o) {
  o.dataset.empty = g.isEmpty(o) ? "true" : "false";
}
const ms = {
  blockTunes: {
    toggler: {
      "Click to tune": "",
      "or drag to move": ""
    }
  },
  inlineToolbar: {
    converter: {
      "Convert to": ""
    }
  },
  toolbar: {
    toolbox: {
      Add: ""
    }
  },
  popover: {
    Filter: "",
    "Nothing found": "",
    "Convert to": ""
  }
}, vs = {
  Text: "",
  Link: "",
  Bold: "",
  Italic: ""
}, bs = {
  link: {
    "Add a link": ""
  },
  stub: {
    "The block can not be displayed correctly.": ""
  }
}, ks = {
  delete: {
    Delete: "",
    "Click to delete": ""
  },
  moveUp: {
    "Move up": ""
  },
  moveDown: {
    "Move down": ""
  }
}, pn = {
  ui: ms,
  toolNames: vs,
  tools: bs,
  blockTunes: ks
}, fn = class ye {
  /**
   * Type-safe translation for internal UI texts:
   * Perform translation of the string by namespace and a key
   *
   * @example I18n.ui(I18nInternalNS.ui.blockTunes.toggler, 'Click to tune')
   * @param internalNamespace - path to translated string in dictionary
   * @param dictKey - dictionary key. Better to use default locale original text
   */
  static ui(e, t) {
    return ye._t(e, t);
  }
  /**
   * Translate for external strings that is not presented in default dictionary.
   * For example, for user-specified tool names
   *
   * @param namespace - path to translated string in dictionary
   * @param dictKey - dictionary key. Better to use default locale original text
   */
  static t(e, t) {
    return ye._t(e, t);
  }
  /**
   * Adjust module for using external dictionary
   *
   * @param dictionary - new messages list to override default
   */
  static setDictionary(e) {
    ye.currentDictionary = e;
  }
  /**
   * Perform translation both for internal and external namespaces
   * If there is no translation found, returns passed key as a translated message
   *
   * @param namespace - path to translated string in dictionary
   * @param dictKey - dictionary key. Better to use default locale original text
   */
  static _t(e, t) {
    const r = ye.getNamespace(e);
    return !r || !r[t] ? t : r[t];
  }
  /**
   * Find messages section by namespace path
   *
   * @param namespace - path to section
   */
  static getNamespace(e) {
    return e.split(".").reduce((t, r) => !t || !Object.keys(t).length ? {} : t[r], ye.currentDictionary);
  }
};
fn.currentDictionary = pn;
let W = fn;
class gn extends Error {
}
class Ye {
  constructor() {
    this.subscribers = {};
  }
  /**
   * Subscribe any event on callback
   *
   * @param eventName - event name
   * @param callback - subscriber
   */
  on(e, t) {
    e in this.subscribers || (this.subscribers[e] = []), this.subscribers[e].push(t);
  }
  /**
   * Subscribe any event on callback. Callback will be called once and be removed from subscribers array after call.
   *
   * @param eventName - event name
   * @param callback - subscriber
   */
  once(e, t) {
    e in this.subscribers || (this.subscribers[e] = []);
    const r = (n) => {
      const i = t(n), s = this.subscribers[e].indexOf(r);
      return s !== -1 && this.subscribers[e].splice(s, 1), i;
    };
    this.subscribers[e].push(r);
  }
  /**
   * Emit callbacks with passed data
   *
   * @param eventName - event name
   * @param data - subscribers get this data when they were fired
   */
  emit(e, t) {
    X(this.subscribers) || !this.subscribers[e] || this.subscribers[e].reduce((r, n) => {
      const i = n(r);
      return i !== void 0 ? i : r;
    }, t);
  }
  /**
   * Unsubscribe callback from event
   *
   * @param eventName - event name
   * @param callback - event handler
   */
  off(e, t) {
    if (this.subscribers[e] === void 0) {
      console.warn(`EventDispatcher .off(): there is no subscribers for event "${e.toString()}". Probably, .off() called before .on()`);
      return;
    }
    for (let r = 0; r < this.subscribers[e].length; r++)
      if (this.subscribers[e][r] === t) {
        delete this.subscribers[e][r];
        break;
      }
  }
  /**
   * Destroyer
   * clears subscribers list
   */
  destroy() {
    this.subscribers = {};
  }
}
function te(o) {
  Object.setPrototypeOf(this, {
    /**
     * Block id
     *
     * @returns {string}
     */
    get id() {
      return o.id;
    },
    /**
     * Tool name
     *
     * @returns {string}
     */
    get name() {
      return o.name;
    },
    /**
     * Tool config passed on Editor's initialization
     *
     * @returns {ToolConfig}
     */
    get config() {
      return o.config;
    },
    /**
     * .ce-block element, that wraps plugin contents
     *
     * @returns {HTMLElement}
     */
    get holder() {
      return o.holder;
    },
    /**
     * True if Block content is empty
     *
     * @returns {boolean}
     */
    get isEmpty() {
      return o.isEmpty;
    },
    /**
     * True if Block is selected with Cross-Block selection
     *
     * @returns {boolean}
     */
    get selected() {
      return o.selected;
    },
    /**
     * Set Block's stretch state
     *
     * @param {boolean} state — state to set
     */
    set stretched(e) {
      o.stretched = e;
    },
    /**
     * True if Block is stretched
     *
     * @returns {boolean}
     */
    get stretched() {
      return o.stretched;
    },
    /**
     * True if Block has inputs to be focused
     */
    get focusable() {
      return o.focusable;
    },
    /**
     * Call Tool method with errors handler under-the-hood
     *
     * @param {string} methodName - method to call
     * @param {object} param - object with parameters
     * @returns {unknown}
     */
    call(e, t) {
      return o.call(e, t);
    },
    /**
     * Save Block content
     *
     * @returns {Promise<void|SavedData>}
     */
    save() {
      return o.save();
    },
    /**
     * Validate Block data
     *
     * @param {BlockToolData} data - data to validate
     * @returns {Promise<boolean>}
     */
    validate(e) {
      return o.validate(e);
    },
    /**
     * Allows to say Editor that Block was changed. Used to manually trigger Editor's 'onChange' callback
     * Can be useful for block changes invisible for editor core.
     */
    dispatchChange() {
      o.dispatchChange();
    },
    /**
     * Tool could specify several entries to be displayed at the Toolbox (for example, "Heading 1", "Heading 2", "Heading 3")
     * This method returns the entry that is related to the Block (depended on the Block data)
     */
    getActiveToolboxEntry() {
      return o.getActiveToolboxEntry();
    }
  });
}
let Xe = class {
  constructor() {
    this.allListeners = [];
  }
  /**
   * Assigns event listener on element and returns unique identifier
   *
   * @param {EventTarget} element - DOM element that needs to be listened
   * @param {string} eventType - event type
   * @param {Function} handler - method that will be fired on event
   * @param {boolean|AddEventListenerOptions} options - useCapture or {capture, passive, once}
   */
  on(e, t, r, n = !1) {
    const i = us("l"), s = {
      id: i,
      element: e,
      eventType: t,
      handler: r,
      options: n
    };
    if (!this.findOne(e, t, r))
      return this.allListeners.push(s), e.addEventListener(t, r, n), i;
  }
  /**
   * Removes event listener from element
   *
   * @param {EventTarget} element - DOM element that we removing listener
   * @param {string} eventType - event type
   * @param {Function} handler - remove handler, if element listens several handlers on the same event type
   * @param {boolean|AddEventListenerOptions} options - useCapture or {capture, passive, once}
   */
  off(e, t, r, n) {
    const i = this.findAll(e, t, r);
    i.forEach((s, a) => {
      const l = this.allListeners.indexOf(i[a]);
      l > -1 && (this.allListeners.splice(l, 1), s.element.removeEventListener(s.eventType, s.handler, s.options));
    });
  }
  /**
   * Removes listener by id
   *
   * @param {string} id - listener identifier
   */
  offById(e) {
    const t = this.findById(e);
    t && t.element.removeEventListener(t.eventType, t.handler, t.options);
  }
  /**
   * Finds and returns first listener by passed params
   *
   * @param {EventTarget} element - event target
   * @param {string} [eventType] - event type
   * @param {Function} [handler] - event handler
   * @returns {ListenerData|null}
   */
  findOne(e, t, r) {
    const n = this.findAll(e, t, r);
    return n.length > 0 ? n[0] : null;
  }
  /**
   * Return all stored listeners by passed params
   *
   * @param {EventTarget} element - event target
   * @param {string} eventType - event type
   * @param {Function} handler - event handler
   * @returns {ListenerData[]}
   */
  findAll(e, t, r) {
    let n;
    const i = e ? this.findByEventTarget(e) : [];
    return e && t && r ? n = i.filter((s) => s.eventType === t && s.handler === r) : e && t ? n = i.filter((s) => s.eventType === t) : n = i, n;
  }
  /**
   * Removes all listeners
   */
  removeAll() {
    this.allListeners.map((e) => {
      e.element.removeEventListener(e.eventType, e.handler, e.options);
    }), this.allListeners = [];
  }
  /**
   * Module cleanup on destruction
   */
  destroy() {
    this.removeAll();
  }
  /**
   * Search method: looks for listener by passed element
   *
   * @param {EventTarget} element - searching element
   * @returns {Array} listeners that found on element
   */
  findByEventTarget(e) {
    return this.allListeners.filter((t) => {
      if (t.element === e)
        return t;
    });
  }
  /**
   * Search method: looks for listener by passed event type
   *
   * @param {string} eventType - event type
   * @returns {ListenerData[]} listeners that found on element
   */
  findByType(e) {
    return this.allListeners.filter((t) => {
      if (t.eventType === e)
        return t;
    });
  }
  /**
   * Search method: looks for listener by passed handler
   *
   * @param {Function} handler - event handler
   * @returns {ListenerData[]} listeners that found on element
   */
  findByHandler(e) {
    return this.allListeners.filter((t) => {
      if (t.handler === e)
        return t;
    });
  }
  /**
   * Returns listener data found by id
   *
   * @param {string} id - listener identifier
   * @returns {ListenerData}
   */
  findById(e) {
    return this.allListeners.find((t) => t.id === e);
  }
}, N = class mn {
  /**
   * @class
   * @param options - Module options
   * @param options.config - Module config
   * @param options.eventsDispatcher - Common event bus
   */
  constructor({ config: e, eventsDispatcher: t }) {
    if (this.nodes = {}, this.listeners = new Xe(), this.readOnlyMutableListeners = {
      /**
       * Assigns event listener on DOM element and pushes into special array that might be removed
       *
       * @param {EventTarget} element - DOM Element
       * @param {string} eventType - Event name
       * @param {Function} handler - Event handler
       * @param {boolean|AddEventListenerOptions} options - Listening options
       */
      on: (r, n, i, s = !1) => {
        this.mutableListenerIds.push(
          this.listeners.on(r, n, i, s)
        );
      },
      /**
       * Clears all mutable listeners
       */
      clearAll: () => {
        for (const r of this.mutableListenerIds)
          this.listeners.offById(r);
        this.mutableListenerIds = [];
      }
    }, this.mutableListenerIds = [], new.target === mn)
      throw new TypeError("Constructors for abstract class Module are not allowed.");
    this.config = e, this.eventsDispatcher = t;
  }
  /**
   * Editor modules setter
   *
   * @param {EditorModules} Editor - Editor's Modules
   */
  set state(e) {
    this.Editor = e;
  }
  /**
   * Remove memorized nodes
   */
  removeAllNodes() {
    for (const e in this.nodes) {
      const t = this.nodes[e];
      t instanceof HTMLElement && t.remove();
    }
  }
  /**
   * Returns true if current direction is RTL (Right-To-Left)
   */
  get isRtl() {
    return this.config.i18n.direction === "rtl";
  }
}, L = class se {
  constructor() {
    this.instance = null, this.selection = null, this.savedSelectionRange = null, this.isFakeBackgroundEnabled = !1, this.commandBackground = "backColor";
  }
  /**
   * Editor styles
   *
   * @returns {{editorWrapper: string, editorZone: string}}
   */
  static get CSS() {
    return {
      editorWrapper: "codex-editor",
      editorZone: "codex-editor__redactor"
    };
  }
  /**
   * Returns selected anchor
   * {@link https://developer.mozilla.org/ru/docs/Web/API/Selection/anchorNode}
   *
   * @returns {Node|null}
   */
  static get anchorNode() {
    const e = window.getSelection();
    return e ? e.anchorNode : null;
  }
  /**
   * Returns selected anchor element
   *
   * @returns {Element|null}
   */
  static get anchorElement() {
    const e = window.getSelection();
    if (!e)
      return null;
    const t = e.anchorNode;
    return t ? g.isElement(t) ? t : t.parentElement : null;
  }
  /**
   * Returns selection offset according to the anchor node
   * {@link https://developer.mozilla.org/ru/docs/Web/API/Selection/anchorOffset}
   *
   * @returns {number|null}
   */
  static get anchorOffset() {
    const e = window.getSelection();
    return e ? e.anchorOffset : null;
  }
  /**
   * Is current selection range collapsed
   *
   * @returns {boolean|null}
   */
  static get isCollapsed() {
    const e = window.getSelection();
    return e ? e.isCollapsed : null;
  }
  /**
   * Check current selection if it is at Editor's zone
   *
   * @returns {boolean}
   */
  static get isAtEditor() {
    return this.isSelectionAtEditor(se.get());
  }
  /**
   * Check if passed selection is at Editor's zone
   *
   * @param selection - Selection object to check
   */
  static isSelectionAtEditor(e) {
    if (!e)
      return !1;
    let t = e.anchorNode || e.focusNode;
    t && t.nodeType === Node.TEXT_NODE && (t = t.parentNode);
    let r = null;
    return t && t instanceof Element && (r = t.closest(`.${se.CSS.editorZone}`)), r ? r.nodeType === Node.ELEMENT_NODE : !1;
  }
  /**
   * Check if passed range at Editor zone
   *
   * @param range - range to check
   */
  static isRangeAtEditor(e) {
    if (!e)
      return;
    let t = e.startContainer;
    t && t.nodeType === Node.TEXT_NODE && (t = t.parentNode);
    let r = null;
    return t && t instanceof Element && (r = t.closest(`.${se.CSS.editorZone}`)), r ? r.nodeType === Node.ELEMENT_NODE : !1;
  }
  /**
   * Methods return boolean that true if selection exists on the page
   */
  static get isSelectionExists() {
    return !!se.get().anchorNode;
  }
  /**
   * Return first range
   *
   * @returns {Range|null}
   */
  static get range() {
    return this.getRangeFromSelection(this.get());
  }
  /**
   * Returns range from passed Selection object
   *
   * @param selection - Selection object to get Range from
   */
  static getRangeFromSelection(e) {
    return e && e.rangeCount ? e.getRangeAt(0) : null;
  }
  /**
   * Calculates position and size of selected text
   *
   * @returns {DOMRect | ClientRect}
   */
  static get rect() {
    let e = document.selection, t, r = {
      x: 0,
      y: 0,
      width: 0,
      height: 0
    };
    if (e && e.type !== "Control")
      return e = e, t = e.createRange(), r.x = t.boundingLeft, r.y = t.boundingTop, r.width = t.boundingWidth, r.height = t.boundingHeight, r;
    if (!window.getSelection)
      return j("Method window.getSelection is not supported", "warn"), r;
    if (e = window.getSelection(), e.rangeCount === null || isNaN(e.rangeCount))
      return j("Method SelectionUtils.rangeCount is not supported", "warn"), r;
    if (e.rangeCount === 0)
      return r;
    if (t = e.getRangeAt(0).cloneRange(), t.getBoundingClientRect && (r = t.getBoundingClientRect()), r.x === 0 && r.y === 0) {
      const n = document.createElement("span");
      if (n.getBoundingClientRect) {
        n.appendChild(document.createTextNode("​")), t.insertNode(n), r = n.getBoundingClientRect();
        const i = n.parentNode;
        i.removeChild(n), i.normalize();
      }
    }
    return r;
  }
  /**
   * Returns selected text as String
   *
   * @returns {string}
   */
  static get text() {
    return window.getSelection ? window.getSelection().toString() : "";
  }
  /**
   * Returns window SelectionUtils
   * {@link https://developer.mozilla.org/ru/docs/Web/API/Window/getSelection}
   *
   * @returns {Selection}
   */
  static get() {
    return window.getSelection();
  }
  /**
   * Set focus to contenteditable or native input element
   *
   * @param element - element where to set focus
   * @param offset - offset of cursor
   */
  static setCursor(e, t = 0) {
    const r = document.createRange(), n = window.getSelection();
    return g.isNativeInput(e) ? g.canSetCaret(e) ? (e.focus(), e.selectionStart = e.selectionEnd = t, e.getBoundingClientRect()) : void 0 : (r.setStart(e, t), r.setEnd(e, t), n.removeAllRanges(), n.addRange(r), r.getBoundingClientRect());
  }
  /**
   * Check if current range exists and belongs to container
   *
   * @param container - where range should be
   */
  static isRangeInsideContainer(e) {
    const t = se.range;
    return t === null ? !1 : e.contains(t.startContainer);
  }
  /**
   * Adds fake cursor to the current range
   */
  static addFakeCursor() {
    const e = se.range;
    if (e === null)
      return;
    const t = g.make("span", "codex-editor__fake-cursor");
    t.dataset.mutationFree = "true", e.collapse(), e.insertNode(t);
  }
  /**
   * Check if passed element contains a fake cursor
   *
   * @param el - where to check
   */
  static isFakeCursorInsideContainer(e) {
    return g.find(e, ".codex-editor__fake-cursor") !== null;
  }
  /**
   * Removes fake cursor from a container
   *
   * @param container - container to look for
   */
  static removeFakeCursor(e = document.body) {
    const t = g.find(e, ".codex-editor__fake-cursor");
    t && t.remove();
  }
  /**
   * Removes fake background
   */
  removeFakeBackground() {
    this.isFakeBackgroundEnabled && (document.execCommand(this.commandBackground, !1, "transparent"), this.isFakeBackgroundEnabled = !1);
  }
  /**
   * Sets fake background
   */
  setFakeBackground() {
    document.execCommand(this.commandBackground, !1, "#a8d6ff"), this.isFakeBackgroundEnabled = !0;
  }
  /**
   * Save SelectionUtils's range
   */
  save() {
    this.savedSelectionRange = se.range;
  }
  /**
   * Restore saved SelectionUtils's range
   */
  restore() {
    if (!this.savedSelectionRange)
      return;
    const e = window.getSelection();
    e.removeAllRanges(), e.addRange(this.savedSelectionRange);
  }
  /**
   * Clears saved selection
   */
  clearSaved() {
    this.savedSelectionRange = null;
  }
  /**
   * Collapse current selection
   */
  collapseToEnd() {
    const e = window.getSelection(), t = document.createRange();
    t.selectNodeContents(e.focusNode), t.collapse(!1), e.removeAllRanges(), e.addRange(t);
  }
  /**
   * Looks ahead to find passed tag from current selection
   *
   * @param  {string} tagName       - tag to found
   * @param  {string} [className]   - tag's class name
   * @param  {number} [searchDepth] - count of tags that can be included. For better performance.
   * @returns {HTMLElement|null}
   */
  findParentTag(e, t, r = 10) {
    const n = window.getSelection();
    let i = null;
    return !n || !n.anchorNode || !n.focusNode ? null : ([
      /** the Node in which the selection begins */
      n.anchorNode,
      /** the Node in which the selection ends */
      n.focusNode
    ].forEach((s) => {
      let a = r;
      for (; a > 0 && s.parentNode && !(s.tagName === e && (i = s, t && s.classList && !s.classList.contains(t) && (i = null), i)); )
        s = s.parentNode, a--;
    }), i);
  }
  /**
   * Expands selection range to the passed parent node
   *
   * @param {HTMLElement} element - element which contents should be selected
   */
  expandToTag(e) {
    const t = window.getSelection();
    t.removeAllRanges();
    const r = document.createRange();
    r.selectNodeContents(e), t.addRange(r);
  }
};
function ws(o, e) {
  const { type: t, target: r, addedNodes: n, removedNodes: i } = o;
  return o.type === "attributes" && o.attributeName === "data-empty" ? !1 : !!(e.contains(r) || t === "childList" && (Array.from(n).some((s) => s === e) || Array.from(i).some((s) => s === e)));
}
const Wt = "redactor dom changed", vn = "block changed", bn = "fake cursor is about to be toggled", kn = "fake cursor have been set", We = "editor mobile layout toggled";
function qt(o, e) {
  if (!o.conversionConfig)
    return !1;
  const t = o.conversionConfig[e];
  return R(t) || ie(t);
}
function ct(o, e) {
  return qt(o.tool, e);
}
function wn(o, e) {
  return Object.entries(o).some(([t, r]) => e[t] && ps(e[t], r));
}
async function yn(o, e) {
  const t = (await o.save()).data, r = e.find((n) => n.name === o.name);
  return r !== void 0 && !qt(r, "export") ? [] : e.reduce((n, i) => {
    if (!qt(i, "import") || i.toolbox === void 0)
      return n;
    const s = i.toolbox.filter((a) => {
      if (X(a) || a.icon === void 0)
        return !1;
      if (a.data !== void 0) {
        if (wn(a.data, t))
          return !1;
      } else if (i.name === o.name)
        return !1;
      return !0;
    });
    return n.push({
      ...i,
      toolbox: s
    }), n;
  }, []);
}
function zr(o, e) {
  return o.mergeable ? o.name === e.name ? !0 : ct(e, "export") && ct(o, "import") : !1;
}
function ys(o, e) {
  const t = e == null ? void 0 : e.export;
  return R(t) ? t(o) : ie(t) ? o[t] : (t !== void 0 && j("Conversion «export» property must be a string or function. String means key of saved data object to export. Function should export processed string to export."), "");
}
function Vr(o, e, t) {
  const r = e == null ? void 0 : e.import;
  return R(r) ? r(o, t) : ie(r) ? {
    [r]: o
  } : (r !== void 0 && j("Conversion «import» property must be a string or function. String means key of tool data to import. Function accepts a imported string and return composed tool data."), {});
}
var D = /* @__PURE__ */ ((o) => (o.Default = "default", o.Separator = "separator", o.Html = "html", o))(D || {}), oe = /* @__PURE__ */ ((o) => (o.APPEND_CALLBACK = "appendCallback", o.RENDERED = "rendered", o.MOVED = "moved", o.UPDATED = "updated", o.REMOVED = "removed", o.ON_PASTE = "onPaste", o))(oe || {});
let ne = class ae extends Ye {
  /**
   * @param options - block constructor options
   * @param [options.id] - block's id. Will be generated if omitted.
   * @param options.data - Tool's initial data
   * @param options.tool — block's tool
   * @param options.api - Editor API module for pass it to the Block Tunes
   * @param options.readOnly - Read-Only flag
   * @param [eventBus] - Editor common event bus. Allows to subscribe on some Editor events. Could be omitted when "virtual" Block is created. See BlocksAPI@composeBlockData.
   */
  constructor({
    id: e = ds(),
    data: t,
    tool: r,
    readOnly: n,
    tunesData: i
  }, s) {
    super(), this.cachedInputs = [], this.toolRenderedElement = null, this.tunesInstances = /* @__PURE__ */ new Map(), this.defaultTunesInstances = /* @__PURE__ */ new Map(), this.unavailableTunesData = {}, this.inputIndex = 0, this.editorEventBus = null, this.handleFocus = () => {
      this.dropInputsCache(), this.updateCurrentInput();
    }, this.didMutated = (a = void 0) => {
      const l = a === void 0, c = a instanceof InputEvent;
      !l && !c && this.detectToolRootChange(a);
      let d;
      l || c ? d = !0 : d = !(a.length > 0 && a.every((h) => {
        const { addedNodes: u, removedNodes: f, target: p } = h;
        return [
          ...Array.from(u),
          ...Array.from(f),
          p
        ].some((k) => (g.isElement(k) || (k = k.parentElement), k && k.closest('[data-mutation-free="true"]') !== null));
      })), d && (this.dropInputsCache(), this.updateCurrentInput(), this.toggleInputsEmptyMark(), this.call(
        "updated"
        /* UPDATED */
      ), this.emit("didMutated", this));
    }, this.name = r.name, this.id = e, this.settings = r.settings, this.config = r.settings.config || {}, this.editorEventBus = s || null, this.blockAPI = new te(this), this.tool = r, this.toolInstance = r.create(t, this.blockAPI, n), this.tunes = r.tunes, this.composeTunes(i), this.holder = this.compose(), window.requestIdleCallback(() => {
      this.watchBlockMutations(), this.addInputEvents(), this.toggleInputsEmptyMark();
    });
  }
  /**
   * CSS classes for the Block
   *
   * @returns {{wrapper: string, content: string}}
   */
  static get CSS() {
    return {
      wrapper: "ce-block",
      wrapperStretched: "ce-block--stretched",
      content: "ce-block__content",
      selected: "ce-block--selected",
      dropTarget: "ce-block--drop-target"
    };
  }
  /**
   * Find and return all editable elements (contenteditable and native inputs) in the Tool HTML
   */
  get inputs() {
    if (this.cachedInputs.length !== 0)
      return this.cachedInputs;
    const e = g.findAllInputs(this.holder);
    return this.inputIndex > e.length - 1 && (this.inputIndex = e.length - 1), this.cachedInputs = e, e;
  }
  /**
   * Return current Tool`s input
   * If Block doesn't contain inputs, return undefined
   */
  get currentInput() {
    return this.inputs[this.inputIndex];
  }
  /**
   * Set input index to the passed element
   *
   * @param element - HTML Element to set as current input
   */
  set currentInput(e) {
    const t = this.inputs.findIndex((r) => r === e || r.contains(e));
    t !== -1 && (this.inputIndex = t);
  }
  /**
   * Return first Tool`s input
   * If Block doesn't contain inputs, return undefined
   */
  get firstInput() {
    return this.inputs[0];
  }
  /**
   * Return first Tool`s input
   * If Block doesn't contain inputs, return undefined
   */
  get lastInput() {
    const e = this.inputs;
    return e[e.length - 1];
  }
  /**
   * Return next Tool`s input or undefined if it doesn't exist
   * If Block doesn't contain inputs, return undefined
   */
  get nextInput() {
    return this.inputs[this.inputIndex + 1];
  }
  /**
   * Return previous Tool`s input or undefined if it doesn't exist
   * If Block doesn't contain inputs, return undefined
   */
  get previousInput() {
    return this.inputs[this.inputIndex - 1];
  }
  /**
   * Get Block's JSON data
   *
   * @returns {object}
   */
  get data() {
    return this.save().then((e) => e && !X(e.data) ? e.data : {});
  }
  /**
   * Returns tool's sanitizer config
   *
   * @returns {object}
   */
  get sanitize() {
    return this.tool.sanitizeConfig;
  }
  /**
   * is block mergeable
   * We plugin have merge function then we call it mergeable
   *
   * @returns {boolean}
   */
  get mergeable() {
    return R(this.toolInstance.merge);
  }
  /**
   * If Block contains inputs, it is focusable
   */
  get focusable() {
    return this.inputs.length !== 0;
  }
  /**
   * Check block for emptiness
   *
   * @returns {boolean}
   */
  get isEmpty() {
    const e = g.isEmpty(this.pluginsContent, "/"), t = !this.hasMedia;
    return e && t;
  }
  /**
   * Check if block has a media content such as images, iframe and other
   *
   * @returns {boolean}
   */
  get hasMedia() {
    const e = [
      "img",
      "iframe",
      "video",
      "audio",
      "source",
      "input",
      "textarea",
      "twitterwidget"
    ];
    return !!this.holder.querySelector(e.join(","));
  }
  /**
   * Set selected state
   * We don't need to mark Block as Selected when it is empty
   *
   * @param {boolean} state - 'true' to select, 'false' to remove selection
   */
  set selected(e) {
    var t, r;
    this.holder.classList.toggle(ae.CSS.selected, e);
    const n = e === !0 && L.isRangeInsideContainer(this.holder), i = e === !1 && L.isFakeCursorInsideContainer(this.holder);
    (n || i) && ((t = this.editorEventBus) == null || t.emit(bn, { state: e }), n ? L.addFakeCursor() : L.removeFakeCursor(this.holder), (r = this.editorEventBus) == null || r.emit(kn, { state: e }));
  }
  /**
   * Returns True if it is Selected
   *
   * @returns {boolean}
   */
  get selected() {
    return this.holder.classList.contains(ae.CSS.selected);
  }
  /**
   * Set stretched state
   *
   * @param {boolean} state - 'true' to enable, 'false' to disable stretched state
   */
  set stretched(e) {
    this.holder.classList.toggle(ae.CSS.wrapperStretched, e);
  }
  /**
   * Return Block's stretched state
   *
   * @returns {boolean}
   */
  get stretched() {
    return this.holder.classList.contains(ae.CSS.wrapperStretched);
  }
  /**
   * Toggle drop target state
   *
   * @param {boolean} state - 'true' if block is drop target, false otherwise
   */
  set dropTarget(e) {
    this.holder.classList.toggle(ae.CSS.dropTarget, e);
  }
  /**
   * Returns Plugins content
   *
   * @returns {HTMLElement}
   */
  get pluginsContent() {
    return this.toolRenderedElement;
  }
  /**
   * Calls Tool's method
   *
   * Method checks tool property {MethodName}. Fires method with passes params If it is instance of Function
   *
   * @param {string} methodName - method to call
   * @param {object} params - method argument
   */
  call(e, t) {
    if (R(this.toolInstance[e])) {
      e === "appendCallback" && j(
        "`appendCallback` hook is deprecated and will be removed in the next major release. Use `rendered` hook instead",
        "warn"
      );
      try {
        this.toolInstance[e].call(this.toolInstance, t);
      } catch (r) {
        j(`Error during '${e}' call: ${r.message}`, "error");
      }
    }
  }
  /**
   * Call plugins merge method
   *
   * @param {BlockToolData} data - data to merge
   */
  async mergeWith(e) {
    await this.toolInstance.merge(e);
  }
  /**
   * Extracts data from Block
   * Groups Tool's save processing time
   *
   * @returns {object}
   */
  async save() {
    const e = await this.toolInstance.save(this.pluginsContent), t = this.unavailableTunesData;
    [
      ...this.tunesInstances.entries(),
      ...this.defaultTunesInstances.entries()
    ].forEach(([i, s]) => {
      if (R(s.save))
        try {
          t[i] = s.save();
        } catch (a) {
          j(`Tune ${s.constructor.name} save method throws an Error %o`, "warn", a);
        }
    });
    const r = window.performance.now();
    let n;
    return Promise.resolve(e).then((i) => (n = window.performance.now(), {
      id: this.id,
      tool: this.name,
      data: i,
      tunes: t,
      time: n - r
    })).catch((i) => {
      j(`Saving process for ${this.name} tool failed due to the ${i}`, "log", "red");
    });
  }
  /**
   * Uses Tool's validation method to check the correctness of output data
   * Tool's validation method is optional
   *
   * @description Method returns true|false whether data passed the validation or not
   * @param {BlockToolData} data - data to validate
   * @returns {Promise<boolean>} valid
   */
  async validate(e) {
    let t = !0;
    return this.toolInstance.validate instanceof Function && (t = await this.toolInstance.validate(e)), t;
  }
  /**
   * Returns data to render in Block Tunes menu.
   * Splits block tunes into 2 groups: block specific tunes and common tunes
   */
  getTunes() {
    const e = [], t = [], r = typeof this.toolInstance.renderSettings == "function" ? this.toolInstance.renderSettings() : [];
    return g.isElement(r) ? e.push({
      type: D.Html,
      element: r
    }) : Array.isArray(r) ? e.push(...r) : e.push(r), [
      ...this.tunesInstances.values(),
      ...this.defaultTunesInstances.values()
    ].map((n) => n.render()).forEach((n) => {
      g.isElement(n) ? t.push({
        type: D.Html,
        element: n
      }) : Array.isArray(n) ? t.push(...n) : t.push(n);
    }), {
      toolTunes: e,
      commonTunes: t
    };
  }
  /**
   * Update current input index with selection anchor node
   */
  updateCurrentInput() {
    this.currentInput = g.isNativeInput(document.activeElement) || !L.anchorNode ? document.activeElement : L.anchorNode;
  }
  /**
   * Allows to say Editor that Block was changed. Used to manually trigger Editor's 'onChange' callback
   * Can be useful for block changes invisible for editor core.
   */
  dispatchChange() {
    this.didMutated();
  }
  /**
   * Call Tool instance destroy method
   */
  destroy() {
    this.unwatchBlockMutations(), this.removeInputEvents(), super.destroy(), R(this.toolInstance.destroy) && this.toolInstance.destroy();
  }
  /**
   * Tool could specify several entries to be displayed at the Toolbox (for example, "Heading 1", "Heading 2", "Heading 3")
   * This method returns the entry that is related to the Block (depended on the Block data)
   */
  async getActiveToolboxEntry() {
    const e = this.tool.toolbox;
    if (e.length === 1)
      return Promise.resolve(this.tool.toolbox[0]);
    const t = await this.data, r = e;
    return r == null ? void 0 : r.find((n) => wn(n.data, t));
  }
  /**
   * Exports Block data as string using conversion config
   */
  async exportDataAsString() {
    const e = await this.data;
    return ys(e, this.tool.conversionConfig);
  }
  /**
   * Make default Block wrappers and put Tool`s content there
   *
   * @returns {HTMLDivElement}
   */
  compose() {
    const e = g.make("div", ae.CSS.wrapper), t = g.make("div", ae.CSS.content), r = this.toolInstance.render();
    e.dataset.id = this.id, this.toolRenderedElement = r, t.appendChild(this.toolRenderedElement);
    let n = t;
    return [...this.tunesInstances.values(), ...this.defaultTunesInstances.values()].forEach((i) => {
      if (R(i.wrap))
        try {
          n = i.wrap(n);
        } catch (s) {
          j(`Tune ${i.constructor.name} wrap method throws an Error %o`, "warn", s);
        }
    }), e.appendChild(n), e;
  }
  /**
   * Instantiate Block Tunes
   *
   * @param tunesData - current Block tunes data
   * @private
   */
  composeTunes(e) {
    Array.from(this.tunes.values()).forEach((t) => {
      (t.isInternal ? this.defaultTunesInstances : this.tunesInstances).set(t.name, t.create(e[t.name], this.blockAPI));
    }), Object.entries(e).forEach(([t, r]) => {
      this.tunesInstances.has(t) || (this.unavailableTunesData[t] = r);
    });
  }
  /**
   * Adds focus event listeners to all inputs and contenteditable
   */
  addInputEvents() {
    this.inputs.forEach((e) => {
      e.addEventListener("focus", this.handleFocus), g.isNativeInput(e) && e.addEventListener("input", this.didMutated);
    });
  }
  /**
   * removes focus event listeners from all inputs and contenteditable
   */
  removeInputEvents() {
    this.inputs.forEach((e) => {
      e.removeEventListener("focus", this.handleFocus), g.isNativeInput(e) && e.removeEventListener("input", this.didMutated);
    });
  }
  /**
   * Listen common editor Dom Changed event and detect mutations related to the  Block
   */
  watchBlockMutations() {
    var e;
    this.redactorDomChangedCallback = (t) => {
      const { mutations: r } = t;
      r.some((n) => ws(n, this.toolRenderedElement)) && this.didMutated(r);
    }, (e = this.editorEventBus) == null || e.on(Wt, this.redactorDomChangedCallback);
  }
  /**
   * Remove redactor dom change event listener
   */
  unwatchBlockMutations() {
    var e;
    (e = this.editorEventBus) == null || e.off(Wt, this.redactorDomChangedCallback);
  }
  /**
   * Sometimes Tool can replace own main element, for example H2 -> H4 or UL -> OL
   * We need to detect such changes and update a link to tools main element with the new one
   *
   * @param mutations - records of block content mutations
   */
  detectToolRootChange(e) {
    e.forEach((t) => {
      if (Array.from(t.removedNodes).includes(this.toolRenderedElement)) {
        const r = t.addedNodes[t.addedNodes.length - 1];
        this.toolRenderedElement = r;
      }
    });
  }
  /**
   * Clears inputs cached value
   */
  dropInputsCache() {
    this.cachedInputs = [];
  }
  /**
   * Mark inputs with 'data-empty' attribute with the empty state
   */
  toggleInputsEmptyMark() {
    this.inputs.forEach(un);
  }
};
class xs extends N {
  constructor() {
    super(...arguments), this.insert = (e = this.config.defaultBlock, t = {}, r = {}, n, i, s, a) => {
      const l = this.Editor.BlockManager.insert({
        id: a,
        tool: e,
        data: t,
        index: n,
        needToFocus: i,
        replace: s
      });
      return new te(l);
    }, this.composeBlockData = async (e) => {
      const t = this.Editor.Tools.blockTools.get(e);
      return new ne({
        tool: t,
        api: this.Editor.API,
        readOnly: !0,
        data: {},
        tunesData: {}
      }).data;
    }, this.update = async (e, t, r) => {
      const { BlockManager: n } = this.Editor, i = n.getBlockById(e);
      if (i === void 0)
        throw new Error(`Block with id "${e}" not found`);
      const s = await n.update(i, t, r);
      return new te(s);
    }, this.convert = async (e, t, r) => {
      var n, i;
      const { BlockManager: s, Tools: a } = this.Editor, l = s.getBlockById(e);
      if (!l)
        throw new Error(`Block with id "${e}" not found`);
      const c = a.blockTools.get(l.name), d = a.blockTools.get(t);
      if (!d)
        throw new Error(`Block Tool with type "${t}" not found`);
      const h = ((n = c == null ? void 0 : c.conversionConfig) == null ? void 0 : n.export) !== void 0, u = ((i = d.conversionConfig) == null ? void 0 : i.import) !== void 0;
      if (h && u) {
        const f = await s.convert(l, t, r);
        return new te(f);
      } else {
        const f = [
          h ? !1 : lt(l.name),
          u ? !1 : lt(t)
        ].filter(Boolean).join(" and ");
        throw new Error(`Conversion from "${l.name}" to "${t}" is not possible. ${f} tool(s) should provide a "conversionConfig"`);
      }
    }, this.insertMany = (e, t = this.Editor.BlockManager.blocks.length - 1) => {
      this.validateIndex(t);
      const r = e.map(({ id: n, type: i, data: s }) => this.Editor.BlockManager.composeBlock({
        id: n,
        tool: i || this.config.defaultBlock,
        data: s
      }));
      return this.Editor.BlockManager.insertMany(r, t), r.map((n) => new te(n));
    };
  }
  /**
   * Available methods
   *
   * @returns {Blocks}
   */
  get methods() {
    return {
      clear: () => this.clear(),
      render: (e) => this.render(e),
      renderFromHTML: (e) => this.renderFromHTML(e),
      delete: (e) => this.delete(e),
      swap: (e, t) => this.swap(e, t),
      move: (e, t) => this.move(e, t),
      getBlockByIndex: (e) => this.getBlockByIndex(e),
      getById: (e) => this.getById(e),
      getCurrentBlockIndex: () => this.getCurrentBlockIndex(),
      getBlockIndex: (e) => this.getBlockIndex(e),
      getBlocksCount: () => this.getBlocksCount(),
      getBlockByElement: (e) => this.getBlockByElement(e),
      stretchBlock: (e, t = !0) => this.stretchBlock(e, t),
      insertNewBlock: () => this.insertNewBlock(),
      insert: this.insert,
      insertMany: this.insertMany,
      update: this.update,
      composeBlockData: this.composeBlockData,
      convert: this.convert
    };
  }
  /**
   * Returns Blocks count
   *
   * @returns {number}
   */
  getBlocksCount() {
    return this.Editor.BlockManager.blocks.length;
  }
  /**
   * Returns current block index
   *
   * @returns {number}
   */
  getCurrentBlockIndex() {
    return this.Editor.BlockManager.currentBlockIndex;
  }
  /**
   * Returns the index of Block by id;
   *
   * @param id - block id
   */
  getBlockIndex(e) {
    const t = this.Editor.BlockManager.getBlockById(e);
    if (!t) {
      Y("There is no block with id `" + e + "`", "warn");
      return;
    }
    return this.Editor.BlockManager.getBlockIndex(t);
  }
  /**
   * Returns BlockAPI object by Block index
   *
   * @param {number} index - index to get
   */
  getBlockByIndex(e) {
    const t = this.Editor.BlockManager.getBlockByIndex(e);
    if (t === void 0) {
      Y("There is no block at index `" + e + "`", "warn");
      return;
    }
    return new te(t);
  }
  /**
   * Returns BlockAPI object by Block id
   *
   * @param id - id of block to get
   */
  getById(e) {
    const t = this.Editor.BlockManager.getBlockById(e);
    return t === void 0 ? (Y("There is no block with id `" + e + "`", "warn"), null) : new te(t);
  }
  /**
   * Get Block API object by any child html element
   *
   * @param element - html element to get Block by
   */
  getBlockByElement(e) {
    const t = this.Editor.BlockManager.getBlock(e);
    if (t === void 0) {
      Y("There is no block corresponding to element `" + e + "`", "warn");
      return;
    }
    return new te(t);
  }
  /**
   * Call Block Manager method that swap Blocks
   *
   * @param {number} fromIndex - position of first Block
   * @param {number} toIndex - position of second Block
   * @deprecated — use 'move' instead
   */
  swap(e, t) {
    j(
      "`blocks.swap()` method is deprecated and will be removed in the next major release. Use `block.move()` method instead",
      "info"
    ), this.Editor.BlockManager.swap(e, t);
  }
  /**
   * Move block from one index to another
   *
   * @param {number} toIndex - index to move to
   * @param {number} fromIndex - index to move from
   */
  move(e, t) {
    this.Editor.BlockManager.move(e, t);
  }
  /**
   * Deletes Block
   *
   * @param {number} blockIndex - index of Block to delete
   */
  delete(e = this.Editor.BlockManager.currentBlockIndex) {
    try {
      const t = this.Editor.BlockManager.getBlockByIndex(e);
      this.Editor.BlockManager.removeBlock(t);
    } catch (t) {
      Y(t, "warn");
      return;
    }
    this.Editor.BlockManager.blocks.length === 0 && this.Editor.BlockManager.insert(), this.Editor.BlockManager.currentBlock && this.Editor.Caret.setToBlock(this.Editor.BlockManager.currentBlock, this.Editor.Caret.positions.END), this.Editor.Toolbar.close();
  }
  /**
   * Clear Editor's area
   */
  async clear() {
    await this.Editor.BlockManager.clear(!0), this.Editor.InlineToolbar.close();
  }
  /**
   * Fills Editor with Blocks data
   *
   * @param {OutputData} data — Saved Editor data
   */
  async render(e) {
    if (e === void 0 || e.blocks === void 0)
      throw new Error("Incorrect data passed to the render() method");
    this.Editor.ModificationsObserver.disable(), await this.Editor.BlockManager.clear(), await this.Editor.Renderer.render(e.blocks), this.Editor.ModificationsObserver.enable();
  }
  /**
   * Render passed HTML string
   *
   * @param {string} data - HTML string to render
   * @returns {Promise<void>}
   */
  async renderFromHTML(e) {
    return await this.Editor.BlockManager.clear(), this.Editor.Paste.processText(e, !0);
  }
  /**
   * Stretch Block's content
   *
   * @param {number} index - index of Block to stretch
   * @param {boolean} status - true to enable, false to disable
   * @deprecated Use BlockAPI interface to stretch Blocks
   */
  stretchBlock(e, t = !0) {
    zt(
      !0,
      "blocks.stretchBlock()",
      "BlockAPI"
    );
    const r = this.Editor.BlockManager.getBlockByIndex(e);
    r && (r.stretched = t);
  }
  /**
   * Insert new Block
   * After set caret to this Block
   *
   * @todo remove in 3.0.0
   * @deprecated with insert() method
   */
  insertNewBlock() {
    j("Method blocks.insertNewBlock() is deprecated and it will be removed in the next major release. Use blocks.insert() instead.", "warn"), this.insert();
  }
  /**
   * Validated block index and throws an error if it's invalid
   *
   * @param index - index to validate
   */
  validateIndex(e) {
    if (typeof e != "number")
      throw new Error("Index should be a number");
    if (e < 0)
      throw new Error("Index should be greater than or equal to 0");
    if (e === null)
      throw new Error("Index should be greater than or equal to 0");
  }
}
function Cs(o, e) {
  return typeof o == "number" ? e.BlockManager.getBlockByIndex(o) : typeof o == "string" ? e.BlockManager.getBlockById(o) : e.BlockManager.getBlockById(o.id);
}
class Es extends N {
  constructor() {
    super(...arguments), this.setToFirstBlock = (e = this.Editor.Caret.positions.DEFAULT, t = 0) => this.Editor.BlockManager.firstBlock ? (this.Editor.Caret.setToBlock(this.Editor.BlockManager.firstBlock, e, t), !0) : !1, this.setToLastBlock = (e = this.Editor.Caret.positions.DEFAULT, t = 0) => this.Editor.BlockManager.lastBlock ? (this.Editor.Caret.setToBlock(this.Editor.BlockManager.lastBlock, e, t), !0) : !1, this.setToPreviousBlock = (e = this.Editor.Caret.positions.DEFAULT, t = 0) => this.Editor.BlockManager.previousBlock ? (this.Editor.Caret.setToBlock(this.Editor.BlockManager.previousBlock, e, t), !0) : !1, this.setToNextBlock = (e = this.Editor.Caret.positions.DEFAULT, t = 0) => this.Editor.BlockManager.nextBlock ? (this.Editor.Caret.setToBlock(this.Editor.BlockManager.nextBlock, e, t), !0) : !1, this.setToBlock = (e, t = this.Editor.Caret.positions.DEFAULT, r = 0) => {
      const n = Cs(e, this.Editor);
      return n === void 0 ? !1 : (this.Editor.Caret.setToBlock(n, t, r), !0);
    }, this.focus = (e = !1) => e ? this.setToLastBlock(this.Editor.Caret.positions.END) : this.setToFirstBlock(this.Editor.Caret.positions.START);
  }
  /**
   * Available methods
   *
   * @returns {Caret}
   */
  get methods() {
    return {
      setToFirstBlock: this.setToFirstBlock,
      setToLastBlock: this.setToLastBlock,
      setToPreviousBlock: this.setToPreviousBlock,
      setToNextBlock: this.setToNextBlock,
      setToBlock: this.setToBlock,
      focus: this.focus
    };
  }
}
class Ts extends N {
  /**
   * Available methods
   *
   * @returns {Events}
   */
  get methods() {
    return {
      emit: (e, t) => this.emit(e, t),
      off: (e, t) => this.off(e, t),
      on: (e, t) => this.on(e, t)
    };
  }
  /**
   * Subscribe on Events
   *
   * @param {string} eventName - event name to subscribe
   * @param {Function} callback - event handler
   */
  on(e, t) {
    this.eventsDispatcher.on(e, t);
  }
  /**
   * Emit event with data
   *
   * @param {string} eventName - event to emit
   * @param {object} data - event's data
   */
  emit(e, t) {
    this.eventsDispatcher.emit(e, t);
  }
  /**
   * Unsubscribe from Event
   *
   * @param {string} eventName - event to unsubscribe
   * @param {Function} callback - event handler
   */
  off(e, t) {
    this.eventsDispatcher.off(e, t);
  }
}
let Ss = class xn extends N {
  /**
   * Return namespace section for tool or block tune
   *
   * @param toolName - tool name
   * @param isTune - is tool a block tune
   */
  static getNamespace(e, t) {
    return t ? `blockTunes.${e}` : `tools.${e}`;
  }
  /**
   * Return I18n API methods with global dictionary access
   */
  get methods() {
    return {
      t: () => {
        Y("I18n.t() method can be accessed only from Tools", "warn");
      }
    };
  }
  /**
   * Return I18n API methods with tool namespaced dictionary
   *
   * @param toolName - tool name
   * @param isTune - is tool a block tune
   */
  getMethodsForTool(e, t) {
    return Object.assign(
      this.methods,
      {
        t: (r) => W.t(xn.getNamespace(e, t), r)
      }
    );
  }
};
class Bs extends N {
  /**
   * Editor.js Core API modules
   */
  get methods() {
    return {
      blocks: this.Editor.BlocksAPI.methods,
      caret: this.Editor.CaretAPI.methods,
      tools: this.Editor.ToolsAPI.methods,
      events: this.Editor.EventsAPI.methods,
      listeners: this.Editor.ListenersAPI.methods,
      notifier: this.Editor.NotifierAPI.methods,
      sanitizer: this.Editor.SanitizerAPI.methods,
      saver: this.Editor.SaverAPI.methods,
      selection: this.Editor.SelectionAPI.methods,
      styles: this.Editor.StylesAPI.classes,
      toolbar: this.Editor.ToolbarAPI.methods,
      inlineToolbar: this.Editor.InlineToolbarAPI.methods,
      tooltip: this.Editor.TooltipAPI.methods,
      i18n: this.Editor.I18nAPI.methods,
      readOnly: this.Editor.ReadOnlyAPI.methods,
      ui: this.Editor.UiAPI.methods
    };
  }
  /**
   * Returns Editor.js Core API methods for passed tool
   *
   * @param toolName - tool name
   * @param isTune - is tool a block tune
   */
  getMethodsForTool(e, t) {
    return Object.assign(
      this.methods,
      {
        i18n: this.Editor.I18nAPI.getMethodsForTool(e, t)
      }
    );
  }
}
class Ms extends N {
  /**
   * Available methods
   *
   * @returns {InlineToolbar}
   */
  get methods() {
    return {
      close: () => this.close(),
      open: () => this.open()
    };
  }
  /**
   * Open Inline Toolbar
   */
  open() {
    this.Editor.InlineToolbar.tryToShow();
  }
  /**
   * Close Inline Toolbar
   */
  close() {
    this.Editor.InlineToolbar.close();
  }
}
class _s extends N {
  /**
   * Available methods
   *
   * @returns {Listeners}
   */
  get methods() {
    return {
      on: (e, t, r, n) => this.on(e, t, r, n),
      off: (e, t, r, n) => this.off(e, t, r, n),
      offById: (e) => this.offById(e)
    };
  }
  /**
   * Ads a DOM event listener. Return it's id.
   *
   * @param {HTMLElement} element - Element to set handler to
   * @param {string} eventType - event type
   * @param {() => void} handler - event handler
   * @param {boolean} useCapture - capture event or not
   */
  on(e, t, r, n) {
    return this.listeners.on(e, t, r, n);
  }
  /**
   * Removes DOM listener from element
   *
   * @param {Element} element - Element to remove handler from
   * @param eventType - event type
   * @param handler - event handler
   * @param {boolean} useCapture - capture event or not
   */
  off(e, t, r, n) {
    this.listeners.off(e, t, r, n);
  }
  /**
   * Removes DOM listener by the listener id
   *
   * @param id - id of the listener to remove
   */
  offById(e) {
    this.listeners.offById(e);
  }
}
var Cn = { exports: {} };
(function(o, e) {
  (function(t, r) {
    o.exports = r();
  })(window, function() {
    return (function(t) {
      var r = {};
      function n(i) {
        if (r[i])
          return r[i].exports;
        var s = r[i] = { i, l: !1, exports: {} };
        return t[i].call(s.exports, s, s.exports, n), s.l = !0, s.exports;
      }
      return n.m = t, n.c = r, n.d = function(i, s, a) {
        n.o(i, s) || Object.defineProperty(i, s, { enumerable: !0, get: a });
      }, n.r = function(i) {
        typeof Symbol < "u" && Symbol.toStringTag && Object.defineProperty(i, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(i, "__esModule", { value: !0 });
      }, n.t = function(i, s) {
        if (1 & s && (i = n(i)), 8 & s || 4 & s && typeof i == "object" && i && i.__esModule)
          return i;
        var a = /* @__PURE__ */ Object.create(null);
        if (n.r(a), Object.defineProperty(a, "default", { enumerable: !0, value: i }), 2 & s && typeof i != "string")
          for (var l in i)
            n.d(a, l, (function(c) {
              return i[c];
            }).bind(null, l));
        return a;
      }, n.n = function(i) {
        var s = i && i.__esModule ? function() {
          return i.default;
        } : function() {
          return i;
        };
        return n.d(s, "a", s), s;
      }, n.o = function(i, s) {
        return Object.prototype.hasOwnProperty.call(i, s);
      }, n.p = "/", n(n.s = 0);
    })([function(t, r, n) {
      n(1), /*!
      * Codex JavaScript Notification module
      * https://github.com/codex-team/js-notifier
      */
      t.exports = (function() {
        var i = n(6), s = "cdx-notify--bounce-in", a = null;
        return { show: function(l) {
          if (l.message) {
            (function() {
              if (a)
                return !0;
              a = i.getWrapper(), document.body.appendChild(a);
            })();
            var c = null, d = l.time || 8e3;
            switch (l.type) {
              case "confirm":
                c = i.confirm(l);
                break;
              case "prompt":
                c = i.prompt(l);
                break;
              default:
                c = i.alert(l), window.setTimeout(function() {
                  c.remove();
                }, d);
            }
            a.appendChild(c), c.classList.add(s);
          }
        } };
      })();
    }, function(t, r, n) {
      var i = n(2);
      typeof i == "string" && (i = [[t.i, i, ""]]);
      var s = { hmr: !0, transform: void 0, insertInto: void 0 };
      n(4)(i, s), i.locals && (t.exports = i.locals);
    }, function(t, r, n) {
      (t.exports = n(3)(!1)).push([t.i, `.cdx-notify--error{background:#fffbfb!important}.cdx-notify--error::before{background:#fb5d5d!important}.cdx-notify__input{max-width:130px;padding:5px 10px;background:#f7f7f7;border:0;border-radius:3px;font-size:13px;color:#656b7c;outline:0}.cdx-notify__input:-ms-input-placeholder{color:#656b7c}.cdx-notify__input::placeholder{color:#656b7c}.cdx-notify__input:focus:-ms-input-placeholder{color:rgba(101,107,124,.3)}.cdx-notify__input:focus::placeholder{color:rgba(101,107,124,.3)}.cdx-notify__button{border:none;border-radius:3px;font-size:13px;padding:5px 10px;cursor:pointer}.cdx-notify__button:last-child{margin-left:10px}.cdx-notify__button--cancel{background:#f2f5f7;box-shadow:0 2px 1px 0 rgba(16,19,29,0);color:#656b7c}.cdx-notify__button--cancel:hover{background:#eee}.cdx-notify__button--confirm{background:#34c992;box-shadow:0 1px 1px 0 rgba(18,49,35,.05);color:#fff}.cdx-notify__button--confirm:hover{background:#33b082}.cdx-notify__btns-wrapper{display:-ms-flexbox;display:flex;-ms-flex-flow:row nowrap;flex-flow:row nowrap;margin-top:5px}.cdx-notify__cross{position:absolute;top:5px;right:5px;width:10px;height:10px;padding:5px;opacity:.54;cursor:pointer}.cdx-notify__cross::after,.cdx-notify__cross::before{content:'';position:absolute;left:9px;top:5px;height:12px;width:2px;background:#575d67}.cdx-notify__cross::before{transform:rotate(-45deg)}.cdx-notify__cross::after{transform:rotate(45deg)}.cdx-notify__cross:hover{opacity:1}.cdx-notifies{position:fixed;z-index:2;bottom:20px;left:20px;font-family:-apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,Oxygen,Ubuntu,Cantarell,"Fira Sans","Droid Sans","Helvetica Neue",sans-serif}.cdx-notify{position:relative;width:220px;margin-top:15px;padding:13px 16px;background:#fff;box-shadow:0 11px 17px 0 rgba(23,32,61,.13);border-radius:5px;font-size:14px;line-height:1.4em;word-wrap:break-word}.cdx-notify::before{content:'';position:absolute;display:block;top:0;left:0;width:3px;height:calc(100% - 6px);margin:3px;border-radius:5px;background:0 0}@keyframes bounceIn{0%{opacity:0;transform:scale(.3)}50%{opacity:1;transform:scale(1.05)}70%{transform:scale(.9)}100%{transform:scale(1)}}.cdx-notify--bounce-in{animation-name:bounceIn;animation-duration:.6s;animation-iteration-count:1}.cdx-notify--success{background:#fafffe!important}.cdx-notify--success::before{background:#41ffb1!important}`, ""]);
    }, function(t, r) {
      t.exports = function(n) {
        var i = [];
        return i.toString = function() {
          return this.map(function(s) {
            var a = (function(l, c) {
              var d = l[1] || "", h = l[3];
              if (!h)
                return d;
              if (c && typeof btoa == "function") {
                var u = (p = h, "/*# sourceMappingURL=data:application/json;charset=utf-8;base64," + btoa(unescape(encodeURIComponent(JSON.stringify(p)))) + " */"), f = h.sources.map(function(k) {
                  return "/*# sourceURL=" + h.sourceRoot + k + " */";
                });
                return [d].concat(f).concat([u]).join(`
`);
              }
              var p;
              return [d].join(`
`);
            })(s, n);
            return s[2] ? "@media " + s[2] + "{" + a + "}" : a;
          }).join("");
        }, i.i = function(s, a) {
          typeof s == "string" && (s = [[null, s, ""]]);
          for (var l = {}, c = 0; c < this.length; c++) {
            var d = this[c][0];
            typeof d == "number" && (l[d] = !0);
          }
          for (c = 0; c < s.length; c++) {
            var h = s[c];
            typeof h[0] == "number" && l[h[0]] || (a && !h[2] ? h[2] = a : a && (h[2] = "(" + h[2] + ") and (" + a + ")"), i.push(h));
          }
        }, i;
      };
    }, function(t, r, n) {
      var i, s, a = {}, l = (i = function() {
        return window && document && document.all && !window.atob;
      }, function() {
        return s === void 0 && (s = i.apply(this, arguments)), s;
      }), c = /* @__PURE__ */ (function(w) {
        var b = {};
        return function(E) {
          if (typeof E == "function")
            return E();
          if (b[E] === void 0) {
            var y = (function(B) {
              return document.querySelector(B);
            }).call(this, E);
            if (window.HTMLIFrameElement && y instanceof window.HTMLIFrameElement)
              try {
                y = y.contentDocument.head;
              } catch {
                y = null;
              }
            b[E] = y;
          }
          return b[E];
        };
      })(), d = null, h = 0, u = [], f = n(5);
      function p(w, b) {
        for (var E = 0; E < w.length; E++) {
          var y = w[E], B = a[y.id];
          if (B) {
            B.refs++;
            for (var M = 0; M < B.parts.length; M++)
              B.parts[M](y.parts[M]);
            for (; M < y.parts.length; M++)
              B.parts.push(S(y.parts[M], b));
          } else {
            var P = [];
            for (M = 0; M < y.parts.length; M++)
              P.push(S(y.parts[M], b));
            a[y.id] = { id: y.id, refs: 1, parts: P };
          }
        }
      }
      function k(w, b) {
        for (var E = [], y = {}, B = 0; B < w.length; B++) {
          var M = w[B], P = b.base ? M[0] + b.base : M[0], O = { css: M[1], media: M[2], sourceMap: M[3] };
          y[P] ? y[P].parts.push(O) : E.push(y[P] = { id: P, parts: [O] });
        }
        return E;
      }
      function T(w, b) {
        var E = c(w.insertInto);
        if (!E)
          throw new Error("Couldn't find a style target. This probably means that the value for the 'insertInto' parameter is invalid.");
        var y = u[u.length - 1];
        if (w.insertAt === "top")
          y ? y.nextSibling ? E.insertBefore(b, y.nextSibling) : E.appendChild(b) : E.insertBefore(b, E.firstChild), u.push(b);
        else if (w.insertAt === "bottom")
          E.appendChild(b);
        else {
          if (typeof w.insertAt != "object" || !w.insertAt.before)
            throw new Error(`[Style Loader]

 Invalid value for parameter 'insertAt' ('options.insertAt') found.
 Must be 'top', 'bottom', or Object.
 (https://github.com/webpack-contrib/style-loader#insertat)
`);
          var B = c(w.insertInto + " " + w.insertAt.before);
          E.insertBefore(b, B);
        }
      }
      function v(w) {
        if (w.parentNode === null)
          return !1;
        w.parentNode.removeChild(w);
        var b = u.indexOf(w);
        b >= 0 && u.splice(b, 1);
      }
      function m(w) {
        var b = document.createElement("style");
        return w.attrs.type === void 0 && (w.attrs.type = "text/css"), C(b, w.attrs), T(w, b), b;
      }
      function C(w, b) {
        Object.keys(b).forEach(function(E) {
          w.setAttribute(E, b[E]);
        });
      }
      function S(w, b) {
        var E, y, B, M;
        if (b.transform && w.css) {
          if (!(M = b.transform(w.css)))
            return function() {
            };
          w.css = M;
        }
        if (b.singleton) {
          var P = h++;
          E = d || (d = m(b)), y = I.bind(null, E, P, !1), B = I.bind(null, E, P, !0);
        } else
          w.sourceMap && typeof URL == "function" && typeof URL.createObjectURL == "function" && typeof URL.revokeObjectURL == "function" && typeof Blob == "function" && typeof btoa == "function" ? (E = (function(O) {
            var $ = document.createElement("link");
            return O.attrs.type === void 0 && (O.attrs.type = "text/css"), O.attrs.rel = "stylesheet", C($, O.attrs), T(O, $), $;
          })(b), y = (function(O, $, ue) {
            var ee = ue.css, Le = ue.sourceMap, Ui = $.convertToAbsoluteUrls === void 0 && Le;
            ($.convertToAbsoluteUrls || Ui) && (ee = f(ee)), Le && (ee += `
/*# sourceMappingURL=data:application/json;base64,` + btoa(unescape(encodeURIComponent(JSON.stringify(Le)))) + " */");
            var zi = new Blob([ee], { type: "text/css" }), Rr = O.href;
            O.href = URL.createObjectURL(zi), Rr && URL.revokeObjectURL(Rr);
          }).bind(null, E, b), B = function() {
            v(E), E.href && URL.revokeObjectURL(E.href);
          }) : (E = m(b), y = (function(O, $) {
            var ue = $.css, ee = $.media;
            if (ee && O.setAttribute("media", ee), O.styleSheet)
              O.styleSheet.cssText = ue;
            else {
              for (; O.firstChild; )
                O.removeChild(O.firstChild);
              O.appendChild(document.createTextNode(ue));
            }
          }).bind(null, E), B = function() {
            v(E);
          });
        return y(w), function(O) {
          if (O) {
            if (O.css === w.css && O.media === w.media && O.sourceMap === w.sourceMap)
              return;
            y(w = O);
          } else
            B();
        };
      }
      t.exports = function(w, b) {
        if (typeof DEBUG < "u" && DEBUG && typeof document != "object")
          throw new Error("The style-loader cannot be used in a non-browser environment");
        (b = b || {}).attrs = typeof b.attrs == "object" ? b.attrs : {}, b.singleton || typeof b.singleton == "boolean" || (b.singleton = l()), b.insertInto || (b.insertInto = "head"), b.insertAt || (b.insertAt = "bottom");
        var E = k(w, b);
        return p(E, b), function(y) {
          for (var B = [], M = 0; M < E.length; M++) {
            var P = E[M];
            (O = a[P.id]).refs--, B.push(O);
          }
          for (y && p(k(y, b), b), M = 0; M < B.length; M++) {
            var O;
            if ((O = B[M]).refs === 0) {
              for (var $ = 0; $ < O.parts.length; $++)
                O.parts[$]();
              delete a[O.id];
            }
          }
        };
      };
      var _, x = (_ = [], function(w, b) {
        return _[w] = b, _.filter(Boolean).join(`
`);
      });
      function I(w, b, E, y) {
        var B = E ? "" : y.css;
        if (w.styleSheet)
          w.styleSheet.cssText = x(b, B);
        else {
          var M = document.createTextNode(B), P = w.childNodes;
          P[b] && w.removeChild(P[b]), P.length ? w.insertBefore(M, P[b]) : w.appendChild(M);
        }
      }
    }, function(t, r) {
      t.exports = function(n) {
        var i = typeof window < "u" && window.location;
        if (!i)
          throw new Error("fixUrls requires window.location");
        if (!n || typeof n != "string")
          return n;
        var s = i.protocol + "//" + i.host, a = s + i.pathname.replace(/\/[^\/]*$/, "/");
        return n.replace(/url\s*\(((?:[^)(]|\((?:[^)(]+|\([^)(]*\))*\))*)\)/gi, function(l, c) {
          var d, h = c.trim().replace(/^"(.*)"$/, function(u, f) {
            return f;
          }).replace(/^'(.*)'$/, function(u, f) {
            return f;
          });
          return /^(#|data:|http:\/\/|https:\/\/|file:\/\/\/|\s*$)/i.test(h) ? l : (d = h.indexOf("//") === 0 ? h : h.indexOf("/") === 0 ? s + h : a + h.replace(/^\.\//, ""), "url(" + JSON.stringify(d) + ")");
        });
      };
    }, function(t, r, n) {
      var i, s, a, l, c, d, h, u, f;
      t.exports = (i = "cdx-notifies", s = "cdx-notify", a = "cdx-notify__cross", l = "cdx-notify__button--confirm", c = "cdx-notify__button--cancel", d = "cdx-notify__input", h = "cdx-notify__button", u = "cdx-notify__btns-wrapper", { alert: f = function(p) {
        var k = document.createElement("DIV"), T = document.createElement("DIV"), v = p.message, m = p.style;
        return k.classList.add(s), m && k.classList.add(s + "--" + m), k.innerHTML = v, T.classList.add(a), T.addEventListener("click", k.remove.bind(k)), k.appendChild(T), k;
      }, confirm: function(p) {
        var k = f(p), T = document.createElement("div"), v = document.createElement("button"), m = document.createElement("button"), C = k.querySelector("." + a), S = p.cancelHandler, _ = p.okHandler;
        return T.classList.add(u), v.innerHTML = p.okText || "Confirm", m.innerHTML = p.cancelText || "Cancel", v.classList.add(h), m.classList.add(h), v.classList.add(l), m.classList.add(c), S && typeof S == "function" && (m.addEventListener("click", S), C.addEventListener("click", S)), _ && typeof _ == "function" && v.addEventListener("click", _), v.addEventListener("click", k.remove.bind(k)), m.addEventListener("click", k.remove.bind(k)), T.appendChild(v), T.appendChild(m), k.appendChild(T), k;
      }, prompt: function(p) {
        var k = f(p), T = document.createElement("div"), v = document.createElement("button"), m = document.createElement("input"), C = k.querySelector("." + a), S = p.cancelHandler, _ = p.okHandler;
        return T.classList.add(u), v.innerHTML = p.okText || "Ok", v.classList.add(h), v.classList.add(l), m.classList.add(d), p.placeholder && m.setAttribute("placeholder", p.placeholder), p.default && (m.value = p.default), p.inputType && (m.type = p.inputType), S && typeof S == "function" && C.addEventListener("click", S), _ && typeof _ == "function" && v.addEventListener("click", function() {
          _(m.value);
        }), v.addEventListener("click", k.remove.bind(k)), T.appendChild(m), T.appendChild(v), k.appendChild(T), k;
      }, getWrapper: function() {
        var p = document.createElement("DIV");
        return p.classList.add(i), p;
      } });
    }]);
  });
})(Cn);
var Ls = Cn.exports;
const Is = /* @__PURE__ */ vt(Ls);
class Os {
  /**
   * Show web notification
   *
   * @param {NotifierOptions | ConfirmNotifierOptions | PromptNotifierOptions} options - notification options
   */
  show(e) {
    Is.show(e);
  }
}
class As extends N {
  /**
   * @param moduleConfiguration - Module Configuration
   * @param moduleConfiguration.config - Editor's config
   * @param moduleConfiguration.eventsDispatcher - Editor's event dispatcher
   */
  constructor({ config: e, eventsDispatcher: t }) {
    super({
      config: e,
      eventsDispatcher: t
    }), this.notifier = new Os();
  }
  /**
   * Available methods
   */
  get methods() {
    return {
      show: (e) => this.show(e)
    };
  }
  /**
   * Show notification
   *
   * @param {NotifierOptions} options - message option
   */
  show(e) {
    return this.notifier.show(e);
  }
}
class Ps extends N {
  /**
   * Available methods
   */
  get methods() {
    const e = () => this.isEnabled;
    return {
      toggle: (t) => this.toggle(t),
      get isEnabled() {
        return e();
      }
    };
  }
  /**
   * Set or toggle read-only state
   *
   * @param {boolean|undefined} state - set or toggle state
   * @returns {boolean} current value
   */
  toggle(e) {
    return this.Editor.ReadOnly.toggle(e);
  }
  /**
   * Returns current read-only state
   */
  get isEnabled() {
    return this.Editor.ReadOnly.isEnabled;
  }
}
var En = { exports: {} };
(function(o, e) {
  (function(t, r) {
    o.exports = r();
  })(Ve, function() {
    function t(h) {
      var u = h.tags, f = Object.keys(u), p = f.map(function(k) {
        return typeof u[k];
      }).every(function(k) {
        return k === "object" || k === "boolean" || k === "function";
      });
      if (!p)
        throw new Error("The configuration was invalid");
      this.config = h;
    }
    var r = ["P", "LI", "TD", "TH", "DIV", "H1", "H2", "H3", "H4", "H5", "H6", "PRE"];
    function n(h) {
      return r.indexOf(h.nodeName) !== -1;
    }
    var i = ["A", "B", "STRONG", "I", "EM", "SUB", "SUP", "U", "STRIKE"];
    function s(h) {
      return i.indexOf(h.nodeName) !== -1;
    }
    t.prototype.clean = function(h) {
      const u = document.implementation.createHTMLDocument(), f = u.createElement("div");
      return f.innerHTML = h, this._sanitize(u, f), f.innerHTML;
    }, t.prototype._sanitize = function(h, u) {
      var f = a(h, u), p = f.firstChild();
      if (p)
        do {
          if (p.nodeType === Node.TEXT_NODE)
            if (p.data.trim() === "" && (p.previousElementSibling && n(p.previousElementSibling) || p.nextElementSibling && n(p.nextElementSibling))) {
              u.removeChild(p), this._sanitize(h, u);
              break;
            } else
              continue;
          if (p.nodeType === Node.COMMENT_NODE) {
            u.removeChild(p), this._sanitize(h, u);
            break;
          }
          var k = s(p), T;
          k && (T = Array.prototype.some.call(p.childNodes, n));
          var v = !!u.parentNode, m = n(u) && n(p) && v, C = p.nodeName.toLowerCase(), S = l(this.config, C, p), _ = k && T;
          if (_ || c(p, S) || !this.config.keepNestedBlockElements && m) {
            if (!(p.nodeName === "SCRIPT" || p.nodeName === "STYLE"))
              for (; p.childNodes.length > 0; )
                u.insertBefore(p.childNodes[0], p);
            u.removeChild(p), this._sanitize(h, u);
            break;
          }
          for (var x = 0; x < p.attributes.length; x += 1) {
            var I = p.attributes[x];
            d(I, S, p) && (p.removeAttribute(I.name), x = x - 1);
          }
          this._sanitize(h, p);
        } while (p = f.nextSibling());
    };
    function a(h, u) {
      return h.createTreeWalker(
        u,
        NodeFilter.SHOW_TEXT | NodeFilter.SHOW_ELEMENT | NodeFilter.SHOW_COMMENT,
        null,
        !1
      );
    }
    function l(h, u, f) {
      return typeof h.tags[u] == "function" ? h.tags[u](f) : h.tags[u];
    }
    function c(h, u) {
      return typeof u > "u" ? !0 : typeof u == "boolean" ? !u : !1;
    }
    function d(h, u, f) {
      var p = h.name.toLowerCase();
      return u === !0 ? !1 : typeof u[p] == "function" ? !u[p](h.value, f) : typeof u[p] > "u" || u[p] === !1 ? !0 : typeof u[p] == "string" ? u[p] !== h.value : !1;
    }
    return t;
  });
})(En);
var Ns = En.exports;
const js = /* @__PURE__ */ vt(Ns);
function to(o, e) {
  return o.map((t) => {
    const r = R(e) ? e(t.tool) : e;
    return X(r) || (t.data = oo(t.data, r)), t;
  });
}
function J(o, e = {}) {
  const t = {
    tags: e
  };
  return new js(t).clean(o);
}
function oo(o, e) {
  return Array.isArray(o) ? Ds(o, e) : U(o) ? Rs(o, e) : ie(o) ? Hs(o, e) : o;
}
function Ds(o, e) {
  return o.map((t) => oo(t, e));
}
function Rs(o, e) {
  const t = {};
  for (const r in o) {
    if (!Object.prototype.hasOwnProperty.call(o, r))
      continue;
    const n = o[r], i = Fs(e[r]) ? e[r] : e;
    t[r] = oo(n, i);
  }
  return t;
}
function Hs(o, e) {
  return U(e) ? J(o, e) : e === !1 ? J(o, {}) : o;
}
function Fs(o) {
  return U(o) || ns(o) || R(o);
}
class $s extends N {
  /**
   * Available methods
   *
   * @returns {SanitizerConfig}
   */
  get methods() {
    return {
      clean: (e, t) => this.clean(e, t)
    };
  }
  /**
   * Perform sanitizing of a string
   *
   * @param {string} taintString - what to sanitize
   * @param {SanitizerConfig} config - sanitizer config
   * @returns {string}
   */
  clean(e, t) {
    return J(e, t);
  }
}
class Us extends N {
  /**
   * Available methods
   *
   * @returns {Saver}
   */
  get methods() {
    return {
      save: () => this.save()
    };
  }
  /**
   * Return Editor's data
   *
   * @returns {OutputData}
   */
  save() {
    const e = "Editor's content can not be saved in read-only mode";
    return this.Editor.ReadOnly.isEnabled ? (Y(e, "warn"), Promise.reject(new Error(e))) : this.Editor.Saver.save();
  }
}
class zs extends N {
  constructor() {
    super(...arguments), this.selectionUtils = new L();
  }
  /**
   * Available methods
   *
   * @returns {SelectionAPIInterface}
   */
  get methods() {
    return {
      findParentTag: (e, t) => this.findParentTag(e, t),
      expandToTag: (e) => this.expandToTag(e),
      save: () => this.selectionUtils.save(),
      restore: () => this.selectionUtils.restore(),
      setFakeBackground: () => this.selectionUtils.setFakeBackground(),
      removeFakeBackground: () => this.selectionUtils.removeFakeBackground()
    };
  }
  /**
   * Looks ahead from selection and find passed tag with class name
   *
   * @param {string} tagName - tag to find
   * @param {string} className - tag's class name
   * @returns {HTMLElement|null}
   */
  findParentTag(e, t) {
    return this.selectionUtils.findParentTag(e, t);
  }
  /**
   * Expand selection to passed tag
   *
   * @param {HTMLElement} node - tag that should contain selection
   */
  expandToTag(e) {
    this.selectionUtils.expandToTag(e);
  }
}
class Vs extends N {
  /**
   * Available methods
   */
  get methods() {
    return {
      getBlockTools: () => Array.from(this.Editor.Tools.blockTools.values())
    };
  }
}
class Ws extends N {
  /**
   * Exported classes
   */
  get classes() {
    return {
      /**
       * Base Block styles
       */
      block: "cdx-block",
      /**
       * Inline Tools styles
       */
      inlineToolButton: "ce-inline-tool",
      inlineToolButtonActive: "ce-inline-tool--active",
      /**
       * UI elements
       */
      input: "cdx-input",
      loader: "cdx-loader",
      button: "cdx-button",
      /**
       * Settings styles
       */
      settingsButton: "cdx-settings-button",
      settingsButtonActive: "cdx-settings-button--active"
    };
  }
}
class qs extends N {
  /**
   * Available methods
   *
   * @returns {Toolbar}
   */
  get methods() {
    return {
      close: () => this.close(),
      open: () => this.open(),
      toggleBlockSettings: (e) => this.toggleBlockSettings(e),
      toggleToolbox: (e) => this.toggleToolbox(e)
    };
  }
  /**
   * Open toolbar
   */
  open() {
    this.Editor.Toolbar.moveAndOpen();
  }
  /**
   * Close toolbar and all included elements
   */
  close() {
    this.Editor.Toolbar.close();
  }
  /**
   * Toggles Block Setting of the current block
   *
   * @param {boolean} openingState —  opening state of Block Setting
   */
  toggleBlockSettings(e) {
    if (this.Editor.BlockManager.currentBlockIndex === -1) {
      Y("Could't toggle the Toolbar because there is no block selected ", "warn");
      return;
    }
    e ?? !this.Editor.BlockSettings.opened ? (this.Editor.Toolbar.moveAndOpen(), this.Editor.BlockSettings.open()) : this.Editor.BlockSettings.close();
  }
  /**
   * Open toolbox
   *
   * @param {boolean} openingState - Opening state of toolbox
   */
  toggleToolbox(e) {
    if (this.Editor.BlockManager.currentBlockIndex === -1) {
      Y("Could't toggle the Toolbox because there is no block selected ", "warn");
      return;
    }
    e ?? !this.Editor.Toolbar.toolbox.opened ? (this.Editor.Toolbar.moveAndOpen(), this.Editor.Toolbar.toolbox.open()) : this.Editor.Toolbar.toolbox.close();
  }
}
var Tn = { exports: {} };
/*!
 * CodeX.Tooltips
 * 
 * @version 1.0.5
 * 
 * @licence MIT
 * @author CodeX <https://codex.so>
 * 
 * 
 */
(function(o, e) {
  (function(t, r) {
    o.exports = r();
  })(window, function() {
    return (function(t) {
      var r = {};
      function n(i) {
        if (r[i])
          return r[i].exports;
        var s = r[i] = { i, l: !1, exports: {} };
        return t[i].call(s.exports, s, s.exports, n), s.l = !0, s.exports;
      }
      return n.m = t, n.c = r, n.d = function(i, s, a) {
        n.o(i, s) || Object.defineProperty(i, s, { enumerable: !0, get: a });
      }, n.r = function(i) {
        typeof Symbol < "u" && Symbol.toStringTag && Object.defineProperty(i, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(i, "__esModule", { value: !0 });
      }, n.t = function(i, s) {
        if (1 & s && (i = n(i)), 8 & s || 4 & s && typeof i == "object" && i && i.__esModule)
          return i;
        var a = /* @__PURE__ */ Object.create(null);
        if (n.r(a), Object.defineProperty(a, "default", { enumerable: !0, value: i }), 2 & s && typeof i != "string")
          for (var l in i)
            n.d(a, l, (function(c) {
              return i[c];
            }).bind(null, l));
        return a;
      }, n.n = function(i) {
        var s = i && i.__esModule ? function() {
          return i.default;
        } : function() {
          return i;
        };
        return n.d(s, "a", s), s;
      }, n.o = function(i, s) {
        return Object.prototype.hasOwnProperty.call(i, s);
      }, n.p = "", n(n.s = 0);
    })([function(t, r, n) {
      t.exports = n(1);
    }, function(t, r, n) {
      n.r(r), n.d(r, "default", function() {
        return i;
      });
      class i {
        constructor() {
          this.nodes = { wrapper: null, content: null }, this.showed = !1, this.offsetTop = 10, this.offsetLeft = 10, this.offsetRight = 10, this.hidingDelay = 0, this.handleWindowScroll = () => {
            this.showed && this.hide(!0);
          }, this.loadStyles(), this.prepare(), window.addEventListener("scroll", this.handleWindowScroll, { passive: !0 });
        }
        get CSS() {
          return { tooltip: "ct", tooltipContent: "ct__content", tooltipShown: "ct--shown", placement: { left: "ct--left", bottom: "ct--bottom", right: "ct--right", top: "ct--top" } };
        }
        show(a, l, c) {
          this.nodes.wrapper || this.prepare(), this.hidingTimeout && clearTimeout(this.hidingTimeout);
          const d = Object.assign({ placement: "bottom", marginTop: 0, marginLeft: 0, marginRight: 0, marginBottom: 0, delay: 70, hidingDelay: 0 }, c);
          if (d.hidingDelay && (this.hidingDelay = d.hidingDelay), this.nodes.content.innerHTML = "", typeof l == "string")
            this.nodes.content.appendChild(document.createTextNode(l));
          else {
            if (!(l instanceof Node))
              throw Error("[CodeX Tooltip] Wrong type of «content» passed. It should be an instance of Node or String. But " + typeof l + " given.");
            this.nodes.content.appendChild(l);
          }
          switch (this.nodes.wrapper.classList.remove(...Object.values(this.CSS.placement)), d.placement) {
            case "top":
              this.placeTop(a, d);
              break;
            case "left":
              this.placeLeft(a, d);
              break;
            case "right":
              this.placeRight(a, d);
              break;
            case "bottom":
            default:
              this.placeBottom(a, d);
          }
          d && d.delay ? this.showingTimeout = setTimeout(() => {
            this.nodes.wrapper.classList.add(this.CSS.tooltipShown), this.showed = !0;
          }, d.delay) : (this.nodes.wrapper.classList.add(this.CSS.tooltipShown), this.showed = !0);
        }
        hide(a = !1) {
          if (this.hidingDelay && !a)
            return this.hidingTimeout && clearTimeout(this.hidingTimeout), void (this.hidingTimeout = setTimeout(() => {
              this.hide(!0);
            }, this.hidingDelay));
          this.nodes.wrapper.classList.remove(this.CSS.tooltipShown), this.showed = !1, this.showingTimeout && clearTimeout(this.showingTimeout);
        }
        onHover(a, l, c) {
          a.addEventListener("mouseenter", () => {
            this.show(a, l, c);
          }), a.addEventListener("mouseleave", () => {
            this.hide();
          });
        }
        destroy() {
          this.nodes.wrapper.remove(), window.removeEventListener("scroll", this.handleWindowScroll);
        }
        prepare() {
          this.nodes.wrapper = this.make("div", this.CSS.tooltip), this.nodes.content = this.make("div", this.CSS.tooltipContent), this.append(this.nodes.wrapper, this.nodes.content), this.append(document.body, this.nodes.wrapper);
        }
        loadStyles() {
          const a = "codex-tooltips-style";
          if (document.getElementById(a))
            return;
          const l = n(2), c = this.make("style", null, { textContent: l.toString(), id: a });
          this.prepend(document.head, c);
        }
        placeBottom(a, l) {
          const c = a.getBoundingClientRect(), d = c.left + a.clientWidth / 2 - this.nodes.wrapper.offsetWidth / 2, h = c.bottom + window.pageYOffset + this.offsetTop + l.marginTop;
          this.applyPlacement("bottom", d, h);
        }
        placeTop(a, l) {
          const c = a.getBoundingClientRect(), d = c.left + a.clientWidth / 2 - this.nodes.wrapper.offsetWidth / 2, h = c.top + window.pageYOffset - this.nodes.wrapper.clientHeight - this.offsetTop;
          this.applyPlacement("top", d, h);
        }
        placeLeft(a, l) {
          const c = a.getBoundingClientRect(), d = c.left - this.nodes.wrapper.offsetWidth - this.offsetLeft - l.marginLeft, h = c.top + window.pageYOffset + a.clientHeight / 2 - this.nodes.wrapper.offsetHeight / 2;
          this.applyPlacement("left", d, h);
        }
        placeRight(a, l) {
          const c = a.getBoundingClientRect(), d = c.right + this.offsetRight + l.marginRight, h = c.top + window.pageYOffset + a.clientHeight / 2 - this.nodes.wrapper.offsetHeight / 2;
          this.applyPlacement("right", d, h);
        }
        applyPlacement(a, l, c) {
          this.nodes.wrapper.classList.add(this.CSS.placement[a]), this.nodes.wrapper.style.left = l + "px", this.nodes.wrapper.style.top = c + "px";
        }
        make(a, l = null, c = {}) {
          const d = document.createElement(a);
          Array.isArray(l) ? d.classList.add(...l) : l && d.classList.add(l);
          for (const h in c)
            c.hasOwnProperty(h) && (d[h] = c[h]);
          return d;
        }
        append(a, l) {
          Array.isArray(l) ? l.forEach((c) => a.appendChild(c)) : a.appendChild(l);
        }
        prepend(a, l) {
          Array.isArray(l) ? (l = l.reverse()).forEach((c) => a.prepend(c)) : a.prepend(l);
        }
      }
    }, function(t, r) {
      t.exports = `.ct{z-index:999;opacity:0;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;pointer-events:none;-webkit-transition:opacity 50ms ease-in,-webkit-transform 70ms cubic-bezier(.215,.61,.355,1);transition:opacity 50ms ease-in,-webkit-transform 70ms cubic-bezier(.215,.61,.355,1);transition:opacity 50ms ease-in,transform 70ms cubic-bezier(.215,.61,.355,1);transition:opacity 50ms ease-in,transform 70ms cubic-bezier(.215,.61,.355,1),-webkit-transform 70ms cubic-bezier(.215,.61,.355,1);will-change:opacity,top,left;-webkit-box-shadow:0 8px 12px 0 rgba(29,32,43,.17),0 4px 5px -3px rgba(5,6,12,.49);box-shadow:0 8px 12px 0 rgba(29,32,43,.17),0 4px 5px -3px rgba(5,6,12,.49);border-radius:9px}.ct,.ct:before{position:absolute;top:0;left:0}.ct:before{content:"";bottom:0;right:0;background-color:#1d202b;z-index:-1;border-radius:4px}@supports(-webkit-mask-box-image:url("")){.ct:before{border-radius:0;-webkit-mask-box-image:url('data:image/svg+xml;charset=utf-8,<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24"><path d="M10.71 0h2.58c3.02 0 4.64.42 6.1 1.2a8.18 8.18 0 013.4 3.4C23.6 6.07 24 7.7 24 10.71v2.58c0 3.02-.42 4.64-1.2 6.1a8.18 8.18 0 01-3.4 3.4c-1.47.8-3.1 1.21-6.11 1.21H10.7c-3.02 0-4.64-.42-6.1-1.2a8.18 8.18 0 01-3.4-3.4C.4 17.93 0 16.3 0 13.29V10.7c0-3.02.42-4.64 1.2-6.1a8.18 8.18 0 013.4-3.4C6.07.4 7.7 0 10.71 0z"/></svg>') 48% 41% 37.9% 53.3%}}@media (--mobile){.ct{display:none}}.ct__content{padding:6px 10px;color:#cdd1e0;font-size:12px;text-align:center;letter-spacing:.02em;line-height:1em}.ct:after{content:"";width:8px;height:8px;position:absolute;background-color:#1d202b;z-index:-1}.ct--bottom{-webkit-transform:translateY(5px);transform:translateY(5px)}.ct--bottom:after{top:-3px;left:50%;-webkit-transform:translateX(-50%) rotate(-45deg);transform:translateX(-50%) rotate(-45deg)}.ct--top{-webkit-transform:translateY(-5px);transform:translateY(-5px)}.ct--top:after{top:auto;bottom:-3px;left:50%;-webkit-transform:translateX(-50%) rotate(-45deg);transform:translateX(-50%) rotate(-45deg)}.ct--left{-webkit-transform:translateX(-5px);transform:translateX(-5px)}.ct--left:after{top:50%;left:auto;right:0;-webkit-transform:translate(41.6%,-50%) rotate(-45deg);transform:translate(41.6%,-50%) rotate(-45deg)}.ct--right{-webkit-transform:translateX(5px);transform:translateX(5px)}.ct--right:after{top:50%;left:0;-webkit-transform:translate(-41.6%,-50%) rotate(-45deg);transform:translate(-41.6%,-50%) rotate(-45deg)}.ct--shown{opacity:1;-webkit-transform:none;transform:none}`;
    }]).default;
  });
})(Tn);
var Ks = Tn.exports;
const Ys = /* @__PURE__ */ vt(Ks);
let Z = null;
function ro() {
  Z || (Z = new Ys());
}
function Xs(o, e, t) {
  ro(), Z == null || Z.show(o, e, t);
}
function dt(o = !1) {
  ro(), Z == null || Z.hide(o);
}
function ht(o, e, t) {
  ro(), Z == null || Z.onHover(o, e, t);
}
function Zs() {
  Z == null || Z.destroy(), Z = null;
}
class Gs extends N {
  /**
   * @class
   * @param moduleConfiguration - Module Configuration
   * @param moduleConfiguration.config - Editor's config
   * @param moduleConfiguration.eventsDispatcher - Editor's event dispatcher
   */
  constructor({ config: e, eventsDispatcher: t }) {
    super({
      config: e,
      eventsDispatcher: t
    });
  }
  /**
   * Available methods
   */
  get methods() {
    return {
      show: (e, t, r) => this.show(e, t, r),
      hide: () => this.hide(),
      onHover: (e, t, r) => this.onHover(e, t, r)
    };
  }
  /**
   * Method show tooltip on element with passed HTML content
   *
   * @param {HTMLElement} element - element on which tooltip should be shown
   * @param {TooltipContent} content - tooltip content
   * @param {TooltipOptions} options - tooltip options
   */
  show(e, t, r) {
    Xs(e, t, r);
  }
  /**
   * Method hides tooltip on HTML page
   */
  hide() {
    dt();
  }
  /**
   * Decorator for showing Tooltip by mouseenter/mouseleave
   *
   * @param {HTMLElement} element - element on which tooltip should be shown
   * @param {TooltipContent} content - tooltip content
   * @param {TooltipOptions} options - tooltip options
   */
  onHover(e, t, r) {
    ht(e, t, r);
  }
}
class Js extends N {
  /**
   * Available methods / getters
   */
  get methods() {
    return {
      nodes: this.editorNodes
      /**
       * There can be added some UI methods, like toggleThinMode() etc
       */
    };
  }
  /**
   * Exported classes
   */
  get editorNodes() {
    return {
      /**
       * Top-level editor instance wrapper
       */
      wrapper: this.Editor.UI.nodes.wrapper,
      /**
       * Element that holds all the Blocks
       */
      redactor: this.Editor.UI.nodes.redactor
    };
  }
}
function Sn(o, e) {
  const t = {};
  return Object.entries(o).forEach(([r, n]) => {
    if (U(n)) {
      const i = e ? `${e}.${r}` : r;
      Object.values(n).every((s) => ie(s)) ? t[r] = i : t[r] = Sn(n, i);
      return;
    }
    t[r] = n;
  }), t;
}
const K = Sn(pn);
function Qs(o, e) {
  const t = {};
  return Object.keys(o).forEach((r) => {
    const n = e[r];
    n !== void 0 ? t[n] = o[r] : t[r] = o[r];
  }), t;
}
const Bn = class Re {
  /**
   * @param {HTMLElement[]} nodeList — the list of iterable HTML-items
   * @param {string} focusedCssClass - user-provided CSS-class that will be set in flipping process
   */
  constructor(e, t) {
    this.cursor = -1, this.items = [], this.items = e || [], this.focusedCssClass = t;
  }
  /**
   * Returns Focused button Node
   *
   * @returns {HTMLElement}
   */
  get currentItem() {
    return this.cursor === -1 ? null : this.items[this.cursor];
  }
  /**
   * Sets cursor to specified position
   *
   * @param cursorPosition - new cursor position
   */
  setCursor(e) {
    e < this.items.length && e >= -1 && (this.dropCursor(), this.cursor = e, this.items[this.cursor].classList.add(this.focusedCssClass));
  }
  /**
   * Sets items. Can be used when iterable items changed dynamically
   *
   * @param {HTMLElement[]} nodeList - nodes to iterate
   */
  setItems(e) {
    this.items = e;
  }
  /**
   * Sets cursor next to the current
   */
  next() {
    this.cursor = this.leafNodesAndReturnIndex(Re.directions.RIGHT);
  }
  /**
   * Sets cursor before current
   */
  previous() {
    this.cursor = this.leafNodesAndReturnIndex(Re.directions.LEFT);
  }
  /**
   * Sets cursor to the default position and removes CSS-class from previously focused item
   */
  dropCursor() {
    this.cursor !== -1 && (this.items[this.cursor].classList.remove(this.focusedCssClass), this.cursor = -1);
  }
  /**
   * Leafs nodes inside the target list from active element
   *
   * @param {string} direction - leaf direction. Can be 'left' or 'right'
   * @returns {number} index of focused node
   */
  leafNodesAndReturnIndex(e) {
    if (this.items.length === 0)
      return this.cursor;
    let t = this.cursor;
    return t === -1 ? t = e === Re.directions.RIGHT ? -1 : 0 : this.items[t].classList.remove(this.focusedCssClass), e === Re.directions.RIGHT ? t = (t + 1) % this.items.length : t = (this.items.length + t - 1) % this.items.length, g.canSetCaret(this.items[t]) && at(() => L.setCursor(this.items[t]), 50)(), this.items[t].classList.add(this.focusedCssClass), t;
  }
};
Bn.directions = {
  RIGHT: "right",
  LEFT: "left"
};
let Oe = Bn, ut = class Kt {
  /**
   * @param options - different constructing settings
   */
  constructor(e) {
    this.iterator = null, this.activated = !1, this.flipCallbacks = [], this.onKeyDown = (t) => {
      if (!(!this.isEventReadyForHandling(t) || t.shiftKey === !0))
        switch (Kt.usedKeys.includes(t.keyCode) && t.preventDefault(), t.keyCode) {
          case A.TAB:
            this.handleTabPress(t);
            break;
          case A.LEFT:
          case A.UP:
            this.flipLeft();
            break;
          case A.RIGHT:
          case A.DOWN:
            this.flipRight();
            break;
          case A.ENTER:
            this.handleEnterPress(t);
            break;
        }
    }, this.iterator = new Oe(e.items, e.focusedItemClass), this.activateCallback = e.activateCallback, this.allowedKeys = e.allowedKeys || Kt.usedKeys;
  }
  /**
   * True if flipper is currently activated
   */
  get isActivated() {
    return this.activated;
  }
  /**
   * Array of keys (codes) that is handled by Flipper
   * Used to:
   *  - preventDefault only for this keys, not all keydowns (@see constructor)
   *  - to skip external behaviours only for these keys, when filler is activated (@see BlockEvents@arrowRightAndDown)
   */
  static get usedKeys() {
    return [
      A.TAB,
      A.LEFT,
      A.RIGHT,
      A.ENTER,
      A.UP,
      A.DOWN
    ];
  }
  /**
   * Active tab/arrows handling by flipper
   *
   * @param items - Some modules (like, InlineToolbar, BlockSettings) might refresh buttons dynamically
   * @param cursorPosition - index of the item that should be focused once flipper is activated
   */
  activate(e, t) {
    this.activated = !0, e && this.iterator.setItems(e), t !== void 0 && this.iterator.setCursor(t), document.addEventListener("keydown", this.onKeyDown, !0);
  }
  /**
   * Disable tab/arrows handling by flipper
   */
  deactivate() {
    this.activated = !1, this.dropCursor(), document.removeEventListener("keydown", this.onKeyDown);
  }
  /**
   * Focus first item
   */
  focusFirst() {
    this.dropCursor(), this.flipRight();
  }
  /**
   * Focuses previous flipper iterator item
   */
  flipLeft() {
    this.iterator.previous(), this.flipCallback();
  }
  /**
   * Focuses next flipper iterator item
   */
  flipRight() {
    this.iterator.next(), this.flipCallback();
  }
  /**
   * Return true if some button is focused
   */
  hasFocus() {
    return !!this.iterator.currentItem;
  }
  /**
   * Registeres function that should be executed on each navigation action
   *
   * @param cb - function to execute
   */
  onFlip(e) {
    this.flipCallbacks.push(e);
  }
  /**
   * Unregisteres function that is executed on each navigation action
   *
   * @param cb - function to stop executing
   */
  removeOnFlip(e) {
    this.flipCallbacks = this.flipCallbacks.filter((t) => t !== e);
  }
  /**
   * Drops flipper's iterator cursor
   *
   * @see DomIterator#dropCursor
   */
  dropCursor() {
    this.iterator.dropCursor();
  }
  /**
   * This function is fired before handling flipper keycodes
   * The result of this function defines if it is need to be handled or not
   *
   * @param {KeyboardEvent} event - keydown keyboard event
   * @returns {boolean}
   */
  isEventReadyForHandling(e) {
    return this.activated && this.allowedKeys.includes(e.keyCode);
  }
  /**
   * When flipper is activated tab press will leaf the items
   *
   * @param {KeyboardEvent} event - tab keydown event
   */
  handleTabPress(e) {
    switch (e.shiftKey ? Oe.directions.LEFT : Oe.directions.RIGHT) {
      case Oe.directions.RIGHT:
        this.flipRight();
        break;
      case Oe.directions.LEFT:
        this.flipLeft();
        break;
    }
  }
  /**
   * Enter press will click current item if flipper is activated
   *
   * @param {KeyboardEvent} event - enter keydown event
   */
  handleEnterPress(e) {
    this.activated && (this.iterator.currentItem && (e.stopPropagation(), e.preventDefault(), this.iterator.currentItem.click()), R(this.activateCallback) && this.activateCallback(this.iterator.currentItem));
  }
  /**
   * Fired after flipping in any direction
   */
  flipCallback() {
    this.iterator.currentItem && this.iterator.currentItem.scrollIntoViewIfNeeded(), this.flipCallbacks.forEach((e) => e());
  }
};
const ea = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9 12L9 7.1C9 7.04477 9.04477 7 9.1 7H10.4C11.5 7 14 7.1 14 9.5C14 9.5 14 12 11 12M9 12V16.8C9 16.9105 9.08954 17 9.2 17H12.5C14 17 15 16 15 14.5C15 11.7046 11 12 11 12M9 12H11"/></svg>', ta = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 10L11.8586 14.8586C11.9367 14.9367 12.0633 14.9367 12.1414 14.8586L17 10"/></svg>', oa = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M14.5 17.5L9.64142 12.6414C9.56331 12.5633 9.56331 12.4367 9.64142 12.3586L14.5 7.5"/></svg>', ra = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9.58284 17.5L14.4414 12.6414C14.5195 12.5633 14.5195 12.4367 14.4414 12.3586L9.58284 7.5"/></svg>', na = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 15L11.8586 10.1414C11.9367 10.0633 12.0633 10.0633 12.1414 10.1414L17 15"/></svg>', ia = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8 8L12 12M12 12L16 16M12 12L16 8M12 12L8 16"/></svg>', sa = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><circle cx="12" cy="12" r="4" stroke="currentColor" stroke-width="2"/></svg>', aa = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M13.34 10C12.4223 12.7337 11 17 11 17"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M14.21 7H14.2"/></svg>', Wr = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.69998 12.6L7.67896 12.62C6.53993 13.7048 6.52012 15.5155 7.63516 16.625V16.625C8.72293 17.7073 10.4799 17.7102 11.5712 16.6314L13.0263 15.193C14.0703 14.1609 14.2141 12.525 13.3662 11.3266L13.22 11.12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16.22 11.12L16.3564 10.9805C17.2895 10.0265 17.3478 8.5207 16.4914 7.49733V7.49733C15.5691 6.39509 13.9269 6.25143 12.8271 7.17675L11.3901 8.38588C10.0935 9.47674 9.95706 11.4241 11.0888 12.6852L11.12 12.72"/></svg>', la = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.40999 7.29999H9.4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 7.29999H14.59"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.30999 12H9.3"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 12H14.59"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.40999 16.7H9.4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 16.7H14.59"/></svg>', ca = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 7V12M12 17V12M17 12H12M12 12H7"/></svg>', Mn = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M11.5 17.5L5 11M5 11V15.5M5 11H9.5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12.5 6.5L19 13M19 13V8.5M19 13H14.5"/></svg>', da = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><circle cx="10.5" cy="10.5" r="5.5" stroke="currentColor" stroke-width="2"/><line x1="15.4142" x2="19" y1="15" y2="18.5858" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>', ha = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M15.7795 11.5C15.7795 11.5 16.053 11.1962 16.5497 10.6722C17.4442 9.72856 17.4701 8.2475 16.5781 7.30145V7.30145C15.6482 6.31522 14.0873 6.29227 13.1288 7.25073L11.8796 8.49999"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8.24517 12.3883C8.24517 12.3883 7.97171 12.6922 7.47504 13.2161C6.58051 14.1598 6.55467 15.6408 7.44666 16.5869V16.5869C8.37653 17.5731 9.93744 17.5961 10.8959 16.6376L12.1452 15.3883"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17.7802 15.1032L16.597 14.9422C16.0109 14.8624 15.4841 15.3059 15.4627 15.8969L15.4199 17.0818"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6.39064 9.03238L7.58432 9.06668C8.17551 9.08366 8.6522 8.58665 8.61056 7.99669L8.5271 6.81397"/><line x1="12.1142" x2="11.7" y1="12.2" y2="11.7858" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>', ua = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/><line x1="12" x2="12" y1="9" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 15.02V15.01"/></svg>', pa = "__", fa = "--";
function he(o) {
  return (e, t) => [[o, e].filter((r) => !!r).join(pa), t].filter((r) => !!r).join(fa);
}
const Ae = he("ce-hint"), Pe = {
  root: Ae(),
  alignedStart: Ae(null, "align-left"),
  alignedCenter: Ae(null, "align-center"),
  title: Ae("title"),
  description: Ae("description")
};
class ga {
  /**
   * Constructs the hint content instance
   *
   * @param params - hint content parameters
   */
  constructor(e) {
    this.nodes = {
      root: g.make("div", [Pe.root, e.alignment === "center" ? Pe.alignedCenter : Pe.alignedStart]),
      title: g.make("div", Pe.title, { textContent: e.title })
    }, this.nodes.root.appendChild(this.nodes.title), e.description !== void 0 && (this.nodes.description = g.make("div", Pe.description, { textContent: e.description }), this.nodes.root.appendChild(this.nodes.description));
  }
  /**
   * Returns the root element of the hint content
   */
  getElement() {
    return this.nodes.root;
  }
}
let no = class {
  /**
   * Constructs the instance
   *
   * @param params - instance parameters
   */
  constructor(e) {
    this.params = e;
  }
  /**
   * Item name if exists
   */
  get name() {
    if (this.params !== void 0 && "name" in this.params)
      return this.params.name;
  }
  /**
   * Destroys the instance
   */
  destroy() {
    dt();
  }
  /**
   * Called when children popover is opened (if exists)
   */
  onChildrenOpen() {
    var e;
    this.params !== void 0 && "children" in this.params && typeof ((e = this.params.children) == null ? void 0 : e.onOpen) == "function" && this.params.children.onOpen();
  }
  /**
   * Called when children popover is closed (if exists)
   */
  onChildrenClose() {
    var e;
    this.params !== void 0 && "children" in this.params && typeof ((e = this.params.children) == null ? void 0 : e.onClose) == "function" && this.params.children.onClose();
  }
  /**
   * Called on popover item click
   */
  handleClick() {
    var e, t;
    this.params !== void 0 && "onActivate" in this.params && ((t = (e = this.params).onActivate) == null || t.call(e, this.params));
  }
  /**
   * Adds hint to the item element if hint data is provided
   *
   * @param itemElement - popover item root element to add hint to
   * @param hintData - hint data
   */
  addHint(e, t) {
    const r = new ga(t);
    ht(e, r.getElement(), {
      placement: t.position,
      hidingDelay: 100
    });
  }
  /**
   * Returns item children that are represented as popover items
   */
  get children() {
    var e;
    return this.params !== void 0 && "children" in this.params && ((e = this.params.children) == null ? void 0 : e.items) !== void 0 ? this.params.children.items : [];
  }
  /**
   * Returns true if item has any type of children
   */
  get hasChildren() {
    return this.children.length > 0;
  }
  /**
   * Returns true if item children should be open instantly after popover is opened and not on item click/hover
   */
  get isChildrenOpen() {
    var e;
    return this.params !== void 0 && "children" in this.params && ((e = this.params.children) == null ? void 0 : e.isOpen) === !0;
  }
  /**
   * True if item children items should be navigatable via keyboard
   */
  get isChildrenFlippable() {
    var e;
    return !(this.params === void 0 || !("children" in this.params) || ((e = this.params.children) == null ? void 0 : e.isFlippable) === !1);
  }
  /**
   * Returns true if item has children that should be searchable
   */
  get isChildrenSearchable() {
    var e;
    return this.params !== void 0 && "children" in this.params && ((e = this.params.children) == null ? void 0 : e.searchable) === !0;
  }
  /**
   * True if popover should close once item is activated
   */
  get closeOnActivate() {
    return this.params !== void 0 && "closeOnActivate" in this.params && this.params.closeOnActivate;
  }
  /**
   * True if item is active
   */
  get isActive() {
    return this.params === void 0 || !("isActive" in this.params) ? !1 : typeof this.params.isActive == "function" ? this.params.isActive() : this.params.isActive === !0;
  }
};
const q = he("ce-popover-item"), H = {
  container: q(),
  active: q(null, "active"),
  disabled: q(null, "disabled"),
  focused: q(null, "focused"),
  hidden: q(null, "hidden"),
  confirmationState: q(null, "confirmation"),
  noHover: q(null, "no-hover"),
  noFocus: q(null, "no-focus"),
  title: q("title"),
  secondaryTitle: q("secondary-title"),
  icon: q("icon"),
  iconTool: q("icon", "tool"),
  iconChevronRight: q("icon", "chevron-right"),
  wobbleAnimation: he("wobble")()
};
let fe = class extends no {
  /**
   * Constructs popover item instance
   *
   * @param params - popover item construction params
   * @param renderParams - popover item render params.
   * The parameters that are not set by user via popover api but rather depend on technical implementation
   */
  constructor(e, t) {
    super(e), this.params = e, this.nodes = {
      root: null,
      icon: null
    }, this.confirmationState = null, this.removeSpecialFocusBehavior = () => {
      var r;
      (r = this.nodes.root) == null || r.classList.remove(H.noFocus);
    }, this.removeSpecialHoverBehavior = () => {
      var r;
      (r = this.nodes.root) == null || r.classList.remove(H.noHover);
    }, this.onErrorAnimationEnd = () => {
      var r, n;
      (r = this.nodes.icon) == null || r.classList.remove(H.wobbleAnimation), (n = this.nodes.icon) == null || n.removeEventListener("animationend", this.onErrorAnimationEnd);
    }, this.nodes.root = this.make(e, t);
  }
  /**
   * True if item is disabled and hence not clickable
   */
  get isDisabled() {
    return this.params.isDisabled === !0;
  }
  /**
   * Exposes popover item toggle parameter
   */
  get toggle() {
    return this.params.toggle;
  }
  /**
   * Item title
   */
  get title() {
    return this.params.title;
  }
  /**
   * True if confirmation state is enabled for popover item
   */
  get isConfirmationStateEnabled() {
    return this.confirmationState !== null;
  }
  /**
   * True if item is focused in keyboard navigation process
   */
  get isFocused() {
    return this.nodes.root === null ? !1 : this.nodes.root.classList.contains(H.focused);
  }
  /**
   * Returns popover item root element
   */
  getElement() {
    return this.nodes.root;
  }
  /**
   * Called on popover item click
   */
  handleClick() {
    if (this.isConfirmationStateEnabled && this.confirmationState !== null) {
      this.activateOrEnableConfirmationMode(this.confirmationState);
      return;
    }
    this.activateOrEnableConfirmationMode(this.params);
  }
  /**
   * Toggles item active state
   *
   * @param isActive - true if item should strictly should become active
   */
  toggleActive(e) {
    var t;
    (t = this.nodes.root) == null || t.classList.toggle(H.active, e);
  }
  /**
   * Toggles item hidden state
   *
   * @param isHidden - true if item should be hidden
   */
  toggleHidden(e) {
    var t;
    (t = this.nodes.root) == null || t.classList.toggle(H.hidden, e);
  }
  /**
   * Resets popover item to its original state
   */
  reset() {
    this.isConfirmationStateEnabled && this.disableConfirmationMode();
  }
  /**
   * Method called once item becomes focused during keyboard navigation
   */
  onFocus() {
    this.disableSpecialHoverAndFocusBehavior();
  }
  /**
   * Constructs HTML element corresponding to popover item params
   *
   * @param params - item construction params
   * @param renderParams - popover item render params
   */
  make(e, t) {
    var r, n;
    const i = (t == null ? void 0 : t.wrapperTag) || "div", s = g.make(i, H.container, {
      type: i === "button" ? "button" : void 0
    });
    return e.name && (s.dataset.itemName = e.name), this.nodes.icon = g.make("div", [H.icon, H.iconTool], {
      innerHTML: e.icon || sa
    }), s.appendChild(this.nodes.icon), e.title !== void 0 && s.appendChild(g.make("div", H.title, {
      innerHTML: e.title || ""
    })), e.secondaryLabel && s.appendChild(g.make("div", H.secondaryTitle, {
      textContent: e.secondaryLabel
    })), this.hasChildren && s.appendChild(g.make("div", [H.icon, H.iconChevronRight], {
      innerHTML: ra
    })), this.isActive && s.classList.add(H.active), e.isDisabled && s.classList.add(H.disabled), e.hint !== void 0 && ((r = t == null ? void 0 : t.hint) == null ? void 0 : r.enabled) !== !1 && this.addHint(s, {
      ...e.hint,
      position: ((n = t == null ? void 0 : t.hint) == null ? void 0 : n.position) || "right"
    }), s;
  }
  /**
   * Activates confirmation mode for the item.
   *
   * @param newState - new popover item params that should be applied
   */
  enableConfirmationMode(e) {
    if (this.nodes.root === null)
      return;
    const t = {
      ...this.params,
      ...e,
      confirmation: "confirmation" in e ? e.confirmation : void 0
    }, r = this.make(t);
    this.nodes.root.innerHTML = r.innerHTML, this.nodes.root.classList.add(H.confirmationState), this.confirmationState = e, this.enableSpecialHoverAndFocusBehavior();
  }
  /**
   * Returns item to its original state
   */
  disableConfirmationMode() {
    if (this.nodes.root === null)
      return;
    const e = this.make(this.params);
    this.nodes.root.innerHTML = e.innerHTML, this.nodes.root.classList.remove(H.confirmationState), this.confirmationState = null, this.disableSpecialHoverAndFocusBehavior();
  }
  /**
   * Enables special focus and hover behavior for item in confirmation state.
   * This is needed to prevent item from being highlighted as hovered/focused just after click.
   */
  enableSpecialHoverAndFocusBehavior() {
    var e, t, r;
    (e = this.nodes.root) == null || e.classList.add(H.noHover), (t = this.nodes.root) == null || t.classList.add(H.noFocus), (r = this.nodes.root) == null || r.addEventListener("mouseleave", this.removeSpecialHoverBehavior, { once: !0 });
  }
  /**
   * Disables special focus and hover behavior
   */
  disableSpecialHoverAndFocusBehavior() {
    var e;
    this.removeSpecialFocusBehavior(), this.removeSpecialHoverBehavior(), (e = this.nodes.root) == null || e.removeEventListener("mouseleave", this.removeSpecialHoverBehavior);
  }
  /**
   * Executes item's onActivate callback if the item has no confirmation configured
   *
   * @param item - item to activate or bring to confirmation mode
   */
  activateOrEnableConfirmationMode(e) {
    var t;
    if (!("confirmation" in e) || e.confirmation === void 0)
      try {
        (t = e.onActivate) == null || t.call(e, e), this.disableConfirmationMode();
      } catch {
        this.animateError();
      }
    else
      this.enableConfirmationMode(e.confirmation);
  }
  /**
   * Animates item which symbolizes that error occured while executing 'onActivate()' callback
   */
  animateError() {
    var e, t, r;
    (e = this.nodes.icon) != null && e.classList.contains(H.wobbleAnimation) || ((t = this.nodes.icon) == null || t.classList.add(H.wobbleAnimation), (r = this.nodes.icon) == null || r.addEventListener("animationend", this.onErrorAnimationEnd));
  }
};
const It = he("ce-popover-item-separator"), Ot = {
  container: It(),
  line: It("line"),
  hidden: It(null, "hidden")
};
class _n extends no {
  /**
   * Constructs the instance
   */
  constructor() {
    super(), this.nodes = {
      root: g.make("div", Ot.container),
      line: g.make("div", Ot.line)
    }, this.nodes.root.appendChild(this.nodes.line);
  }
  /**
   * Returns popover separator root element
   */
  getElement() {
    return this.nodes.root;
  }
  /**
   * Toggles item hidden state
   *
   * @param isHidden - true if item should be hidden
   */
  toggleHidden(e) {
    var t;
    (t = this.nodes.root) == null || t.classList.toggle(Ot.hidden, e);
  }
}
var Q = /* @__PURE__ */ ((o) => (o.Closed = "closed", o.ClosedOnActivate = "closed-on-activate", o))(Q || {});
const z = he("ce-popover"), F = {
  popover: z(),
  popoverContainer: z("container"),
  popoverOpenTop: z(null, "open-top"),
  popoverOpenLeft: z(null, "open-left"),
  popoverOpened: z(null, "opened"),
  search: z("search"),
  nothingFoundMessage: z("nothing-found-message"),
  nothingFoundMessageDisplayed: z("nothing-found-message", "displayed"),
  items: z("items"),
  overlay: z("overlay"),
  overlayHidden: z("overlay", "hidden"),
  popoverNested: z(null, "nested"),
  getPopoverNestedClass: (o) => z(null, `nested-level-${o.toString()}`),
  popoverInline: z(null, "inline"),
  popoverHeader: z("header")
};
var Te = /* @__PURE__ */ ((o) => (o.NestingLevel = "--nesting-level", o.PopoverHeight = "--popover-height", o.InlinePopoverWidth = "--inline-popover-width", o.TriggerItemLeft = "--trigger-item-left", o.TriggerItemTop = "--trigger-item-top", o))(Te || {});
const qr = he("ce-popover-item-html"), Kr = {
  root: qr(),
  hidden: qr(null, "hidden")
};
class qe extends no {
  /**
   * Constructs the instance
   *
   * @param params – instance parameters
   * @param renderParams – popover item render params.
   * The parameters that are not set by user via popover api but rather depend on technical implementation
   */
  constructor(e, t) {
    var r, n;
    super(e), this.nodes = {
      root: g.make("div", Kr.root)
    }, this.nodes.root.appendChild(e.element), e.name && (this.nodes.root.dataset.itemName = e.name), e.hint !== void 0 && ((r = t == null ? void 0 : t.hint) == null ? void 0 : r.enabled) !== !1 && this.addHint(this.nodes.root, {
      ...e.hint,
      position: ((n = t == null ? void 0 : t.hint) == null ? void 0 : n.position) || "right"
    });
  }
  /**
   * Returns popover item root element
   */
  getElement() {
    return this.nodes.root;
  }
  /**
   * Toggles item hidden state
   *
   * @param isHidden - true if item should be hidden
   */
  toggleHidden(e) {
    var t;
    (t = this.nodes.root) == null || t.classList.toggle(Kr.hidden, e);
  }
  /**
   * Returns list of buttons and inputs inside custom content
   */
  getControls() {
    const e = this.nodes.root.querySelectorAll(
      `button, ${g.allInputsSelector}`
    );
    return Array.from(e);
  }
}
class Ln extends Ye {
  /**
   * Constructs the instance
   *
   * @param params - popover construction params
   * @param itemsRenderParams - popover item render params.
   * The parameters that are not set by user via popover api but rather depend on technical implementation
   */
  constructor(e, t = {}) {
    super(), this.params = e, this.itemsRenderParams = t, this.listeners = new Xe(), this.messages = {
      nothingFound: "Nothing found",
      search: "Search"
    }, this.items = this.buildItems(e.items), e.messages && (this.messages = {
      ...this.messages,
      ...e.messages
    }), this.nodes = {}, this.nodes.popoverContainer = g.make("div", [F.popoverContainer]), this.nodes.nothingFoundMessage = g.make("div", [F.nothingFoundMessage], {
      textContent: this.messages.nothingFound
    }), this.nodes.popoverContainer.appendChild(this.nodes.nothingFoundMessage), this.nodes.items = g.make("div", [F.items]), this.items.forEach((r) => {
      const n = r.getElement();
      n !== null && this.nodes.items.appendChild(n);
    }), this.nodes.popoverContainer.appendChild(this.nodes.items), this.listeners.on(this.nodes.popoverContainer, "click", (r) => this.handleClick(r)), this.nodes.popover = g.make("div", [
      F.popover,
      this.params.class
    ]), this.nodes.popover.appendChild(this.nodes.popoverContainer);
  }
  /**
   * List of default popover items that are searchable and may have confirmation state
   */
  get itemsDefault() {
    return this.items.filter((e) => e instanceof fe);
  }
  /**
   * Returns HTML element corresponding to the popover
   */
  getElement() {
    return this.nodes.popover;
  }
  /**
   * Open popover
   */
  show() {
    this.nodes.popover.classList.add(F.popoverOpened), this.search !== void 0 && this.search.focus();
  }
  /**
   * Closes popover
   */
  hide() {
    this.nodes.popover.classList.remove(F.popoverOpened), this.nodes.popover.classList.remove(F.popoverOpenTop), this.itemsDefault.forEach((e) => e.reset()), this.search !== void 0 && this.search.clear(), this.emit(Q.Closed);
  }
  /**
   * Clears memory
   */
  destroy() {
    var e;
    this.items.forEach((t) => t.destroy()), this.nodes.popover.remove(), this.listeners.removeAll(), (e = this.search) == null || e.destroy();
  }
  /**
   * Looks for the item by name and imitates click on it
   *
   * @param name - name of the item to activate
   */
  activateItemByName(e) {
    const t = this.items.find((r) => r.name === e);
    this.handleItemClick(t);
  }
  /**
   * Factory method for creating popover items
   *
   * @param items - list of items params
   */
  buildItems(e) {
    return e.map((t) => {
      switch (t.type) {
        case D.Separator:
          return new _n();
        case D.Html:
          return new qe(t, this.itemsRenderParams[D.Html]);
        default:
          return new fe(t, this.itemsRenderParams[D.Default]);
      }
    });
  }
  /**
   * Retrieves popover item that is the target of the specified event
   *
   * @param event - event to retrieve popover item from
   */
  getTargetItem(e) {
    return this.items.filter((t) => t instanceof fe || t instanceof qe).find((t) => {
      const r = t.getElement();
      return r === null ? !1 : e.composedPath().includes(r);
    });
  }
  /**
   * Handles popover item click
   *
   * @param item - item to handle click of
   */
  handleItemClick(e) {
    if (!("isDisabled" in e && e.isDisabled)) {
      if (e.hasChildren) {
        this.showNestedItems(e), "handleClick" in e && typeof e.handleClick == "function" && e.handleClick();
        return;
      }
      this.itemsDefault.filter((t) => t !== e).forEach((t) => t.reset()), "handleClick" in e && typeof e.handleClick == "function" && e.handleClick(), this.toggleItemActivenessIfNeeded(e), e.closeOnActivate && (this.hide(), this.emit(Q.ClosedOnActivate));
    }
  }
  /**
   * Handles clicks inside popover
   *
   * @param event - item to handle click of
   */
  handleClick(e) {
    const t = this.getTargetItem(e);
    t !== void 0 && this.handleItemClick(t);
  }
  /**
   * - Toggles item active state, if clicked popover item has property 'toggle' set to true.
   *
   * - Performs radiobutton-like behavior if the item has property 'toggle' set to string key.
   * (All the other items with the same key get inactive, and the item gets active)
   *
   * @param clickedItem - popover item that was clicked
   */
  toggleItemActivenessIfNeeded(e) {
    if (e instanceof fe && (e.toggle === !0 && e.toggleActive(), typeof e.toggle == "string")) {
      const t = this.itemsDefault.filter((r) => r.toggle === e.toggle);
      if (t.length === 1) {
        e.toggleActive();
        return;
      }
      t.forEach((r) => {
        r.toggleActive(r === e);
      });
    }
  }
}
var pt = /* @__PURE__ */ ((o) => (o.Search = "search", o))(pt || {});
const At = he("cdx-search-field"), Pt = {
  wrapper: At(),
  icon: At("icon"),
  input: At("input")
};
class ma extends Ye {
  /**
   * @param options - available config
   * @param options.items - searchable items list
   * @param options.placeholder - input placeholder
   */
  constructor({ items: e, placeholder: t }) {
    super(), this.listeners = new Xe(), this.items = e, this.wrapper = g.make("div", Pt.wrapper);
    const r = g.make("div", Pt.icon, {
      innerHTML: da
    });
    this.input = g.make("input", Pt.input, {
      placeholder: t,
      /**
       * Used to prevent focusing on the input by Tab key
       * (Popover in the Toolbar lays below the blocks,
       * so Tab in the last block will focus this hidden input if this property is not set)
       */
      tabIndex: -1
    }), this.wrapper.appendChild(r), this.wrapper.appendChild(this.input), this.listeners.on(this.input, "input", () => {
      this.searchQuery = this.input.value, this.emit(pt.Search, {
        query: this.searchQuery,
        items: this.foundItems
      });
    });
  }
  /**
   * Returns search field element
   */
  getElement() {
    return this.wrapper;
  }
  /**
   * Sets focus to the input
   */
  focus() {
    this.input.focus();
  }
  /**
   * Clears search query and results
   */
  clear() {
    this.input.value = "", this.searchQuery = "", this.emit(pt.Search, {
      query: "",
      items: this.foundItems
    });
  }
  /**
   * Clears memory
   */
  destroy() {
    this.listeners.removeAll();
  }
  /**
   * Returns list of found items for the current search query
   */
  get foundItems() {
    return this.items.filter((e) => this.checkItem(e));
  }
  /**
   * Contains logic for checking whether passed item conforms the search query
   *
   * @param item - item to be checked
   */
  checkItem(e) {
    var t, r;
    const n = ((t = e.title) == null ? void 0 : t.toLowerCase()) || "", i = (r = this.searchQuery) == null ? void 0 : r.toLowerCase();
    return i !== void 0 ? n.includes(i) : !1;
  }
}
var va = Object.defineProperty, ba = Object.getOwnPropertyDescriptor, ka = (o, e, t, r) => {
  for (var n = ba(e, t), i = o.length - 1, s; i >= 0; i--)
    (s = o[i]) && (n = s(e, t, n) || n);
  return n && va(e, t, n), n;
};
const In = class On extends Ln {
  /**
   * Construct the instance
   *
   * @param params - popover params
   * @param itemsRenderParams – popover item render params.
   * The parameters that are not set by user via popover api but rather depend on technical implementation
   */
  constructor(e, t) {
    super(e, t), this.nestingLevel = 0, this.nestedPopoverTriggerItem = null, this.previouslyHoveredItem = null, this.scopeElement = document.body, this.hide = () => {
      var r;
      super.hide(), this.destroyNestedPopoverIfExists(), (r = this.flipper) == null || r.deactivate(), this.previouslyHoveredItem = null;
    }, this.onFlip = () => {
      const r = this.itemsDefault.find((n) => n.isFocused);
      r == null || r.onFocus();
    }, this.onSearch = (r) => {
      var n;
      const i = r.query === "", s = r.items.length === 0;
      this.items.forEach((l) => {
        let c = !1;
        l instanceof fe ? c = !r.items.includes(l) : (l instanceof _n || l instanceof qe) && (c = s || !i), l.toggleHidden(c);
      }), this.toggleNothingFoundMessage(s);
      const a = r.query === "" ? this.flippableElements : r.items.map((l) => l.getElement());
      (n = this.flipper) != null && n.isActivated && (this.flipper.deactivate(), this.flipper.activate(a));
    }, e.nestingLevel !== void 0 && (this.nestingLevel = e.nestingLevel), this.nestingLevel > 0 && this.nodes.popover.classList.add(F.popoverNested), e.scopeElement !== void 0 && (this.scopeElement = e.scopeElement), this.nodes.popoverContainer !== null && this.listeners.on(this.nodes.popoverContainer, "mouseover", (r) => this.handleHover(r)), e.searchable && this.addSearch(), e.flippable !== !1 && (this.flipper = new ut({
      items: this.flippableElements,
      focusedItemClass: H.focused,
      allowedKeys: [
        A.TAB,
        A.UP,
        A.DOWN,
        A.ENTER
      ]
    }), this.flipper.onFlip(this.onFlip));
  }
  /**
   * Returns true if some item inside popover is focused
   */
  hasFocus() {
    return this.flipper === void 0 ? !1 : this.flipper.hasFocus();
  }
  /**
   * Scroll position inside items container of the popover
   */
  get scrollTop() {
    return this.nodes.items === null ? 0 : this.nodes.items.scrollTop;
  }
  /**
   * Returns visible element offset top
   */
  get offsetTop() {
    return this.nodes.popoverContainer === null ? 0 : this.nodes.popoverContainer.offsetTop;
  }
  /**
   * Open popover
   */
  show() {
    var e;
    this.nodes.popover.style.setProperty(Te.PopoverHeight, this.size.height + "px"), this.shouldOpenBottom || this.nodes.popover.classList.add(F.popoverOpenTop), this.shouldOpenRight || this.nodes.popover.classList.add(F.popoverOpenLeft), super.show(), (e = this.flipper) == null || e.activate(this.flippableElements);
  }
  /**
   * Clears memory
   */
  destroy() {
    this.hide(), super.destroy();
  }
  /**
   * Handles displaying nested items for the item.
   *
   * @param item – item to show nested popover for
   */
  showNestedItems(e) {
    this.nestedPopover !== null && this.nestedPopover !== void 0 || (this.nestedPopoverTriggerItem = e, this.showNestedPopoverForItem(e));
  }
  /**
   * Handles hover events inside popover items container
   *
   * @param event - hover event data
   */
  handleHover(e) {
    const t = this.getTargetItem(e);
    t !== void 0 && this.previouslyHoveredItem !== t && (this.destroyNestedPopoverIfExists(), this.previouslyHoveredItem = t, t.hasChildren && this.showNestedPopoverForItem(t));
  }
  /**
   * Sets CSS variable with position of item near which nested popover should be displayed.
   * Is used for correct positioning of the nested popover
   *
   * @param nestedPopoverEl - nested popover element
   * @param item – item near which nested popover should be displayed
   */
  setTriggerItemPosition(e, t) {
    const r = t.getElement(), n = (r ? r.offsetTop : 0) - this.scrollTop, i = this.offsetTop + n;
    e.style.setProperty(Te.TriggerItemTop, i + "px");
  }
  /**
   * Destroys existing nested popover
   */
  destroyNestedPopoverIfExists() {
    var e, t;
    this.nestedPopover === void 0 || this.nestedPopover === null || (this.nestedPopover.off(Q.ClosedOnActivate, this.hide), this.nestedPopover.hide(), this.nestedPopover.destroy(), this.nestedPopover.getElement().remove(), this.nestedPopover = null, (e = this.flipper) == null || e.activate(this.flippableElements), (t = this.nestedPopoverTriggerItem) == null || t.onChildrenClose());
  }
  /**
   * Creates and displays nested popover for specified item.
   * Is used only on desktop
   *
   * @param item - item to display nested popover by
   */
  showNestedPopoverForItem(e) {
    var t;
    this.nestedPopover = new On({
      searchable: e.isChildrenSearchable,
      items: e.children,
      nestingLevel: this.nestingLevel + 1,
      flippable: e.isChildrenFlippable,
      messages: this.messages
    }), e.onChildrenOpen(), this.nestedPopover.on(Q.ClosedOnActivate, this.hide);
    const r = this.nestedPopover.getElement();
    return this.nodes.popover.appendChild(r), this.setTriggerItemPosition(r, e), r.style.setProperty(Te.NestingLevel, this.nestedPopover.nestingLevel.toString()), this.nestedPopover.show(), (t = this.flipper) == null || t.deactivate(), this.nestedPopover;
  }
  /**
   * Checks if popover should be opened bottom.
   * It should happen when there is enough space below or not enough space above
   */
  get shouldOpenBottom() {
    if (this.nodes.popover === void 0 || this.nodes.popover === null)
      return !1;
    const e = this.nodes.popoverContainer.getBoundingClientRect(), t = this.scopeElement.getBoundingClientRect(), r = this.size.height, n = e.top + r, i = e.top - r, s = Math.min(window.innerHeight, t.bottom);
    return i < t.top || n <= s;
  }
  /**
   * Checks if popover should be opened left.
   * It should happen when there is enough space in the right or not enough space in the left
   */
  get shouldOpenRight() {
    if (this.nodes.popover === void 0 || this.nodes.popover === null)
      return !1;
    const e = this.nodes.popover.getBoundingClientRect(), t = this.scopeElement.getBoundingClientRect(), r = this.size.width, n = e.right + r, i = e.left - r, s = Math.min(window.innerWidth, t.right);
    return i < t.left || n <= s;
  }
  get size() {
    var e;
    const t = {
      height: 0,
      width: 0
    };
    if (this.nodes.popover === null)
      return t;
    const r = this.nodes.popover.cloneNode(!0);
    r.style.visibility = "hidden", r.style.position = "absolute", r.style.top = "-1000px", r.classList.add(F.popoverOpened), (e = r.querySelector("." + F.popoverNested)) == null || e.remove(), document.body.appendChild(r);
    const n = r.querySelector("." + F.popoverContainer);
    return t.height = n.offsetHeight, t.width = n.offsetWidth, r.remove(), t;
  }
  /**
   * Returns list of elements available for keyboard navigation.
   */
  get flippableElements() {
    return this.items.map((e) => {
      if (e instanceof fe)
        return e.getElement();
      if (e instanceof qe)
        return e.getControls();
    }).flat().filter((e) => e != null);
  }
  /**
   * Adds search to the popover
   */
  addSearch() {
    this.search = new ma({
      items: this.itemsDefault,
      placeholder: this.messages.search
    }), this.search.on(pt.Search, this.onSearch);
    const e = this.search.getElement();
    e.classList.add(F.search), this.nodes.popoverContainer.insertBefore(e, this.nodes.popoverContainer.firstChild);
  }
  /**
   * Toggles nothing found message visibility
   *
   * @param isDisplayed - true if the message should be displayed
   */
  toggleNothingFoundMessage(e) {
    this.nodes.nothingFoundMessage.classList.toggle(F.nothingFoundMessageDisplayed, e);
  }
};
ka([
  Me
], In.prototype, "size");
let io = In;
class wa extends io {
  /**
   * Constructs the instance
   *
   * @param params - instance parameters
   */
  constructor(e) {
    const t = !_e();
    super(
      {
        ...e,
        class: F.popoverInline
      },
      {
        [D.Default]: {
          /**
           * We use button instead of div here to fix bug associated with focus loss (which leads to selection change) on click in safari
           *
           * @todo figure out better way to solve the issue
           */
          wrapperTag: "button",
          hint: {
            position: "top",
            alignment: "center",
            enabled: t
          }
        },
        [D.Html]: {
          hint: {
            position: "top",
            alignment: "center",
            enabled: t
          }
        }
      }
    ), this.items.forEach((r) => {
      !(r instanceof fe) && !(r instanceof qe) || r.hasChildren && r.isChildrenOpen && this.showNestedItems(r);
    });
  }
  /**
   * Returns visible element offset top
   */
  get offsetLeft() {
    return this.nodes.popoverContainer === null ? 0 : this.nodes.popoverContainer.offsetLeft;
  }
  /**
   * Open popover
   */
  show() {
    this.nestingLevel === 0 && this.nodes.popover.style.setProperty(
      Te.InlinePopoverWidth,
      this.size.width + "px"
    ), super.show();
  }
  /**
   * Disable hover event handling.
   * Overrides parent's class behavior
   */
  handleHover() {
  }
  /**
   * Sets CSS variable with position of item near which nested popover should be displayed.
   * Is used to position nested popover right below clicked item
   *
   * @param nestedPopoverEl - nested popover element
   * @param item – item near which nested popover should be displayed
   */
  setTriggerItemPosition(e, t) {
    const r = t.getElement(), n = r ? r.offsetLeft : 0, i = this.offsetLeft + n;
    e.style.setProperty(
      Te.TriggerItemLeft,
      i + "px"
    );
  }
  /**
   * Handles displaying nested items for the item.
   * Overriding in order to add toggling behaviour
   *
   * @param item – item to toggle nested popover for
   */
  showNestedItems(e) {
    if (this.nestedPopoverTriggerItem === e) {
      this.destroyNestedPopoverIfExists(), this.nestedPopoverTriggerItem = null;
      return;
    }
    super.showNestedItems(e);
  }
  /**
   * Creates and displays nested popover for specified item.
   * Is used only on desktop
   *
   * @param item - item to display nested popover by
   */
  showNestedPopoverForItem(e) {
    const t = super.showNestedPopoverForItem(e);
    return t.getElement().classList.add(F.getPopoverNestedClass(t.nestingLevel)), t;
  }
  /**
   * Overrides default item click handling.
   * Helps to close nested popover once other item is clicked.
   *
   * @param item - clicked item
   */
  handleItemClick(e) {
    var t;
    e !== this.nestedPopoverTriggerItem && ((t = this.nestedPopoverTriggerItem) == null || t.handleClick(), super.destroyNestedPopoverIfExists()), super.handleItemClick(e);
  }
}
const An = class He {
  constructor() {
    this.scrollPosition = null;
  }
  /**
   * Locks body element scroll
   */
  lock() {
    Vt ? this.lockHard() : document.body.classList.add(He.CSS.scrollLocked);
  }
  /**
   * Unlocks body element scroll
   */
  unlock() {
    Vt ? this.unlockHard() : document.body.classList.remove(He.CSS.scrollLocked);
  }
  /**
   * Locks scroll in a hard way (via setting fixed position to body element)
   */
  lockHard() {
    this.scrollPosition = window.pageYOffset, document.documentElement.style.setProperty(
      "--window-scroll-offset",
      `${this.scrollPosition}px`
    ), document.body.classList.add(He.CSS.scrollLockedHard);
  }
  /**
   * Unlocks hard scroll lock
   */
  unlockHard() {
    document.body.classList.remove(He.CSS.scrollLockedHard), this.scrollPosition !== null && window.scrollTo(0, this.scrollPosition), this.scrollPosition = null;
  }
};
An.CSS = {
  scrollLocked: "ce-scroll-locked",
  scrollLockedHard: "ce-scroll-locked--hard"
};
let ya = An;
const Nt = he("ce-popover-header"), jt = {
  root: Nt(),
  text: Nt("text"),
  backButton: Nt("back-button")
};
class xa {
  /**
   * Constructs the instance
   *
   * @param params - popover header params
   */
  constructor({ text: e, onBackButtonClick: t }) {
    this.listeners = new Xe(), this.text = e, this.onBackButtonClick = t, this.nodes = {
      root: g.make("div", [jt.root]),
      backButton: g.make("button", [jt.backButton]),
      text: g.make("div", [jt.text])
    }, this.nodes.backButton.innerHTML = oa, this.nodes.root.appendChild(this.nodes.backButton), this.listeners.on(this.nodes.backButton, "click", this.onBackButtonClick), this.nodes.text.innerText = this.text, this.nodes.root.appendChild(this.nodes.text);
  }
  /**
   * Returns popover header root html element
   */
  getElement() {
    return this.nodes.root;
  }
  /**
   * Destroys the instance
   */
  destroy() {
    this.nodes.root.remove(), this.listeners.destroy();
  }
}
class Ca {
  constructor() {
    this.history = [];
  }
  /**
   * Push new popover state
   *
   * @param state - new state
   */
  push(e) {
    this.history.push(e);
  }
  /**
   * Pop last popover state
   */
  pop() {
    return this.history.pop();
  }
  /**
   * Title retrieved from the current state
   */
  get currentTitle() {
    return this.history.length === 0 ? "" : this.history[this.history.length - 1].title;
  }
  /**
   * Items list retrieved from the current state
   */
  get currentItems() {
    return this.history.length === 0 ? [] : this.history[this.history.length - 1].items;
  }
  /**
   * Returns history to initial popover state
   */
  reset() {
    for (; this.history.length > 1; )
      this.pop();
  }
}
class Pn extends Ln {
  /**
   * Construct the instance
   *
   * @param params - popover params
   */
  constructor(e) {
    super(e, {
      [D.Default]: {
        hint: {
          enabled: !1
        }
      },
      [D.Html]: {
        hint: {
          enabled: !1
        }
      }
    }), this.scrollLocker = new ya(), this.history = new Ca(), this.isHidden = !0, this.nodes.overlay = g.make("div", [F.overlay, F.overlayHidden]), this.nodes.popover.insertBefore(this.nodes.overlay, this.nodes.popover.firstChild), this.listeners.on(this.nodes.overlay, "click", () => {
      this.hide();
    }), this.history.push({ items: e.items });
  }
  /**
   * Open popover
   */
  show() {
    this.nodes.overlay.classList.remove(F.overlayHidden), super.show(), this.scrollLocker.lock(), this.isHidden = !1;
  }
  /**
   * Closes popover
   */
  hide() {
    this.isHidden || (super.hide(), this.nodes.overlay.classList.add(F.overlayHidden), this.scrollLocker.unlock(), this.history.reset(), this.isHidden = !0);
  }
  /**
   * Clears memory
   */
  destroy() {
    super.destroy(), this.scrollLocker.unlock();
  }
  /**
   * Handles displaying nested items for the item
   *
   * @param item – item to show nested popover for
   */
  showNestedItems(e) {
    this.updateItemsAndHeader(e.children, e.title), this.history.push({
      title: e.title,
      items: e.children
    });
  }
  /**
   * Removes rendered popover items and header and displays new ones
   *
   * @param items - new popover items
   * @param title - new popover header text
   */
  updateItemsAndHeader(e, t) {
    if (this.header !== null && this.header !== void 0 && (this.header.destroy(), this.header = null), t !== void 0) {
      this.header = new xa({
        text: t,
        onBackButtonClick: () => {
          this.history.pop(), this.updateItemsAndHeader(this.history.currentItems, this.history.currentTitle);
        }
      });
      const r = this.header.getElement();
      r !== null && this.nodes.popoverContainer.insertBefore(r, this.nodes.popoverContainer.firstChild);
    }
    this.items.forEach((r) => {
      var n;
      return (n = r.getElement()) == null ? void 0 : n.remove();
    }), this.items = this.buildItems(e), this.items.forEach((r) => {
      var n;
      const i = r.getElement();
      i !== null && ((n = this.nodes.items) == null || n.appendChild(i));
    });
  }
}
class Ea extends N {
  constructor() {
    super(...arguments), this.opened = !1, this.hasMobileLayoutToggleListener = !1, this.selection = new L(), this.popover = null, this.close = () => {
      this.opened && (this.opened = !1, L.isAtEditor || this.selection.restore(), this.selection.clearSaved(), !this.Editor.CrossBlockSelection.isCrossBlockSelectionStarted && this.Editor.BlockManager.currentBlock && this.Editor.BlockSelection.unselectBlock(this.Editor.BlockManager.currentBlock), this.eventsDispatcher.emit(this.events.closed), this.popover && (this.popover.off(Q.Closed, this.onPopoverClose), this.popover.destroy(), this.popover.getElement().remove(), this.popover = null));
    }, this.onPopoverClose = () => {
      this.close();
    };
  }
  /**
   * Module Events
   */
  get events() {
    return {
      opened: "block-settings-opened",
      closed: "block-settings-closed"
    };
  }
  /**
   * Block Settings CSS
   */
  get CSS() {
    return {
      settings: "ce-settings"
    };
  }
  /**
   * Getter for inner popover's flipper instance
   *
   * @todo remove once BlockSettings becomes standalone non-module class
   */
  get flipper() {
    var e;
    if (this.popover !== null)
      return "flipper" in this.popover ? (e = this.popover) == null ? void 0 : e.flipper : void 0;
  }
  /**
   * Panel with block settings with 2 sections:
   *  - Tool's Settings
   *  - Default Settings [Move, Remove, etc]
   */
  make() {
    this.nodes.wrapper = g.make("div", [this.CSS.settings]), this.eventsDispatcher.on(We, this.close), this.hasMobileLayoutToggleListener = !0;
  }
  /**
   * Destroys module
   */
  destroy() {
    this.removeAllNodes(), this.listeners.destroy(), this.hasMobileLayoutToggleListener && (this.eventsDispatcher.off(We, this.close), this.hasMobileLayoutToggleListener = !1);
  }
  /**
   * Open Block Settings pane
   *
   * @param targetBlock - near which Block we should open BlockSettings
   */
  async open(e = this.Editor.BlockManager.currentBlock) {
    var t;
    this.opened = !0, this.selection.save(), this.Editor.BlockSelection.selectBlock(e), this.Editor.BlockSelection.clearCache();
    const { toolTunes: r, commonTunes: n } = e.getTunes();
    this.eventsDispatcher.emit(this.events.opened);
    const i = _e() ? Pn : io;
    this.popover = new i({
      searchable: !0,
      items: await this.getTunesItems(e, n, r),
      scopeElement: this.Editor.API.methods.ui.nodes.redactor,
      messages: {
        nothingFound: W.ui(K.ui.popover, "Nothing found"),
        search: W.ui(K.ui.popover, "Filter")
      }
    }), this.popover.on(Q.Closed, this.onPopoverClose), (t = this.nodes.wrapper) == null || t.append(this.popover.getElement()), this.popover.show();
  }
  /**
   * Returns root block settings element
   */
  getElement() {
    return this.nodes.wrapper;
  }
  /**
   * Returns list of items to be displayed in block tunes menu.
   * Merges tool specific tunes, conversion menu and common tunes in one list in predefined order
   *
   * @param currentBlock –  block we are about to open block tunes for
   * @param commonTunes – common tunes
   * @param toolTunes - tool specific tunes
   */
  async getTunesItems(e, t, r) {
    const n = [];
    r !== void 0 && r.length > 0 && (n.push(...r), n.push({
      type: D.Separator
    }));
    const i = Array.from(this.Editor.Tools.blockTools.values()), s = (await yn(e, i)).reduce((a, l) => (l.toolbox.forEach((c) => {
      a.push({
        icon: c.icon,
        title: W.t(K.toolNames, c.title),
        name: l.name,
        closeOnActivate: !0,
        onActivate: async () => {
          const { BlockManager: d, Caret: h, Toolbar: u } = this.Editor, f = await d.convert(e, l.name, c.data);
          u.close(), h.setToBlock(f, h.positions.END);
        }
      });
    }), a), []);
    return s.length > 0 && (n.push({
      icon: Mn,
      name: "convert-to",
      title: W.ui(K.ui.popover, "Convert to"),
      children: {
        searchable: !0,
        items: s
      }
    }), n.push({
      type: D.Separator
    })), n.push(...t), n.map((a) => this.resolveTuneAliases(a));
  }
  /**
   * Resolves aliases in tunes menu items
   *
   * @param item - item with resolved aliases
   */
  resolveTuneAliases(e) {
    if (e.type === D.Separator || e.type === D.Html)
      return e;
    const t = Qs(e, { label: "title" });
    return e.confirmation && (t.confirmation = this.resolveTuneAliases(e.confirmation)), t;
  }
}
var Nn = { exports: {} };
/*!
 * Library for handling keyboard shortcuts
 * @copyright CodeX (https://codex.so)
 * @license MIT
 * @author CodeX (https://codex.so)
 * @version 1.2.0
 */
(function(o, e) {
  (function(t, r) {
    o.exports = r();
  })(window, function() {
    return (function(t) {
      var r = {};
      function n(i) {
        if (r[i])
          return r[i].exports;
        var s = r[i] = { i, l: !1, exports: {} };
        return t[i].call(s.exports, s, s.exports, n), s.l = !0, s.exports;
      }
      return n.m = t, n.c = r, n.d = function(i, s, a) {
        n.o(i, s) || Object.defineProperty(i, s, { enumerable: !0, get: a });
      }, n.r = function(i) {
        typeof Symbol < "u" && Symbol.toStringTag && Object.defineProperty(i, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(i, "__esModule", { value: !0 });
      }, n.t = function(i, s) {
        if (1 & s && (i = n(i)), 8 & s || 4 & s && typeof i == "object" && i && i.__esModule)
          return i;
        var a = /* @__PURE__ */ Object.create(null);
        if (n.r(a), Object.defineProperty(a, "default", { enumerable: !0, value: i }), 2 & s && typeof i != "string")
          for (var l in i)
            n.d(a, l, (function(c) {
              return i[c];
            }).bind(null, l));
        return a;
      }, n.n = function(i) {
        var s = i && i.__esModule ? function() {
          return i.default;
        } : function() {
          return i;
        };
        return n.d(s, "a", s), s;
      }, n.o = function(i, s) {
        return Object.prototype.hasOwnProperty.call(i, s);
      }, n.p = "", n(n.s = 0);
    })([function(t, r, n) {
      function i(l, c) {
        for (var d = 0; d < c.length; d++) {
          var h = c[d];
          h.enumerable = h.enumerable || !1, h.configurable = !0, "value" in h && (h.writable = !0), Object.defineProperty(l, h.key, h);
        }
      }
      function s(l, c, d) {
        return c && i(l.prototype, c), d && i(l, d), l;
      }
      n.r(r);
      var a = (function() {
        function l(c) {
          var d = this;
          (function(h, u) {
            if (!(h instanceof u))
              throw new TypeError("Cannot call a class as a function");
          })(this, l), this.commands = {}, this.keys = {}, this.name = c.name, this.parseShortcutName(c.name), this.element = c.on, this.callback = c.callback, this.executeShortcut = function(h) {
            d.execute(h);
          }, this.element.addEventListener("keydown", this.executeShortcut, !1);
        }
        return s(l, null, [{ key: "supportedCommands", get: function() {
          return { SHIFT: ["SHIFT"], CMD: ["CMD", "CONTROL", "COMMAND", "WINDOWS", "CTRL"], ALT: ["ALT", "OPTION"] };
        } }, { key: "keyCodes", get: function() {
          return { 0: 48, 1: 49, 2: 50, 3: 51, 4: 52, 5: 53, 6: 54, 7: 55, 8: 56, 9: 57, A: 65, B: 66, C: 67, D: 68, E: 69, F: 70, G: 71, H: 72, I: 73, J: 74, K: 75, L: 76, M: 77, N: 78, O: 79, P: 80, Q: 81, R: 82, S: 83, T: 84, U: 85, V: 86, W: 87, X: 88, Y: 89, Z: 90, BACKSPACE: 8, ENTER: 13, ESCAPE: 27, LEFT: 37, UP: 38, RIGHT: 39, DOWN: 40, INSERT: 45, DELETE: 46, ".": 190 };
        } }]), s(l, [{ key: "parseShortcutName", value: function(c) {
          c = c.split("+");
          for (var d = 0; d < c.length; d++) {
            c[d] = c[d].toUpperCase();
            var h = !1;
            for (var u in l.supportedCommands)
              if (l.supportedCommands[u].includes(c[d])) {
                h = this.commands[u] = !0;
                break;
              }
            h || (this.keys[c[d]] = !0);
          }
          for (var f in l.supportedCommands)
            this.commands[f] || (this.commands[f] = !1);
        } }, { key: "execute", value: function(c) {
          var d, h = { CMD: c.ctrlKey || c.metaKey, SHIFT: c.shiftKey, ALT: c.altKey }, u = !0;
          for (d in this.commands)
            this.commands[d] !== h[d] && (u = !1);
          var f, p = !0;
          for (f in this.keys)
            p = p && c.keyCode === l.keyCodes[f];
          u && p && this.callback(c);
        } }, { key: "remove", value: function() {
          this.element.removeEventListener("keydown", this.executeShortcut);
        } }]), l;
      })();
      r.default = a;
    }]).default;
  });
})(Nn);
var Ta = Nn.exports;
const Sa = /* @__PURE__ */ vt(Ta);
class Ba {
  constructor() {
    this.registeredShortcuts = /* @__PURE__ */ new Map();
  }
  /**
   * Register shortcut
   *
   * @param shortcut - shortcut options
   */
  add(e) {
    if (this.findShortcut(e.on, e.name))
      throw Error(
        `Shortcut ${e.name} is already registered for ${e.on}. Please remove it before add a new handler.`
      );
    const t = new Sa({
      name: e.name,
      on: e.on,
      callback: e.handler
    }), r = this.registeredShortcuts.get(e.on) || [];
    this.registeredShortcuts.set(e.on, [...r, t]);
  }
  /**
   * Remove shortcut
   *
   * @param element - Element shortcut is set for
   * @param name - shortcut name
   */
  remove(e, t) {
    const r = this.findShortcut(e, t);
    if (!r)
      return;
    r.remove();
    const n = this.registeredShortcuts.get(e).filter((i) => i !== r);
    if (n.length === 0) {
      this.registeredShortcuts.delete(e);
      return;
    }
    this.registeredShortcuts.set(e, n);
  }
  /**
   * Get Shortcut instance if exist
   *
   * @param element - Element shorcut is set for
   * @param shortcut - shortcut name
   * @returns {number} index - shortcut index if exist
   */
  findShortcut(e, t) {
    return (this.registeredShortcuts.get(e) || []).find(({ name: r }) => r === t);
  }
}
const Be = new Ba();
var Ma = Object.defineProperty, _a = Object.getOwnPropertyDescriptor, jn = (o, e, t, r) => {
  for (var n = _a(e, t), i = o.length - 1, s; i >= 0; i--)
    (s = o[i]) && (n = s(e, t, n) || n);
  return n && Ma(e, t, n), n;
}, Qe = /* @__PURE__ */ ((o) => (o.Opened = "toolbox-opened", o.Closed = "toolbox-closed", o.BlockAdded = "toolbox-block-added", o))(Qe || {});
const so = class Dn extends Ye {
  /**
   * Toolbox constructor
   *
   * @param options - available parameters
   * @param options.api - Editor API methods
   * @param options.tools - Tools available to check whether some of them should be displayed at the Toolbox or not
   */
  constructor({ api: e, tools: t, i18nLabels: r }) {
    super(), this.opened = !1, this.listeners = new Xe(), this.popover = null, this.handleMobileLayoutToggle = () => {
      this.destroyPopover(), this.initPopover();
    }, this.onPopoverClose = () => {
      this.opened = !1, this.emit(
        "toolbox-closed"
        /* Closed */
      );
    }, this.api = e, this.tools = t, this.i18nLabels = r, this.enableShortcuts(), this.nodes = {
      toolbox: g.make("div", Dn.CSS.toolbox)
    }, this.initPopover(), this.api.events.on(We, this.handleMobileLayoutToggle);
  }
  /**
   * Returns True if Toolbox is Empty and nothing to show
   *
   * @returns {boolean}
   */
  get isEmpty() {
    return this.toolsToBeDisplayed.length === 0;
  }
  /**
   * CSS styles
   */
  static get CSS() {
    return {
      toolbox: "ce-toolbox"
    };
  }
  /**
   * Returns root block settings element
   */
  getElement() {
    return this.nodes.toolbox;
  }
  /**
   * Returns true if the Toolbox has the Flipper activated and the Flipper has selected button
   */
  hasFocus() {
    if (this.popover !== null)
      return "hasFocus" in this.popover ? this.popover.hasFocus() : void 0;
  }
  /**
   * Destroy Module
   */
  destroy() {
    var e;
    super.destroy(), this.nodes && this.nodes.toolbox && this.nodes.toolbox.remove(), this.removeAllShortcuts(), (e = this.popover) == null || e.off(Q.Closed, this.onPopoverClose), this.listeners.destroy(), this.api.events.off(We, this.handleMobileLayoutToggle);
  }
  /**
   * Toolbox Tool's button click handler
   *
   * @param toolName - tool type to be activated
   * @param blockDataOverrides - Block data predefined by the activated Toolbox item
   */
  toolButtonActivated(e, t) {
    this.insertNewBlock(e, t);
  }
  /**
   * Open Toolbox with Tools
   */
  open() {
    var e;
    this.isEmpty || ((e = this.popover) == null || e.show(), this.opened = !0, this.emit(
      "toolbox-opened"
      /* Opened */
    ));
  }
  /**
   * Close Toolbox
   */
  close() {
    var e;
    (e = this.popover) == null || e.hide(), this.opened = !1, this.emit(
      "toolbox-closed"
      /* Closed */
    );
  }
  /**
   * Close Toolbox
   */
  toggle() {
    this.opened ? this.close() : this.open();
  }
  /**
   * Creates toolbox popover and appends it inside wrapper element
   */
  initPopover() {
    var e;
    const t = _e() ? Pn : io;
    this.popover = new t({
      scopeElement: this.api.ui.nodes.redactor,
      searchable: !0,
      messages: {
        nothingFound: this.i18nLabels.nothingFound,
        search: this.i18nLabels.filter
      },
      items: this.toolboxItemsToBeDisplayed
    }), this.popover.on(Q.Closed, this.onPopoverClose), (e = this.nodes.toolbox) == null || e.append(this.popover.getElement());
  }
  /**
   * Destroys popover instance and removes it from DOM
   */
  destroyPopover() {
    this.popover !== null && (this.popover.hide(), this.popover.off(Q.Closed, this.onPopoverClose), this.popover.destroy(), this.popover = null), this.nodes.toolbox !== null && (this.nodes.toolbox.innerHTML = "");
  }
  get toolsToBeDisplayed() {
    const e = [];
    return this.tools.forEach((t) => {
      t.toolbox && e.push(t);
    }), e;
  }
  get toolboxItemsToBeDisplayed() {
    const e = (t, r, n = !0) => ({
      icon: t.icon,
      title: W.t(K.toolNames, t.title || lt(r.name)),
      name: r.name,
      onActivate: () => {
        this.toolButtonActivated(r.name, t.data);
      },
      secondaryLabel: r.shortcut && n ? eo(r.shortcut) : ""
    });
    return this.toolsToBeDisplayed.reduce((t, r) => (Array.isArray(r.toolbox) ? r.toolbox.forEach((n, i) => {
      t.push(e(n, r, i === 0));
    }) : r.toolbox !== void 0 && t.push(e(r.toolbox, r)), t), []);
  }
  /**
   * Iterate all tools and enable theirs shortcuts if specified
   */
  enableShortcuts() {
    this.toolsToBeDisplayed.forEach((e) => {
      const t = e.shortcut;
      t && this.enableShortcutForTool(e.name, t);
    });
  }
  /**
   * Enable shortcut Block Tool implemented shortcut
   *
   * @param {string} toolName - Tool name
   * @param {string} shortcut - shortcut according to the ShortcutData Module format
   */
  enableShortcutForTool(e, t) {
    Be.add({
      name: t,
      on: this.api.ui.nodes.redactor,
      handler: async (r) => {
        r.preventDefault();
        const n = this.api.blocks.getCurrentBlockIndex(), i = this.api.blocks.getBlockByIndex(n);
        if (i)
          try {
            const s = await this.api.blocks.convert(i.id, e);
            this.api.caret.setToBlock(s, "end");
            return;
          } catch {
          }
        this.insertNewBlock(e);
      }
    });
  }
  /**
   * Removes all added shortcuts
   * Fired when the Read-Only mode is activated
   */
  removeAllShortcuts() {
    this.toolsToBeDisplayed.forEach((e) => {
      const t = e.shortcut;
      t && Be.remove(this.api.ui.nodes.redactor, t);
    });
  }
  /**
   * Inserts new block
   * Can be called when button clicked on Toolbox or by ShortcutData
   *
   * @param {string} toolName - Tool name
   * @param blockDataOverrides - predefined Block data
   */
  async insertNewBlock(e, t) {
    const r = this.api.blocks.getCurrentBlockIndex(), n = this.api.blocks.getBlockByIndex(r);
    if (!n)
      return;
    const i = n.isEmpty ? r : r + 1;
    let s;
    if (t) {
      const l = await this.api.blocks.composeBlockData(e);
      s = Object.assign(l, t);
    }
    const a = this.api.blocks.insert(
      e,
      s,
      void 0,
      i,
      void 0,
      n.isEmpty
    );
    a.call(oe.APPEND_CALLBACK), this.api.caret.setToBlock(i), this.emit("toolbox-block-added", {
      block: a
    }), this.api.toolbar.close();
  }
};
jn([
  Me
], so.prototype, "toolsToBeDisplayed");
jn([
  Me
], so.prototype, "toolboxItemsToBeDisplayed");
let La = so;
const Rn = "block hovered";
async function Ia(o, e) {
  const t = navigator.keyboard;
  if (!t)
    return e;
  try {
    return (await t.getLayoutMap()).get(o) || e;
  } catch (r) {
    return console.error(r), e;
  }
}
class Oa extends N {
  /**
   * @class
   * @param moduleConfiguration - Module Configuration
   * @param moduleConfiguration.config - Editor's config
   * @param moduleConfiguration.eventsDispatcher - Editor's event dispatcher
   */
  constructor({ config: e, eventsDispatcher: t }) {
    super({
      config: e,
      eventsDispatcher: t
    }), this.toolboxInstance = null;
  }
  /**
   * CSS styles
   *
   * @returns {object}
   */
  get CSS() {
    return {
      toolbar: "ce-toolbar",
      content: "ce-toolbar__content",
      actions: "ce-toolbar__actions",
      actionsOpened: "ce-toolbar__actions--opened",
      toolbarOpened: "ce-toolbar--opened",
      openedToolboxHolderModifier: "codex-editor--toolbox-opened",
      plusButton: "ce-toolbar__plus",
      plusButtonShortcut: "ce-toolbar__plus-shortcut",
      settingsToggler: "ce-toolbar__settings-btn",
      settingsTogglerHidden: "ce-toolbar__settings-btn--hidden"
    };
  }
  /**
   * Returns the Toolbar opening state
   *
   * @returns {boolean}
   */
  get opened() {
    return this.nodes.wrapper.classList.contains(this.CSS.toolbarOpened);
  }
  /**
   * Public interface for accessing the Toolbox
   */
  get toolbox() {
    var e;
    return {
      opened: (e = this.toolboxInstance) == null ? void 0 : e.opened,
      close: () => {
        var t;
        (t = this.toolboxInstance) == null || t.close();
      },
      open: () => {
        if (this.toolboxInstance === null) {
          j("toolbox.open() called before initialization is finished", "warn");
          return;
        }
        this.Editor.BlockManager.currentBlock = this.hoveredBlock, this.toolboxInstance.open();
      },
      toggle: () => {
        if (this.toolboxInstance === null) {
          j("toolbox.toggle() called before initialization is finished", "warn");
          return;
        }
        this.toolboxInstance.toggle();
      },
      hasFocus: () => {
        var t;
        return (t = this.toolboxInstance) == null ? void 0 : t.hasFocus();
      }
    };
  }
  /**
   * Block actions appearance manipulations
   */
  get blockActions() {
    return {
      hide: () => {
        this.nodes.actions.classList.remove(this.CSS.actionsOpened);
      },
      show: () => {
        this.nodes.actions.classList.add(this.CSS.actionsOpened);
      }
    };
  }
  /**
   * Methods for working with Block Tunes toggler
   */
  get blockTunesToggler() {
    return {
      hide: () => this.nodes.settingsToggler.classList.add(this.CSS.settingsTogglerHidden),
      show: () => this.nodes.settingsToggler.classList.remove(this.CSS.settingsTogglerHidden)
    };
  }
  /**
   * Toggles read-only mode
   *
   * @param {boolean} readOnlyEnabled - read-only mode
   */
  toggleReadOnly(e) {
    e ? (this.destroy(), this.Editor.BlockSettings.destroy(), this.disableModuleBindings()) : window.requestIdleCallback(() => {
      this.drawUI(), this.enableModuleBindings();
    }, { timeout: 2e3 });
  }
  /**
   * Move Toolbar to the passed (or current) Block
   *
   * @param block - block to move Toolbar near it
   */
  moveAndOpen(e = this.Editor.BlockManager.currentBlock) {
    if (this.toolboxInstance === null) {
      j("Can't open Toolbar since Editor initialization is not finished yet", "warn");
      return;
    }
    if (this.toolboxInstance.opened && this.toolboxInstance.close(), this.Editor.BlockSettings.opened && this.Editor.BlockSettings.close(), !e)
      return;
    this.hoveredBlock = e;
    const t = e.holder, { isMobile: r } = this.Editor.UI;
    let n;
    const i = 20, s = e.firstInput, a = t.getBoundingClientRect(), l = s !== void 0 ? s.getBoundingClientRect() : null, c = l !== null ? l.top - a.top : null, d = c !== null ? c > i : void 0;
    if (r)
      n = t.offsetTop + t.offsetHeight;
    else if (s === void 0 || d) {
      const h = parseInt(window.getComputedStyle(e.pluginsContent).paddingTop);
      n = t.offsetTop + h;
    } else {
      const h = gs(s), u = parseInt(window.getComputedStyle(this.nodes.plusButton).height, 10);
      n = t.offsetTop + h - u + 8 + c;
    }
    this.nodes.wrapper.style.top = `${Math.floor(n)}px`, this.Editor.BlockManager.blocks.length === 1 && e.isEmpty ? this.blockTunesToggler.hide() : this.blockTunesToggler.show(), this.open();
  }
  /**
   * Close the Toolbar
   */
  close() {
    var e, t;
    this.Editor.ReadOnly.isEnabled || ((e = this.nodes.wrapper) == null || e.classList.remove(this.CSS.toolbarOpened), this.blockActions.hide(), (t = this.toolboxInstance) == null || t.close(), this.Editor.BlockSettings.close(), this.reset());
  }
  /**
   * Reset the Toolbar position to prevent DOM height growth, for example after blocks deletion
   */
  reset() {
    this.nodes.wrapper.style.top = "unset";
  }
  /**
   * Open Toolbar with Plus Button and Actions
   *
   * @param {boolean} withBlockActions - by default, Toolbar opens with Block Actions.
   *                                     This flag allows to open Toolbar without Actions.
   */
  open(e = !0) {
    this.nodes.wrapper.classList.add(this.CSS.toolbarOpened), e ? this.blockActions.show() : this.blockActions.hide();
  }
  /**
   * Draws Toolbar elements
   */
  async make() {
    this.nodes.wrapper = g.make("div", this.CSS.toolbar), ["content", "actions"].forEach((i) => {
      this.nodes[i] = g.make("div", this.CSS[i]);
    }), g.append(this.nodes.wrapper, this.nodes.content), g.append(this.nodes.content, this.nodes.actions), this.nodes.plusButton = g.make("div", this.CSS.plusButton, {
      innerHTML: ca
    }), g.append(this.nodes.actions, this.nodes.plusButton), this.readOnlyMutableListeners.on(this.nodes.plusButton, "click", () => {
      dt(!0), this.plusButtonClicked();
    }, !1);
    const e = g.make("div");
    e.appendChild(document.createTextNode(W.ui(K.ui.toolbar.toolbox, "Add"))), e.appendChild(g.make("div", this.CSS.plusButtonShortcut, {
      textContent: "/"
    })), ht(this.nodes.plusButton, e, {
      hidingDelay: 400
    }), this.nodes.settingsToggler = g.make("span", this.CSS.settingsToggler, {
      innerHTML: la
    }), g.append(this.nodes.actions, this.nodes.settingsToggler);
    const t = g.make("div"), r = g.text(W.ui(K.ui.blockTunes.toggler, "Click to tune")), n = await Ia("Slash", "/");
    t.appendChild(r), t.appendChild(g.make("div", this.CSS.plusButtonShortcut, {
      textContent: eo(`CMD + ${n}`)
    })), ht(this.nodes.settingsToggler, t, {
      hidingDelay: 400
    }), g.append(this.nodes.actions, this.makeToolbox()), g.append(this.nodes.actions, this.Editor.BlockSettings.getElement()), g.append(this.Editor.UI.nodes.wrapper, this.nodes.wrapper);
  }
  /**
   * Creates the Toolbox instance and return it's rendered element
   */
  makeToolbox() {
    return this.toolboxInstance = new La({
      api: this.Editor.API.methods,
      tools: this.Editor.Tools.blockTools,
      i18nLabels: {
        filter: W.ui(K.ui.popover, "Filter"),
        nothingFound: W.ui(K.ui.popover, "Nothing found")
      }
    }), this.toolboxInstance.on(Qe.Opened, () => {
      this.Editor.UI.nodes.wrapper.classList.add(this.CSS.openedToolboxHolderModifier);
    }), this.toolboxInstance.on(Qe.Closed, () => {
      this.Editor.UI.nodes.wrapper.classList.remove(this.CSS.openedToolboxHolderModifier);
    }), this.toolboxInstance.on(Qe.BlockAdded, ({ block: e }) => {
      const { BlockManager: t, Caret: r } = this.Editor, n = t.getBlockById(e.id);
      n.inputs.length === 0 && (n === t.lastBlock ? (t.insertAtEnd(), r.setToBlock(t.lastBlock)) : r.setToBlock(t.nextBlock));
    }), this.toolboxInstance.getElement();
  }
  /**
   * Handler for Plus Button
   */
  plusButtonClicked() {
    var e;
    this.Editor.BlockManager.currentBlock = this.hoveredBlock, (e = this.toolboxInstance) == null || e.toggle();
  }
  /**
   * Enable bindings
   */
  enableModuleBindings() {
    this.readOnlyMutableListeners.on(this.nodes.settingsToggler, "mousedown", (e) => {
      var t;
      e.stopPropagation(), this.settingsTogglerClicked(), (t = this.toolboxInstance) != null && t.opened && this.toolboxInstance.close(), dt(!0);
    }, !0), _e() || this.eventsDispatcher.on(Rn, (e) => {
      var t;
      this.Editor.BlockSettings.opened || (t = this.toolboxInstance) != null && t.opened || this.moveAndOpen(e.block);
    });
  }
  /**
   * Disable bindings
   */
  disableModuleBindings() {
    this.readOnlyMutableListeners.clearAll();
  }
  /**
   * Clicks on the Block Settings toggler
   */
  settingsTogglerClicked() {
    this.Editor.BlockManager.currentBlock = this.hoveredBlock, this.Editor.BlockSettings.opened ? this.Editor.BlockSettings.close() : this.Editor.BlockSettings.open(this.hoveredBlock);
  }
  /**
   * Draws Toolbar UI
   *
   * Toolbar contains BlockSettings and Toolbox.
   * That's why at first we draw its components and then Toolbar itself
   *
   * Steps:
   *  - Make Toolbar dependent components like BlockSettings, Toolbox and so on
   *  - Make itself and append dependent nodes to itself
   *
   */
  drawUI() {
    this.Editor.BlockSettings.make(), this.make();
  }
  /**
   * Removes all created and saved HTMLElements
   * It is used in Read-Only mode
   */
  destroy() {
    this.removeAllNodes(), this.toolboxInstance && this.toolboxInstance.destroy();
  }
}
var ge = /* @__PURE__ */ ((o) => (o[o.Block = 0] = "Block", o[o.Inline = 1] = "Inline", o[o.Tune = 2] = "Tune", o))(ge || {}), et = /* @__PURE__ */ ((o) => (o.Shortcut = "shortcut", o.Toolbox = "toolbox", o.EnabledInlineTools = "inlineToolbar", o.EnabledBlockTunes = "tunes", o.Config = "config", o))(et || {}), Hn = /* @__PURE__ */ ((o) => (o.Shortcut = "shortcut", o.SanitizeConfig = "sanitize", o))(Hn || {}), xe = /* @__PURE__ */ ((o) => (o.IsEnabledLineBreaks = "enableLineBreaks", o.Toolbox = "toolbox", o.ConversionConfig = "conversionConfig", o.IsReadOnlySupported = "isReadOnlySupported", o.PasteConfig = "pasteConfig", o))(xe || {}), ft = /* @__PURE__ */ ((o) => (o.IsInline = "isInline", o.Title = "title", o.IsReadOnlySupported = "isReadOnlySupported", o))(ft || {}), Yt = /* @__PURE__ */ ((o) => (o.IsTune = "isTune", o))(Yt || {});
let ao = class {
  /**
   * @class
   * @param {ConstructorOptions} options - Constructor options
   */
  constructor({
    name: e,
    constructable: t,
    config: r,
    api: n,
    isDefault: i,
    isInternal: s = !1,
    defaultPlaceholder: a
  }) {
    this.api = n, this.name = e, this.constructable = t, this.config = r, this.isDefault = i, this.isInternal = s, this.defaultPlaceholder = a;
  }
  /**
   * Returns Tool user configuration
   */
  get settings() {
    const e = this.config.config || {};
    return this.isDefault && !("placeholder" in e) && this.defaultPlaceholder && (e.placeholder = this.defaultPlaceholder), e;
  }
  /**
   * Calls Tool's reset method
   */
  reset() {
    if (R(this.constructable.reset))
      return this.constructable.reset();
  }
  /**
   * Calls Tool's prepare method
   */
  prepare() {
    if (R(this.constructable.prepare))
      return this.constructable.prepare({
        toolName: this.name,
        config: this.settings
      });
  }
  /**
   * Returns shortcut for Tool (internal or specified by user)
   */
  get shortcut() {
    const e = this.constructable.shortcut;
    return this.config.shortcut || e;
  }
  /**
   * Returns Tool's sanitizer configuration
   */
  get sanitizeConfig() {
    return this.constructable.sanitize || {};
  }
  /**
   * Returns true if Tools is inline
   */
  isInline() {
    return this.type === ge.Inline;
  }
  /**
   * Returns true if Tools is block
   */
  isBlock() {
    return this.type === ge.Block;
  }
  /**
   * Returns true if Tools is tune
   */
  isTune() {
    return this.type === ge.Tune;
  }
};
class Aa extends N {
  /**
   * @param moduleConfiguration - Module Configuration
   * @param moduleConfiguration.config - Editor's config
   * @param moduleConfiguration.eventsDispatcher - Editor's event dispatcher
   */
  constructor({ config: e, eventsDispatcher: t }) {
    super({
      config: e,
      eventsDispatcher: t
    }), this.CSS = {
      inlineToolbar: "ce-inline-toolbar"
    }, this.opened = !1, this.popover = null, this.toolbarVerticalMargin = _e() ? 20 : 6, this.tools = /* @__PURE__ */ new Map(), window.requestIdleCallback(() => {
      this.make();
    }, { timeout: 2e3 });
  }
  /**
   *  Moving / appearance
   *  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   */
  /**
   * Shows Inline Toolbar if something is selected
   *
   * @param [needToClose] - pass true to close toolbar if it is not allowed.
   *                                  Avoid to use it just for closing IT, better call .close() clearly.
   */
  async tryToShow(e = !1) {
    e && this.close(), this.allowedToShow() && (await this.open(), this.Editor.Toolbar.close());
  }
  /**
   * Hides Inline Toolbar
   */
  close() {
    var e, t;
    if (this.opened) {
      for (const [r, n] of this.tools) {
        const i = this.getToolShortcut(r.name);
        i !== void 0 && Be.remove(this.Editor.UI.nodes.redactor, i), R(n.clear) && n.clear();
      }
      this.tools = /* @__PURE__ */ new Map(), this.reset(), this.opened = !1, (e = this.popover) == null || e.hide(), (t = this.popover) == null || t.destroy(), this.popover = null;
    }
  }
  /**
   * Check if node is contained by Inline Toolbar
   *
   * @param {Node} node — node to check
   */
  containsNode(e) {
    return this.nodes.wrapper === void 0 ? !1 : this.nodes.wrapper.contains(e);
  }
  /**
   * Removes UI and its components
   */
  destroy() {
    var e;
    this.removeAllNodes(), (e = this.popover) == null || e.destroy(), this.popover = null;
  }
  /**
   * Making DOM
   */
  make() {
    this.nodes.wrapper = g.make("div", [
      this.CSS.inlineToolbar,
      ...this.isRtl ? [this.Editor.UI.CSS.editorRtlFix] : []
    ]), g.append(this.Editor.UI.nodes.wrapper, this.nodes.wrapper);
  }
  /**
   * Shows Inline Toolbar
   */
  async open() {
    var e;
    if (this.opened)
      return;
    this.opened = !0, this.popover !== null && this.popover.destroy(), this.createToolsInstances();
    const t = await this.getPopoverItems();
    this.popover = new wa({
      items: t,
      scopeElement: this.Editor.API.methods.ui.nodes.redactor,
      messages: {
        nothingFound: W.ui(K.ui.popover, "Nothing found"),
        search: W.ui(K.ui.popover, "Filter")
      }
    }), this.move(this.popover.size.width), (e = this.nodes.wrapper) == null || e.append(this.popover.getElement()), this.popover.show();
  }
  /**
   * Move Toolbar to the selected text
   *
   * @param popoverWidth - width of the toolbar popover
   */
  move(e) {
    const t = L.rect, r = this.Editor.UI.nodes.wrapper.getBoundingClientRect(), n = {
      x: t.x - r.x,
      y: t.y + t.height - // + window.scrollY
      r.top + this.toolbarVerticalMargin
    };
    n.x + e + r.x > this.Editor.UI.contentRect.right && (n.x = this.Editor.UI.contentRect.right - e - r.x), this.nodes.wrapper.style.left = Math.floor(n.x) + "px", this.nodes.wrapper.style.top = Math.floor(n.y) + "px";
  }
  /**
   * Clear orientation classes and reset position
   */
  reset() {
    this.nodes.wrapper.style.left = "0", this.nodes.wrapper.style.top = "0";
  }
  /**
   * Need to show Inline Toolbar or not
   */
  allowedToShow() {
    const e = ["IMG", "INPUT"], t = L.get(), r = L.text;
    if (!t || !t.anchorNode || t.isCollapsed || r.length < 1)
      return !1;
    const n = g.isElement(t.anchorNode) ? t.anchorNode : t.anchorNode.parentElement;
    if (n === null || t !== null && e.includes(n.tagName))
      return !1;
    const i = this.Editor.BlockManager.getBlock(t.anchorNode);
    return !i || this.getTools().some((s) => i.tool.inlineTools.has(s.name)) === !1 ? !1 : n.closest("[contenteditable]") !== null;
  }
  /**
   *  Working with Tools
   *  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   */
  /**
   * Returns tools that are available for current block
   *
   * Used to check if Inline Toolbar could be shown
   * and to render tools in the Inline Toolbar
   */
  getTools() {
    const e = this.Editor.BlockManager.currentBlock;
    return e ? Array.from(e.tool.inlineTools.values()).filter((t) => !(this.Editor.ReadOnly.isEnabled && t.isReadOnlySupported !== !0)) : [];
  }
  /**
   * Constructs tools instances and saves them to this.tools
   */
  createToolsInstances() {
    this.tools = /* @__PURE__ */ new Map(), this.getTools().forEach((e) => {
      const t = e.create();
      this.tools.set(e, t);
    });
  }
  /**
   * Returns Popover Items for tools segregated by their appearance type: regular items and custom html elements.
   */
  async getPopoverItems() {
    const e = [];
    let t = 0;
    for (const [r, n] of this.tools) {
      const i = await n.render(), s = this.getToolShortcut(r.name);
      if (s !== void 0)
        try {
          this.enableShortcuts(r.name, s);
        } catch {
        }
      const a = s !== void 0 ? eo(s) : void 0, l = W.t(
        K.toolNames,
        r.title || lt(r.name)
      );
      [i].flat().forEach((c) => {
        var d, h;
        const u = {
          name: r.name,
          onActivate: () => {
            this.toolClicked(n);
          },
          hint: {
            title: l,
            description: a
          }
        };
        if (g.isElement(c)) {
          const f = {
            ...u,
            element: c,
            type: D.Html
          };
          if (R(n.renderActions)) {
            const p = n.renderActions();
            f.children = {
              isOpen: (d = n.checkState) == null ? void 0 : d.call(n, L.get()),
              /** Disable keyboard navigation in actions, as it might conflict with enter press handling */
              isFlippable: !1,
              items: [
                {
                  type: D.Html,
                  element: p
                }
              ]
            };
          } else
            (h = n.checkState) == null || h.call(n, L.get());
          e.push(f);
        } else if (c.type === D.Html)
          e.push({
            ...u,
            ...c,
            type: D.Html
          });
        else if (c.type === D.Separator)
          e.push({
            type: D.Separator
          });
        else {
          const f = {
            ...u,
            ...c,
            type: D.Default
          };
          "children" in f && t !== 0 && e.push({
            type: D.Separator
          }), e.push(f), "children" in f && t < this.tools.size - 1 && e.push({
            type: D.Separator
          });
        }
      }), t++;
    }
    return e;
  }
  /**
   * Get shortcut name for tool
   *
   * @param toolName — Tool name
   */
  getToolShortcut(e) {
    const { Tools: t } = this.Editor, r = t.inlineTools.get(e), n = t.internal.inlineTools;
    return Array.from(n.keys()).includes(e) ? this.inlineTools[e][Hn.Shortcut] : r == null ? void 0 : r.shortcut;
  }
  /**
   * Enable Tool shortcut with Editor Shortcuts Module
   *
   * @param toolName - tool name
   * @param shortcut - shortcut according to the ShortcutData Module format
   */
  enableShortcuts(e, t) {
    Be.add({
      name: t,
      handler: (r) => {
        var n;
        const { currentBlock: i } = this.Editor.BlockManager;
        i && i.tool.enabledInlineTools && (r.preventDefault(), (n = this.popover) == null || n.activateItemByName(e));
      },
      /**
       * We need to bind shortcut to the document to make it work in read-only mode
       */
      on: document
    });
  }
  /**
   * Inline Tool button clicks
   *
   * @param tool - Tool's instance
   */
  toolClicked(e) {
    var t;
    const r = L.range;
    (t = e.surround) == null || t.call(e, r), this.checkToolsState();
  }
  /**
   * Check Tools` state by selection
   */
  checkToolsState() {
    var e;
    (e = this.tools) == null || e.forEach((t) => {
      var r;
      (r = t.checkState) == null || r.call(t, L.get());
    });
  }
  /**
   * Get inline tools tools
   * Tools that has isInline is true
   */
  get inlineTools() {
    const e = {};
    return Array.from(this.Editor.Tools.inlineTools.entries()).forEach(([t, r]) => {
      e[t] = r.create();
    }), e;
  }
}
function Fn() {
  const o = window.getSelection();
  if (o === null)
    return [null, 0];
  let e = o.focusNode, t = o.focusOffset;
  return e === null ? [null, 0] : (e.nodeType !== Node.TEXT_NODE && e.childNodes.length > 0 && (e.childNodes[t] ? (e = e.childNodes[t], t = 0) : (e = e.childNodes[t - 1], t = e.textContent.length)), [e, t]);
}
function $n(o, e, t, r) {
  const n = document.createRange();
  r === "left" ? (n.setStart(o, 0), n.setEnd(e, t)) : (n.setStart(e, t), n.setEnd(o, o.childNodes.length));
  const i = n.cloneContents(), s = document.createElement("div");
  s.appendChild(i);
  const a = s.textContent || "";
  return fs(a);
}
function tt(o) {
  const e = g.getDeepestNode(o);
  if (e === null || g.isEmpty(o))
    return !0;
  if (g.isNativeInput(e))
    return e.selectionEnd === 0;
  if (g.isEmpty(o))
    return !0;
  const [t, r] = Fn();
  return t === null ? !1 : $n(o, t, r, "left");
}
function ot(o) {
  const e = g.getDeepestNode(o, !0);
  if (e === null)
    return !0;
  if (g.isNativeInput(e))
    return e.selectionEnd === e.value.length;
  const [t, r] = Fn();
  return t === null ? !1 : $n(o, t, r, "right");
}
var Un = {}, lo = {}, bt = {}, ve = {}, co = {}, ho = {};
Object.defineProperty(ho, "__esModule", { value: !0 });
ho.allInputsSelector = Pa;
function Pa() {
  var o = ["text", "password", "email", "number", "search", "tel", "url"];
  return "[contenteditable=true], textarea, input:not([type]), " + o.map(function(e) {
    return 'input[type="'.concat(e, '"]');
  }).join(", ");
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.allInputsSelector = void 0;
  var e = ho;
  Object.defineProperty(o, "allInputsSelector", { enumerable: !0, get: function() {
    return e.allInputsSelector;
  } });
})(co);
var be = {}, uo = {};
Object.defineProperty(uo, "__esModule", { value: !0 });
uo.isNativeInput = Na;
function Na(o) {
  var e = [
    "INPUT",
    "TEXTAREA"
  ];
  return o && o.tagName ? e.includes(o.tagName) : !1;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isNativeInput = void 0;
  var e = uo;
  Object.defineProperty(o, "isNativeInput", { enumerable: !0, get: function() {
    return e.isNativeInput;
  } });
})(be);
var zn = {}, po = {};
Object.defineProperty(po, "__esModule", { value: !0 });
po.append = ja;
function ja(o, e) {
  Array.isArray(e) ? e.forEach(function(t) {
    o.appendChild(t);
  }) : o.appendChild(e);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.append = void 0;
  var e = po;
  Object.defineProperty(o, "append", { enumerable: !0, get: function() {
    return e.append;
  } });
})(zn);
var fo = {}, go = {};
Object.defineProperty(go, "__esModule", { value: !0 });
go.blockElements = Da;
function Da() {
  return [
    "address",
    "article",
    "aside",
    "blockquote",
    "canvas",
    "div",
    "dl",
    "dt",
    "fieldset",
    "figcaption",
    "figure",
    "footer",
    "form",
    "h1",
    "h2",
    "h3",
    "h4",
    "h5",
    "h6",
    "header",
    "hgroup",
    "hr",
    "li",
    "main",
    "nav",
    "noscript",
    "ol",
    "output",
    "p",
    "pre",
    "ruby",
    "section",
    "table",
    "tbody",
    "thead",
    "tr",
    "tfoot",
    "ul",
    "video"
  ];
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.blockElements = void 0;
  var e = go;
  Object.defineProperty(o, "blockElements", { enumerable: !0, get: function() {
    return e.blockElements;
  } });
})(fo);
var Vn = {}, mo = {};
Object.defineProperty(mo, "__esModule", { value: !0 });
mo.calculateBaseline = Ra;
function Ra(o) {
  var e = window.getComputedStyle(o), t = parseFloat(e.fontSize), r = parseFloat(e.lineHeight) || t * 1.2, n = parseFloat(e.paddingTop), i = parseFloat(e.borderTopWidth), s = parseFloat(e.marginTop), a = t * 0.8, l = (r - t) / 2, c = s + i + n + l + a;
  return c;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.calculateBaseline = void 0;
  var e = mo;
  Object.defineProperty(o, "calculateBaseline", { enumerable: !0, get: function() {
    return e.calculateBaseline;
  } });
})(Vn);
var Wn = {}, vo = {}, bo = {}, ko = {};
Object.defineProperty(ko, "__esModule", { value: !0 });
ko.isContentEditable = Ha;
function Ha(o) {
  return o.contentEditable === "true";
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isContentEditable = void 0;
  var e = ko;
  Object.defineProperty(o, "isContentEditable", { enumerable: !0, get: function() {
    return e.isContentEditable;
  } });
})(bo);
Object.defineProperty(vo, "__esModule", { value: !0 });
vo.canSetCaret = Ua;
var Fa = be, $a = bo;
function Ua(o) {
  var e = !0;
  if ((0, Fa.isNativeInput)(o))
    switch (o.type) {
      case "file":
      case "checkbox":
      case "radio":
      case "hidden":
      case "submit":
      case "button":
      case "image":
      case "reset":
        e = !1;
        break;
    }
  else
    e = (0, $a.isContentEditable)(o);
  return e;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.canSetCaret = void 0;
  var e = vo;
  Object.defineProperty(o, "canSetCaret", { enumerable: !0, get: function() {
    return e.canSetCaret;
  } });
})(Wn);
var kt = {}, wo = {};
function za(o, e, t) {
  const r = t.value !== void 0 ? "value" : "get", n = t[r], i = `#${e}Cache`;
  if (t[r] = function(...s) {
    return this[i] === void 0 && (this[i] = n.apply(this, s)), this[i];
  }, r === "get" && t.set) {
    const s = t.set;
    t.set = function(a) {
      delete o[i], s.apply(this, a);
    };
  }
  return t;
}
function qn() {
  const o = {
    win: !1,
    mac: !1,
    x11: !1,
    linux: !1
  }, e = Object.keys(o).find((t) => window.navigator.appVersion.toLowerCase().indexOf(t) !== -1);
  return e !== void 0 && (o[e] = !0), o;
}
function yo(o) {
  return o != null && o !== "" && (typeof o != "object" || Object.keys(o).length > 0);
}
function Va(o) {
  return !yo(o);
}
const Wa = () => typeof window < "u" && window.navigator !== null && yo(window.navigator.platform) && (/iP(ad|hone|od)/.test(window.navigator.platform) || window.navigator.platform === "MacIntel" && window.navigator.maxTouchPoints > 1);
function qa(o) {
  const e = qn();
  return o = o.replace(/shift/gi, "⇧").replace(/backspace/gi, "⌫").replace(/enter/gi, "⏎").replace(/up/gi, "↑").replace(/left/gi, "→").replace(/down/gi, "↓").replace(/right/gi, "←").replace(/escape/gi, "⎋").replace(/insert/gi, "Ins").replace(/delete/gi, "␡").replace(/\+/gi, "+"), e.mac ? o = o.replace(/ctrl|cmd/gi, "⌘").replace(/alt/gi, "⌥") : o = o.replace(/cmd/gi, "Ctrl").replace(/windows/gi, "WIN"), o;
}
function Ka(o) {
  return o[0].toUpperCase() + o.slice(1);
}
function Ya(o) {
  const e = document.createElement("div");
  e.style.position = "absolute", e.style.left = "-999px", e.style.bottom = "-999px", e.innerHTML = o, document.body.appendChild(e);
  const t = window.getSelection(), r = document.createRange();
  if (r.selectNode(e), t === null)
    throw new Error("Cannot copy text to clipboard");
  t.removeAllRanges(), t.addRange(r), document.execCommand("copy"), document.body.removeChild(e);
}
function Xa(o, e, t) {
  let r;
  return (...n) => {
    const i = this, s = () => {
      r = void 0, t !== !0 && o.apply(i, n);
    }, a = t === !0 && r !== void 0;
    window.clearTimeout(r), r = window.setTimeout(s, e), a && o.apply(i, n);
  };
}
function ce(o) {
  return Object.prototype.toString.call(o).match(/\s([a-zA-Z]+)/)[1].toLowerCase();
}
function Za(o) {
  return ce(o) === "boolean";
}
function Kn(o) {
  return ce(o) === "function" || ce(o) === "asyncfunction";
}
function Ga(o) {
  return Kn(o) && /^\s*class\s+/.test(o.toString());
}
function Ja(o) {
  return ce(o) === "number";
}
function rt(o) {
  return ce(o) === "object";
}
function Qa(o) {
  return Promise.resolve(o) === o;
}
function el(o) {
  return ce(o) === "string";
}
function tl(o) {
  return ce(o) === "undefined";
}
function Xt(o, ...e) {
  if (!e.length)
    return o;
  const t = e.shift();
  if (rt(o) && rt(t))
    for (const r in t)
      rt(t[r]) ? (o[r] === void 0 && Object.assign(o, { [r]: {} }), Xt(o[r], t[r])) : Object.assign(o, { [r]: t[r] });
  return Xt(o, ...e);
}
function ol(o, e, t) {
  const r = `«${e}» is deprecated and will be removed in the next major release. Please use the «${t}» instead.`;
  o && console.warn(r);
}
function rl(o) {
  try {
    return new URL(o).href;
  } catch {
  }
  return o.substring(0, 2) === "//" ? window.location.protocol + o : window.location.origin + o;
}
function nl(o) {
  return o > 47 && o < 58 || o === 32 || o === 13 || o === 229 || o > 64 && o < 91 || o > 95 && o < 112 || o > 185 && o < 193 || o > 218 && o < 223;
}
const il = {
  BACKSPACE: 8,
  TAB: 9,
  ENTER: 13,
  SHIFT: 16,
  CTRL: 17,
  ALT: 18,
  ESC: 27,
  SPACE: 32,
  LEFT: 37,
  UP: 38,
  DOWN: 40,
  RIGHT: 39,
  DELETE: 46,
  META: 91,
  SLASH: 191
}, sl = {
  LEFT: 0,
  WHEEL: 1,
  RIGHT: 2,
  BACKWARD: 3,
  FORWARD: 4
};
let al = class {
  constructor() {
    this.completed = Promise.resolve();
  }
  /**
   * Add new promise to queue
   * @param operation - promise should be added to queue
   */
  add(o) {
    return new Promise((e, t) => {
      this.completed = this.completed.then(o).then(e).catch(t);
    });
  }
};
function ll(o, e, t = void 0) {
  let r, n, i, s = null, a = 0;
  t || (t = {});
  const l = function() {
    a = t.leading === !1 ? 0 : Date.now(), s = null, i = o.apply(r, n), s === null && (r = n = null);
  };
  return function() {
    const c = Date.now();
    !a && t.leading === !1 && (a = c);
    const d = e - (c - a);
    return r = this, n = arguments, d <= 0 || d > e ? (s && (clearTimeout(s), s = null), a = c, i = o.apply(r, n), s === null && (r = n = null)) : !s && t.trailing !== !1 && (s = setTimeout(l, d)), i;
  };
}
const cl = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
  __proto__: null,
  PromiseQueue: al,
  beautifyShortcut: qa,
  cacheable: za,
  capitalize: Ka,
  copyTextToClipboard: Ya,
  debounce: Xa,
  deepMerge: Xt,
  deprecationAssert: ol,
  getUserOS: qn,
  getValidUrl: rl,
  isBoolean: Za,
  isClass: Ga,
  isEmpty: Va,
  isFunction: Kn,
  isIosDevice: Wa,
  isNumber: Ja,
  isObject: rt,
  isPrintableKey: nl,
  isPromise: Qa,
  isString: el,
  isUndefined: tl,
  keyCodes: il,
  mouseButtons: sl,
  notEmpty: yo,
  throttle: ll,
  typeOf: ce
}, Symbol.toStringTag, { value: "Module" })), xo = /* @__PURE__ */ es(cl);
Object.defineProperty(wo, "__esModule", { value: !0 });
wo.containsOnlyInlineElements = ul;
var dl = xo, hl = fo;
function ul(o) {
  var e;
  (0, dl.isString)(o) ? (e = document.createElement("div"), e.innerHTML = o) : e = o;
  var t = function(r) {
    return !(0, hl.blockElements)().includes(r.tagName.toLowerCase()) && Array.from(r.children).every(t);
  };
  return Array.from(e.children).every(t);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.containsOnlyInlineElements = void 0;
  var e = wo;
  Object.defineProperty(o, "containsOnlyInlineElements", { enumerable: !0, get: function() {
    return e.containsOnlyInlineElements;
  } });
})(kt);
var Yn = {}, Co = {}, wt = {}, Eo = {};
Object.defineProperty(Eo, "__esModule", { value: !0 });
Eo.make = pl;
function pl(o, e, t) {
  var r;
  e === void 0 && (e = null), t === void 0 && (t = {});
  var n = document.createElement(o);
  if (Array.isArray(e)) {
    var i = e.filter(function(a) {
      return a !== void 0;
    });
    (r = n.classList).add.apply(r, i);
  } else
    e !== null && n.classList.add(e);
  for (var s in t)
    Object.prototype.hasOwnProperty.call(t, s) && (n[s] = t[s]);
  return n;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.make = void 0;
  var e = Eo;
  Object.defineProperty(o, "make", { enumerable: !0, get: function() {
    return e.make;
  } });
})(wt);
Object.defineProperty(Co, "__esModule", { value: !0 });
Co.fragmentToString = gl;
var fl = wt;
function gl(o) {
  var e = (0, fl.make)("div");
  return e.appendChild(o), e.innerHTML;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.fragmentToString = void 0;
  var e = Co;
  Object.defineProperty(o, "fragmentToString", { enumerable: !0, get: function() {
    return e.fragmentToString;
  } });
})(Yn);
var Xn = {}, To = {};
Object.defineProperty(To, "__esModule", { value: !0 });
To.getContentLength = vl;
var ml = be;
function vl(o) {
  var e, t;
  return (0, ml.isNativeInput)(o) ? o.value.length : o.nodeType === Node.TEXT_NODE ? o.length : (t = (e = o.textContent) === null || e === void 0 ? void 0 : e.length) !== null && t !== void 0 ? t : 0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getContentLength = void 0;
  var e = To;
  Object.defineProperty(o, "getContentLength", { enumerable: !0, get: function() {
    return e.getContentLength;
  } });
})(Xn);
var So = {}, Bo = {}, Yr = Ve && Ve.__spreadArray || function(o, e, t) {
  if (t || arguments.length === 2)
    for (var r = 0, n = e.length, i; r < n; r++)
      (i || !(r in e)) && (i || (i = Array.prototype.slice.call(e, 0, r)), i[r] = e[r]);
  return o.concat(i || Array.prototype.slice.call(e));
};
Object.defineProperty(Bo, "__esModule", { value: !0 });
Bo.getDeepestBlockElements = Zn;
var bl = kt;
function Zn(o) {
  return (0, bl.containsOnlyInlineElements)(o) ? [o] : Array.from(o.children).reduce(function(e, t) {
    return Yr(Yr([], e, !0), Zn(t), !0);
  }, []);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getDeepestBlockElements = void 0;
  var e = Bo;
  Object.defineProperty(o, "getDeepestBlockElements", { enumerable: !0, get: function() {
    return e.getDeepestBlockElements;
  } });
})(So);
var Gn = {}, Mo = {}, yt = {}, _o = {};
Object.defineProperty(_o, "__esModule", { value: !0 });
_o.isLineBreakTag = kl;
function kl(o) {
  return [
    "BR",
    "WBR"
  ].includes(o.tagName);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isLineBreakTag = void 0;
  var e = _o;
  Object.defineProperty(o, "isLineBreakTag", { enumerable: !0, get: function() {
    return e.isLineBreakTag;
  } });
})(yt);
var xt = {}, Lo = {};
Object.defineProperty(Lo, "__esModule", { value: !0 });
Lo.isSingleTag = wl;
function wl(o) {
  return [
    "AREA",
    "BASE",
    "BR",
    "COL",
    "COMMAND",
    "EMBED",
    "HR",
    "IMG",
    "INPUT",
    "KEYGEN",
    "LINK",
    "META",
    "PARAM",
    "SOURCE",
    "TRACK",
    "WBR"
  ].includes(o.tagName);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isSingleTag = void 0;
  var e = Lo;
  Object.defineProperty(o, "isSingleTag", { enumerable: !0, get: function() {
    return e.isSingleTag;
  } });
})(xt);
Object.defineProperty(Mo, "__esModule", { value: !0 });
Mo.getDeepestNode = Jn;
var yl = be, xl = yt, Cl = xt;
function Jn(o, e) {
  e === void 0 && (e = !1);
  var t = e ? "lastChild" : "firstChild", r = e ? "previousSibling" : "nextSibling";
  if (o.nodeType === Node.ELEMENT_NODE && o[t]) {
    var n = o[t];
    if ((0, Cl.isSingleTag)(n) && !(0, yl.isNativeInput)(n) && !(0, xl.isLineBreakTag)(n))
      if (n[r])
        n = n[r];
      else if (n.parentNode !== null && n.parentNode[r])
        n = n.parentNode[r];
      else
        return n.parentNode;
    return Jn(n, e);
  }
  return o;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getDeepestNode = void 0;
  var e = Mo;
  Object.defineProperty(o, "getDeepestNode", { enumerable: !0, get: function() {
    return e.getDeepestNode;
  } });
})(Gn);
var Qn = {}, Io = {}, Ze = Ve && Ve.__spreadArray || function(o, e, t) {
  if (t || arguments.length === 2)
    for (var r = 0, n = e.length, i; r < n; r++)
      (i || !(r in e)) && (i || (i = Array.prototype.slice.call(e, 0, r)), i[r] = e[r]);
  return o.concat(i || Array.prototype.slice.call(e));
};
Object.defineProperty(Io, "__esModule", { value: !0 });
Io.findAllInputs = Ml;
var El = kt, Tl = So, Sl = co, Bl = be;
function Ml(o) {
  return Array.from(o.querySelectorAll((0, Sl.allInputsSelector)())).reduce(function(e, t) {
    return (0, Bl.isNativeInput)(t) || (0, El.containsOnlyInlineElements)(t) ? Ze(Ze([], e, !0), [t], !1) : Ze(Ze([], e, !0), (0, Tl.getDeepestBlockElements)(t), !0);
  }, []);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.findAllInputs = void 0;
  var e = Io;
  Object.defineProperty(o, "findAllInputs", { enumerable: !0, get: function() {
    return e.findAllInputs;
  } });
})(Qn);
var ei = {}, Oo = {};
Object.defineProperty(Oo, "__esModule", { value: !0 });
Oo.isCollapsedWhitespaces = _l;
function _l(o) {
  return !/[^\t\n\r ]/.test(o);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isCollapsedWhitespaces = void 0;
  var e = Oo;
  Object.defineProperty(o, "isCollapsedWhitespaces", { enumerable: !0, get: function() {
    return e.isCollapsedWhitespaces;
  } });
})(ei);
var Ao = {}, Po = {};
Object.defineProperty(Po, "__esModule", { value: !0 });
Po.isElement = Il;
var Ll = xo;
function Il(o) {
  return (0, Ll.isNumber)(o) ? !1 : !!o && !!o.nodeType && o.nodeType === Node.ELEMENT_NODE;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isElement = void 0;
  var e = Po;
  Object.defineProperty(o, "isElement", { enumerable: !0, get: function() {
    return e.isElement;
  } });
})(Ao);
var ti = {}, No = {}, jo = {}, Do = {};
Object.defineProperty(Do, "__esModule", { value: !0 });
Do.isLeaf = Ol;
function Ol(o) {
  return o === null ? !1 : o.childNodes.length === 0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isLeaf = void 0;
  var e = Do;
  Object.defineProperty(o, "isLeaf", { enumerable: !0, get: function() {
    return e.isLeaf;
  } });
})(jo);
var Ro = {}, Ho = {};
Object.defineProperty(Ho, "__esModule", { value: !0 });
Ho.isNodeEmpty = Dl;
var Al = yt, Pl = Ao, Nl = be, jl = xt;
function Dl(o, e) {
  var t = "";
  return (0, jl.isSingleTag)(o) && !(0, Al.isLineBreakTag)(o) ? !1 : ((0, Pl.isElement)(o) && (0, Nl.isNativeInput)(o) ? t = o.value : o.textContent !== null && (t = o.textContent.replace("​", "")), e !== void 0 && (t = t.replace(new RegExp(e, "g"), "")), t.trim().length === 0);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isNodeEmpty = void 0;
  var e = Ho;
  Object.defineProperty(o, "isNodeEmpty", { enumerable: !0, get: function() {
    return e.isNodeEmpty;
  } });
})(Ro);
Object.defineProperty(No, "__esModule", { value: !0 });
No.isEmpty = Fl;
var Rl = jo, Hl = Ro;
function Fl(o, e) {
  o.normalize();
  for (var t = [o]; t.length > 0; ) {
    var r = t.shift();
    if (r) {
      if (o = r, (0, Rl.isLeaf)(o) && !(0, Hl.isNodeEmpty)(o, e))
        return !1;
      t.push.apply(t, Array.from(o.childNodes));
    }
  }
  return !0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isEmpty = void 0;
  var e = No;
  Object.defineProperty(o, "isEmpty", { enumerable: !0, get: function() {
    return e.isEmpty;
  } });
})(ti);
var oi = {}, Fo = {};
Object.defineProperty(Fo, "__esModule", { value: !0 });
Fo.isFragment = Ul;
var $l = xo;
function Ul(o) {
  return (0, $l.isNumber)(o) ? !1 : !!o && !!o.nodeType && o.nodeType === Node.DOCUMENT_FRAGMENT_NODE;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isFragment = void 0;
  var e = Fo;
  Object.defineProperty(o, "isFragment", { enumerable: !0, get: function() {
    return e.isFragment;
  } });
})(oi);
var ri = {}, $o = {};
Object.defineProperty($o, "__esModule", { value: !0 });
$o.isHTMLString = Vl;
var zl = wt;
function Vl(o) {
  var e = (0, zl.make)("div");
  return e.innerHTML = o, e.childElementCount > 0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isHTMLString = void 0;
  var e = $o;
  Object.defineProperty(o, "isHTMLString", { enumerable: !0, get: function() {
    return e.isHTMLString;
  } });
})(ri);
var ni = {}, Uo = {};
Object.defineProperty(Uo, "__esModule", { value: !0 });
Uo.offset = Wl;
function Wl(o) {
  var e = o.getBoundingClientRect(), t = window.pageXOffset || document.documentElement.scrollLeft, r = window.pageYOffset || document.documentElement.scrollTop, n = e.top + r, i = e.left + t;
  return {
    top: n,
    left: i,
    bottom: n + e.height,
    right: i + e.width
  };
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.offset = void 0;
  var e = Uo;
  Object.defineProperty(o, "offset", { enumerable: !0, get: function() {
    return e.offset;
  } });
})(ni);
var ii = {}, zo = {};
Object.defineProperty(zo, "__esModule", { value: !0 });
zo.prepend = ql;
function ql(o, e) {
  Array.isArray(e) ? (e = e.reverse(), e.forEach(function(t) {
    return o.prepend(t);
  })) : o.prepend(e);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.prepend = void 0;
  var e = zo;
  Object.defineProperty(o, "prepend", { enumerable: !0, get: function() {
    return e.prepend;
  } });
})(ii);
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.prepend = o.offset = o.make = o.isLineBreakTag = o.isSingleTag = o.isNodeEmpty = o.isLeaf = o.isHTMLString = o.isFragment = o.isEmpty = o.isElement = o.isContentEditable = o.isCollapsedWhitespaces = o.findAllInputs = o.isNativeInput = o.allInputsSelector = o.getDeepestNode = o.getDeepestBlockElements = o.getContentLength = o.fragmentToString = o.containsOnlyInlineElements = o.canSetCaret = o.calculateBaseline = o.blockElements = o.append = void 0;
  var e = co;
  Object.defineProperty(o, "allInputsSelector", { enumerable: !0, get: function() {
    return e.allInputsSelector;
  } });
  var t = be;
  Object.defineProperty(o, "isNativeInput", { enumerable: !0, get: function() {
    return t.isNativeInput;
  } });
  var r = zn;
  Object.defineProperty(o, "append", { enumerable: !0, get: function() {
    return r.append;
  } });
  var n = fo;
  Object.defineProperty(o, "blockElements", { enumerable: !0, get: function() {
    return n.blockElements;
  } });
  var i = Vn;
  Object.defineProperty(o, "calculateBaseline", { enumerable: !0, get: function() {
    return i.calculateBaseline;
  } });
  var s = Wn;
  Object.defineProperty(o, "canSetCaret", { enumerable: !0, get: function() {
    return s.canSetCaret;
  } });
  var a = kt;
  Object.defineProperty(o, "containsOnlyInlineElements", { enumerable: !0, get: function() {
    return a.containsOnlyInlineElements;
  } });
  var l = Yn;
  Object.defineProperty(o, "fragmentToString", { enumerable: !0, get: function() {
    return l.fragmentToString;
  } });
  var c = Xn;
  Object.defineProperty(o, "getContentLength", { enumerable: !0, get: function() {
    return c.getContentLength;
  } });
  var d = So;
  Object.defineProperty(o, "getDeepestBlockElements", { enumerable: !0, get: function() {
    return d.getDeepestBlockElements;
  } });
  var h = Gn;
  Object.defineProperty(o, "getDeepestNode", { enumerable: !0, get: function() {
    return h.getDeepestNode;
  } });
  var u = Qn;
  Object.defineProperty(o, "findAllInputs", { enumerable: !0, get: function() {
    return u.findAllInputs;
  } });
  var f = ei;
  Object.defineProperty(o, "isCollapsedWhitespaces", { enumerable: !0, get: function() {
    return f.isCollapsedWhitespaces;
  } });
  var p = bo;
  Object.defineProperty(o, "isContentEditable", { enumerable: !0, get: function() {
    return p.isContentEditable;
  } });
  var k = Ao;
  Object.defineProperty(o, "isElement", { enumerable: !0, get: function() {
    return k.isElement;
  } });
  var T = ti;
  Object.defineProperty(o, "isEmpty", { enumerable: !0, get: function() {
    return T.isEmpty;
  } });
  var v = oi;
  Object.defineProperty(o, "isFragment", { enumerable: !0, get: function() {
    return v.isFragment;
  } });
  var m = ri;
  Object.defineProperty(o, "isHTMLString", { enumerable: !0, get: function() {
    return m.isHTMLString;
  } });
  var C = jo;
  Object.defineProperty(o, "isLeaf", { enumerable: !0, get: function() {
    return C.isLeaf;
  } });
  var S = Ro;
  Object.defineProperty(o, "isNodeEmpty", { enumerable: !0, get: function() {
    return S.isNodeEmpty;
  } });
  var _ = yt;
  Object.defineProperty(o, "isLineBreakTag", { enumerable: !0, get: function() {
    return _.isLineBreakTag;
  } });
  var x = xt;
  Object.defineProperty(o, "isSingleTag", { enumerable: !0, get: function() {
    return x.isSingleTag;
  } });
  var I = wt;
  Object.defineProperty(o, "make", { enumerable: !0, get: function() {
    return I.make;
  } });
  var w = ni;
  Object.defineProperty(o, "offset", { enumerable: !0, get: function() {
    return w.offset;
  } });
  var b = ii;
  Object.defineProperty(o, "prepend", { enumerable: !0, get: function() {
    return b.prepend;
  } });
})(ve);
var Ct = {};
Object.defineProperty(Ct, "__esModule", { value: !0 });
Ct.getContenteditableSlice = Yl;
var Kl = ve;
function Yl(o, e, t, r, n) {
  var i;
  n === void 0 && (n = !1);
  var s = document.createRange();
  if (r === "left" ? (s.setStart(o, 0), s.setEnd(e, t)) : (s.setStart(e, t), s.setEnd(o, o.childNodes.length)), n === !0) {
    var a = s.extractContents();
    return (0, Kl.fragmentToString)(a);
  }
  var l = s.cloneContents(), c = document.createElement("div");
  c.appendChild(l);
  var d = (i = c.textContent) !== null && i !== void 0 ? i : "";
  return d;
}
Object.defineProperty(bt, "__esModule", { value: !0 });
bt.checkContenteditableSliceForEmptiness = Gl;
var Xl = ve, Zl = Ct;
function Gl(o, e, t, r) {
  var n = (0, Zl.getContenteditableSlice)(o, e, t, r);
  return (0, Xl.isCollapsedWhitespaces)(n);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.checkContenteditableSliceForEmptiness = void 0;
  var e = bt;
  Object.defineProperty(o, "checkContenteditableSliceForEmptiness", { enumerable: !0, get: function() {
    return e.checkContenteditableSliceForEmptiness;
  } });
})(lo);
var si = {};
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getContenteditableSlice = void 0;
  var e = Ct;
  Object.defineProperty(o, "getContenteditableSlice", { enumerable: !0, get: function() {
    return e.getContenteditableSlice;
  } });
})(si);
var ai = {}, Vo = {};
Object.defineProperty(Vo, "__esModule", { value: !0 });
Vo.focus = Ql;
var Jl = ve;
function Ql(o, e) {
  var t, r;
  if (e === void 0 && (e = !0), (0, Jl.isNativeInput)(o)) {
    o.focus();
    var n = e ? 0 : o.value.length;
    o.setSelectionRange(n, n);
  } else {
    var i = document.createRange(), s = window.getSelection();
    if (!s)
      return;
    var a = function(u) {
      var f = document.createTextNode("");
      u.appendChild(f), i.setStart(f, 0), i.setEnd(f, 0);
    }, l = function(u) {
      return u != null;
    }, c = o.childNodes, d = e ? c[0] : c[c.length - 1];
    if (l(d)) {
      for (; l(d) && d.nodeType !== Node.TEXT_NODE; )
        d = e ? d.firstChild : d.lastChild;
      if (l(d) && d.nodeType === Node.TEXT_NODE) {
        var h = (r = (t = d.textContent) === null || t === void 0 ? void 0 : t.length) !== null && r !== void 0 ? r : 0, n = e ? 0 : h;
        i.setStart(d, n), i.setEnd(d, n);
      } else
        a(o);
    } else
      a(o);
    s.removeAllRanges(), s.addRange(i);
  }
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.focus = void 0;
  var e = Vo;
  Object.defineProperty(o, "focus", { enumerable: !0, get: function() {
    return e.focus;
  } });
})(ai);
var Wo = {}, Et = {};
Object.defineProperty(Et, "__esModule", { value: !0 });
Et.getCaretNodeAndOffset = ec;
function ec() {
  var o = window.getSelection();
  if (o === null)
    return [null, 0];
  var e = o.focusNode, t = o.focusOffset;
  return e === null ? [null, 0] : (e.nodeType !== Node.TEXT_NODE && e.childNodes.length > 0 && (e.childNodes[t] !== void 0 ? (e = e.childNodes[t], t = 0) : (e = e.childNodes[t - 1], e.textContent !== null && (t = e.textContent.length))), [e, t]);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getCaretNodeAndOffset = void 0;
  var e = Et;
  Object.defineProperty(o, "getCaretNodeAndOffset", { enumerable: !0, get: function() {
    return e.getCaretNodeAndOffset;
  } });
})(Wo);
var li = {}, Tt = {};
Object.defineProperty(Tt, "__esModule", { value: !0 });
Tt.getRange = tc;
function tc() {
  var o = window.getSelection();
  return o && o.rangeCount ? o.getRangeAt(0) : null;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getRange = void 0;
  var e = Tt;
  Object.defineProperty(o, "getRange", { enumerable: !0, get: function() {
    return e.getRange;
  } });
})(li);
var ci = {}, qo = {};
Object.defineProperty(qo, "__esModule", { value: !0 });
qo.isCaretAtEndOfInput = nc;
var Xr = ve, oc = Wo, rc = lo;
function nc(o) {
  var e = (0, Xr.getDeepestNode)(o, !0);
  if (e === null)
    return !0;
  if ((0, Xr.isNativeInput)(e))
    return e.selectionEnd === e.value.length;
  var t = (0, oc.getCaretNodeAndOffset)(), r = t[0], n = t[1];
  return r === null ? !1 : (0, rc.checkContenteditableSliceForEmptiness)(o, r, n, "right");
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isCaretAtEndOfInput = void 0;
  var e = qo;
  Object.defineProperty(o, "isCaretAtEndOfInput", { enumerable: !0, get: function() {
    return e.isCaretAtEndOfInput;
  } });
})(ci);
var di = {}, Ko = {};
Object.defineProperty(Ko, "__esModule", { value: !0 });
Ko.isCaretAtStartOfInput = ac;
var Ge = ve, ic = Et, sc = bt;
function ac(o) {
  var e = (0, Ge.getDeepestNode)(o);
  if (e === null || (0, Ge.isEmpty)(o))
    return !0;
  if ((0, Ge.isNativeInput)(e))
    return e.selectionEnd === 0;
  if ((0, Ge.isEmpty)(o))
    return !0;
  var t = (0, ic.getCaretNodeAndOffset)(), r = t[0], n = t[1];
  return r === null ? !1 : (0, sc.checkContenteditableSliceForEmptiness)(o, r, n, "left");
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isCaretAtStartOfInput = void 0;
  var e = Ko;
  Object.defineProperty(o, "isCaretAtStartOfInput", { enumerable: !0, get: function() {
    return e.isCaretAtStartOfInput;
  } });
})(di);
var hi = {}, Yo = {};
Object.defineProperty(Yo, "__esModule", { value: !0 });
Yo.save = dc;
var lc = ve, cc = Tt;
function dc() {
  var o = (0, cc.getRange)(), e = (0, lc.make)("span");
  if (e.id = "cursor", e.hidden = !0, !!o)
    return o.insertNode(e), function() {
      var t = window.getSelection();
      t && (o.setStartAfter(e), o.setEndAfter(e), t.removeAllRanges(), t.addRange(o), setTimeout(function() {
        e.remove();
      }, 150));
    };
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.save = void 0;
  var e = Yo;
  Object.defineProperty(o, "save", { enumerable: !0, get: function() {
    return e.save;
  } });
})(hi);
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.save = o.isCaretAtStartOfInput = o.isCaretAtEndOfInput = o.getRange = o.getCaretNodeAndOffset = o.focus = o.getContenteditableSlice = o.checkContenteditableSliceForEmptiness = void 0;
  var e = lo;
  Object.defineProperty(o, "checkContenteditableSliceForEmptiness", { enumerable: !0, get: function() {
    return e.checkContenteditableSliceForEmptiness;
  } });
  var t = si;
  Object.defineProperty(o, "getContenteditableSlice", { enumerable: !0, get: function() {
    return t.getContenteditableSlice;
  } });
  var r = ai;
  Object.defineProperty(o, "focus", { enumerable: !0, get: function() {
    return r.focus;
  } });
  var n = Wo;
  Object.defineProperty(o, "getCaretNodeAndOffset", { enumerable: !0, get: function() {
    return n.getCaretNodeAndOffset;
  } });
  var i = li;
  Object.defineProperty(o, "getRange", { enumerable: !0, get: function() {
    return i.getRange;
  } });
  var s = ci;
  Object.defineProperty(o, "isCaretAtEndOfInput", { enumerable: !0, get: function() {
    return s.isCaretAtEndOfInput;
  } });
  var a = di;
  Object.defineProperty(o, "isCaretAtStartOfInput", { enumerable: !0, get: function() {
    return a.isCaretAtStartOfInput;
  } });
  var l = hi;
  Object.defineProperty(o, "save", { enumerable: !0, get: function() {
    return l.save;
  } });
})(Un);
class hc extends N {
  /**
   * All keydowns on Block
   *
   * @param {KeyboardEvent} event - keydown
   */
  keydown(e) {
    switch (this.beforeKeydownProcessing(e), e.keyCode) {
      case A.BACKSPACE:
        this.backspace(e);
        break;
      case A.DELETE:
        this.delete(e);
        break;
      case A.ENTER:
        this.enter(e);
        break;
      case A.DOWN:
      case A.RIGHT:
        this.arrowRightAndDown(e);
        break;
      case A.UP:
      case A.LEFT:
        this.arrowLeftAndUp(e);
        break;
      case A.TAB:
        this.tabPressed(e);
        break;
    }
    e.key === "/" && !e.ctrlKey && !e.metaKey && this.slashPressed(e), e.code === "Slash" && (e.ctrlKey || e.metaKey) && (e.preventDefault(), this.commandSlashPressed());
  }
  /**
   * Fires on keydown before event processing
   *
   * @param {KeyboardEvent} event - keydown
   */
  beforeKeydownProcessing(e) {
    this.needToolbarClosing(e) && cn(e.keyCode) && (this.Editor.Toolbar.close(), e.ctrlKey || e.metaKey || e.altKey || e.shiftKey || this.Editor.BlockSelection.clearSelection(e));
  }
  /**
   * Key up on Block:
   * - shows Inline Toolbar if something selected
   * - shows conversion toolbar with 85% of block selection
   *
   * @param {KeyboardEvent} event - keyup event
   */
  keyup(e) {
    e.shiftKey || this.Editor.UI.checkEmptiness();
  }
  /**
   * Add drop target styles
   *
   * @param {DragEvent} event - drag over event
   */
  dragOver(e) {
    const t = this.Editor.BlockManager.getBlockByChildNode(e.target);
    t.dropTarget = !0;
  }
  /**
   * Remove drop target style
   *
   * @param {DragEvent} event - drag leave event
   */
  dragLeave(e) {
    const t = this.Editor.BlockManager.getBlockByChildNode(e.target);
    t.dropTarget = !1;
  }
  /**
   * Copying selected blocks
   * Before putting to the clipboard we sanitize all blocks and then copy to the clipboard
   *
   * @param {ClipboardEvent} event - clipboard event
   */
  handleCommandC(e) {
    const { BlockSelection: t } = this.Editor;
    t.anyBlockSelected && t.copySelectedBlocks(e);
  }
  /**
   * Copy and Delete selected Blocks
   *
   * @param {ClipboardEvent} event - clipboard event
   */
  handleCommandX(e) {
    const { BlockSelection: t, BlockManager: r, Caret: n } = this.Editor;
    t.anyBlockSelected && t.copySelectedBlocks(e).then(() => {
      const i = r.removeSelectedBlocks(), s = r.insertDefaultBlockAtIndex(i, !0);
      n.setToBlock(s, n.positions.START), t.clearSelection(e);
    });
  }
  /**
   * Tab pressed inside a Block.
   *
   * @param {KeyboardEvent} event - keydown
   */
  tabPressed(e) {
    const { InlineToolbar: t, Caret: r } = this.Editor;
    t.opened || (e.shiftKey ? r.navigatePrevious(!0) : r.navigateNext(!0)) && e.preventDefault();
  }
  /**
   * '/' + 'command' keydown inside a Block
   */
  commandSlashPressed() {
    this.Editor.BlockSelection.selectedBlocks.length > 1 || this.activateBlockSettings();
  }
  /**
   * '/' keydown inside a Block
   *
   * @param event - keydown
   */
  slashPressed(e) {
    !this.Editor.UI.nodes.wrapper.contains(e.target) || !this.Editor.BlockManager.currentBlock.isEmpty || (e.preventDefault(), this.Editor.Caret.insertContentAtCaretPosition("/"), this.activateToolbox());
  }
  /**
   * ENTER pressed on block
   *
   * @param {KeyboardEvent} event - keydown
   */
  enter(e) {
    const { BlockManager: t, UI: r } = this.Editor, n = t.currentBlock;
    if (n === void 0 || n.tool.isLineBreaksEnabled || r.someToolbarOpened && r.someFlipperButtonFocused || e.shiftKey && !Vt)
      return;
    let i = n;
    n.currentInput !== void 0 && tt(n.currentInput) && !n.hasMedia ? this.Editor.BlockManager.insertDefaultBlockAtIndex(this.Editor.BlockManager.currentBlockIndex) : n.currentInput && ot(n.currentInput) ? i = this.Editor.BlockManager.insertDefaultBlockAtIndex(this.Editor.BlockManager.currentBlockIndex + 1) : i = this.Editor.BlockManager.split(), this.Editor.Caret.setToBlock(i), this.Editor.Toolbar.moveAndOpen(i), e.preventDefault();
  }
  /**
   * Handle backspace keydown on Block
   *
   * @param {KeyboardEvent} event - keydown
   */
  backspace(e) {
    const { BlockManager: t, Caret: r } = this.Editor, { currentBlock: n, previousBlock: i } = t;
    if (!(n === void 0 || !L.isCollapsed || !n.currentInput || !tt(n.currentInput))) {
      if (e.preventDefault(), this.Editor.Toolbar.close(), n.currentInput !== n.firstInput) {
        r.navigatePrevious();
        return;
      }
      if (i !== null) {
        if (i.isEmpty) {
          t.removeBlock(i);
          return;
        }
        if (n.isEmpty) {
          t.removeBlock(n);
          const s = t.currentBlock;
          r.setToBlock(s, r.positions.END);
          return;
        }
        zr(i, n) ? this.mergeBlocks(i, n) : r.setToBlock(i, r.positions.END);
      }
    }
  }
  /**
   * Handles delete keydown on Block
   * Removes char after the caret.
   * If caret is at the end of the block, merge next block with current
   *
   * @param {KeyboardEvent} event - keydown
   */
  delete(e) {
    const { BlockManager: t, Caret: r } = this.Editor, { currentBlock: n, nextBlock: i } = t;
    if (!(!L.isCollapsed || !ot(n.currentInput))) {
      if (e.preventDefault(), this.Editor.Toolbar.close(), n.currentInput !== n.lastInput) {
        r.navigateNext();
        return;
      }
      if (i !== null) {
        if (i.isEmpty) {
          t.removeBlock(i);
          return;
        }
        if (n.isEmpty) {
          t.removeBlock(n), r.setToBlock(i, r.positions.START);
          return;
        }
        zr(n, i) ? this.mergeBlocks(n, i) : r.setToBlock(i, r.positions.START);
      }
    }
  }
  /**
   * Merge passed Blocks
   *
   * @param targetBlock - to which Block we want to merge
   * @param blockToMerge - what Block we want to merge
   */
  mergeBlocks(e, t) {
    const { BlockManager: r, Toolbar: n } = this.Editor;
    e.lastInput !== void 0 && (Un.focus(e.lastInput, !1), r.mergeBlocks(e, t).then(() => {
      n.close();
    }));
  }
  /**
   * Handle right and down keyboard keys
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  arrowRightAndDown(e) {
    const t = ut.usedKeys.includes(e.keyCode) && (!e.shiftKey || e.keyCode === A.TAB);
    if (this.Editor.UI.someToolbarOpened && t)
      return;
    this.Editor.Toolbar.close();
    const { currentBlock: r } = this.Editor.BlockManager, n = ((r == null ? void 0 : r.currentInput) !== void 0 ? ot(r.currentInput) : void 0) || this.Editor.BlockSelection.anyBlockSelected;
    if (e.shiftKey && e.keyCode === A.DOWN && n) {
      this.Editor.CrossBlockSelection.toggleBlockSelectedState();
      return;
    }
    if (e.keyCode === A.DOWN || e.keyCode === A.RIGHT && !this.isRtl ? this.Editor.Caret.navigateNext() : this.Editor.Caret.navigatePrevious()) {
      e.preventDefault();
      return;
    }
    at(() => {
      this.Editor.BlockManager.currentBlock && this.Editor.BlockManager.currentBlock.updateCurrentInput();
    }, 20)(), this.Editor.BlockSelection.clearSelection(e);
  }
  /**
   * Handle left and up keyboard keys
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  arrowLeftAndUp(e) {
    if (this.Editor.UI.someToolbarOpened) {
      if (ut.usedKeys.includes(e.keyCode) && (!e.shiftKey || e.keyCode === A.TAB))
        return;
      this.Editor.UI.closeAllToolbars();
    }
    this.Editor.Toolbar.close();
    const { currentBlock: t } = this.Editor.BlockManager, r = ((t == null ? void 0 : t.currentInput) !== void 0 ? tt(t.currentInput) : void 0) || this.Editor.BlockSelection.anyBlockSelected;
    if (e.shiftKey && e.keyCode === A.UP && r) {
      this.Editor.CrossBlockSelection.toggleBlockSelectedState(!1);
      return;
    }
    if (e.keyCode === A.UP || e.keyCode === A.LEFT && !this.isRtl ? this.Editor.Caret.navigatePrevious() : this.Editor.Caret.navigateNext()) {
      e.preventDefault();
      return;
    }
    at(() => {
      this.Editor.BlockManager.currentBlock && this.Editor.BlockManager.currentBlock.updateCurrentInput();
    }, 20)(), this.Editor.BlockSelection.clearSelection(e);
  }
  /**
   * Cases when we need to close Toolbar
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  needToolbarClosing(e) {
    const t = e.keyCode === A.ENTER && this.Editor.Toolbar.toolbox.opened, r = e.keyCode === A.ENTER && this.Editor.BlockSettings.opened, n = e.keyCode === A.ENTER && this.Editor.InlineToolbar.opened, i = e.keyCode === A.TAB;
    return !(e.shiftKey || i || t || r || n);
  }
  /**
   * If Toolbox is not open, then just open it and show plus button
   */
  activateToolbox() {
    this.Editor.Toolbar.opened || this.Editor.Toolbar.moveAndOpen(), this.Editor.Toolbar.toolbox.open();
  }
  /**
   * Open Toolbar and show BlockSettings before flipping Tools
   */
  activateBlockSettings() {
    this.Editor.Toolbar.opened || this.Editor.Toolbar.moveAndOpen(), this.Editor.BlockSettings.opened || this.Editor.BlockSettings.open();
  }
}
let Dt = class {
  /**
   * @class
   * @param {HTMLElement} workingArea — editor`s working node
   */
  constructor(e) {
    this.blocks = [], this.workingArea = e;
  }
  /**
   * Get length of Block instances array
   *
   * @returns {number}
   */
  get length() {
    return this.blocks.length;
  }
  /**
   * Get Block instances array
   *
   * @returns {Block[]}
   */
  get array() {
    return this.blocks;
  }
  /**
   * Get blocks html elements array
   *
   * @returns {HTMLElement[]}
   */
  get nodes() {
    return dn(this.workingArea.children);
  }
  /**
   * Proxy trap to implement array-like setter
   *
   * @example
   * blocks[0] = new Block(...)
   * @param {Blocks} instance — Blocks instance
   * @param {PropertyKey} property — block index or any Blocks class property key to set
   * @param {Block} value — value to set
   * @returns {boolean}
   */
  static set(e, t, r) {
    return isNaN(Number(t)) ? (Reflect.set(e, t, r), !0) : (e.insert(+t, r), !0);
  }
  /**
   * Proxy trap to implement array-like getter
   *
   * @param {Blocks} instance — Blocks instance
   * @param {PropertyKey} property — Blocks class property key
   * @returns {Block|*}
   */
  static get(e, t) {
    return isNaN(Number(t)) ? Reflect.get(e, t) : e.get(+t);
  }
  /**
   * Push new Block to the blocks array and append it to working area
   *
   * @param {Block} block - Block to add
   */
  push(e) {
    this.blocks.push(e), this.insertToDOM(e);
  }
  /**
   * Swaps blocks with indexes first and second
   *
   * @param {number} first - first block index
   * @param {number} second - second block index
   * @deprecated — use 'move' instead
   */
  swap(e, t) {
    const r = this.blocks[t];
    g.swap(this.blocks[e].holder, r.holder), this.blocks[t] = this.blocks[e], this.blocks[e] = r;
  }
  /**
   * Move a block from one to another index
   *
   * @param {number} toIndex - new index of the block
   * @param {number} fromIndex - block to move
   */
  move(e, t) {
    const r = this.blocks.splice(t, 1)[0], n = e - 1, i = Math.max(0, n), s = this.blocks[i];
    e > 0 ? this.insertToDOM(r, "afterend", s) : this.insertToDOM(r, "beforebegin", s), this.blocks.splice(e, 0, r);
    const a = this.composeBlockEvent("move", {
      fromIndex: t,
      toIndex: e
    });
    r.call(oe.MOVED, a);
  }
  /**
   * Insert new Block at passed index
   *
   * @param {number} index — index to insert Block
   * @param {Block} block — Block to insert
   * @param {boolean} replace — it true, replace block on given index
   */
  insert(e, t, r = !1) {
    if (!this.length) {
      this.push(t);
      return;
    }
    e > this.length && (e = this.length), r && (this.blocks[e].holder.remove(), this.blocks[e].call(oe.REMOVED));
    const n = r ? 1 : 0;
    if (this.blocks.splice(e, n, t), e > 0) {
      const i = this.blocks[e - 1];
      this.insertToDOM(t, "afterend", i);
    } else {
      const i = this.blocks[e + 1];
      i ? this.insertToDOM(t, "beforebegin", i) : this.insertToDOM(t);
    }
  }
  /**
   * Replaces block under passed index with passed block
   *
   * @param index - index of existed block
   * @param block - new block
   */
  replace(e, t) {
    if (this.blocks[e] === void 0)
      throw Error("Incorrect index");
    this.blocks[e].holder.replaceWith(t.holder), this.blocks[e] = t;
  }
  /**
   * Inserts several blocks at once
   *
   * @param blocks - blocks to insert
   * @param index - index to insert blocks at
   */
  insertMany(e, t) {
    const r = new DocumentFragment();
    for (const n of e)
      r.appendChild(n.holder);
    if (this.length > 0) {
      if (t > 0) {
        const n = Math.min(t - 1, this.length - 1);
        this.blocks[n].holder.after(r);
      } else
        t === 0 && this.workingArea.prepend(r);
      this.blocks.splice(t, 0, ...e);
    } else
      this.blocks.push(...e), this.workingArea.appendChild(r);
    e.forEach((n) => n.call(oe.RENDERED));
  }
  /**
   * Remove block
   *
   * @param {number} index - index of Block to remove
   */
  remove(e) {
    isNaN(e) && (e = this.length - 1), this.blocks[e].holder.remove(), this.blocks[e].call(oe.REMOVED), this.blocks.splice(e, 1);
  }
  /**
   * Remove all blocks
   */
  removeAll() {
    this.workingArea.innerHTML = "", this.blocks.forEach((e) => e.call(oe.REMOVED)), this.blocks.length = 0;
  }
  /**
   * Insert Block after passed target
   *
   * @todo decide if this method is necessary
   * @param {Block} targetBlock — target after which Block should be inserted
   * @param {Block} newBlock — Block to insert
   */
  insertAfter(e, t) {
    const r = this.blocks.indexOf(e);
    this.insert(r + 1, t);
  }
  /**
   * Get Block by index
   *
   * @param {number} index — Block index
   * @returns {Block}
   */
  get(e) {
    return this.blocks[e];
  }
  /**
   * Return index of passed Block
   *
   * @param {Block} block - Block to find
   * @returns {number}
   */
  indexOf(e) {
    return this.blocks.indexOf(e);
  }
  /**
   * Insert new Block into DOM
   *
   * @param {Block} block - Block to insert
   * @param {InsertPosition} position — insert position (if set, will use insertAdjacentElement)
   * @param {Block} target — Block related to position
   */
  insertToDOM(e, t, r) {
    t ? r.holder.insertAdjacentElement(t, e.holder) : this.workingArea.appendChild(e.holder), e.call(oe.RENDERED);
  }
  /**
   * Composes Block event with passed type and details
   *
   * @param {string} type - event type
   * @param {object} detail - event detail
   */
  composeBlockEvent(e, t) {
    return new CustomEvent(e, {
      detail: t
    });
  }
};
const Zr = "block-removed", Gr = "block-added", uc = "block-moved", Jr = "block-changed";
class pc {
  constructor() {
    this.completed = Promise.resolve();
  }
  /**
   * Add new promise to queue
   *
   * @param operation - promise should be added to queue
   */
  add(e) {
    return new Promise((t, r) => {
      this.completed = this.completed.then(e).then(t).catch(r);
    });
  }
}
class fc extends N {
  constructor() {
    super(...arguments), this._currentBlockIndex = -1, this._blocks = null;
  }
  /**
   * Returns current Block index
   *
   * @returns {number}
   */
  get currentBlockIndex() {
    return this._currentBlockIndex;
  }
  /**
   * Set current Block index and fire Block lifecycle callbacks
   *
   * @param {number} newIndex - index of Block to set as current
   */
  set currentBlockIndex(e) {
    this._currentBlockIndex = e;
  }
  /**
   * returns first Block
   *
   * @returns {Block}
   */
  get firstBlock() {
    return this._blocks[0];
  }
  /**
   * returns last Block
   *
   * @returns {Block}
   */
  get lastBlock() {
    return this._blocks[this._blocks.length - 1];
  }
  /**
   * Get current Block instance
   *
   * @returns {Block}
   */
  get currentBlock() {
    return this._blocks[this.currentBlockIndex];
  }
  /**
   * Set passed Block as a current
   *
   * @param block - block to set as a current
   */
  set currentBlock(e) {
    this.currentBlockIndex = this.getBlockIndex(e);
  }
  /**
   * Returns next Block instance
   *
   * @returns {Block|null}
   */
  get nextBlock() {
    return this.currentBlockIndex === this._blocks.length - 1 ? null : this._blocks[this.currentBlockIndex + 1];
  }
  /**
   * Return first Block with inputs after current Block
   *
   * @returns {Block | undefined}
   */
  get nextContentfulBlock() {
    return this.blocks.slice(this.currentBlockIndex + 1).find((e) => !!e.inputs.length);
  }
  /**
   * Return first Block with inputs before current Block
   *
   * @returns {Block | undefined}
   */
  get previousContentfulBlock() {
    return this.blocks.slice(0, this.currentBlockIndex).reverse().find((e) => !!e.inputs.length);
  }
  /**
   * Returns previous Block instance
   *
   * @returns {Block|null}
   */
  get previousBlock() {
    return this.currentBlockIndex === 0 ? null : this._blocks[this.currentBlockIndex - 1];
  }
  /**
   * Get array of Block instances
   *
   * @returns {Block[]} {@link Blocks#array}
   */
  get blocks() {
    return this._blocks.array;
  }
  /**
   * Check if each Block is empty
   *
   * @returns {boolean}
   */
  get isEditorEmpty() {
    return this.blocks.every((e) => e.isEmpty);
  }
  /**
   * Should be called after Editor.UI preparation
   * Define this._blocks property
   */
  prepare() {
    const e = new Dt(this.Editor.UI.nodes.redactor);
    this._blocks = new Proxy(e, {
      set: Dt.set,
      get: Dt.get
    }), this.listeners.on(
      document,
      "copy",
      (t) => this.Editor.BlockEvents.handleCommandC(t)
    );
  }
  /**
   * Toggle read-only state
   *
   * If readOnly is true:
   *  - Unbind event handlers from created Blocks
   *
   * if readOnly is false:
   *  - Bind event handlers to all existing Blocks
   *
   * @param {boolean} readOnlyEnabled - "read only" state
   */
  toggleReadOnly(e) {
    e ? this.disableModuleBindings() : this.enableModuleBindings();
  }
  /**
   * Creates Block instance by tool name
   *
   * @param {object} options - block creation options
   * @param {string} options.tool - tools passed in editor config {@link EditorConfig#tools}
   * @param {string} [options.id] - unique id for this block
   * @param {BlockToolData} [options.data] - constructor params
   * @returns {Block}
   */
  composeBlock({
    tool: e,
    data: t = {},
    id: r = void 0,
    tunes: n = {}
  }) {
    const i = this.Editor.ReadOnly.isEnabled, s = this.Editor.Tools.blockTools.get(e), a = new ne({
      id: r,
      data: t,
      tool: s,
      api: this.Editor.API,
      readOnly: i,
      tunesData: n
    }, this.eventsDispatcher);
    return i || window.requestIdleCallback(() => {
      this.bindBlockEvents(a);
    }, { timeout: 2e3 }), a;
  }
  /**
   * Insert new block into _blocks
   *
   * @param {object} options - insert options
   * @param {string} [options.id] - block's unique id
   * @param {string} [options.tool] - plugin name, by default method inserts the default block type
   * @param {object} [options.data] - plugin data
   * @param {number} [options.index] - index where to insert new Block
   * @param {boolean} [options.needToFocus] - flag shows if needed to update current Block index
   * @param {boolean} [options.replace] - flag shows if block by passed index should be replaced with inserted one
   * @returns {Block}
   */
  insert({
    id: e = void 0,
    tool: t = this.config.defaultBlock,
    data: r = {},
    index: n,
    needToFocus: i = !0,
    replace: s = !1,
    tunes: a = {}
  } = {}) {
    let l = n;
    l === void 0 && (l = this.currentBlockIndex + (s ? 0 : 1));
    const c = this.composeBlock({
      id: e,
      tool: t,
      data: r,
      tunes: a
    });
    return s && this.blockDidMutated(Zr, this.getBlockByIndex(l), {
      index: l
    }), this._blocks.insert(l, c, s), this.blockDidMutated(Gr, c, {
      index: l
    }), i ? this.currentBlockIndex = l : l <= this.currentBlockIndex && this.currentBlockIndex++, c;
  }
  /**
   * Inserts several blocks at once
   *
   * @param blocks - blocks to insert
   * @param index - index where to insert
   */
  insertMany(e, t = 0) {
    this._blocks.insertMany(e, t);
  }
  /**
   * Update Block data.
   *
   * Currently we don't have an 'update' method in the Tools API, so we just create a new block with the same id and type
   * Should not trigger 'block-removed' or 'block-added' events.
   *
   * If neither data nor tunes is provided, return the provided block instead.
   *
   * @param block - block to update
   * @param data - (optional) new data
   * @param tunes - (optional) tune data
   */
  async update(e, t, r) {
    if (!t && !r)
      return e;
    const n = await e.data, i = this.composeBlock({
      id: e.id,
      tool: e.name,
      data: Object.assign({}, n, t ?? {}),
      tunes: r ?? e.tunes
    }), s = this.getBlockIndex(e);
    return this._blocks.replace(s, i), this.blockDidMutated(Jr, i, {
      index: s
    }), i;
  }
  /**
   * Replace passed Block with the new one with specified Tool and data
   *
   * @param block - block to replace
   * @param newTool - new Tool name
   * @param data - new Tool data
   */
  replace(e, t, r) {
    const n = this.getBlockIndex(e);
    return this.insert({
      tool: t,
      data: r,
      index: n,
      replace: !0
    });
  }
  /**
   * Insert pasted content. Call onPaste callback after insert.
   *
   * @param {string} toolName - name of Tool to insert
   * @param {PasteEvent} pasteEvent - pasted data
   * @param {boolean} replace - should replace current block
   */
  paste(e, t, r = !1) {
    const n = this.insert({
      tool: e,
      replace: r
    });
    try {
      window.requestIdleCallback(() => {
        n.call(oe.ON_PASTE, t);
      });
    } catch (i) {
      j(`${e}: onPaste callback call is failed`, "error", i);
    }
    return n;
  }
  /**
   * Insert new default block at passed index
   *
   * @param {number} index - index where Block should be inserted
   * @param {boolean} needToFocus - if true, updates current Block index
   *
   * TODO: Remove method and use insert() with index instead (?)
   * @returns {Block} inserted Block
   */
  insertDefaultBlockAtIndex(e, t = !1) {
    const r = this.composeBlock({ tool: this.config.defaultBlock });
    return this._blocks[e] = r, this.blockDidMutated(Gr, r, {
      index: e
    }), t ? this.currentBlockIndex = e : e <= this.currentBlockIndex && this.currentBlockIndex++, r;
  }
  /**
   * Always inserts at the end
   *
   * @returns {Block}
   */
  insertAtEnd() {
    return this.currentBlockIndex = this.blocks.length - 1, this.insert();
  }
  /**
   * Merge two blocks
   *
   * @param {Block} targetBlock - previous block will be append to this block
   * @param {Block} blockToMerge - block that will be merged with target block
   * @returns {Promise} - the sequence that can be continued
   */
  async mergeBlocks(e, t) {
    let r;
    if (e.name === t.name && e.mergeable) {
      const n = await t.data;
      if (X(n)) {
        console.error("Could not merge Block. Failed to extract original Block data.");
        return;
      }
      const [i] = to([n], e.tool.sanitizeConfig);
      r = i;
    } else if (e.mergeable && ct(t, "export") && ct(e, "import")) {
      const n = await t.exportDataAsString(), i = J(n, e.tool.sanitizeConfig);
      r = Vr(i, e.tool.conversionConfig);
    }
    r !== void 0 && (await e.mergeWith(r), this.removeBlock(t), this.currentBlockIndex = this._blocks.indexOf(e));
  }
  /**
   * Remove passed Block
   *
   * @param block - Block to remove
   * @param addLastBlock - if true, adds new default block at the end. @todo remove this logic and use event-bus instead
   */
  removeBlock(e, t = !0) {
    return new Promise((r) => {
      const n = this._blocks.indexOf(e);
      if (!this.validateIndex(n))
        throw new Error("Can't find a Block to remove");
      this._blocks.remove(n), e.destroy(), this.blockDidMutated(Zr, e, {
        index: n
      }), this.currentBlockIndex >= n && this.currentBlockIndex--, this.blocks.length ? n === 0 && (this.currentBlockIndex = 0) : (this.unsetCurrentBlock(), t && this.insert()), r();
    });
  }
  /**
   * Remove only selected Blocks
   * and returns first Block index where started removing...
   *
   * @returns {number|undefined}
   */
  removeSelectedBlocks() {
    let e;
    for (let t = this.blocks.length - 1; t >= 0; t--)
      this.blocks[t].selected && (this.removeBlock(this.blocks[t]), e = t);
    return e;
  }
  /**
   * Attention!
   * After removing insert the new default typed Block and focus on it
   * Removes all blocks
   */
  removeAllBlocks() {
    for (let e = this.blocks.length - 1; e >= 0; e--)
      this._blocks.remove(e);
    this.unsetCurrentBlock(), this.insert(), this.currentBlock.firstInput.focus();
  }
  /**
   * Split current Block
   * 1. Extract content from Caret position to the Block`s end
   * 2. Insert a new Block below current one with extracted content
   *
   * @returns {Block}
   */
  split() {
    const e = this.Editor.Caret.extractFragmentFromCaretPosition(), t = g.make("div");
    t.appendChild(e);
    const r = {
      text: g.isEmpty(t) ? "" : t.innerHTML
    };
    return this.insert({ data: r });
  }
  /**
   * Returns Block by passed index
   *
   * @param {number} index - index to get. -1 to get last
   * @returns {Block}
   */
  getBlockByIndex(e) {
    return e === -1 && (e = this._blocks.length - 1), this._blocks[e];
  }
  /**
   * Returns an index for passed Block
   *
   * @param block - block to find index
   */
  getBlockIndex(e) {
    return this._blocks.indexOf(e);
  }
  /**
   * Returns the Block by passed id
   *
   * @param id - id of block to get
   * @returns {Block}
   */
  getBlockById(e) {
    return this._blocks.array.find((t) => t.id === e);
  }
  /**
   * Get Block instance by html element
   *
   * @param {Node} element - html element to get Block by
   */
  getBlock(e) {
    g.isElement(e) || (e = e.parentNode);
    const t = this._blocks.nodes, r = e.closest(`.${ne.CSS.wrapper}`), n = t.indexOf(r);
    if (n >= 0)
      return this._blocks[n];
  }
  /**
   * 1) Find first-level Block from passed child Node
   * 2) Mark it as current
   *
   * @param {Node} childNode - look ahead from this node.
   * @returns {Block | undefined} can return undefined in case when the passed child note is not a part of the current editor instance
   */
  setCurrentBlockByChildNode(e) {
    g.isElement(e) || (e = e.parentNode);
    const t = e.closest(`.${ne.CSS.wrapper}`);
    if (!t)
      return;
    const r = t.closest(`.${this.Editor.UI.CSS.editorWrapper}`);
    if (r != null && r.isEqualNode(this.Editor.UI.nodes.wrapper))
      return this.currentBlockIndex = this._blocks.nodes.indexOf(t), this.currentBlock.updateCurrentInput(), this.currentBlock;
  }
  /**
   * Return block which contents passed node
   *
   * @param {Node} childNode - node to get Block by
   * @returns {Block}
   */
  getBlockByChildNode(e) {
    if (!e || !(e instanceof Node))
      return;
    g.isElement(e) || (e = e.parentNode);
    const t = e.closest(`.${ne.CSS.wrapper}`);
    return this.blocks.find((r) => r.holder === t);
  }
  /**
   * Swap Blocks Position
   *
   * @param {number} fromIndex - index of first block
   * @param {number} toIndex - index of second block
   * @deprecated — use 'move' instead
   */
  swap(e, t) {
    this._blocks.swap(e, t), this.currentBlockIndex = t;
  }
  /**
   * Move a block to a new index
   *
   * @param {number} toIndex - index where to move Block
   * @param {number} fromIndex - index of Block to move
   */
  move(e, t = this.currentBlockIndex) {
    if (isNaN(e) || isNaN(t)) {
      j("Warning during 'move' call: incorrect indices provided.", "warn");
      return;
    }
    if (!this.validateIndex(e) || !this.validateIndex(t)) {
      j("Warning during 'move' call: indices cannot be lower than 0 or greater than the amount of blocks.", "warn");
      return;
    }
    this._blocks.move(e, t), this.currentBlockIndex = e, this.blockDidMutated(uc, this.currentBlock, {
      fromIndex: t,
      toIndex: e
    });
  }
  /**
   * Converts passed Block to the new Tool
   * Uses Conversion Config
   *
   * @param blockToConvert - Block that should be converted
   * @param targetToolName - name of the Tool to convert to
   * @param blockDataOverrides - optional new Block data overrides
   */
  async convert(e, t, r) {
    if (!await e.save())
      throw new Error("Could not convert Block. Failed to extract original Block data.");
    const n = this.Editor.Tools.blockTools.get(t);
    if (!n)
      throw new Error(`Could not convert Block. Tool «${t}» not found.`);
    const i = await e.exportDataAsString(), s = J(
      i,
      n.sanitizeConfig
    );
    let a = Vr(s, n.conversionConfig, n.settings);
    return r && (a = Object.assign(a, r)), this.replace(e, n.name, a);
  }
  /**
   * Sets current Block Index -1 which means unknown
   * and clear highlights
   */
  unsetCurrentBlock() {
    this.currentBlockIndex = -1;
  }
  /**
   * Clears Editor
   *
   * @param {boolean} needToAddDefaultBlock - 1) in internal calls (for example, in api.blocks.render)
   *                                             we don't need to add an empty default block
   *                                        2) in api.blocks.clear we should add empty block
   */
  async clear(e = !1) {
    const t = new pc();
    [...this.blocks].forEach((r) => {
      t.add(async () => {
        await this.removeBlock(r, !1);
      });
    }), await t.completed, this.unsetCurrentBlock(), e && this.insert(), this.Editor.UI.checkEmptiness();
  }
  /**
   * Cleans up all the block tools' resources
   * This is called when editor is destroyed
   */
  async destroy() {
    await Promise.all(this.blocks.map((e) => e.destroy()));
  }
  /**
   * Bind Block events
   *
   * @param {Block} block - Block to which event should be bound
   */
  bindBlockEvents(e) {
    const { BlockEvents: t } = this.Editor;
    this.readOnlyMutableListeners.on(e.holder, "keydown", (r) => {
      t.keydown(r);
    }), this.readOnlyMutableListeners.on(e.holder, "keyup", (r) => {
      t.keyup(r);
    }), this.readOnlyMutableListeners.on(e.holder, "dragover", (r) => {
      t.dragOver(r);
    }), this.readOnlyMutableListeners.on(e.holder, "dragleave", (r) => {
      t.dragLeave(r);
    }), e.on("didMutated", (r) => this.blockDidMutated(Jr, r, {
      index: this.getBlockIndex(r)
    }));
  }
  /**
   * Disable mutable handlers and bindings
   */
  disableModuleBindings() {
    this.readOnlyMutableListeners.clearAll();
  }
  /**
   * Enables all module handlers and bindings for all Blocks
   */
  enableModuleBindings() {
    this.readOnlyMutableListeners.on(
      document,
      "cut",
      (e) => this.Editor.BlockEvents.handleCommandX(e)
    ), this.blocks.forEach((e) => {
      this.bindBlockEvents(e);
    });
  }
  /**
   * Validates that the given index is not lower than 0 or higher than the amount of blocks
   *
   * @param {number} index - index of blocks array to validate
   * @returns {boolean}
   */
  validateIndex(e) {
    return !(e < 0 || e >= this._blocks.length);
  }
  /**
   * Block mutation callback
   *
   * @param mutationType - what happened with block
   * @param block - mutated block
   * @param detailData - additional data to pass with change event
   */
  blockDidMutated(e, t, r) {
    const n = new CustomEvent(e, {
      detail: {
        target: new te(t),
        ...r
      }
    });
    return this.eventsDispatcher.emit(vn, {
      event: n
    }), t;
  }
}
class gc extends N {
  constructor() {
    super(...arguments), this.anyBlockSelectedCache = null, this.needToSelectAll = !1, this.nativeInputSelected = !1, this.readyToBlockSelection = !1;
  }
  /**
   * Sanitizer Config
   *
   * @returns {SanitizerConfig}
   */
  get sanitizerConfig() {
    return {
      p: {},
      h1: {},
      h2: {},
      h3: {},
      h4: {},
      h5: {},
      h6: {},
      ol: {},
      ul: {},
      li: {},
      br: !0,
      img: {
        src: !0,
        width: !0,
        height: !0
      },
      a: {
        href: !0
      },
      b: {},
      i: {},
      u: {}
    };
  }
  /**
   * Flag that identifies all Blocks selection
   *
   * @returns {boolean}
   */
  get allBlocksSelected() {
    const { BlockManager: e } = this.Editor;
    return e.blocks.every((t) => t.selected === !0);
  }
  /**
   * Set selected all blocks
   *
   * @param {boolean} state - state to set
   */
  set allBlocksSelected(e) {
    const { BlockManager: t } = this.Editor;
    t.blocks.forEach((r) => {
      r.selected = e;
    }), this.clearCache();
  }
  /**
   * Flag that identifies any Block selection
   *
   * @returns {boolean}
   */
  get anyBlockSelected() {
    const { BlockManager: e } = this.Editor;
    return this.anyBlockSelectedCache === null && (this.anyBlockSelectedCache = e.blocks.some((t) => t.selected === !0)), this.anyBlockSelectedCache;
  }
  /**
   * Return selected Blocks array
   *
   * @returns {Block[]}
   */
  get selectedBlocks() {
    return this.Editor.BlockManager.blocks.filter((e) => e.selected);
  }
  /**
   * Module Preparation
   * Registers Shortcuts CMD+A and CMD+C
   * to select all and copy them
   */
  prepare() {
    this.selection = new L(), Be.add({
      name: "CMD+A",
      handler: (e) => {
        const { BlockManager: t, ReadOnly: r } = this.Editor;
        if (r.isEnabled) {
          e.preventDefault(), this.selectAllBlocks();
          return;
        }
        t.currentBlock && this.handleCommandA(e);
      },
      on: this.Editor.UI.nodes.redactor
    });
  }
  /**
   * Toggle read-only state
   *
   *  - Remove all ranges
   *  - Unselect all Blocks
   */
  toggleReadOnly() {
    L.get().removeAllRanges(), this.allBlocksSelected = !1;
  }
  /**
   * Remove selection of Block
   *
   * @param {number?} index - Block index according to the BlockManager's indexes
   */
  unSelectBlockByIndex(e) {
    const { BlockManager: t } = this.Editor;
    let r;
    isNaN(e) ? r = t.currentBlock : r = t.getBlockByIndex(e), r.selected = !1, this.clearCache();
  }
  /**
   * Clear selection from Blocks
   *
   * @param {Event} reason - event caused clear of selection
   * @param {boolean} restoreSelection - if true, restore saved selection
   */
  clearSelection(e, t = !1) {
    const { BlockManager: r, Caret: n, RectangleSelection: i } = this.Editor;
    this.needToSelectAll = !1, this.nativeInputSelected = !1, this.readyToBlockSelection = !1;
    const s = e && e instanceof KeyboardEvent, a = s && cn(e.keyCode);
    if (this.anyBlockSelected && s && a && !L.isSelectionExists) {
      const l = r.removeSelectedBlocks();
      r.insertDefaultBlockAtIndex(l, !0), n.setToBlock(r.currentBlock), at(() => {
        const c = e.key;
        n.insertContentAtCaretPosition(c.length > 1 ? "" : c);
      }, 20)();
    }
    if (this.Editor.CrossBlockSelection.clear(e), !this.anyBlockSelected || i.isRectActivated()) {
      this.Editor.RectangleSelection.clearSelection();
      return;
    }
    t && this.selection.restore(), this.allBlocksSelected = !1;
  }
  /**
   * Reduce each Block and copy its content
   *
   * @param {ClipboardEvent} e - copy/cut event
   * @returns {Promise<void>}
   */
  copySelectedBlocks(e) {
    e.preventDefault();
    const t = g.make("div");
    this.selectedBlocks.forEach((i) => {
      const s = J(i.holder.innerHTML, this.sanitizerConfig), a = g.make("p");
      a.innerHTML = s, t.appendChild(a);
    });
    const r = Array.from(t.childNodes).map((i) => i.textContent).join(`

`), n = t.innerHTML;
    return e.clipboardData.setData("text/plain", r), e.clipboardData.setData("text/html", n), Promise.all(this.selectedBlocks.map((i) => i.save())).then((i) => {
      try {
        e.clipboardData.setData(this.Editor.Paste.MIME_TYPE, JSON.stringify(i));
      } catch {
      }
    });
  }
  /**
   * Select Block by its index
   *
   * @param {number?} index - Block index according to the BlockManager's indexes
   */
  selectBlockByIndex(e) {
    const { BlockManager: t } = this.Editor, r = t.getBlockByIndex(e);
    r !== void 0 && this.selectBlock(r);
  }
  /**
   * Select passed Block
   *
   * @param {Block} block - Block to select
   */
  selectBlock(e) {
    this.selection.save(), L.get().removeAllRanges(), e.selected = !0, this.clearCache(), this.Editor.InlineToolbar.close();
  }
  /**
   * Remove selection from passed Block
   *
   * @param {Block} block - Block to unselect
   */
  unselectBlock(e) {
    e.selected = !1, this.clearCache();
  }
  /**
   * Clear anyBlockSelected cache
   */
  clearCache() {
    this.anyBlockSelectedCache = null;
  }
  /**
   * Module destruction
   * De-registers Shortcut CMD+A
   */
  destroy() {
    Be.remove(this.Editor.UI.nodes.redactor, "CMD+A");
  }
  /**
   * First CMD+A selects all input content by native behaviour,
   * next CMD+A keypress selects all blocks
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  handleCommandA(e) {
    if (this.Editor.RectangleSelection.clearSelection(), g.isNativeInput(e.target) && !this.readyToBlockSelection) {
      this.readyToBlockSelection = !0;
      return;
    }
    const t = this.Editor.BlockManager.getBlock(e.target), r = t.inputs;
    if (r.length > 1 && !this.readyToBlockSelection) {
      this.readyToBlockSelection = !0;
      return;
    }
    if (r.length === 1 && !this.needToSelectAll) {
      this.needToSelectAll = !0;
      return;
    }
    this.needToSelectAll ? (e.preventDefault(), this.selectAllBlocks(), this.needToSelectAll = !1, this.readyToBlockSelection = !1) : this.readyToBlockSelection && (e.preventDefault(), this.selectBlock(t), this.needToSelectAll = !0);
  }
  /**
   * Select All Blocks
   * Each Block has selected setter that makes Block copyable
   */
  selectAllBlocks() {
    this.selection.save(), L.get().removeAllRanges(), this.allBlocksSelected = !0, this.Editor.InlineToolbar.close();
  }
}
let mc = class Zt extends N {
  /**
   * Allowed caret positions in input
   *
   * @static
   * @returns {{START: string, END: string, DEFAULT: string}}
   */
  get positions() {
    return {
      START: "start",
      END: "end",
      DEFAULT: "default"
    };
  }
  /**
   * Elements styles that can be useful for Caret Module
   */
  static get CSS() {
    return {
      shadowCaret: "cdx-shadow-caret"
    };
  }
  /**
   * Method gets Block instance and puts caret to the text node with offset
   * There two ways that method applies caret position:
   *   - first found text node: sets at the beginning, but you can pass an offset
   *   - last found text node: sets at the end of the node. Also, you can customize the behaviour
   *
   * @param {Block} block - Block class
   * @param {string} position - position where to set caret.
   *                            If default - leave default behaviour and apply offset if it's passed
   * @param {number} offset - caret offset regarding to the block content
   */
  setToBlock(e, t = this.positions.DEFAULT, r = 0) {
    var n;
    const { BlockManager: i, BlockSelection: s } = this.Editor;
    if (s.clearSelection(), !e.focusable) {
      (n = window.getSelection()) == null || n.removeAllRanges(), s.selectBlock(e), i.currentBlock = e;
      return;
    }
    let a;
    switch (t) {
      case this.positions.START:
        a = e.firstInput;
        break;
      case this.positions.END:
        a = e.lastInput;
        break;
      default:
        a = e.currentInput;
    }
    if (!a)
      return;
    let l, c = r;
    if (t === this.positions.START)
      l = g.getDeepestNode(a, !1), c = 0;
    else if (t === this.positions.END)
      l = g.getDeepestNode(a, !0), c = g.getContentLength(l);
    else {
      const { node: d, offset: h } = g.getNodeByOffset(a, r);
      d ? (l = d, c = h) : (l = g.getDeepestNode(a, !1), c = 0);
    }
    this.set(l, c), i.setCurrentBlockByChildNode(e.holder), i.currentBlock.currentInput = a;
  }
  /**
   * Set caret to the current input of current Block.
   *
   * @param {HTMLElement} input - input where caret should be set
   * @param {string} position - position of the caret.
   *                            If default - leave default behaviour and apply offset if it's passed
   * @param {number} offset - caret offset regarding to the text node
   */
  setToInput(e, t = this.positions.DEFAULT, r = 0) {
    const { currentBlock: n } = this.Editor.BlockManager, i = g.getDeepestNode(e);
    switch (t) {
      case this.positions.START:
        this.set(i, 0);
        break;
      case this.positions.END:
        this.set(i, g.getContentLength(i));
        break;
      default:
        r && this.set(i, r);
    }
    n.currentInput = e;
  }
  /**
   * Creates Document Range and sets caret to the element with offset
   *
   * @param {HTMLElement} element - target node.
   * @param {number} offset - offset
   */
  set(e, t = 0) {
    const { top: r, bottom: n } = L.setCursor(e, t), { innerHeight: i } = window;
    r < 0 ? window.scrollBy(0, r - 30) : n > i && window.scrollBy(0, n - i + 30);
  }
  /**
   * Set Caret to the last Block
   * If last block is not empty, append another empty block
   */
  setToTheLastBlock() {
    const e = this.Editor.BlockManager.lastBlock;
    if (e)
      if (e.tool.isDefault && e.isEmpty)
        this.setToBlock(e);
      else {
        const t = this.Editor.BlockManager.insertAtEnd();
        this.setToBlock(t);
      }
  }
  /**
   * Extract content fragment of current Block from Caret position to the end of the Block
   */
  extractFragmentFromCaretPosition() {
    const e = L.get();
    if (e.rangeCount) {
      const t = e.getRangeAt(0), r = this.Editor.BlockManager.currentBlock.currentInput;
      if (t.deleteContents(), r)
        if (g.isNativeInput(r)) {
          const n = r, i = document.createDocumentFragment(), s = n.value.substring(0, n.selectionStart), a = n.value.substring(n.selectionStart);
          return i.textContent = a, n.value = s, i;
        } else {
          const n = t.cloneRange();
          return n.selectNodeContents(r), n.setStart(t.endContainer, t.endOffset), n.extractContents();
        }
    }
  }
  /**
   * Set's caret to the next Block or Tool`s input
   * Before moving caret, we should check if caret position is at the end of Plugins node
   * Using {@link Dom#getDeepestNode} to get a last node and match with current selection
   *
   * @param {boolean} force - pass true to skip check for caret position
   */
  navigateNext(e = !1) {
    const { BlockManager: t } = this.Editor, { currentBlock: r, nextBlock: n } = t;
    if (r === void 0)
      return !1;
    const { nextInput: i, currentInput: s } = r, a = s !== void 0 ? ot(s) : void 0;
    let l = n;
    const c = e || a || !r.focusable;
    if (i && c)
      return this.setToInput(i, this.positions.START), !0;
    if (l === null) {
      if (r.tool.isDefault || !c)
        return !1;
      l = t.insertAtEnd();
    }
    return c ? (this.setToBlock(l, this.positions.START), !0) : !1;
  }
  /**
   * Set's caret to the previous Tool`s input or Block
   * Before moving caret, we should check if caret position is start of the Plugins node
   * Using {@link Dom#getDeepestNode} to get a last node and match with current selection
   *
   * @param {boolean} force - pass true to skip check for caret position
   */
  navigatePrevious(e = !1) {
    const { currentBlock: t, previousBlock: r } = this.Editor.BlockManager;
    if (!t)
      return !1;
    const { previousInput: n, currentInput: i } = t, s = i !== void 0 ? tt(i) : void 0, a = e || s || !t.focusable;
    return n && a ? (this.setToInput(n, this.positions.END), !0) : r !== null && a ? (this.setToBlock(r, this.positions.END), !0) : !1;
  }
  /**
   * Inserts shadow element after passed element where caret can be placed
   *
   * @param {Element} element - element after which shadow caret should be inserted
   */
  createShadow(e) {
    const t = document.createElement("span");
    t.classList.add(Zt.CSS.shadowCaret), e.insertAdjacentElement("beforeend", t);
  }
  /**
   * Restores caret position
   *
   * @param {HTMLElement} element - element where caret should be restored
   */
  restoreCaret(e) {
    const t = e.querySelector(`.${Zt.CSS.shadowCaret}`);
    if (!t)
      return;
    new L().expandToTag(t);
    const r = document.createRange();
    r.selectNode(t), r.extractContents();
  }
  /**
   * Inserts passed content at caret position
   *
   * @param {string} content - content to insert
   */
  insertContentAtCaretPosition(e) {
    const t = document.createDocumentFragment(), r = document.createElement("div"), n = L.get(), i = L.range;
    r.innerHTML = e, Array.from(r.childNodes).forEach((c) => t.appendChild(c)), t.childNodes.length === 0 && t.appendChild(new Text());
    const s = t.lastChild;
    i.deleteContents(), i.insertNode(t);
    const a = document.createRange(), l = s.nodeType === Node.TEXT_NODE ? s : s.firstChild;
    l !== null && l.textContent !== null && a.setStart(l, l.textContent.length), n.removeAllRanges(), n.addRange(a);
  }
};
class vc extends N {
  constructor() {
    super(...arguments), this.onMouseUp = () => {
      this.listeners.off(document, "mouseover", this.onMouseOver), this.listeners.off(document, "mouseup", this.onMouseUp);
    }, this.onMouseOver = (e) => {
      const { BlockManager: t, BlockSelection: r } = this.Editor;
      if (e.relatedTarget === null && e.target === null)
        return;
      const n = t.getBlockByChildNode(e.relatedTarget) || this.lastSelectedBlock, i = t.getBlockByChildNode(e.target);
      if (!(!n || !i) && i !== n) {
        if (n === this.firstSelectedBlock) {
          L.get().removeAllRanges(), n.selected = !0, i.selected = !0, r.clearCache();
          return;
        }
        if (i === this.firstSelectedBlock) {
          n.selected = !1, i.selected = !1, r.clearCache();
          return;
        }
        this.Editor.InlineToolbar.close(), this.toggleBlocksSelectedState(n, i), this.lastSelectedBlock = i;
      }
    };
  }
  /**
   * Module preparation
   *
   * @returns {Promise}
   */
  async prepare() {
    this.listeners.on(document, "mousedown", (e) => {
      this.enableCrossBlockSelection(e);
    });
  }
  /**
   * Sets up listeners
   *
   * @param {MouseEvent} event - mouse down event
   */
  watchSelection(e) {
    if (e.button !== os.LEFT)
      return;
    const { BlockManager: t } = this.Editor;
    this.firstSelectedBlock = t.getBlock(e.target), this.lastSelectedBlock = this.firstSelectedBlock, this.listeners.on(document, "mouseover", this.onMouseOver), this.listeners.on(document, "mouseup", this.onMouseUp);
  }
  /**
   * Return boolean is cross block selection started:
   * there should be at least 2 selected blocks
   */
  get isCrossBlockSelectionStarted() {
    return !!this.firstSelectedBlock && !!this.lastSelectedBlock && this.firstSelectedBlock !== this.lastSelectedBlock;
  }
  /**
   * Change selection state of the next Block
   * Used for CBS via Shift + arrow keys
   *
   * @param {boolean} next - if true, toggle next block. Previous otherwise
   */
  toggleBlockSelectedState(e = !0) {
    const { BlockManager: t, BlockSelection: r } = this.Editor;
    this.lastSelectedBlock || (this.lastSelectedBlock = this.firstSelectedBlock = t.currentBlock), this.firstSelectedBlock === this.lastSelectedBlock && (this.firstSelectedBlock.selected = !0, r.clearCache(), L.get().removeAllRanges());
    const n = t.blocks.indexOf(this.lastSelectedBlock) + (e ? 1 : -1), i = t.blocks[n];
    i && (this.lastSelectedBlock.selected !== i.selected ? (i.selected = !0, r.clearCache()) : (this.lastSelectedBlock.selected = !1, r.clearCache()), this.lastSelectedBlock = i, this.Editor.InlineToolbar.close(), i.holder.scrollIntoView({
      block: "nearest"
    }));
  }
  /**
   * Clear saved state
   *
   * @param {Event} reason - event caused clear of selection
   */
  clear(e) {
    const { BlockManager: t, BlockSelection: r, Caret: n } = this.Editor, i = t.blocks.indexOf(this.firstSelectedBlock), s = t.blocks.indexOf(this.lastSelectedBlock);
    if (r.anyBlockSelected && i > -1 && s > -1 && e && e instanceof KeyboardEvent)
      switch (e.keyCode) {
        case A.DOWN:
        case A.RIGHT:
          n.setToBlock(t.blocks[Math.max(i, s)], n.positions.END);
          break;
        case A.UP:
        case A.LEFT:
          n.setToBlock(t.blocks[Math.min(i, s)], n.positions.START);
          break;
        default:
          n.setToBlock(t.blocks[Math.max(i, s)], n.positions.END);
      }
    this.firstSelectedBlock = this.lastSelectedBlock = null;
  }
  /**
   * Enables Cross Block Selection
   *
   * @param {MouseEvent} event - mouse down event
   */
  enableCrossBlockSelection(e) {
    const { UI: t } = this.Editor;
    L.isCollapsed || this.Editor.BlockSelection.clearSelection(e), t.nodes.redactor.contains(e.target) ? this.watchSelection(e) : this.Editor.BlockSelection.clearSelection(e);
  }
  /**
   * Change blocks selection state between passed two blocks.
   *
   * @param {Block} firstBlock - first block in range
   * @param {Block} lastBlock - last block in range
   */
  toggleBlocksSelectedState(e, t) {
    const { BlockManager: r, BlockSelection: n } = this.Editor, i = r.blocks.indexOf(e), s = r.blocks.indexOf(t), a = e.selected !== t.selected;
    for (let l = Math.min(i, s); l <= Math.max(i, s); l++) {
      const c = r.blocks[l];
      c !== this.firstSelectedBlock && c !== (a ? e : t) && (r.blocks[l].selected = !r.blocks[l].selected, n.clearCache());
    }
  }
}
class bc extends N {
  constructor() {
    super(...arguments), this.isStartedAtEditor = !1;
  }
  /**
   * Toggle read-only state
   *
   * if state is true:
   *  - disable all drag-n-drop event handlers
   *
   * if state is false:
   *  - restore drag-n-drop event handlers
   *
   * @param {boolean} readOnlyEnabled - "read only" state
   */
  toggleReadOnly(e) {
    e ? this.disableModuleBindings() : this.enableModuleBindings();
  }
  /**
   * Add drag events listeners to editor zone
   */
  enableModuleBindings() {
    const { UI: e } = this.Editor;
    this.readOnlyMutableListeners.on(e.nodes.holder, "drop", async (t) => {
      await this.processDrop(t);
    }, !0), this.readOnlyMutableListeners.on(e.nodes.holder, "dragstart", () => {
      this.processDragStart();
    }), this.readOnlyMutableListeners.on(e.nodes.holder, "dragover", (t) => {
      this.processDragOver(t);
    }, !0);
  }
  /**
   * Unbind drag-n-drop event handlers
   */
  disableModuleBindings() {
    this.readOnlyMutableListeners.clearAll();
  }
  /**
   * Handle drop event
   *
   * @param {DragEvent} dropEvent - drop event
   */
  async processDrop(e) {
    const {
      BlockManager: t,
      Paste: r,
      Caret: n
    } = this.Editor;
    e.preventDefault(), t.blocks.forEach((s) => {
      s.dropTarget = !1;
    }), L.isAtEditor && !L.isCollapsed && this.isStartedAtEditor && document.execCommand("delete"), this.isStartedAtEditor = !1;
    const i = t.setCurrentBlockByChildNode(e.target);
    if (i)
      this.Editor.Caret.setToBlock(i, n.positions.END);
    else {
      const s = t.setCurrentBlockByChildNode(t.lastBlock.holder);
      this.Editor.Caret.setToBlock(s, n.positions.END);
    }
    await r.processDataTransfer(e.dataTransfer, !0);
  }
  /**
   * Handle drag start event
   */
  processDragStart() {
    L.isAtEditor && !L.isCollapsed && (this.isStartedAtEditor = !0), this.Editor.InlineToolbar.close();
  }
  /**
   * @param {DragEvent} dragEvent - drag event
   */
  processDragOver(e) {
    e.preventDefault();
  }
}
const kc = 180, wc = 400;
class yc extends N {
  /**
   * Prepare the module
   *
   * @param options - options used by the modification observer module
   * @param options.config - Editor configuration object
   * @param options.eventsDispatcher - common Editor event bus
   */
  constructor({ config: e, eventsDispatcher: t }) {
    super({
      config: e,
      eventsDispatcher: t
    }), this.disabled = !1, this.batchingTimeout = null, this.batchingOnChangeQueue = /* @__PURE__ */ new Map(), this.batchTime = wc, this.mutationObserver = new MutationObserver((r) => {
      this.redactorChanged(r);
    }), this.eventsDispatcher.on(vn, (r) => {
      this.particularBlockChanged(r.event);
    }), this.eventsDispatcher.on(bn, () => {
      this.disable();
    }), this.eventsDispatcher.on(kn, () => {
      this.enable();
    });
  }
  /**
   * Enables onChange event
   */
  enable() {
    this.mutationObserver.observe(
      this.Editor.UI.nodes.redactor,
      {
        childList: !0,
        subtree: !0,
        characterData: !0,
        attributes: !0
      }
    ), this.disabled = !1;
  }
  /**
   * Disables onChange event
   */
  disable() {
    this.mutationObserver.disconnect(), this.disabled = !0;
  }
  /**
   * Call onChange event passed to Editor.js configuration
   *
   * @param event - some of our custom change events
   */
  particularBlockChanged(e) {
    this.disabled || !R(this.config.onChange) || (this.batchingOnChangeQueue.set(`block:${e.detail.target.id}:event:${e.type}`, e), this.batchingTimeout && clearTimeout(this.batchingTimeout), this.batchingTimeout = setTimeout(() => {
      let t;
      this.batchingOnChangeQueue.size === 1 ? t = this.batchingOnChangeQueue.values().next().value : t = Array.from(this.batchingOnChangeQueue.values()), this.config.onChange && this.config.onChange(this.Editor.API.methods, t), this.batchingOnChangeQueue.clear();
    }, this.batchTime));
  }
  /**
   * Fired on every blocks wrapper dom change
   *
   * @param mutations - mutations happened
   */
  redactorChanged(e) {
    this.eventsDispatcher.emit(Wt, {
      mutations: e
    });
  }
}
const ui = class pi extends N {
  constructor() {
    super(...arguments), this.MIME_TYPE = "application/x-editor-js", this.toolsTags = {}, this.tagsByTool = {}, this.toolsPatterns = [], this.toolsFiles = {}, this.exceptionList = [], this.processTool = (e) => {
      try {
        const t = e.create({}, {}, !1);
        if (e.pasteConfig === !1) {
          this.exceptionList.push(e.name);
          return;
        }
        if (!R(t.onPaste))
          return;
        this.getTagsConfig(e), this.getFilesConfig(e), this.getPatternsConfig(e);
      } catch (t) {
        j(
          `Paste handling for «${e.name}» Tool hasn't been set up because of the error`,
          "warn",
          t
        );
      }
    }, this.handlePasteEvent = async (e) => {
      const { BlockManager: t, Toolbar: r } = this.Editor, n = t.setCurrentBlockByChildNode(e.target);
      !n || this.isNativeBehaviour(e.target) && !e.clipboardData.types.includes("Files") || n && this.exceptionList.includes(n.name) || (e.preventDefault(), this.processDataTransfer(e.clipboardData), r.close());
    };
  }
  /**
   * Set onPaste callback and collect tools` paste configurations
   */
  async prepare() {
    this.processTools();
  }
  /**
   * Set read-only state
   *
   * @param {boolean} readOnlyEnabled - read only flag value
   */
  toggleReadOnly(e) {
    e ? this.unsetCallback() : this.setCallback();
  }
  /**
   * Handle pasted or dropped data transfer object
   *
   * @param {DataTransfer} dataTransfer - pasted or dropped data transfer object
   * @param {boolean} isDragNDrop - true if data transfer comes from drag'n'drop events
   */
  async processDataTransfer(e, t = !1) {
    const { Tools: r } = this.Editor, n = e.types;
    if ((n.includes ? n.includes("Files") : n.contains("Files")) && !X(this.toolsFiles)) {
      await this.processFiles(e.files);
      return;
    }
    const i = e.getData(this.MIME_TYPE), s = e.getData("text/plain");
    let a = e.getData("text/html");
    if (i)
      try {
        this.insertEditorJSData(JSON.parse(i));
        return;
      } catch {
      }
    t && s.trim() && a.trim() && (a = "<p>" + (a.trim() ? a : s) + "</p>");
    const l = Object.keys(this.toolsTags).reduce((h, u) => (h[u.toLowerCase()] = this.toolsTags[u].sanitizationConfig ?? {}, h), {}), c = Object.assign({}, l, r.getAllInlineToolsSanitizeConfig(), { br: {} }), d = J(a, c);
    !d.trim() || d.trim() === s || !g.isHTMLString(d) ? await this.processText(s) : await this.processText(d, !0);
  }
  /**
   * Process pasted text and divide them into Blocks
   *
   * @param {string} data - text to process. Can be HTML or plain.
   * @param {boolean} isHTML - if passed string is HTML, this parameter should be true
   */
  async processText(e, t = !1) {
    const { Caret: r, BlockManager: n } = this.Editor, i = t ? this.processHTML(e) : this.processPlain(e);
    if (!i.length)
      return;
    if (i.length === 1) {
      i[0].isBlock ? this.processSingleBlock(i.pop()) : this.processInlinePaste(i.pop());
      return;
    }
    const s = n.currentBlock && n.currentBlock.tool.isDefault && n.currentBlock.isEmpty;
    i.map(
      async (a, l) => this.insertBlock(a, l === 0 && s)
    ), n.currentBlock && r.setToBlock(n.currentBlock, r.positions.END);
  }
  /**
   * Set onPaste callback handler
   */
  setCallback() {
    this.listeners.on(this.Editor.UI.nodes.holder, "paste", this.handlePasteEvent);
  }
  /**
   * Unset onPaste callback handler
   */
  unsetCallback() {
    this.listeners.off(this.Editor.UI.nodes.holder, "paste", this.handlePasteEvent);
  }
  /**
   * Get and process tool`s paste configs
   */
  processTools() {
    const e = this.Editor.Tools.blockTools;
    Array.from(e.values()).forEach(this.processTool);
  }
  /**
   * Get tags name list from either tag name or sanitization config.
   *
   * @param {string | object} tagOrSanitizeConfig - tag name or sanitize config object.
   * @returns {string[]} array of tags.
   */
  collectTagNames(e) {
    return ie(e) ? [e] : U(e) ? Object.keys(e) : [];
  }
  /**
   * Get tags to substitute by Tool
   *
   * @param tool - BlockTool object
   */
  getTagsConfig(e) {
    if (e.pasteConfig === !1)
      return;
    const t = e.pasteConfig.tags || [], r = [];
    t.forEach((n) => {
      const i = this.collectTagNames(n);
      r.push(...i), i.forEach((s) => {
        if (Object.prototype.hasOwnProperty.call(this.toolsTags, s)) {
          j(
            `Paste handler for «${e.name}» Tool on «${s}» tag is skipped because it is already used by «${this.toolsTags[s].tool.name}» Tool.`,
            "warn"
          );
          return;
        }
        const a = U(n) ? n[s] : null;
        this.toolsTags[s.toUpperCase()] = {
          tool: e,
          sanitizationConfig: a
        };
      });
    }), this.tagsByTool[e.name] = r.map((n) => n.toUpperCase());
  }
  /**
   * Get files` types and extensions to substitute by Tool
   *
   * @param tool - BlockTool object
   */
  getFilesConfig(e) {
    if (e.pasteConfig === !1)
      return;
    const { files: t = {} } = e.pasteConfig;
    let { extensions: r, mimeTypes: n } = t;
    !r && !n || (r && !Array.isArray(r) && (j(`«extensions» property of the onDrop config for «${e.name}» Tool should be an array`), r = []), n && !Array.isArray(n) && (j(`«mimeTypes» property of the onDrop config for «${e.name}» Tool should be an array`), n = []), n && (n = n.filter((i) => as(i) ? !0 : (j(`MIME type value «${i}» for the «${e.name}» Tool is not a valid MIME type`, "warn"), !1))), this.toolsFiles[e.name] = {
      extensions: r || [],
      mimeTypes: n || []
    });
  }
  /**
   * Get RegExp patterns to substitute by Tool
   *
   * @param tool - BlockTool object
   */
  getPatternsConfig(e) {
    e.pasteConfig === !1 || !e.pasteConfig.patterns || X(e.pasteConfig.patterns) || Object.entries(e.pasteConfig.patterns).forEach(([t, r]) => {
      r instanceof RegExp || j(
        `Pattern ${r} for «${e.name}» Tool is skipped because it should be a Regexp instance.`,
        "warn"
      ), this.toolsPatterns.push({
        key: t,
        pattern: r,
        tool: e
      });
    });
  }
  /**
   * Check if browser behavior suits better
   *
   * @param {EventTarget} element - element where content has been pasted
   * @returns {boolean}
   */
  isNativeBehaviour(e) {
    return g.isNativeInput(e);
  }
  /**
   * Get files from data transfer object and insert related Tools
   *
   * @param {FileList} items - pasted or dropped items
   */
  async processFiles(e) {
    const { BlockManager: t } = this.Editor;
    let r;
    r = await Promise.all(
      Array.from(e).map((i) => this.processFile(i))
    ), r = r.filter((i) => !!i);
    const n = t.currentBlock.tool.isDefault && t.currentBlock.isEmpty;
    r.forEach(
      (i, s) => {
        t.paste(i.type, i.event, s === 0 && n);
      }
    );
  }
  /**
   * Get information about file and find Tool to handle it
   *
   * @param {File} file - file to process
   */
  async processFile(e) {
    const t = ss(e), r = Object.entries(this.toolsFiles).find(([i, { mimeTypes: s, extensions: a }]) => {
      const [l, c] = e.type.split("/"), d = a.find((u) => u.toLowerCase() === t.toLowerCase()), h = s.find((u) => {
        const [f, p] = u.split("/");
        return f === l && (p === c || p === "*");
      });
      return !!d || !!h;
    });
    if (!r)
      return;
    const [n] = r;
    return {
      event: this.composePasteEvent("file", {
        file: e
      }),
      type: n
    };
  }
  /**
   * Split HTML string to blocks and return it as array of Block data
   *
   * @param {string} innerHTML - html string to process
   * @returns {PasteData[]}
   */
  processHTML(e) {
    const { Tools: t } = this.Editor, r = g.make("DIV");
    return r.innerHTML = e, this.getNodes(r).map((n) => {
      let i, s = t.defaultTool, a = !1;
      switch (n.nodeType) {
        case Node.DOCUMENT_FRAGMENT_NODE:
          i = g.make("div"), i.appendChild(n);
          break;
        case Node.ELEMENT_NODE:
          i = n, a = !0, this.toolsTags[i.tagName] && (s = this.toolsTags[i.tagName].tool);
          break;
      }
      const { tags: l } = s.pasteConfig || { tags: [] }, c = l.reduce((u, f) => (this.collectTagNames(f).forEach((p) => {
        const k = U(f) ? f[p] : null;
        u[p.toLowerCase()] = k || {};
      }), u), {}), d = Object.assign({}, c, s.baseSanitizeConfig);
      if (i.tagName.toLowerCase() === "table") {
        const u = J(i.outerHTML, d);
        i = g.make("div", void 0, {
          innerHTML: u
        }).firstChild;
      } else
        i.innerHTML = J(i.innerHTML, d);
      const h = this.composePasteEvent("tag", {
        data: i
      });
      return {
        content: i,
        isBlock: a,
        tool: s.name,
        event: h
      };
    }).filter((n) => {
      const i = g.isEmpty(n.content), s = g.isSingleTag(n.content);
      return !i || s;
    });
  }
  /**
   * Split plain text by new line symbols and return it as array of Block data
   *
   * @param {string} plain - string to process
   * @returns {PasteData[]}
   */
  processPlain(e) {
    const { defaultBlock: t } = this.config;
    if (!e)
      return [];
    const r = t;
    return e.split(/\r?\n/).filter((n) => n.trim()).map((n) => {
      const i = g.make("div");
      i.textContent = n;
      const s = this.composePasteEvent("tag", {
        data: i
      });
      return {
        content: i,
        tool: r,
        isBlock: !1,
        event: s
      };
    });
  }
  /**
   * Process paste of single Block tool content
   *
   * @param {PasteData} dataToInsert - data of Block to insert
   */
  async processSingleBlock(e) {
    const { Caret: t, BlockManager: r } = this.Editor, { currentBlock: n } = r;
    if (!n || e.tool !== n.name || !g.containsOnlyInlineElements(e.content.innerHTML)) {
      this.insertBlock(e, (n == null ? void 0 : n.tool.isDefault) && n.isEmpty);
      return;
    }
    t.insertContentAtCaretPosition(e.content.innerHTML);
  }
  /**
   * Process paste to single Block:
   * 1. Find patterns` matches
   * 2. Insert new block if it is not the same type as current one
   * 3. Just insert text if there is no substitutions
   *
   * @param {PasteData} dataToInsert - data of Block to insert
   */
  async processInlinePaste(e) {
    const { BlockManager: t, Caret: r } = this.Editor, { content: n } = e;
    if (t.currentBlock && t.currentBlock.tool.isDefault && n.textContent.length < pi.PATTERN_PROCESSING_MAX_LENGTH) {
      const i = await this.processPattern(n.textContent);
      if (i) {
        const s = t.currentBlock && t.currentBlock.tool.isDefault && t.currentBlock.isEmpty, a = t.paste(i.tool, i.event, s);
        r.setToBlock(a, r.positions.END);
        return;
      }
    }
    if (t.currentBlock && t.currentBlock.currentInput) {
      const i = t.currentBlock.tool.baseSanitizeConfig;
      document.execCommand(
        "insertHTML",
        !1,
        J(n.innerHTML, i)
      );
    } else
      this.insertBlock(e);
  }
  /**
   * Get patterns` matches
   *
   * @param {string} text - text to process
   * @returns {Promise<{event: PasteEvent, tool: string}>}
   */
  async processPattern(e) {
    const t = this.toolsPatterns.find((r) => {
      const n = r.pattern.exec(e);
      return n ? e === n.shift() : !1;
    });
    return t ? {
      event: this.composePasteEvent("pattern", {
        key: t.key,
        data: e
      }),
      tool: t.tool.name
    } : void 0;
  }
  /**
   * Insert pasted Block content to Editor
   *
   * @param {PasteData} data - data to insert
   * @param {boolean} canReplaceCurrentBlock - if true and is current Block is empty, will replace current Block
   * @returns {void}
   */
  insertBlock(e, t = !1) {
    const { BlockManager: r, Caret: n } = this.Editor, { currentBlock: i } = r;
    let s;
    if (t && i && i.isEmpty) {
      s = r.paste(e.tool, e.event, !0), n.setToBlock(s, n.positions.END);
      return;
    }
    s = r.paste(e.tool, e.event), n.setToBlock(s, n.positions.END);
  }
  /**
   * Insert data passed as application/x-editor-js JSON
   *
   * @param {Array} blocks — Blocks' data to insert
   * @returns {void}
   */
  insertEditorJSData(e) {
    const { BlockManager: t, Caret: r, Tools: n } = this.Editor;
    to(
      e,
      (i) => n.blockTools.get(i).sanitizeConfig
    ).forEach(({ tool: i, data: s }, a) => {
      let l = !1;
      a === 0 && (l = t.currentBlock && t.currentBlock.tool.isDefault && t.currentBlock.isEmpty);
      const c = t.insert({
        tool: i,
        data: s,
        replace: l
      });
      r.setToBlock(c, r.positions.END);
    });
  }
  /**
   * Fetch nodes from Element node
   *
   * @param {Node} node - current node
   * @param {Node[]} nodes - processed nodes
   * @param {Node} destNode - destination node
   */
  processElementNode(e, t, r) {
    const n = Object.keys(this.toolsTags), i = e, { tool: s } = this.toolsTags[i.tagName] || {}, a = this.tagsByTool[s == null ? void 0 : s.name] || [], l = n.includes(i.tagName), c = g.blockElements.includes(i.tagName.toLowerCase()), d = Array.from(i.children).some(
      ({ tagName: u }) => n.includes(u) && !a.includes(u)
    ), h = Array.from(i.children).some(
      ({ tagName: u }) => g.blockElements.includes(u.toLowerCase())
    );
    if (!c && !l && !d)
      return r.appendChild(i), [...t, r];
    if (l && !d || c && !h && !d)
      return [...t, r, i];
  }
  /**
   * Recursively divide HTML string to two types of nodes:
   * 1. Block element
   * 2. Document Fragments contained text and markup tags like a, b, i etc.
   *
   * @param {Node} wrapper - wrapper of paster HTML content
   * @returns {Node[]}
   */
  getNodes(e) {
    const t = Array.from(e.childNodes);
    let r;
    const n = (i, s) => {
      if (g.isEmpty(s) && !g.isSingleTag(s))
        return i;
      const a = i[i.length - 1];
      let l = new DocumentFragment();
      switch (a && g.isFragment(a) && (l = i.pop()), s.nodeType) {
        case Node.ELEMENT_NODE:
          if (r = this.processElementNode(s, i, l), r)
            return r;
          break;
        case Node.TEXT_NODE:
          return l.appendChild(s), [...i, l];
        default:
          return [...i, l];
      }
      return [...i, ...Array.from(s.childNodes).reduce(n, [])];
    };
    return t.reduce(n, []);
  }
  /**
   * Compose paste event with passed type and detail
   *
   * @param {string} type - event type
   * @param {PasteEventDetail} detail - event detail
   */
  composePasteEvent(e, t) {
    return new CustomEvent(e, {
      detail: t
    });
  }
};
ui.PATTERN_PROCESSING_MAX_LENGTH = 450;
let xc = ui;
class Cc extends N {
  constructor() {
    super(...arguments), this.toolsDontSupportReadOnly = [], this.readOnlyEnabled = !1;
  }
  /**
   * Returns state of read only mode
   */
  get isEnabled() {
    return this.readOnlyEnabled;
  }
  /**
   * Set initial state
   */
  async prepare() {
    const { Tools: e } = this.Editor, { blockTools: t } = e, r = [];
    Array.from(t.entries()).forEach(([n, i]) => {
      i.isReadOnlySupported || r.push(n);
    }), this.toolsDontSupportReadOnly = r, this.config.readOnly && r.length > 0 && this.throwCriticalError(), this.toggle(this.config.readOnly, !0);
  }
  /**
   * Set read-only mode or toggle current state
   * Call all Modules `toggleReadOnly` method and re-render Editor
   *
   * @param state - (optional) read-only state or toggle
   * @param isInitial - (optional) true when editor is initializing
   */
  async toggle(e = !this.readOnlyEnabled, t = !1) {
    e && this.toolsDontSupportReadOnly.length > 0 && this.throwCriticalError();
    const r = this.readOnlyEnabled;
    this.readOnlyEnabled = e;
    for (const i in this.Editor)
      this.Editor[i].toggleReadOnly && this.Editor[i].toggleReadOnly(e);
    if (r === e)
      return this.readOnlyEnabled;
    if (t)
      return this.readOnlyEnabled;
    this.Editor.ModificationsObserver.disable();
    const n = await this.Editor.Saver.save();
    return await this.Editor.BlockManager.clear(), await this.Editor.Renderer.render(n.blocks), this.Editor.ModificationsObserver.enable(), this.readOnlyEnabled;
  }
  /**
   * Throws an error about tools which don't support read-only mode
   */
  throwCriticalError() {
    throw new gn(
      `To enable read-only mode all connected tools should support it. Tools ${this.toolsDontSupportReadOnly.join(", ")} don't support read-only mode.`
    );
  }
}
class Ue extends N {
  constructor() {
    super(...arguments), this.isRectSelectionActivated = !1, this.SCROLL_SPEED = 3, this.HEIGHT_OF_SCROLL_ZONE = 40, this.BOTTOM_SCROLL_ZONE = 1, this.TOP_SCROLL_ZONE = 2, this.MAIN_MOUSE_BUTTON = 0, this.mousedown = !1, this.isScrolling = !1, this.inScrollZone = null, this.startX = 0, this.startY = 0, this.mouseX = 0, this.mouseY = 0, this.stackOfSelected = [], this.listenerIds = [];
  }
  /**
   * CSS classes for the Block
   *
   * @returns {{wrapper: string, content: string}}
   */
  static get CSS() {
    return {
      overlay: "codex-editor-overlay",
      overlayContainer: "codex-editor-overlay__container",
      rect: "codex-editor-overlay__rectangle",
      topScrollZone: "codex-editor-overlay__scroll-zone--top",
      bottomScrollZone: "codex-editor-overlay__scroll-zone--bottom"
    };
  }
  /**
   * Module Preparation
   * Creating rect and hang handlers
   */
  prepare() {
    this.enableModuleBindings();
  }
  /**
   * Init rect params
   *
   * @param {number} pageX - X coord of mouse
   * @param {number} pageY - Y coord of mouse
   */
  startSelection(e, t) {
    const r = document.elementFromPoint(e - window.pageXOffset, t - window.pageYOffset);
    r.closest(`.${this.Editor.Toolbar.CSS.toolbar}`) || (this.Editor.BlockSelection.allBlocksSelected = !1, this.clearSelection(), this.stackOfSelected = []);
    const n = [
      `.${ne.CSS.content}`,
      `.${this.Editor.Toolbar.CSS.toolbar}`,
      `.${this.Editor.InlineToolbar.CSS.inlineToolbar}`
    ], i = r.closest("." + this.Editor.UI.CSS.editorWrapper), s = n.some((a) => !!r.closest(a));
    !i || s || (this.mousedown = !0, this.startX = e, this.startY = t);
  }
  /**
   * Clear all params to end selection
   */
  endSelection() {
    this.mousedown = !1, this.startX = 0, this.startY = 0, this.overlayRectangle.style.display = "none";
  }
  /**
   * is RectSelection Activated
   */
  isRectActivated() {
    return this.isRectSelectionActivated;
  }
  /**
   * Mark that selection is end
   */
  clearSelection() {
    this.isRectSelectionActivated = !1;
  }
  /**
   * Sets Module necessary event handlers
   */
  enableModuleBindings() {
    const { container: e } = this.genHTML();
    this.listeners.on(e, "mousedown", (t) => {
      this.processMouseDown(t);
    }, !1), this.listeners.on(document.body, "mousemove", $t((t) => {
      this.processMouseMove(t);
    }, 10), {
      passive: !0
    }), this.listeners.on(document.body, "mouseleave", () => {
      this.processMouseLeave();
    }), this.listeners.on(window, "scroll", $t((t) => {
      this.processScroll(t);
    }, 10), {
      passive: !0
    }), this.listeners.on(document.body, "mouseup", () => {
      this.processMouseUp();
    }, !1);
  }
  /**
   * Handle mouse down events
   *
   * @param {MouseEvent} mouseEvent - mouse event payload
   */
  processMouseDown(e) {
    e.button === this.MAIN_MOUSE_BUTTON && (e.target.closest(g.allInputsSelector) !== null || this.startSelection(e.pageX, e.pageY));
  }
  /**
   * Handle mouse move events
   *
   * @param {MouseEvent} mouseEvent - mouse event payload
   */
  processMouseMove(e) {
    this.changingRectangle(e), this.scrollByZones(e.clientY);
  }
  /**
   * Handle mouse leave
   */
  processMouseLeave() {
    this.clearSelection(), this.endSelection();
  }
  /**
   * @param {MouseEvent} mouseEvent - mouse event payload
   */
  processScroll(e) {
    this.changingRectangle(e);
  }
  /**
   * Handle mouse up
   */
  processMouseUp() {
    this.clearSelection(), this.endSelection();
  }
  /**
   * Scroll If mouse in scroll zone
   *
   * @param {number} clientY - Y coord of mouse
   */
  scrollByZones(e) {
    if (this.inScrollZone = null, e <= this.HEIGHT_OF_SCROLL_ZONE && (this.inScrollZone = this.TOP_SCROLL_ZONE), document.documentElement.clientHeight - e <= this.HEIGHT_OF_SCROLL_ZONE && (this.inScrollZone = this.BOTTOM_SCROLL_ZONE), !this.inScrollZone) {
      this.isScrolling = !1;
      return;
    }
    this.isScrolling || (this.scrollVertical(this.inScrollZone === this.TOP_SCROLL_ZONE ? -this.SCROLL_SPEED : this.SCROLL_SPEED), this.isScrolling = !0);
  }
  /**
   * Generates required HTML elements
   *
   * @returns {Object<string, Element>}
   */
  genHTML() {
    const { UI: e } = this.Editor, t = e.nodes.holder.querySelector("." + e.CSS.editorWrapper), r = g.make("div", Ue.CSS.overlay, {}), n = g.make("div", Ue.CSS.overlayContainer, {}), i = g.make("div", Ue.CSS.rect, {});
    return n.appendChild(i), r.appendChild(n), t.appendChild(r), this.overlayRectangle = i, {
      container: t,
      overlay: r
    };
  }
  /**
   * Activates scrolling if blockSelection is active and mouse is in scroll zone
   *
   * @param {number} speed - speed of scrolling
   */
  scrollVertical(e) {
    if (!(this.inScrollZone && this.mousedown))
      return;
    const t = window.pageYOffset;
    window.scrollBy(0, e), this.mouseY += window.pageYOffset - t, setTimeout(() => {
      this.scrollVertical(e);
    }, 0);
  }
  /**
   * Handles the change in the rectangle and its effect
   *
   * @param {MouseEvent} event - mouse event
   */
  changingRectangle(e) {
    if (!this.mousedown)
      return;
    e.pageY !== void 0 && (this.mouseX = e.pageX, this.mouseY = e.pageY);
    const { rightPos: t, leftPos: r, index: n } = this.genInfoForMouseSelection(), i = this.startX > t && this.mouseX > t, s = this.startX < r && this.mouseX < r;
    this.rectCrossesBlocks = !(i || s), this.isRectSelectionActivated || (this.rectCrossesBlocks = !1, this.isRectSelectionActivated = !0, this.shrinkRectangleToPoint(), this.overlayRectangle.style.display = "block"), this.updateRectangleSize(), this.Editor.Toolbar.close(), n !== void 0 && (this.trySelectNextBlock(n), this.inverseSelection(), L.get().removeAllRanges());
  }
  /**
   * Shrink rect to singular point
   */
  shrinkRectangleToPoint() {
    this.overlayRectangle.style.left = `${this.startX - window.pageXOffset}px`, this.overlayRectangle.style.top = `${this.startY - window.pageYOffset}px`, this.overlayRectangle.style.bottom = `calc(100% - ${this.startY - window.pageYOffset}px`, this.overlayRectangle.style.right = `calc(100% - ${this.startX - window.pageXOffset}px`;
  }
  /**
   * Select or unselect all of blocks in array if rect is out or in selectable area
   */
  inverseSelection() {
    const e = this.Editor.BlockManager.getBlockByIndex(this.stackOfSelected[0]).selected;
    if (this.rectCrossesBlocks && !e)
      for (const t of this.stackOfSelected)
        this.Editor.BlockSelection.selectBlockByIndex(t);
    if (!this.rectCrossesBlocks && e)
      for (const t of this.stackOfSelected)
        this.Editor.BlockSelection.unSelectBlockByIndex(t);
  }
  /**
   * Updates size of rectangle
   */
  updateRectangleSize() {
    this.mouseY >= this.startY ? (this.overlayRectangle.style.top = `${this.startY - window.pageYOffset}px`, this.overlayRectangle.style.bottom = `calc(100% - ${this.mouseY - window.pageYOffset}px`) : (this.overlayRectangle.style.bottom = `calc(100% - ${this.startY - window.pageYOffset}px`, this.overlayRectangle.style.top = `${this.mouseY - window.pageYOffset}px`), this.mouseX >= this.startX ? (this.overlayRectangle.style.left = `${this.startX - window.pageXOffset}px`, this.overlayRectangle.style.right = `calc(100% - ${this.mouseX - window.pageXOffset}px`) : (this.overlayRectangle.style.right = `calc(100% - ${this.startX - window.pageXOffset}px`, this.overlayRectangle.style.left = `${this.mouseX - window.pageXOffset}px`);
  }
  /**
   * Collects information needed to determine the behavior of the rectangle
   *
   * @returns {object} index - index next Block, leftPos - start of left border of Block, rightPos - right border
   */
  genInfoForMouseSelection() {
    const e = document.body.offsetWidth / 2, t = this.mouseY - window.pageYOffset, r = document.elementFromPoint(e, t), n = this.Editor.BlockManager.getBlockByChildNode(r);
    let i;
    n !== void 0 && (i = this.Editor.BlockManager.blocks.findIndex((d) => d.holder === n.holder));
    const s = this.Editor.BlockManager.lastBlock.holder.querySelector("." + ne.CSS.content), a = Number.parseInt(window.getComputedStyle(s).width, 10) / 2, l = e - a, c = e + a;
    return {
      index: i,
      leftPos: l,
      rightPos: c
    };
  }
  /**
   * Select block with index index
   *
   * @param index - index of block in redactor
   */
  addBlockInSelection(e) {
    this.rectCrossesBlocks && this.Editor.BlockSelection.selectBlockByIndex(e), this.stackOfSelected.push(e);
  }
  /**
   * Adds a block to the selection and determines which blocks should be selected
   *
   * @param {object} index - index of new block in the reactor
   */
  trySelectNextBlock(e) {
    const t = this.stackOfSelected[this.stackOfSelected.length - 1] === e, r = this.stackOfSelected.length, n = 1, i = -1, s = 0;
    if (t)
      return;
    const a = this.stackOfSelected[r - 1] - this.stackOfSelected[r - 2] > 0;
    let l = s;
    r > 1 && (l = a ? n : i);
    const c = e > this.stackOfSelected[r - 1] && l === n, d = e < this.stackOfSelected[r - 1] && l === i, h = !(c || d || l === s);
    if (!h && (e > this.stackOfSelected[r - 1] || this.stackOfSelected[r - 1] === void 0)) {
      let p = this.stackOfSelected[r - 1] + 1 || e;
      for (p; p <= e; p++)
        this.addBlockInSelection(p);
      return;
    }
    if (!h && e < this.stackOfSelected[r - 1]) {
      for (let p = this.stackOfSelected[r - 1] - 1; p >= e; p--)
        this.addBlockInSelection(p);
      return;
    }
    if (!h)
      return;
    let u = r - 1, f;
    for (e > this.stackOfSelected[r - 1] ? f = () => e > this.stackOfSelected[u] : f = () => e < this.stackOfSelected[u]; f(); )
      this.rectCrossesBlocks && this.Editor.BlockSelection.unSelectBlockByIndex(this.stackOfSelected[u]), this.stackOfSelected.pop(), u--;
  }
}
class Ec extends N {
  /**
   * Renders passed blocks as one batch
   *
   * @param blocksData - blocks to render
   */
  async render(e) {
    return new Promise((t) => {
      const { Tools: r, BlockManager: n } = this.Editor;
      if (e.length === 0)
        n.insert();
      else {
        const i = e.map(({ type: s, data: a, tunes: l, id: c }) => {
          r.available.has(s) === !1 && (Y(`Tool «${s}» is not found. Check 'tools' property at the Editor.js config.`, "warn"), a = this.composeStubDataForTool(s, a, c), s = r.stubTool);
          let d;
          try {
            d = n.composeBlock({
              id: c,
              tool: s,
              data: a,
              tunes: l
            });
          } catch (h) {
            j(`Block «${s}» skipped because of plugins error`, "error", {
              data: a,
              error: h
            }), a = this.composeStubDataForTool(s, a, c), s = r.stubTool, d = n.composeBlock({
              id: c,
              tool: s,
              data: a,
              tunes: l
            });
          }
          return d;
        });
        n.insertMany(i);
      }
      window.requestIdleCallback(() => {
        t();
      }, { timeout: 2e3 });
    });
  }
  /**
   * Create data for the Stub Tool that will be used instead of unavailable tool
   *
   * @param tool - unavailable tool name to stub
   * @param data - data of unavailable block
   * @param [id] - id of unavailable block
   */
  composeStubDataForTool(e, t, r) {
    const { Tools: n } = this.Editor;
    let i = e;
    if (n.unavailable.has(e)) {
      const s = n.unavailable.get(e).toolbox;
      s !== void 0 && s[0].title !== void 0 && (i = s[0].title);
    }
    return {
      savedData: {
        id: r,
        type: e,
        data: t
      },
      title: i
    };
  }
}
class Tc extends N {
  /**
   * Composes new chain of Promises to fire them alternatelly
   *
   * @returns {OutputData}
   */
  async save() {
    const { BlockManager: e, Tools: t } = this.Editor, r = e.blocks, n = [];
    try {
      r.forEach((a) => {
        n.push(this.getSavedData(a));
      });
      const i = await Promise.all(n), s = await to(i, (a) => t.blockTools.get(a).sanitizeConfig);
      return this.makeOutput(s);
    } catch (i) {
      Y("Saving failed due to the Error %o", "error", i);
    }
  }
  /**
   * Saves and validates
   *
   * @param {Block} block - Editor's Tool
   * @returns {ValidatedData} - Tool's validated data
   */
  async getSavedData(e) {
    const t = await e.save(), r = t && await e.validate(t.data);
    return {
      ...t,
      isValid: r
    };
  }
  /**
   * Creates output object with saved data, time and version of editor
   *
   * @param {ValidatedData} allExtractedData - data extracted from Blocks
   * @returns {OutputData}
   */
  makeOutput(e) {
    const t = [];
    return e.forEach(({ id: r, tool: n, data: i, tunes: s, isValid: a }) => {
      if (!a) {
        j(`Block «${n}» skipped because saved data is invalid`);
        return;
      }
      if (n === this.Editor.Tools.stubTool) {
        t.push(i);
        return;
      }
      const l = {
        id: r,
        type: n,
        data: i,
        ...!X(s) && {
          tunes: s
        }
      };
      t.push(l);
    }), {
      time: +/* @__PURE__ */ new Date(),
      blocks: t,
      version: "2.31.6"
    };
  }
}
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".ce-paragraph{line-height:1.6em;outline:none}.ce-block:only-of-type .ce-paragraph[data-placeholder-active]:empty:before,.ce-block:only-of-type .ce-paragraph[data-placeholder-active][data-empty=true]:before{content:attr(data-placeholder-active)}.ce-paragraph p:first-of-type{margin-top:0}.ce-paragraph p:last-of-type{margin-bottom:0}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const Sc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8 9V7.2C8 7.08954 8.08954 7 8.2 7L12 7M16 9V7.2C16 7.08954 15.9105 7 15.8 7L12 7M12 7L12 17M12 17H10M12 17H14"/></svg>';
function Bc(o) {
  const e = document.createElement("div");
  e.innerHTML = o.trim();
  const t = document.createDocumentFragment();
  return t.append(...Array.from(e.childNodes)), t;
}
/**
 * Base Paragraph Block for the Editor.js.
 * Represents a regular text block
 *
 * @author CodeX (team@codex.so)
 * @copyright CodeX 2018
 * @license The MIT License (MIT)
 */
class Xo {
  /**
   * Default placeholder for Paragraph Tool
   *
   * @returns {string}
   * @class
   */
  static get DEFAULT_PLACEHOLDER() {
    return "";
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} params - constructor params
   * @param {ParagraphData} params.data - previously saved data
   * @param {ParagraphConfig} params.config - user config for Tool
   * @param {object} params.api - editor.js api
   * @param {boolean} readOnly - read only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this.api = r, this.readOnly = n, this._CSS = {
      block: this.api.styles.block,
      wrapper: "ce-paragraph"
    }, this.readOnly || (this.onKeyUp = this.onKeyUp.bind(this)), this._placeholder = t.placeholder ? t.placeholder : Xo.DEFAULT_PLACEHOLDER, this._data = e ?? {}, this._element = null, this._preserveBlank = t.preserveBlank ?? !1;
  }
  /**
   * Check if text content is empty and set empty string to inner html.
   * We need this because some browsers (e.g. Safari) insert <br> into empty contenteditanle elements
   *
   * @param {KeyboardEvent} e - key up event
   */
  onKeyUp(e) {
    if (e.code !== "Backspace" && e.code !== "Delete" || !this._element)
      return;
    const { textContent: t } = this._element;
    t === "" && (this._element.innerHTML = "");
  }
  /**
   * Create Tool's view
   *
   * @returns {HTMLDivElement}
   * @private
   */
  drawView() {
    const e = document.createElement("DIV");
    return e.classList.add(this._CSS.wrapper, this._CSS.block), e.contentEditable = "false", e.dataset.placeholderActive = this.api.i18n.t(this._placeholder), this._data.text && (e.innerHTML = this._data.text), this.readOnly || (e.contentEditable = "true", e.addEventListener("keyup", this.onKeyUp)), e;
  }
  /**
   * Return Tool's view
   *
   * @returns {HTMLDivElement}
   */
  render() {
    return this._element = this.drawView(), this._element;
  }
  /**
   * Method that specified how to merge two Text blocks.
   * Called by Editor.js by backspace at the beginning of the Block
   *
   * @param {ParagraphData} data
   * @public
   */
  merge(e) {
    if (!this._element)
      return;
    this._data.text += e.text;
    const t = Bc(e.text);
    this._element.appendChild(t), this._element.normalize();
  }
  /**
   * Validate Paragraph block data:
   * - check for emptiness
   *
   * @param {ParagraphData} savedData — data received after saving
   * @returns {boolean} false if saved data is not correct, otherwise true
   * @public
   */
  validate(e) {
    return !(e.text.trim() === "" && !this._preserveBlank);
  }
  /**
   * Extract Tool's data from the view
   *
   * @param {HTMLDivElement} toolsContent - Paragraph tools rendered view
   * @returns {ParagraphData} - saved data
   * @public
   */
  save(e) {
    return {
      text: e.innerHTML
    };
  }
  /**
   * On paste callback fired from Editor.
   *
   * @param {HTMLPasteEvent} event - event with pasted data
   */
  onPaste(e) {
    const t = {
      text: e.detail.data.innerHTML
    };
    this._data = t, window.requestAnimationFrame(() => {
      this._element && (this._element.innerHTML = this._data.text || "");
    });
  }
  /**
   * Enable Conversion Toolbar. Paragraph can be converted to/from other tools
   * @returns {ConversionConfig}
   */
  static get conversionConfig() {
    return {
      export: "text",
      // to convert Paragraph to other block, use 'text' property of saved data
      import: "text"
      // to covert other block's exported string to Paragraph, fill 'text' property of tool data
    };
  }
  /**
   * Sanitizer rules
   * @returns {SanitizerConfig} - Edtior.js sanitizer config
   */
  static get sanitize() {
    return {
      text: {
        br: !0
      }
    };
  }
  /**
   * Returns true to notify the core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Used by Editor paste handling API.
   * Provides configuration to handle P tags.
   *
   * @returns {PasteConfig} - Paragraph Paste Setting
   */
  static get pasteConfig() {
    return {
      tags: ["P"]
    };
  }
  /**
   * Icon and title for displaying at the Toolbox
   *
   * @returns {ToolboxConfig} - Paragraph Toolbox Setting
   */
  static get toolbox() {
    return {
      icon: Sc,
      title: "Text"
    };
  }
}
class Zo {
  constructor() {
    this.commandName = "bold";
  }
  /**
   * Sanitizer Rule
   * Leave <b> tags
   *
   * @returns {object}
   */
  static get sanitize() {
    return {
      b: {}
    };
  }
  /**
   * Create button for Inline Toolbar
   */
  render() {
    return {
      icon: ea,
      name: "bold",
      onActivate: () => {
        document.execCommand(this.commandName);
      },
      isActive: () => document.queryCommandState(this.commandName)
    };
  }
  /**
   * Set a shortcut
   *
   * @returns {boolean}
   */
  get shortcut() {
    return "CMD+B";
  }
}
Zo.isInline = !0;
Zo.title = "Bold";
class Go {
  constructor() {
    this.commandName = "italic", this.CSS = {
      button: "ce-inline-tool",
      buttonActive: "ce-inline-tool--active",
      buttonModifier: "ce-inline-tool--italic"
    }, this.nodes = {
      button: null
    };
  }
  /**
   * Sanitizer Rule
   * Leave <i> tags
   *
   * @returns {object}
   */
  static get sanitize() {
    return {
      i: {}
    };
  }
  /**
   * Create button for Inline Toolbar
   */
  render() {
    return this.nodes.button = document.createElement("button"), this.nodes.button.type = "button", this.nodes.button.classList.add(this.CSS.button, this.CSS.buttonModifier), this.nodes.button.innerHTML = aa, this.nodes.button;
  }
  /**
   * Wrap range with <i> tag
   */
  surround() {
    document.execCommand(this.commandName);
  }
  /**
   * Check selection and set activated state to button if there are <i> tag
   */
  checkState() {
    const e = document.queryCommandState(this.commandName);
    return this.nodes.button.classList.toggle(this.CSS.buttonActive, e), e;
  }
  /**
   * Set a shortcut
   */
  get shortcut() {
    return "CMD+I";
  }
}
Go.isInline = !0;
Go.title = "Italic";
class Jo {
  /**
   * @param api - Editor.js API
   */
  constructor({ api: e }) {
    this.commandLink = "createLink", this.commandUnlink = "unlink", this.ENTER_KEY = 13, this.CSS = {
      button: "ce-inline-tool",
      buttonActive: "ce-inline-tool--active",
      buttonModifier: "ce-inline-tool--link",
      buttonUnlink: "ce-inline-tool--unlink",
      input: "ce-inline-tool-input",
      inputShowed: "ce-inline-tool-input--showed"
    }, this.nodes = {
      button: null,
      input: null
    }, this.inputOpened = !1, this.toolbar = e.toolbar, this.inlineToolbar = e.inlineToolbar, this.notifier = e.notifier, this.i18n = e.i18n, this.selection = new L();
  }
  /**
   * Sanitizer Rule
   * Leave <a> tags
   *
   * @returns {object}
   */
  static get sanitize() {
    return {
      a: {
        href: !0,
        target: "_blank",
        rel: "nofollow"
      }
    };
  }
  /**
   * Create button for Inline Toolbar
   */
  render() {
    return this.nodes.button = document.createElement("button"), this.nodes.button.type = "button", this.nodes.button.classList.add(this.CSS.button, this.CSS.buttonModifier), this.nodes.button.innerHTML = Wr, this.nodes.button;
  }
  /**
   * Input for the link
   */
  renderActions() {
    return this.nodes.input = document.createElement("input"), this.nodes.input.placeholder = this.i18n.t("Add a link"), this.nodes.input.enterKeyHint = "done", this.nodes.input.classList.add(this.CSS.input), this.nodes.input.addEventListener("keydown", (e) => {
      e.keyCode === this.ENTER_KEY && this.enterPressed(e);
    }), this.nodes.input;
  }
  /**
   * Handle clicks on the Inline Toolbar icon
   *
   * @param {Range} range - range to wrap with link
   */
  surround(e) {
    if (e) {
      this.inputOpened ? (this.selection.restore(), this.selection.removeFakeBackground()) : (this.selection.setFakeBackground(), this.selection.save());
      const t = this.selection.findParentTag("A");
      if (t) {
        this.inputOpened ? (this.closeActions(!1), this.checkState()) : (this.selection.expandToTag(t), this.unlink(), this.closeActions(), this.checkState(), this.toolbar.close());
        return;
      }
    }
    this.toggleActions();
  }
  /**
   * Check selection and set activated state to button if there are <a> tag
   */
  checkState() {
    const e = this.selection.findParentTag("A");
    if (e) {
      this.nodes.button.innerHTML = ha, this.nodes.button.classList.add(this.CSS.buttonUnlink), this.nodes.button.classList.add(this.CSS.buttonActive), this.openActions();
      const t = e.getAttribute("href");
      this.nodes.input.defaultValue = t !== "null" ? t : "", this.selection.save();
    } else
      this.nodes.button.innerHTML = Wr, this.nodes.button.classList.remove(this.CSS.buttonUnlink), this.nodes.button.classList.remove(this.CSS.buttonActive);
    return !!e;
  }
  /**
   * Function called with Inline Toolbar closing
   */
  clear() {
    this.closeActions();
  }
  /**
   * Set a shortcut
   */
  get shortcut() {
    return "CMD+K";
  }
  /**
   * Show/close link input
   */
  toggleActions() {
    this.inputOpened ? this.closeActions(!1) : this.openActions(!0);
  }
  /**
   * @param {boolean} needFocus - on link creation we need to focus input. On editing - nope.
   */
  openActions(e = !1) {
    this.nodes.input.classList.add(this.CSS.inputShowed), e && this.nodes.input.focus(), this.inputOpened = !0;
  }
  /**
   * Close input
   *
   * @param {boolean} clearSavedSelection — we don't need to clear saved selection
   *                                        on toggle-clicks on the icon of opened Toolbar
   */
  closeActions(e = !0) {
    if (this.selection.isFakeBackgroundEnabled) {
      const t = new L();
      t.save(), this.selection.restore(), this.selection.removeFakeBackground(), t.restore();
    }
    this.nodes.input.classList.remove(this.CSS.inputShowed), this.nodes.input.value = "", e && this.selection.clearSaved(), this.inputOpened = !1;
  }
  /**
   * Enter pressed on input
   *
   * @param {KeyboardEvent} event - enter keydown event
   */
  enterPressed(e) {
    let t = this.nodes.input.value || "";
    if (!t.trim()) {
      this.selection.restore(), this.unlink(), e.preventDefault(), this.closeActions();
      return;
    }
    if (!this.validateURL(t)) {
      this.notifier.show({
        message: "Pasted link is not valid.",
        style: "error"
      }), j("Incorrect Link pasted", "warn", t);
      return;
    }
    t = this.prepareLink(t), this.selection.restore(), this.selection.removeFakeBackground(), this.insertLink(t), e.preventDefault(), e.stopPropagation(), e.stopImmediatePropagation(), this.selection.collapseToEnd(), this.inlineToolbar.close();
  }
  /**
   * Detects if passed string is URL
   *
   * @param {string} str - string to validate
   * @returns {boolean}
   */
  validateURL(e) {
    return !/\s/.test(e);
  }
  /**
   * Process link before injection
   * - sanitize
   * - add protocol for links like 'google.com'
   *
   * @param {string} link - raw user input
   */
  prepareLink(e) {
    return e = e.trim(), e = this.addProtocol(e), e;
  }
  /**
   * Add 'http' protocol to the links like 'vc.ru', 'google.com'
   *
   * @param {string} link - string to process
   */
  addProtocol(e) {
    if (/^(\w+):(\/\/)?/.test(e))
      return e;
    const t = /^\/[^/\s]/.test(e), r = e.substring(0, 1) === "#", n = /^\/\/[^/\s]/.test(e);
    return !t && !r && !n && (e = "http://" + e), e;
  }
  /**
   * Inserts <a> tag with "href"
   *
   * @param {string} link - "href" value
   */
  insertLink(e) {
    const t = this.selection.findParentTag("A");
    t && this.selection.expandToTag(t), document.execCommand(this.commandLink, !1, e);
  }
  /**
   * Removes <a> tag
   */
  unlink() {
    document.execCommand(this.commandUnlink);
  }
}
Jo.isInline = !0;
Jo.title = "Link";
class fi {
  /**
   * @param api - Editor.js API
   */
  constructor({ api: e }) {
    this.i18nAPI = e.i18n, this.blocksAPI = e.blocks, this.selectionAPI = e.selection, this.toolsAPI = e.tools, this.caretAPI = e.caret;
  }
  /**
   * Returns tool's UI config
   */
  async render() {
    const e = L.get(), t = this.blocksAPI.getBlockByElement(e.anchorNode);
    if (t === void 0)
      return [];
    const r = this.toolsAPI.getBlockTools(), n = await yn(t, r);
    if (n.length === 0)
      return [];
    const i = n.reduce((c, d) => {
      var h;
      return (h = d.toolbox) == null || h.forEach((u) => {
        c.push({
          icon: u.icon,
          title: W.t(K.toolNames, u.title),
          name: d.name,
          closeOnActivate: !0,
          onActivate: async () => {
            const f = await this.blocksAPI.convert(t.id, d.name, u.data);
            this.caretAPI.setToBlock(f, "end");
          }
        });
      }), c;
    }, []), s = await t.getActiveToolboxEntry(), a = s !== void 0 ? s.icon : Mn, l = !_e();
    return {
      icon: a,
      name: "convert-to",
      hint: {
        title: this.i18nAPI.t("Convert to")
      },
      children: {
        searchable: l,
        items: i,
        onOpen: () => {
          l && (this.selectionAPI.setFakeBackground(), this.selectionAPI.save());
        },
        onClose: () => {
          l && (this.selectionAPI.restore(), this.selectionAPI.removeFakeBackground());
        }
      }
    };
  }
}
fi.isInline = !0;
class gi {
  /**
   * @param options - constructor options
   * @param options.data - stub tool data
   * @param options.api - Editor.js API
   */
  constructor({ data: e, api: t }) {
    this.CSS = {
      wrapper: "ce-stub",
      info: "ce-stub__info",
      title: "ce-stub__title",
      subtitle: "ce-stub__subtitle"
    }, this.api = t, this.title = e.title || this.api.i18n.t("Error"), this.subtitle = this.api.i18n.t("The block can not be displayed correctly."), this.savedData = e.savedData, this.wrapper = this.make();
  }
  /**
   * Returns stub holder
   *
   * @returns {HTMLElement}
   */
  render() {
    return this.wrapper;
  }
  /**
   * Return original Tool data
   *
   * @returns {BlockToolData}
   */
  save() {
    return this.savedData;
  }
  /**
   * Create Tool html markup
   *
   * @returns {HTMLElement}
   */
  make() {
    const e = g.make("div", this.CSS.wrapper), t = ua, r = g.make("div", this.CSS.info), n = g.make("div", this.CSS.title, {
      textContent: this.title
    }), i = g.make("div", this.CSS.subtitle, {
      textContent: this.subtitle
    });
    return e.innerHTML = t, r.appendChild(n), r.appendChild(i), e.appendChild(r), e;
  }
}
gi.isReadOnlySupported = !0;
class Mc extends ao {
  constructor() {
    super(...arguments), this.type = ge.Inline;
  }
  /**
   * Returns title for Inline Tool if specified by user
   */
  get title() {
    return this.constructable[ft.Title];
  }
  /**
   * Constructs new InlineTool instance from constructable
   */
  create() {
    return new this.constructable({
      api: this.api,
      config: this.settings
    });
  }
  /**
   * Allows inline tool to be available in read-only mode
   * Can be used, for example, by comments tool
   */
  get isReadOnlySupported() {
    return this.constructable[ft.IsReadOnlySupported] ?? !1;
  }
}
class _c extends ao {
  constructor() {
    super(...arguments), this.type = ge.Tune;
  }
  /**
   * Constructs new BlockTune instance from constructable
   *
   * @param data - Tune data
   * @param block - Block API object
   */
  create(e, t) {
    return new this.constructable({
      api: this.api,
      config: this.settings,
      block: t,
      data: e
    });
  }
}
let G = class Ce extends Map {
  /**
   * Returns Block Tools collection
   */
  get blockTools() {
    const e = Array.from(this.entries()).filter(([, t]) => t.isBlock());
    return new Ce(e);
  }
  /**
   * Returns Inline Tools collection
   */
  get inlineTools() {
    const e = Array.from(this.entries()).filter(([, t]) => t.isInline());
    return new Ce(e);
  }
  /**
   * Returns Block Tunes collection
   */
  get blockTunes() {
    const e = Array.from(this.entries()).filter(([, t]) => t.isTune());
    return new Ce(e);
  }
  /**
   * Returns internal Tools collection
   */
  get internalTools() {
    const e = Array.from(this.entries()).filter(([, t]) => t.isInternal);
    return new Ce(e);
  }
  /**
   * Returns Tools collection provided by user
   */
  get externalTools() {
    const e = Array.from(this.entries()).filter(([, t]) => !t.isInternal);
    return new Ce(e);
  }
};
var Lc = Object.defineProperty, Ic = Object.getOwnPropertyDescriptor, mi = (o, e, t, r) => {
  for (var n = Ic(e, t), i = o.length - 1, s; i >= 0; i--)
    (s = o[i]) && (n = s(e, t, n) || n);
  return n && Lc(e, t, n), n;
};
class Qo extends ao {
  constructor() {
    super(...arguments), this.type = ge.Block, this.inlineTools = new G(), this.tunes = new G();
  }
  /**
   * Creates new Tool instance
   *
   * @param data - Tool data
   * @param block - BlockAPI for current Block
   * @param readOnly - True if Editor is in read-only mode
   */
  create(e, t, r) {
    return new this.constructable({
      data: e,
      block: t,
      readOnly: r,
      api: this.api,
      config: this.settings
    });
  }
  /**
   * Returns true if read-only mode is supported by Tool
   */
  get isReadOnlySupported() {
    return this.constructable[xe.IsReadOnlySupported] === !0;
  }
  /**
   * Returns true if Tool supports linebreaks
   */
  get isLineBreaksEnabled() {
    return this.constructable[xe.IsEnabledLineBreaks];
  }
  /**
   * Returns Tool toolbox configuration (internal or user-specified).
   *
   * Merges internal and user-defined toolbox configs based on the following rules:
   *
   * - If both internal and user-defined toolbox configs are arrays their items are merged.
   * Length of the second one is kept.
   *
   * - If both are objects their properties are merged.
   *
   * - If one is an object and another is an array than internal config is replaced with user-defined
   * config. This is made to allow user to override default tool's toolbox representation (single/multiple entries)
   */
  get toolbox() {
    const e = this.constructable[xe.Toolbox], t = this.config[et.Toolbox];
    if (!X(e) && t !== !1)
      return t ? Array.isArray(e) ? Array.isArray(t) ? t.map((r, n) => {
        const i = e[n];
        return i ? {
          ...i,
          ...r
        } : r;
      }) : [t] : Array.isArray(t) ? t : [
        {
          ...e,
          ...t
        }
      ] : Array.isArray(e) ? e : [e];
  }
  /**
   * Returns Tool conversion configuration
   */
  get conversionConfig() {
    return this.constructable[xe.ConversionConfig];
  }
  /**
   * Returns enabled inline tools for Tool
   */
  get enabledInlineTools() {
    return this.config[et.EnabledInlineTools] || !1;
  }
  /**
   * Returns enabled tunes for Tool
   */
  get enabledBlockTunes() {
    return this.config[et.EnabledBlockTunes];
  }
  /**
   * Returns Tool paste configuration
   */
  get pasteConfig() {
    return this.constructable[xe.PasteConfig] ?? {};
  }
  get sanitizeConfig() {
    const e = super.sanitizeConfig, t = this.baseSanitizeConfig;
    if (X(e))
      return t;
    const r = {};
    for (const n in e)
      if (Object.prototype.hasOwnProperty.call(e, n)) {
        const i = e[n];
        U(i) ? r[n] = Object.assign({}, t, i) : r[n] = i;
      }
    return r;
  }
  get baseSanitizeConfig() {
    const e = {};
    return Array.from(this.inlineTools.values()).forEach((t) => Object.assign(e, t.sanitizeConfig)), Array.from(this.tunes.values()).forEach((t) => Object.assign(e, t.sanitizeConfig)), e;
  }
}
mi([
  Me
], Qo.prototype, "sanitizeConfig");
mi([
  Me
], Qo.prototype, "baseSanitizeConfig");
class Oc {
  /**
   * @class
   * @param config - tools config
   * @param editorConfig - EditorJS config
   * @param api - EditorJS API module
   */
  constructor(e, t, r) {
    this.api = r, this.config = e, this.editorConfig = t;
  }
  /**
   * Returns Tool object based on it's type
   *
   * @param name - tool name
   */
  get(e) {
    const { class: t, isInternal: r = !1, ...n } = this.config[e], i = this.getConstructor(t), s = t[Yt.IsTune];
    return new i({
      name: e,
      constructable: t,
      config: n,
      api: this.api.getMethodsForTool(e, s),
      isDefault: e === this.editorConfig.defaultBlock,
      defaultPlaceholder: this.editorConfig.placeholder,
      isInternal: r
    });
  }
  /**
   * Find appropriate Tool object constructor for Tool constructable
   *
   * @param constructable - Tools constructable
   */
  getConstructor(e) {
    switch (!0) {
      case e[ft.IsInline]:
        return Mc;
      case e[Yt.IsTune]:
        return _c;
      default:
        return Qo;
    }
  }
}
class vi {
  /**
   * MoveDownTune constructor
   *
   * @param {API} api — Editor's API
   */
  constructor({ api: e }) {
    this.CSS = {
      animation: "wobble"
    }, this.api = e;
  }
  /**
   * Tune's appearance in block settings menu
   */
  render() {
    return {
      icon: ta,
      title: this.api.i18n.t("Move down"),
      onActivate: () => this.handleClick(),
      name: "move-down"
    };
  }
  /**
   * Handle clicks on 'move down' button
   */
  handleClick() {
    const e = this.api.blocks.getCurrentBlockIndex(), t = this.api.blocks.getBlockByIndex(e + 1);
    if (!t)
      throw new Error("Unable to move Block down since it is already the last");
    const r = t.holder, n = r.getBoundingClientRect();
    let i = Math.abs(window.innerHeight - r.offsetHeight);
    n.top < window.innerHeight && (i = window.scrollY + r.offsetHeight), window.scrollTo(0, i), this.api.blocks.move(e + 1), this.api.toolbar.toggleBlockSettings(!0);
  }
}
vi.isTune = !0;
class bi {
  /**
   * DeleteTune constructor
   *
   * @param {API} api - Editor's API
   */
  constructor({ api: e }) {
    this.api = e;
  }
  /**
   * Tune's appearance in block settings menu
   */
  render() {
    return {
      icon: ia,
      title: this.api.i18n.t("Delete"),
      name: "delete",
      confirmation: {
        title: this.api.i18n.t("Click to delete"),
        onActivate: () => this.handleClick()
      }
    };
  }
  /**
   * Delete block conditions passed
   */
  handleClick() {
    this.api.blocks.delete();
  }
}
bi.isTune = !0;
class ki {
  /**
   * MoveUpTune constructor
   *
   * @param {API} api - Editor's API
   */
  constructor({ api: e }) {
    this.CSS = {
      animation: "wobble"
    }, this.api = e;
  }
  /**
   * Tune's appearance in block settings menu
   */
  render() {
    return {
      icon: na,
      title: this.api.i18n.t("Move up"),
      onActivate: () => this.handleClick(),
      name: "move-up"
    };
  }
  /**
   * Move current block up
   */
  handleClick() {
    const e = this.api.blocks.getCurrentBlockIndex(), t = this.api.blocks.getBlockByIndex(e), r = this.api.blocks.getBlockByIndex(e - 1);
    if (e === 0 || !t || !r)
      throw new Error("Unable to move Block up since it is already the first");
    const n = t.holder, i = r.holder, s = n.getBoundingClientRect(), a = i.getBoundingClientRect();
    let l;
    a.top > 0 ? l = Math.abs(s.top) - Math.abs(a.top) : l = Math.abs(s.top) + a.height, window.scrollBy(0, -1 * l), this.api.blocks.move(e - 1), this.api.toolbar.toggleBlockSettings(!0);
  }
}
ki.isTune = !0;
var Ac = Object.defineProperty, Pc = Object.getOwnPropertyDescriptor, Nc = (o, e, t, r) => {
  for (var n = Pc(e, t), i = o.length - 1, s; i >= 0; i--)
    (s = o[i]) && (n = s(e, t, n) || n);
  return n && Ac(e, t, n), n;
};
class wi extends N {
  constructor() {
    super(...arguments), this.stubTool = "stub", this.toolsAvailable = new G(), this.toolsUnavailable = new G();
  }
  /**
   * Returns available Tools
   */
  get available() {
    return this.toolsAvailable;
  }
  /**
   * Returns unavailable Tools
   */
  get unavailable() {
    return this.toolsUnavailable;
  }
  /**
   * Return Tools for the Inline Toolbar
   */
  get inlineTools() {
    return this.available.inlineTools;
  }
  /**
   * Return editor block tools
   */
  get blockTools() {
    return this.available.blockTools;
  }
  /**
   * Return available Block Tunes
   *
   * @returns {object} - object of Inline Tool's classes
   */
  get blockTunes() {
    return this.available.blockTunes;
  }
  /**
   * Returns default Tool object
   */
  get defaultTool() {
    return this.blockTools.get(this.config.defaultBlock);
  }
  /**
   * Returns internal tools
   */
  get internal() {
    return this.available.internalTools;
  }
  /**
   * Creates instances via passed or default configuration
   *
   * @returns {Promise<void>}
   */
  async prepare() {
    if (this.validateTools(), this.config.tools = Ut({}, this.internalTools, this.config.tools), !Object.prototype.hasOwnProperty.call(this.config, "tools") || Object.keys(this.config.tools).length === 0)
      throw Error("Can't start without tools");
    const e = this.prepareConfig();
    this.factory = new Oc(e, this.config, this.Editor.API);
    const t = this.getListOfPrepareFunctions(e);
    if (t.length === 0)
      return Promise.resolve();
    await is(t, (r) => {
      this.toolPrepareMethodSuccess(r);
    }, (r) => {
      this.toolPrepareMethodFallback(r);
    }), this.prepareBlockTools();
  }
  getAllInlineToolsSanitizeConfig() {
    const e = {};
    return Array.from(this.inlineTools.values()).forEach((t) => {
      Object.assign(e, t.sanitizeConfig);
    }), e;
  }
  /**
   * Calls each Tool reset method to clean up anything set by Tool
   */
  destroy() {
    Object.values(this.available).forEach(async (e) => {
      R(e.reset) && await e.reset();
    });
  }
  /**
   * Returns internal tools
   * Includes Bold, Italic, Link and Paragraph
   */
  get internalTools() {
    return {
      convertTo: {
        class: fi,
        isInternal: !0
      },
      link: {
        class: Jo,
        isInternal: !0
      },
      bold: {
        class: Zo,
        isInternal: !0
      },
      italic: {
        class: Go,
        isInternal: !0
      },
      paragraph: {
        class: Xo,
        inlineToolbar: !0,
        isInternal: !0
      },
      stub: {
        class: gi,
        isInternal: !0
      },
      moveUp: {
        class: ki,
        isInternal: !0
      },
      delete: {
        class: bi,
        isInternal: !0
      },
      moveDown: {
        class: vi,
        isInternal: !0
      }
    };
  }
  /**
   * Tool prepare method success callback
   *
   * @param {object} data - append tool to available list
   */
  toolPrepareMethodSuccess(e) {
    const t = this.factory.get(e.toolName);
    if (t.isInline()) {
      const r = ["render"].filter((n) => !t.create()[n]);
      if (r.length) {
        j(
          `Incorrect Inline Tool: ${t.name}. Some of required methods is not implemented %o`,
          "warn",
          r
        ), this.toolsUnavailable.set(t.name, t);
        return;
      }
    }
    this.toolsAvailable.set(t.name, t);
  }
  /**
   * Tool prepare method fail callback
   *
   * @param {object} data - append tool to unavailable list
   */
  toolPrepareMethodFallback(e) {
    this.toolsUnavailable.set(e.toolName, this.factory.get(e.toolName));
  }
  /**
   * Binds prepare function of plugins with user or default config
   *
   * @returns {Array} list of functions that needs to be fired sequentially
   * @param config - tools config
   */
  getListOfPrepareFunctions(e) {
    const t = [];
    return Object.entries(e).forEach(([r, n]) => {
      t.push({
        // eslint-disable-next-line @typescript-eslint/no-empty-function
        function: R(n.class.prepare) ? n.class.prepare : () => {
        },
        data: {
          toolName: r,
          config: n.config
        }
      });
    }), t;
  }
  /**
   * Assign enabled Inline Tools and Block Tunes for Block Tool
   */
  prepareBlockTools() {
    Array.from(this.blockTools.values()).forEach((e) => {
      this.assignInlineToolsToBlockTool(e), this.assignBlockTunesToBlockTool(e);
    });
  }
  /**
   * Assign enabled Inline Tools for Block Tool
   *
   * @param tool - Block Tool
   */
  assignInlineToolsToBlockTool(e) {
    if (this.config.inlineToolbar !== !1) {
      if (e.enabledInlineTools === !0) {
        e.inlineTools = new G(
          Array.isArray(this.config.inlineToolbar) ? this.config.inlineToolbar.map((t) => [t, this.inlineTools.get(t)]) : Array.from(this.inlineTools.entries())
        );
        return;
      }
      Array.isArray(e.enabledInlineTools) && (e.inlineTools = new G(
        /** Prepend ConvertTo Inline Tool */
        ["convertTo", ...e.enabledInlineTools].map((t) => [t, this.inlineTools.get(t)])
      ));
    }
  }
  /**
   * Assign enabled Block Tunes for Block Tool
   *
   * @param tool — Block Tool
   */
  assignBlockTunesToBlockTool(e) {
    if (e.enabledBlockTunes !== !1) {
      if (Array.isArray(e.enabledBlockTunes)) {
        const t = new G(
          e.enabledBlockTunes.map((r) => [r, this.blockTunes.get(r)])
        );
        e.tunes = new G([...t, ...this.blockTunes.internalTools]);
        return;
      }
      if (Array.isArray(this.config.tunes)) {
        const t = new G(
          this.config.tunes.map((r) => [r, this.blockTunes.get(r)])
        );
        e.tunes = new G([...t, ...this.blockTunes.internalTools]);
        return;
      }
      e.tunes = this.blockTunes.internalTools;
    }
  }
  /**
   * Validate Tools configuration objects and throw Error for user if it is invalid
   */
  validateTools() {
    for (const e in this.config.tools)
      if (Object.prototype.hasOwnProperty.call(this.config.tools, e)) {
        if (e in this.internalTools)
          return;
        const t = this.config.tools[e];
        if (!R(t) && !R(t.class))
          throw Error(
            `Tool «${e}» must be a constructor function or an object with function in the «class» property`
          );
      }
  }
  /**
   * Unify tools config
   */
  prepareConfig() {
    const e = {};
    for (const t in this.config.tools)
      U(this.config.tools[t]) ? e[t] = this.config.tools[t] : e[t] = { class: this.config.tools[t] };
    return e;
  }
}
Nc([
  Me
], wi.prototype, "getAllInlineToolsSanitizeConfig");
const jc = `:root{--selectionColor: #e1f2ff;--inlineSelectionColor: #d4ecff;--bg-light: #eff2f5;--grayText: #707684;--color-dark: #1D202B;--color-active-icon: #388AE5;--color-gray-border: rgba(201, 201, 204, .48);--content-width: 650px;--narrow-mode-right-padding: 50px;--toolbox-buttons-size: 26px;--toolbox-buttons-size--mobile: 36px;--icon-size: 20px;--icon-size--mobile: 28px;--block-padding-vertical: .4em;--color-line-gray: #EFF0F1 }.codex-editor{position:relative;-webkit-box-sizing:border-box;box-sizing:border-box;z-index:1}.codex-editor .hide{display:none}.codex-editor__redactor [contenteditable]:empty:after{content:"\\feff"}@media (min-width: 651px){.codex-editor--narrow .codex-editor__redactor{margin-right:50px}}@media (min-width: 651px){.codex-editor--narrow.codex-editor--rtl .codex-editor__redactor{margin-left:50px;margin-right:0}}@media (min-width: 651px){.codex-editor--narrow .ce-toolbar__actions{right:-5px}}.codex-editor-copyable{position:absolute;height:1px;width:1px;top:-400%;opacity:.001}.codex-editor-overlay{position:fixed;top:0;left:0;right:0;bottom:0;z-index:999;pointer-events:none;overflow:hidden}.codex-editor-overlay__container{position:relative;pointer-events:auto;z-index:0}.codex-editor-overlay__rectangle{position:absolute;pointer-events:none;background-color:#2eaadc33;border:1px solid transparent}.codex-editor svg{max-height:100%}.codex-editor path{stroke:currentColor}.codex-editor ::-moz-selection{background-color:#d4ecff}.codex-editor ::selection{background-color:#d4ecff}.codex-editor--toolbox-opened [contentEditable=true][data-placeholder]:focus:before{opacity:0!important}.ce-scroll-locked{overflow:hidden}.ce-scroll-locked--hard{overflow:hidden;top:calc(-1 * var(--window-scroll-offset));position:fixed;width:100%}.ce-toolbar{position:absolute;left:0;right:0;top:0;-webkit-transition:opacity .1s ease;transition:opacity .1s ease;will-change:opacity,top;display:none}.ce-toolbar--opened{display:block}.ce-toolbar__content{max-width:650px;margin:0 auto;position:relative}.ce-toolbar__plus{color:#1d202b;cursor:pointer;width:26px;height:26px;border-radius:7px;display:-webkit-inline-box;display:-ms-inline-flexbox;display:inline-flex;-webkit-box-pack:center;-ms-flex-pack:center;justify-content:center;-webkit-box-align:center;-ms-flex-align:center;align-items:center;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;-ms-flex-negative:0;flex-shrink:0}@media (max-width: 650px){.ce-toolbar__plus{width:36px;height:36px}}@media (hover: hover){.ce-toolbar__plus:hover{background-color:#eff2f5}}.ce-toolbar__plus--active{background-color:#eff2f5;-webkit-animation:bounceIn .75s 1;animation:bounceIn .75s 1;-webkit-animation-fill-mode:forwards;animation-fill-mode:forwards}.ce-toolbar__plus-shortcut{opacity:.6;word-spacing:-2px;margin-top:5px}@media (max-width: 650px){.ce-toolbar__plus{position:absolute;background-color:#fff;border:1px solid #E8E8EB;-webkit-box-shadow:0 3px 15px -3px rgba(13,20,33,.13);box-shadow:0 3px 15px -3px #0d142121;border-radius:6px;z-index:2;position:static}.ce-toolbar__plus--left-oriented:before{left:15px;margin-left:0}.ce-toolbar__plus--right-oriented:before{left:auto;right:15px;margin-left:0}}.ce-toolbar__actions{position:absolute;right:100%;opacity:0;display:-webkit-box;display:-ms-flexbox;display:flex;padding-right:5px}.ce-toolbar__actions--opened{opacity:1}@media (max-width: 650px){.ce-toolbar__actions{right:auto}}.ce-toolbar__settings-btn{color:#1d202b;width:26px;height:26px;border-radius:7px;display:-webkit-inline-box;display:-ms-inline-flexbox;display:inline-flex;-webkit-box-pack:center;-ms-flex-pack:center;justify-content:center;-webkit-box-align:center;-ms-flex-align:center;align-items:center;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;margin-left:3px;cursor:pointer;user-select:none}@media (max-width: 650px){.ce-toolbar__settings-btn{width:36px;height:36px}}@media (hover: hover){.ce-toolbar__settings-btn:hover{background-color:#eff2f5}}.ce-toolbar__settings-btn--active{background-color:#eff2f5;-webkit-animation:bounceIn .75s 1;animation:bounceIn .75s 1;-webkit-animation-fill-mode:forwards;animation-fill-mode:forwards}@media (min-width: 651px){.ce-toolbar__settings-btn{width:24px}}.ce-toolbar__settings-btn--hidden{display:none}@media (max-width: 650px){.ce-toolbar__settings-btn{position:absolute;background-color:#fff;border:1px solid #E8E8EB;-webkit-box-shadow:0 3px 15px -3px rgba(13,20,33,.13);box-shadow:0 3px 15px -3px #0d142121;border-radius:6px;z-index:2;position:static}.ce-toolbar__settings-btn--left-oriented:before{left:15px;margin-left:0}.ce-toolbar__settings-btn--right-oriented:before{left:auto;right:15px;margin-left:0}}.ce-toolbar__plus svg,.ce-toolbar__settings-btn svg{width:24px;height:24px}@media (min-width: 651px){.codex-editor--narrow .ce-toolbar__plus{left:5px}}@media (min-width: 651px){.codex-editor--narrow .ce-toolbox .ce-popover{right:0;left:auto;left:initial}}.ce-inline-toolbar{--y-offset: 8px;--color-background-icon-active: rgba(56, 138, 229, .1);--color-text-icon-active: #388AE5;--color-text-primary: black;position:absolute;visibility:hidden;-webkit-transition:opacity .25s ease;transition:opacity .25s ease;will-change:opacity,left,top;top:0;left:0;z-index:3;opacity:1;visibility:visible}.ce-inline-toolbar [hidden]{display:none!important}.ce-inline-toolbar__toggler-and-button-wrapper{display:-webkit-box;display:-ms-flexbox;display:flex;width:100%;padding:0 6px}.ce-inline-toolbar__buttons{display:-webkit-box;display:-ms-flexbox;display:flex}.ce-inline-toolbar__dropdown{display:-webkit-box;display:-ms-flexbox;display:flex;padding:6px;margin:0 6px 0 -6px;-webkit-box-align:center;-ms-flex-align:center;align-items:center;cursor:pointer;border-right:1px solid rgba(201,201,204,.48);-webkit-box-sizing:border-box;box-sizing:border-box}@media (hover: hover){.ce-inline-toolbar__dropdown:hover{background:#eff2f5}}.ce-inline-toolbar__dropdown--hidden{display:none}.ce-inline-toolbar__dropdown-content,.ce-inline-toolbar__dropdown-arrow{display:-webkit-box;display:-ms-flexbox;display:flex}.ce-inline-toolbar__dropdown-content svg,.ce-inline-toolbar__dropdown-arrow svg{width:20px;height:20px}.ce-inline-toolbar__shortcut{opacity:.6;word-spacing:-3px;margin-top:3px}.ce-inline-tool{color:var(--color-text-primary);display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-pack:center;-ms-flex-pack:center;justify-content:center;-webkit-box-align:center;-ms-flex-align:center;align-items:center;border:0;border-radius:4px;line-height:normal;height:100%;padding:0;width:28px;background-color:transparent;cursor:pointer}@media (max-width: 650px){.ce-inline-tool{width:36px;height:36px}}@media (hover: hover){.ce-inline-tool:hover{background-color:#f8f8f8}}.ce-inline-tool svg{display:block;width:20px;height:20px}@media (max-width: 650px){.ce-inline-tool svg{width:28px;height:28px}}.ce-inline-tool--link .icon--unlink,.ce-inline-tool--unlink .icon--link{display:none}.ce-inline-tool--unlink .icon--unlink{display:inline-block;margin-bottom:-1px}.ce-inline-tool-input{background:#F8F8F8;border:1px solid rgba(226,226,229,.2);border-radius:6px;padding:4px 8px;font-size:14px;line-height:22px;outline:none;margin:0;width:100%;-webkit-box-sizing:border-box;box-sizing:border-box;display:none;font-weight:500;-webkit-appearance:none;font-family:inherit}@media (max-width: 650px){.ce-inline-tool-input{font-size:15px;font-weight:500}}.ce-inline-tool-input::-webkit-input-placeholder{color:#707684}.ce-inline-tool-input::-moz-placeholder{color:#707684}.ce-inline-tool-input:-ms-input-placeholder{color:#707684}.ce-inline-tool-input::-ms-input-placeholder{color:#707684}.ce-inline-tool-input::placeholder{color:#707684}.ce-inline-tool-input--showed{display:block}.ce-inline-tool--active{background:var(--color-background-icon-active);color:var(--color-text-icon-active)}@-webkit-keyframes fade-in{0%{opacity:0}to{opacity:1}}@keyframes fade-in{0%{opacity:0}to{opacity:1}}.ce-block{-webkit-animation:fade-in .3s ease;animation:fade-in .3s ease;-webkit-animation-fill-mode:none;animation-fill-mode:none;-webkit-animation-fill-mode:initial;animation-fill-mode:initial}.ce-block:first-of-type{margin-top:0}.ce-block--selected .ce-block__content{background:#e1f2ff}.ce-block--selected .ce-block__content [contenteditable]{-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none}.ce-block--selected .ce-block__content img,.ce-block--selected .ce-block__content .ce-stub{opacity:.55}.ce-block--stretched .ce-block__content{max-width:none}.ce-block__content{position:relative;max-width:650px;margin:0 auto;-webkit-transition:background-color .15s ease;transition:background-color .15s ease}.ce-block--drop-target .ce-block__content:before{content:"";position:absolute;top:100%;left:-20px;margin-top:-1px;height:8px;width:8px;border:solid #388AE5;border-width:1px 1px 0 0;-webkit-transform-origin:right;transform-origin:right;-webkit-transform:rotate(45deg);transform:rotate(45deg)}.ce-block--drop-target .ce-block__content:after{content:"";position:absolute;top:100%;height:1px;width:100%;color:#388ae5;background:repeating-linear-gradient(90deg,#388AE5,#388AE5 1px,#fff 1px,#fff 6px)}.ce-block a{cursor:pointer;-webkit-text-decoration:underline;text-decoration:underline}.ce-block b{font-weight:700}.ce-block i{font-style:italic}@-webkit-keyframes bounceIn{0%,20%,40%,60%,80%,to{-webkit-animation-timing-function:cubic-bezier(.215,.61,.355,1);animation-timing-function:cubic-bezier(.215,.61,.355,1)}0%{-webkit-transform:scale3d(.9,.9,.9);transform:scale3d(.9,.9,.9)}20%{-webkit-transform:scale3d(1.03,1.03,1.03);transform:scale3d(1.03,1.03,1.03)}60%{-webkit-transform:scale3d(1,1,1);transform:scaleZ(1)}}@keyframes bounceIn{0%,20%,40%,60%,80%,to{-webkit-animation-timing-function:cubic-bezier(.215,.61,.355,1);animation-timing-function:cubic-bezier(.215,.61,.355,1)}0%{-webkit-transform:scale3d(.9,.9,.9);transform:scale3d(.9,.9,.9)}20%{-webkit-transform:scale3d(1.03,1.03,1.03);transform:scale3d(1.03,1.03,1.03)}60%{-webkit-transform:scale3d(1,1,1);transform:scaleZ(1)}}@-webkit-keyframes selectionBounce{0%,20%,40%,60%,80%,to{-webkit-animation-timing-function:cubic-bezier(.215,.61,.355,1);animation-timing-function:cubic-bezier(.215,.61,.355,1)}50%{-webkit-transform:scale3d(1.01,1.01,1.01);transform:scale3d(1.01,1.01,1.01)}70%{-webkit-transform:scale3d(1,1,1);transform:scaleZ(1)}}@keyframes selectionBounce{0%,20%,40%,60%,80%,to{-webkit-animation-timing-function:cubic-bezier(.215,.61,.355,1);animation-timing-function:cubic-bezier(.215,.61,.355,1)}50%{-webkit-transform:scale3d(1.01,1.01,1.01);transform:scale3d(1.01,1.01,1.01)}70%{-webkit-transform:scale3d(1,1,1);transform:scaleZ(1)}}@-webkit-keyframes buttonClicked{0%,20%,40%,60%,80%,to{-webkit-animation-timing-function:cubic-bezier(.215,.61,.355,1);animation-timing-function:cubic-bezier(.215,.61,.355,1)}0%{-webkit-transform:scale3d(.95,.95,.95);transform:scale3d(.95,.95,.95)}60%{-webkit-transform:scale3d(1.02,1.02,1.02);transform:scale3d(1.02,1.02,1.02)}80%{-webkit-transform:scale3d(1,1,1);transform:scaleZ(1)}}@keyframes buttonClicked{0%,20%,40%,60%,80%,to{-webkit-animation-timing-function:cubic-bezier(.215,.61,.355,1);animation-timing-function:cubic-bezier(.215,.61,.355,1)}0%{-webkit-transform:scale3d(.95,.95,.95);transform:scale3d(.95,.95,.95)}60%{-webkit-transform:scale3d(1.02,1.02,1.02);transform:scale3d(1.02,1.02,1.02)}80%{-webkit-transform:scale3d(1,1,1);transform:scaleZ(1)}}.cdx-block{padding:.4em 0}.cdx-block::-webkit-input-placeholder{line-height:normal!important}.cdx-input{border:1px solid rgba(201,201,204,.48);-webkit-box-shadow:inset 0 1px 2px 0 rgba(35,44,72,.06);box-shadow:inset 0 1px 2px #232c480f;border-radius:3px;padding:10px 12px;outline:none;width:100%;-webkit-box-sizing:border-box;box-sizing:border-box}.cdx-input[data-placeholder]:before{position:static!important}.cdx-input[data-placeholder]:before{display:inline-block;width:0;white-space:nowrap;pointer-events:none}.cdx-settings-button{display:-webkit-inline-box;display:-ms-inline-flexbox;display:inline-flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center;-webkit-box-pack:center;-ms-flex-pack:center;justify-content:center;border-radius:3px;cursor:pointer;border:0;outline:none;background-color:transparent;vertical-align:bottom;color:inherit;margin:0;min-width:26px;min-height:26px}.cdx-settings-button--focused{background:rgba(34,186,255,.08)!important}.cdx-settings-button--focused{-webkit-box-shadow:inset 0 0 0px 1px rgba(7,161,227,.08);box-shadow:inset 0 0 0 1px #07a1e314}.cdx-settings-button--focused-animated{-webkit-animation-name:buttonClicked;animation-name:buttonClicked;-webkit-animation-duration:.25s;animation-duration:.25s}.cdx-settings-button--active{color:#388ae5}.cdx-settings-button svg{width:auto;height:auto}@media (max-width: 650px){.cdx-settings-button svg{width:28px;height:28px}}@media (max-width: 650px){.cdx-settings-button{width:36px;height:36px;border-radius:8px}}@media (hover: hover){.cdx-settings-button:hover{background-color:#eff2f5}}.cdx-loader{position:relative;border:1px solid rgba(201,201,204,.48)}.cdx-loader:before{content:"";position:absolute;left:50%;top:50%;width:18px;height:18px;margin:-11px 0 0 -11px;border:2px solid rgba(201,201,204,.48);border-left-color:#388ae5;border-radius:50%;-webkit-animation:cdxRotation 1.2s infinite linear;animation:cdxRotation 1.2s infinite linear}@-webkit-keyframes cdxRotation{0%{-webkit-transform:rotate(0deg);transform:rotate(0)}to{-webkit-transform:rotate(360deg);transform:rotate(360deg)}}@keyframes cdxRotation{0%{-webkit-transform:rotate(0deg);transform:rotate(0)}to{-webkit-transform:rotate(360deg);transform:rotate(360deg)}}.cdx-button{padding:13px;border-radius:3px;border:1px solid rgba(201,201,204,.48);font-size:14.9px;background:#fff;-webkit-box-shadow:0 2px 2px 0 rgba(18,30,57,.04);box-shadow:0 2px 2px #121e390a;color:#707684;text-align:center;cursor:pointer}@media (hover: hover){.cdx-button:hover{background:#FBFCFE;-webkit-box-shadow:0 1px 3px 0 rgba(18,30,57,.08);box-shadow:0 1px 3px #121e3914}}.cdx-button svg{height:20px;margin-right:.2em;margin-top:-2px}.ce-stub{display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center;padding:12px 18px;margin:10px 0;border-radius:10px;background:#eff2f5;border:1px solid #EFF0F1;color:#707684;font-size:14px}.ce-stub svg{width:20px;height:20px}.ce-stub__info{margin-left:14px}.ce-stub__title{font-weight:500;text-transform:capitalize}.codex-editor.codex-editor--rtl{direction:rtl}.codex-editor.codex-editor--rtl .cdx-list{padding-left:0;padding-right:40px}.codex-editor.codex-editor--rtl .ce-toolbar__plus{right:-26px;left:auto}.codex-editor.codex-editor--rtl .ce-toolbar__actions{right:auto;left:-26px}@media (max-width: 650px){.codex-editor.codex-editor--rtl .ce-toolbar__actions{margin-left:0;margin-right:auto;padding-right:0;padding-left:10px}}.codex-editor.codex-editor--rtl .ce-settings{left:5px;right:auto}.codex-editor.codex-editor--rtl .ce-settings:before{right:auto;left:25px}.codex-editor.codex-editor--rtl .ce-settings__button:not(:nth-child(3n+3)){margin-left:3px;margin-right:0}.codex-editor.codex-editor--rtl .ce-conversion-tool__icon{margin-right:0;margin-left:10px}.codex-editor.codex-editor--rtl .ce-inline-toolbar__dropdown{border-right:0px solid transparent;border-left:1px solid rgba(201,201,204,.48);margin:0 -6px 0 6px}.codex-editor.codex-editor--rtl .ce-inline-toolbar__dropdown .icon--toggler-down{margin-left:0;margin-right:4px}@media (min-width: 651px){.codex-editor--narrow.codex-editor--rtl .ce-toolbar__plus{left:0;right:5px}}@media (min-width: 651px){.codex-editor--narrow.codex-editor--rtl .ce-toolbar__actions{left:-5px}}.cdx-search-field{--icon-margin-right: 10px;background:#F8F8F8;border:1px solid rgba(226,226,229,.2);border-radius:6px;padding:2px;display:grid;grid-template-columns:auto auto 1fr;grid-template-rows:auto}.cdx-search-field__icon{width:26px;height:26px;display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center;-webkit-box-pack:center;-ms-flex-pack:center;justify-content:center;margin-right:var(--icon-margin-right)}.cdx-search-field__icon svg{width:20px;height:20px;color:#707684}.cdx-search-field__input{font-size:14px;outline:none;font-weight:500;font-family:inherit;border:0;background:transparent;margin:0;padding:0;line-height:22px;min-width:calc(100% - 26px - var(--icon-margin-right))}.cdx-search-field__input::-webkit-input-placeholder{color:#707684;font-weight:500}.cdx-search-field__input::-moz-placeholder{color:#707684;font-weight:500}.cdx-search-field__input:-ms-input-placeholder{color:#707684;font-weight:500}.cdx-search-field__input::-ms-input-placeholder{color:#707684;font-weight:500}.cdx-search-field__input::placeholder{color:#707684;font-weight:500}.ce-popover{--border-radius: 6px;--width: 200px;--max-height: 270px;--padding: 6px;--offset-from-target: 8px;--color-border: #EFF0F1;--color-shadow: rgba(13, 20, 33, .1);--color-background: white;--color-text-primary: black;--color-text-secondary: #707684;--color-border-icon: rgba(201, 201, 204, .48);--color-border-icon-disabled: #EFF0F1;--color-text-icon-active: #388AE5;--color-background-icon-active: rgba(56, 138, 229, .1);--color-background-item-focus: rgba(34, 186, 255, .08);--color-shadow-item-focus: rgba(7, 161, 227, .08);--color-background-item-hover: #F8F8F8;--color-background-item-confirm: #E24A4A;--color-background-item-confirm-hover: #CE4343;--popover-top: calc(100% + var(--offset-from-target));--popover-left: 0;--nested-popover-overlap: 4px;--icon-size: 20px;--item-padding: 3px;--item-height: calc(var(--icon-size) + 2 * var(--item-padding))}.ce-popover__container{min-width:var(--width);width:var(--width);max-height:var(--max-height);border-radius:var(--border-radius);overflow:hidden;-webkit-box-sizing:border-box;box-sizing:border-box;-webkit-box-shadow:0px 3px 15px -3px var(--color-shadow);box-shadow:0 3px 15px -3px var(--color-shadow);position:absolute;left:var(--popover-left);top:var(--popover-top);background:var(--color-background);display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-orient:vertical;-webkit-box-direction:normal;-ms-flex-direction:column;flex-direction:column;z-index:4;opacity:0;max-height:0;pointer-events:none;padding:0;border:none}.ce-popover--opened>.ce-popover__container{opacity:1;padding:var(--padding);max-height:var(--max-height);pointer-events:auto;-webkit-animation:panelShowing .1s ease;animation:panelShowing .1s ease;border:1px solid var(--color-border)}@media (max-width: 650px){.ce-popover--opened>.ce-popover__container{-webkit-animation:panelShowingMobile .25s ease;animation:panelShowingMobile .25s ease}}.ce-popover--open-top .ce-popover__container{--popover-top: calc(-1 * (var(--offset-from-target) + var(--popover-height)))}.ce-popover--open-left .ce-popover__container{--popover-left: calc(-1 * var(--width) + 100%)}.ce-popover__items{overflow-y:auto;-ms-scroll-chaining:none;overscroll-behavior:contain}@media (max-width: 650px){.ce-popover__overlay{position:fixed;top:0;bottom:0;left:0;right:0;background:#1D202B;z-index:3;opacity:.5;-webkit-transition:opacity .12s ease-in;transition:opacity .12s ease-in;will-change:opacity;visibility:visible}}.ce-popover__overlay--hidden{display:none}@media (max-width: 650px){.ce-popover .ce-popover__container{--offset: 5px;position:fixed;max-width:none;min-width:calc(100% - var(--offset) * 2);left:var(--offset);right:var(--offset);bottom:calc(var(--offset) + env(safe-area-inset-bottom));top:auto;border-radius:10px}}.ce-popover__search{margin-bottom:5px}.ce-popover__nothing-found-message{color:#707684;display:none;cursor:default;padding:3px;font-size:14px;line-height:20px;font-weight:500;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.ce-popover__nothing-found-message--displayed{display:block}.ce-popover--nested .ce-popover__container{--popover-left: calc(var(--nesting-level) * (var(--width) - var(--nested-popover-overlap)));top:calc(var(--trigger-item-top) - var(--nested-popover-overlap));position:absolute}.ce-popover--open-top.ce-popover--nested .ce-popover__container{top:calc(var(--trigger-item-top) - var(--popover-height) + var(--item-height) + var(--offset-from-target) + var(--nested-popover-overlap))}.ce-popover--open-left .ce-popover--nested .ce-popover__container{--popover-left: calc(-1 * (var(--nesting-level) + 1) * var(--width) + 100%)}.ce-popover-item-separator{padding:4px 3px}.ce-popover-item-separator--hidden{display:none}.ce-popover-item-separator__line{height:1px;background:var(--color-border);width:100%}.ce-popover-item-html--hidden{display:none}.ce-popover-item{--border-radius: 6px;border-radius:var(--border-radius);display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center;padding:var(--item-padding);color:var(--color-text-primary);-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;border:none;background:transparent}@media (max-width: 650px){.ce-popover-item{padding:4px}}.ce-popover-item:not(:last-of-type){margin-bottom:1px}.ce-popover-item__icon{width:26px;height:26px;display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center;-webkit-box-pack:center;-ms-flex-pack:center;justify-content:center}.ce-popover-item__icon svg{width:20px;height:20px}@media (max-width: 650px){.ce-popover-item__icon{width:36px;height:36px;border-radius:8px}.ce-popover-item__icon svg{width:28px;height:28px}}.ce-popover-item__icon--tool{margin-right:4px}.ce-popover-item__title{font-size:14px;line-height:20px;font-weight:500;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;margin-right:auto}@media (max-width: 650px){.ce-popover-item__title{font-size:16px}}.ce-popover-item__secondary-title{color:var(--color-text-secondary);font-size:12px;white-space:nowrap;letter-spacing:-.1em;padding-right:5px;opacity:.6}@media (max-width: 650px){.ce-popover-item__secondary-title{display:none}}.ce-popover-item--active{background:var(--color-background-icon-active);color:var(--color-text-icon-active)}.ce-popover-item--disabled{color:var(--color-text-secondary);cursor:default;pointer-events:none}.ce-popover-item--focused:not(.ce-popover-item--no-focus){background:var(--color-background-item-focus)!important}.ce-popover-item--hidden{display:none}@media (hover: hover){.ce-popover-item:hover{cursor:pointer}.ce-popover-item:hover:not(.ce-popover-item--no-hover){background-color:var(--color-background-item-hover)}}.ce-popover-item--confirmation{background:var(--color-background-item-confirm)}.ce-popover-item--confirmation .ce-popover-item__title,.ce-popover-item--confirmation .ce-popover-item__icon{color:#fff}@media (hover: hover){.ce-popover-item--confirmation:not(.ce-popover-item--no-hover):hover{background:var(--color-background-item-confirm-hover)}}.ce-popover-item--confirmation:not(.ce-popover-item--no-focus).ce-popover-item--focused{background:var(--color-background-item-confirm-hover)!important}@-webkit-keyframes panelShowing{0%{opacity:0;-webkit-transform:translateY(-8px) scale(.9);transform:translateY(-8px) scale(.9)}70%{opacity:1;-webkit-transform:translateY(2px);transform:translateY(2px)}to{-webkit-transform:translateY(0);transform:translateY(0)}}@keyframes panelShowing{0%{opacity:0;-webkit-transform:translateY(-8px) scale(.9);transform:translateY(-8px) scale(.9)}70%{opacity:1;-webkit-transform:translateY(2px);transform:translateY(2px)}to{-webkit-transform:translateY(0);transform:translateY(0)}}@-webkit-keyframes panelShowingMobile{0%{opacity:0;-webkit-transform:translateY(14px) scale(.98);transform:translateY(14px) scale(.98)}70%{opacity:1;-webkit-transform:translateY(-4px);transform:translateY(-4px)}to{-webkit-transform:translateY(0);transform:translateY(0)}}@keyframes panelShowingMobile{0%{opacity:0;-webkit-transform:translateY(14px) scale(.98);transform:translateY(14px) scale(.98)}70%{opacity:1;-webkit-transform:translateY(-4px);transform:translateY(-4px)}to{-webkit-transform:translateY(0);transform:translateY(0)}}.wobble{-webkit-animation-name:wobble;animation-name:wobble;-webkit-animation-duration:.4s;animation-duration:.4s}@-webkit-keyframes wobble{0%{-webkit-transform:translate3d(0,0,0);transform:translateZ(0)}15%{-webkit-transform:translate3d(-9%,0,0);transform:translate3d(-9%,0,0)}30%{-webkit-transform:translate3d(9%,0,0);transform:translate3d(9%,0,0)}45%{-webkit-transform:translate3d(-4%,0,0);transform:translate3d(-4%,0,0)}60%{-webkit-transform:translate3d(4%,0,0);transform:translate3d(4%,0,0)}75%{-webkit-transform:translate3d(-1%,0,0);transform:translate3d(-1%,0,0)}to{-webkit-transform:translate3d(0,0,0);transform:translateZ(0)}}@keyframes wobble{0%{-webkit-transform:translate3d(0,0,0);transform:translateZ(0)}15%{-webkit-transform:translate3d(-9%,0,0);transform:translate3d(-9%,0,0)}30%{-webkit-transform:translate3d(9%,0,0);transform:translate3d(9%,0,0)}45%{-webkit-transform:translate3d(-4%,0,0);transform:translate3d(-4%,0,0)}60%{-webkit-transform:translate3d(4%,0,0);transform:translate3d(4%,0,0)}75%{-webkit-transform:translate3d(-1%,0,0);transform:translate3d(-1%,0,0)}to{-webkit-transform:translate3d(0,0,0);transform:translateZ(0)}}.ce-popover-header{margin-bottom:8px;margin-top:4px;display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center}.ce-popover-header__text{font-size:18px;font-weight:600}.ce-popover-header__back-button{border:0;background:transparent;width:36px;height:36px;color:var(--color-text-primary)}.ce-popover-header__back-button svg{display:block;width:28px;height:28px}.ce-popover--inline{--height: 38px;--height-mobile: 46px;--container-padding: 4px;position:relative}.ce-popover--inline .ce-popover__custom-content{margin-bottom:0}.ce-popover--inline .ce-popover__items{display:-webkit-box;display:-ms-flexbox;display:flex}.ce-popover--inline .ce-popover__container{-webkit-box-orient:horizontal;-webkit-box-direction:normal;-ms-flex-direction:row;flex-direction:row;padding:var(--container-padding);height:var(--height);top:0;min-width:-webkit-max-content;min-width:-moz-max-content;min-width:max-content;width:-webkit-max-content;width:-moz-max-content;width:max-content;-webkit-animation:none;animation:none}@media (max-width: 650px){.ce-popover--inline .ce-popover__container{height:var(--height-mobile);position:absolute}}.ce-popover--inline .ce-popover-item-separator{padding:0 4px}.ce-popover--inline .ce-popover-item-separator__line{height:100%;width:1px}.ce-popover--inline .ce-popover-item{border-radius:4px;padding:4px}.ce-popover--inline .ce-popover-item__icon--tool{-webkit-box-shadow:none;box-shadow:none;background:transparent;margin-right:0}.ce-popover--inline .ce-popover-item__icon{width:auto;width:initial;height:auto;height:initial}.ce-popover--inline .ce-popover-item__icon svg{width:20px;height:20px}@media (max-width: 650px){.ce-popover--inline .ce-popover-item__icon svg{width:28px;height:28px}}.ce-popover--inline .ce-popover-item:not(:last-of-type){margin-bottom:0;margin-bottom:initial}.ce-popover--inline .ce-popover-item-html{display:-webkit-box;display:-ms-flexbox;display:flex;-webkit-box-align:center;-ms-flex-align:center;align-items:center}.ce-popover--inline .ce-popover-item__icon--chevron-right{-webkit-transform:rotate(90deg);transform:rotate(90deg)}.ce-popover--inline .ce-popover--nested-level-1 .ce-popover__container{--offset: 3px;left:0;top:calc(var(--height) + var(--offset))}@media (max-width: 650px){.ce-popover--inline .ce-popover--nested-level-1 .ce-popover__container{top:calc(var(--height-mobile) + var(--offset))}}.ce-popover--inline .ce-popover--nested .ce-popover__container{min-width:var(--width);width:var(--width);height:-webkit-fit-content;height:-moz-fit-content;height:fit-content;padding:6px;-webkit-box-orient:vertical;-webkit-box-direction:normal;-ms-flex-direction:column;flex-direction:column}.ce-popover--inline .ce-popover--nested .ce-popover__items{display:block;width:100%}.ce-popover--inline .ce-popover--nested .ce-popover-item{border-radius:6px;padding:3px}@media (max-width: 650px){.ce-popover--inline .ce-popover--nested .ce-popover-item{padding:4px}}.ce-popover--inline .ce-popover--nested .ce-popover-item__icon--tool{margin-right:4px}.ce-popover--inline .ce-popover--nested .ce-popover-item__icon{width:26px;height:26px}.ce-popover--inline .ce-popover--nested .ce-popover-item-separator{padding:4px 3px}.ce-popover--inline .ce-popover--nested .ce-popover-item-separator__line{width:100%;height:1px}.codex-editor [data-placeholder]:empty:before,.codex-editor [data-placeholder][data-empty=true]:before{pointer-events:none;color:#707684;cursor:text;content:attr(data-placeholder)}.codex-editor [data-placeholder-active]:empty:before,.codex-editor [data-placeholder-active][data-empty=true]:before{pointer-events:none;color:#707684;cursor:text}.codex-editor [data-placeholder-active]:empty:focus:before,.codex-editor [data-placeholder-active][data-empty=true]:focus:before{content:attr(data-placeholder-active)}
`;
class Dc extends N {
  constructor() {
    super(...arguments), this.isMobile = !1, this.contentRectCache = null, this.resizeDebouncer = Ur(() => {
      this.windowResize();
    }, 200), this.selectionChangeDebounced = Ur(() => {
      this.selectionChanged();
    }, kc), this.documentTouchedListener = (e) => {
      this.documentTouched(e);
    };
  }
  /**
   * Editor.js UI CSS class names
   *
   * @returns {{editorWrapper: string, editorZone: string}}
   */
  get CSS() {
    return {
      editorWrapper: "codex-editor",
      editorWrapperNarrow: "codex-editor--narrow",
      editorZone: "codex-editor__redactor",
      editorZoneHidden: "codex-editor__redactor--hidden",
      editorEmpty: "codex-editor--empty",
      editorRtlFix: "codex-editor--rtl"
    };
  }
  /**
   * Return Width of center column of Editor
   *
   * @returns {DOMRect}
   */
  get contentRect() {
    if (this.contentRectCache !== null)
      return this.contentRectCache;
    const e = this.nodes.wrapper.querySelector(`.${ne.CSS.content}`);
    return e ? (this.contentRectCache = e.getBoundingClientRect(), this.contentRectCache) : {
      width: 650,
      left: 0,
      right: 0
    };
  }
  /**
   * Making main interface
   */
  async prepare() {
    this.setIsMobile(), this.make(), this.loadStyles();
  }
  /**
   * Toggle read-only state
   *
   * If readOnly is true:
   *  - removes all listeners from main UI module elements
   *
   * if readOnly is false:
   *  - enables all listeners to UI module elements
   *
   * @param {boolean} readOnlyEnabled - "read only" state
   */
  toggleReadOnly(e) {
    e ? this.unbindReadOnlySensitiveListeners() : window.requestIdleCallback(() => {
      this.bindReadOnlySensitiveListeners();
    }, {
      timeout: 2e3
    });
  }
  /**
   * Check if Editor is empty and set CSS class to wrapper
   */
  checkEmptiness() {
    const { BlockManager: e } = this.Editor;
    this.nodes.wrapper.classList.toggle(this.CSS.editorEmpty, e.isEditorEmpty);
  }
  /**
   * Check if one of Toolbar is opened
   * Used to prevent global keydowns (for example, Enter) conflicts with Enter-on-toolbar
   *
   * @returns {boolean}
   */
  get someToolbarOpened() {
    const { Toolbar: e, BlockSettings: t, InlineToolbar: r } = this.Editor;
    return !!(t.opened || r.opened || e.toolbox.opened);
  }
  /**
   * Check for some Flipper-buttons is under focus
   */
  get someFlipperButtonFocused() {
    return this.Editor.Toolbar.toolbox.hasFocus() ? !0 : Object.entries(this.Editor).filter(([e, t]) => t.flipper instanceof ut).some(([e, t]) => t.flipper.hasFocus());
  }
  /**
   * Clean editor`s UI
   */
  destroy() {
    this.nodes.holder.innerHTML = "", this.unbindReadOnlyInsensitiveListeners();
  }
  /**
   * Close all Editor's toolbars
   */
  closeAllToolbars() {
    const { Toolbar: e, BlockSettings: t, InlineToolbar: r } = this.Editor;
    t.close(), r.close(), e.toolbox.close();
  }
  /**
   * Check for mobile mode and save the result
   */
  setIsMobile() {
    const e = window.innerWidth < hn;
    e !== this.isMobile && this.eventsDispatcher.emit(We, {
      isEnabled: this.isMobile
    }), this.isMobile = e;
  }
  /**
   * Makes Editor.js interface
   */
  make() {
    this.nodes.holder = g.getHolder(this.config.holder), this.nodes.wrapper = g.make("div", [
      this.CSS.editorWrapper,
      ...this.isRtl ? [this.CSS.editorRtlFix] : []
    ]), this.nodes.redactor = g.make("div", this.CSS.editorZone), this.nodes.holder.offsetWidth < this.contentRect.width && this.nodes.wrapper.classList.add(this.CSS.editorWrapperNarrow), this.nodes.redactor.style.paddingBottom = this.config.minHeight + "px", this.nodes.wrapper.appendChild(this.nodes.redactor), this.nodes.holder.appendChild(this.nodes.wrapper), this.bindReadOnlyInsensitiveListeners();
  }
  /**
   * Appends CSS
   */
  loadStyles() {
    const e = "editor-js-styles";
    if (g.get(e))
      return;
    const t = g.make("style", null, {
      id: e,
      textContent: jc.toString()
    });
    this.config.style && !X(this.config.style) && this.config.style.nonce && t.setAttribute("nonce", this.config.style.nonce), g.prepend(document.head, t);
  }
  /**
   * Adds listeners that should work both in read-only and read-write modes
   */
  bindReadOnlyInsensitiveListeners() {
    this.listeners.on(document, "selectionchange", this.selectionChangeDebounced), this.listeners.on(window, "resize", this.resizeDebouncer, {
      passive: !0
    }), this.listeners.on(this.nodes.redactor, "mousedown", this.documentTouchedListener, {
      capture: !0,
      passive: !0
    }), this.listeners.on(this.nodes.redactor, "touchstart", this.documentTouchedListener, {
      capture: !0,
      passive: !0
    });
  }
  /**
   * Removes listeners that should work both in read-only and read-write modes
   */
  unbindReadOnlyInsensitiveListeners() {
    this.listeners.off(document, "selectionchange", this.selectionChangeDebounced), this.listeners.off(window, "resize", this.resizeDebouncer), this.listeners.off(this.nodes.redactor, "mousedown", this.documentTouchedListener), this.listeners.off(this.nodes.redactor, "touchstart", this.documentTouchedListener);
  }
  /**
   * Adds listeners that should work only in read-only mode
   */
  bindReadOnlySensitiveListeners() {
    this.readOnlyMutableListeners.on(this.nodes.redactor, "click", (e) => {
      this.redactorClicked(e);
    }, !1), this.readOnlyMutableListeners.on(document, "keydown", (e) => {
      this.documentKeydown(e);
    }, !0), this.readOnlyMutableListeners.on(document, "mousedown", (e) => {
      this.documentClicked(e);
    }, !0), this.watchBlockHoveredEvents(), this.enableInputsEmptyMark();
  }
  /**
   * Listen redactor mousemove to emit 'block-hovered' event
   */
  watchBlockHoveredEvents() {
    let e;
    this.readOnlyMutableListeners.on(this.nodes.redactor, "mousemove", $t((t) => {
      const r = t.target.closest(".ce-block");
      this.Editor.BlockSelection.anyBlockSelected || r && e !== r && (e = r, this.eventsDispatcher.emit(Rn, {
        block: this.Editor.BlockManager.getBlockByChildNode(r)
      }));
    }, 20), {
      passive: !0
    });
  }
  /**
   * Unbind events that should work only in read-only mode
   */
  unbindReadOnlySensitiveListeners() {
    this.readOnlyMutableListeners.clearAll();
  }
  /**
   * Resize window handler
   */
  windowResize() {
    this.contentRectCache = null, this.setIsMobile();
  }
  /**
   * All keydowns on document
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  documentKeydown(e) {
    switch (e.keyCode) {
      case A.ENTER:
        this.enterPressed(e);
        break;
      case A.BACKSPACE:
      case A.DELETE:
        this.backspacePressed(e);
        break;
      case A.ESC:
        this.escapePressed(e);
        break;
      default:
        this.defaultBehaviour(e);
        break;
    }
  }
  /**
   * Ignore all other document's keydown events
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  defaultBehaviour(e) {
    const { currentBlock: t } = this.Editor.BlockManager, r = e.target.closest(`.${this.CSS.editorWrapper}`), n = e.altKey || e.ctrlKey || e.metaKey || e.shiftKey;
    if (t !== void 0 && r === null) {
      this.Editor.BlockEvents.keydown(e);
      return;
    }
    r || t && n || (this.Editor.BlockManager.unsetCurrentBlock(), this.Editor.Toolbar.close());
  }
  /**
   * @param {KeyboardEvent} event - keyboard event
   */
  backspacePressed(e) {
    const { BlockManager: t, BlockSelection: r, Caret: n } = this.Editor;
    if (r.anyBlockSelected && !L.isSelectionExists) {
      const i = t.removeSelectedBlocks(), s = t.insertDefaultBlockAtIndex(i, !0);
      n.setToBlock(s, n.positions.START), r.clearSelection(e), e.preventDefault(), e.stopPropagation(), e.stopImmediatePropagation();
    }
  }
  /**
   * Escape pressed
   * If some of Toolbar components are opened, then close it otherwise close Toolbar
   *
   * @param {Event} event - escape keydown event
   */
  escapePressed(e) {
    this.Editor.BlockSelection.clearSelection(e), this.Editor.Toolbar.toolbox.opened ? (this.Editor.Toolbar.toolbox.close(), this.Editor.Caret.setToBlock(this.Editor.BlockManager.currentBlock, this.Editor.Caret.positions.END)) : this.Editor.BlockSettings.opened ? this.Editor.BlockSettings.close() : this.Editor.InlineToolbar.opened ? this.Editor.InlineToolbar.close() : this.Editor.Toolbar.close();
  }
  /**
   * Enter pressed on document
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  enterPressed(e) {
    const { BlockManager: t, BlockSelection: r } = this.Editor;
    if (this.someToolbarOpened)
      return;
    const n = t.currentBlockIndex >= 0;
    if (r.anyBlockSelected && !L.isSelectionExists) {
      r.clearSelection(e), e.preventDefault(), e.stopImmediatePropagation(), e.stopPropagation();
      return;
    }
    if (!this.someToolbarOpened && n && e.target.tagName === "BODY") {
      const i = this.Editor.BlockManager.insert();
      e.preventDefault(), this.Editor.Caret.setToBlock(i), this.Editor.Toolbar.moveAndOpen(i);
    }
    this.Editor.BlockSelection.clearSelection(e);
  }
  /**
   * All clicks on document
   *
   * @param {MouseEvent} event - Click event
   */
  documentClicked(e) {
    var t, r;
    if (!e.isTrusted)
      return;
    const n = e.target;
    this.nodes.holder.contains(n) || L.isAtEditor || (this.Editor.BlockManager.unsetCurrentBlock(), this.Editor.Toolbar.close());
    const i = (t = this.Editor.BlockSettings.nodes.wrapper) == null ? void 0 : t.contains(n), s = (r = this.Editor.Toolbar.nodes.settingsToggler) == null ? void 0 : r.contains(n), a = i || s;
    if (this.Editor.BlockSettings.opened && !a) {
      this.Editor.BlockSettings.close();
      const l = this.Editor.BlockManager.getBlockByChildNode(n);
      this.Editor.Toolbar.moveAndOpen(l);
    }
    this.Editor.BlockSelection.clearSelection(e);
  }
  /**
   * First touch on editor
   * Fired before click
   *
   * Used to change current block — we need to do it before 'selectionChange' event.
   * Also:
   * - Move and show the Toolbar
   * - Set a Caret
   *
   * @param event - touch or mouse event
   */
  documentTouched(e) {
    let t = e.target;
    if (t === this.nodes.redactor) {
      const r = e instanceof MouseEvent ? e.clientX : e.touches[0].clientX, n = e instanceof MouseEvent ? e.clientY : e.touches[0].clientY;
      t = document.elementFromPoint(r, n);
    }
    try {
      this.Editor.BlockManager.setCurrentBlockByChildNode(t);
    } catch {
      this.Editor.RectangleSelection.isRectActivated() || this.Editor.Caret.setToTheLastBlock();
    }
    this.Editor.ReadOnly.isEnabled || this.Editor.Toolbar.moveAndOpen();
  }
  /**
   * All clicks on the redactor zone
   *
   * @param {MouseEvent} event - click event
   * @description
   * - By clicks on the Editor's bottom zone:
   *      - if last Block is empty, set a Caret to this
   *      - otherwise, add a new empty Block and set a Caret to that
   */
  redactorClicked(e) {
    if (!L.isCollapsed)
      return;
    const t = e.target, r = e.metaKey || e.ctrlKey, n = g.getClosestAnchor(t);
    if (n && r) {
      e.stopImmediatePropagation(), e.stopPropagation();
      const i = n.getAttribute("href"), s = cs(i);
      hs(s);
      return;
    }
    this.processBottomZoneClick(e);
  }
  /**
   * Check if user clicks on the Editor's bottom zone:
   *  - set caret to the last block
   *  - or add new empty block
   *
   * @param event - click event
   */
  processBottomZoneClick(e) {
    const t = this.Editor.BlockManager.getBlockByIndex(-1), r = g.offset(t.holder).bottom, n = e.pageY, { BlockSelection: i } = this.Editor;
    if (e.target instanceof Element && e.target.isEqualNode(this.nodes.redactor) && /**
    * If there is cross block selection started, target will be equal to redactor so we need additional check
    */
    !i.anyBlockSelected && /**
    * Prevent caret jumping (to last block) when clicking between blocks
    */
    r < n) {
      e.stopImmediatePropagation(), e.stopPropagation();
      const { BlockManager: s, Caret: a, Toolbar: l } = this.Editor;
      (!s.lastBlock.tool.isDefault || !s.lastBlock.isEmpty) && s.insertAtEnd(), a.setToTheLastBlock(), l.moveAndOpen(s.lastBlock);
    }
  }
  /**
   * Handle selection changes on mobile devices
   * Uses for showing the Inline Toolbar
   */
  selectionChanged() {
    const { CrossBlockSelection: e, BlockSelection: t } = this.Editor, r = L.anchorElement;
    if (e.isCrossBlockSelectionStarted && t.anyBlockSelected && L.get().removeAllRanges(), !r) {
      L.range || this.Editor.InlineToolbar.close();
      return;
    }
    const n = r.closest(`.${ne.CSS.content}`);
    (n === null || n.closest(`.${L.CSS.editorWrapper}`) !== this.nodes.wrapper) && (this.Editor.InlineToolbar.containsNode(r) || this.Editor.InlineToolbar.close(), r.dataset.inlineToolbar !== "true") || (this.Editor.BlockManager.currentBlock || this.Editor.BlockManager.setCurrentBlockByChildNode(r), this.Editor.InlineToolbar.tryToShow(!0));
  }
  /**
   * Editor.js provides and ability to show placeholders for empty contenteditable elements
   *
   * This method watches for input and focus events and toggles 'data-empty' attribute
   * to workaroud the case, when inputs contains only <br>s and has no visible content
   * Then, CSS could rely on this attribute to show placeholders
   */
  enableInputsEmptyMark() {
    function e(t) {
      const r = t.target;
      un(r);
    }
    this.readOnlyMutableListeners.on(this.nodes.wrapper, "input", e), this.readOnlyMutableListeners.on(this.nodes.wrapper, "focusin", e), this.readOnlyMutableListeners.on(this.nodes.wrapper, "focusout", e);
  }
}
const Rc = {
  // API Modules
  BlocksAPI: xs,
  CaretAPI: Es,
  EventsAPI: Ts,
  I18nAPI: Ss,
  API: Bs,
  InlineToolbarAPI: Ms,
  ListenersAPI: _s,
  NotifierAPI: As,
  ReadOnlyAPI: Ps,
  SanitizerAPI: $s,
  SaverAPI: Us,
  SelectionAPI: zs,
  ToolsAPI: Vs,
  StylesAPI: Ws,
  ToolbarAPI: qs,
  TooltipAPI: Gs,
  UiAPI: Js,
  // Toolbar Modules
  BlockSettings: Ea,
  Toolbar: Oa,
  InlineToolbar: Aa,
  // Modules
  BlockEvents: hc,
  BlockManager: fc,
  BlockSelection: gc,
  Caret: mc,
  CrossBlockSelection: vc,
  DragNDrop: bc,
  ModificationsObserver: yc,
  Paste: xc,
  ReadOnly: Cc,
  RectangleSelection: Ue,
  Renderer: Ec,
  Saver: Tc,
  Tools: wi,
  UI: Dc
};
class Hc {
  /**
   * @param {EditorConfig} config - user configuration
   */
  constructor(e) {
    this.moduleInstances = {}, this.eventsDispatcher = new Ye();
    let t, r;
    this.isReady = new Promise((n, i) => {
      t = n, r = i;
    }), Promise.resolve().then(async () => {
      this.configuration = e, this.validate(), this.init(), await this.start(), await this.render();
      const { BlockManager: n, Caret: i, UI: s, ModificationsObserver: a } = this.moduleInstances;
      s.checkEmptiness(), a.enable(), this.configuration.autofocus === !0 && this.configuration.readOnly !== !0 && i.setToBlock(n.blocks[0], i.positions.START), t();
    }).catch((n) => {
      j(`Editor.js is not ready because of ${n}`, "error"), r(n);
    });
  }
  /**
   * Setting for configuration
   *
   * @param {EditorConfig|string} config - Editor's config to set
   */
  set configuration(e) {
    var t, r;
    U(e) ? this.config = {
      ...e
    } : this.config = {
      holder: e
    }, zt(!!this.config.holderId, "config.holderId", "config.holder"), this.config.holderId && !this.config.holder && (this.config.holder = this.config.holderId, this.config.holderId = null), this.config.holder == null && (this.config.holder = "editorjs"), this.config.logLevel || (this.config.logLevel = ln.VERBOSE), rs(this.config.logLevel), zt(!!this.config.initialBlock, "config.initialBlock", "config.defaultBlock"), this.config.defaultBlock = this.config.defaultBlock || this.config.initialBlock || "paragraph", this.config.minHeight = this.config.minHeight !== void 0 ? this.config.minHeight : 300;
    const n = {
      type: this.config.defaultBlock,
      data: {}
    };
    this.config.placeholder = this.config.placeholder || !1, this.config.sanitizer = this.config.sanitizer || {
      p: !0,
      b: !0,
      a: !0
    }, this.config.hideToolbar = this.config.hideToolbar ? this.config.hideToolbar : !1, this.config.tools = this.config.tools || {}, this.config.i18n = this.config.i18n || {}, this.config.data = this.config.data || { blocks: [] }, this.config.onReady = this.config.onReady || (() => {
    }), this.config.onChange = this.config.onChange || (() => {
    }), this.config.inlineToolbar = this.config.inlineToolbar !== void 0 ? this.config.inlineToolbar : !0, (X(this.config.data) || !this.config.data.blocks || this.config.data.blocks.length === 0) && (this.config.data = { blocks: [n] }), this.config.readOnly = this.config.readOnly || !1, (t = this.config.i18n) != null && t.messages && W.setDictionary(this.config.i18n.messages), this.config.i18n.direction = ((r = this.config.i18n) == null ? void 0 : r.direction) || "ltr";
  }
  /**
   * Returns private property
   *
   * @returns {EditorConfig}
   */
  get configuration() {
    return this.config;
  }
  /**
   * Checks for required fields in Editor's config
   */
  validate() {
    const { holderId: e, holder: t } = this.config;
    if (e && t)
      throw Error("«holderId» and «holder» param can't assign at the same time.");
    if (ie(t) && !g.get(t))
      throw Error(`element with ID «${t}» is missing. Pass correct holder's ID.`);
    if (t && U(t) && !g.isElement(t))
      throw Error("«holder» value must be an Element node");
  }
  /**
   * Initializes modules:
   *  - make and save instances
   *  - configure
   */
  init() {
    this.constructModules(), this.configureModules();
  }
  /**
   * Start Editor!
   *
   * Get list of modules that needs to be prepared and return a sequence (Promise)
   *
   * @returns {Promise<void>}
   */
  async start() {
    await [
      "Tools",
      "UI",
      "BlockManager",
      "Paste",
      "BlockSelection",
      "RectangleSelection",
      "CrossBlockSelection",
      "ReadOnly"
    ].reduce(
      (e, t) => e.then(async () => {
        try {
          await this.moduleInstances[t].prepare();
        } catch (r) {
          if (r instanceof gn)
            throw new Error(r.message);
          j(`Module ${t} was skipped because of %o`, "warn", r);
        }
      }),
      Promise.resolve()
    );
  }
  /**
   * Render initial data
   */
  render() {
    return this.moduleInstances.Renderer.render(this.config.data.blocks);
  }
  /**
   * Make modules instances and save it to the @property this.moduleInstances
   */
  constructModules() {
    Object.entries(Rc).forEach(([e, t]) => {
      try {
        this.moduleInstances[e] = new t({
          config: this.configuration,
          eventsDispatcher: this.eventsDispatcher
        });
      } catch (r) {
        j("[constructModules]", `Module ${e} skipped because`, "error", r);
      }
    });
  }
  /**
   * Modules instances configuration:
   *  - pass other modules to the 'state' property
   *  - ...
   */
  configureModules() {
    for (const e in this.moduleInstances)
      Object.prototype.hasOwnProperty.call(this.moduleInstances, e) && (this.moduleInstances[e].state = this.getModulesDiff(e));
  }
  /**
   * Return modules without passed name
   *
   * @param {string} name - module for witch modules difference should be calculated
   */
  getModulesDiff(e) {
    const t = {};
    for (const r in this.moduleInstances)
      r !== e && (t[r] = this.moduleInstances[r]);
    return t;
  }
}
/**
 * Editor.js
 *
 * @license Apache-2.0
 * @see Editor.js <https://editorjs.io>
 * @author CodeX Team <https://codex.so>
 */
class Fc {
  /** Editor version */
  static get version() {
    return "2.31.6";
  }
  /**
   * @param {EditorConfig|string|undefined} [configuration] - user configuration
   */
  constructor(e) {
    let t = () => {
    };
    U(e) && R(e.onReady) && (t = e.onReady);
    const r = new Hc(e);
    this.isReady = r.isReady.then(() => {
      this.exportAPI(r), t();
    });
  }
  /**
   * Export external API methods
   *
   * @param {Core} editor — Editor's instance
   */
  exportAPI(e) {
    const t = ["configuration"], r = () => {
      Object.values(e.moduleInstances).forEach((n) => {
        R(n.destroy) && n.destroy(), n.listeners.removeAll();
      }), Zs(), e = null;
      for (const n in this)
        Object.prototype.hasOwnProperty.call(this, n) && delete this[n];
      Object.setPrototypeOf(this, null);
    };
    t.forEach((n) => {
      this[n] = e[n];
    }), this.destroy = r, Object.setPrototypeOf(this, e.moduleInstances.API.methods), delete this.exportAPI, Object.entries({
      blocks: {
        clear: "clear",
        render: "render"
      },
      caret: {
        focus: "focus"
      },
      events: {
        on: "on",
        off: "off",
        emit: "emit"
      },
      saver: {
        save: "save"
      }
    }).forEach(([n, i]) => {
      Object.entries(i).forEach(([s, a]) => {
        this[a] = e.moduleInstances.API.methods[n][s];
      });
    });
  }
}
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".ce-header{padding:.6em 0 3px;margin:0;line-height:1.25em;outline:none}.ce-header p,.ce-header div{padding:0!important;margin:0!important}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const $c = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19 17V10.2135C19 10.1287 18.9011 10.0824 18.836 10.1367L16 12.5"/></svg>', Uc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 11C16 10 19 9.5 19 12C19 13.9771 16.0684 13.9997 16.0012 16.8981C15.9999 16.9533 16.0448 17 16.1 17L19.3 17"/></svg>', zc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 11C16 10.5 16.8323 10 17.6 10C18.3677 10 19.5 10.311 19.5 11.5C19.5 12.5315 18.7474 12.9022 18.548 12.9823C18.5378 12.9864 18.5395 13.0047 18.5503 13.0063C18.8115 13.0456 20 13.3065 20 14.8C20 16 19.5 17 17.8 17C17.8 17 16 17 16 16.3"/></svg>', Vc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 10L15.2834 14.8511C15.246 14.9178 15.294 15 15.3704 15C16.8489 15 18.7561 15 20.2 15M19 17C19 15.7187 19 14.8813 19 13.6"/></svg>', Wc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 15.9C16 15.9 16.3768 17 17.8 17C19.5 17 20 15.6199 20 14.7C20 12.7323 17.6745 12.0486 16.1635 12.9894C16.094 13.0327 16 12.9846 16 12.9027V10.1C16 10.0448 16.0448 10 16.1 10H19.8"/></svg>', qc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19.5 10C16.5 10.5 16 13.3285 16 15M16 15V15C16 16.1046 16.8954 17 18 17H18.3246C19.3251 17 20.3191 16.3492 20.2522 15.3509C20.0612 12.4958 16 12.6611 16 15Z"/></svg>', Kc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9 7L9 12M9 17V12M9 12L15 12M15 7V12M15 17L15 12"/></svg>';
/**
 * Header block for the Editor.js.
 *
 * @author CodeX (team@ifmo.su)
 * @copyright CodeX 2018
 * @license MIT
 * @version 2.0.0
 */
let Yc = class {
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this.api = r, this.readOnly = n, this._config = t ?? null, this._data = this.normalizeData(e), this._element = this.getTag();
  }
  /**
   * Styles
   */
  get _CSS() {
    return {
      block: this.api.styles.block,
      wrapper: "ce-header"
    };
  }
  /**
   * Check if data is valid
   * 
   * @param {any} data - data to check
   * @returns {data is HeaderData}
   * @private
   */
  isHeaderData(e) {
    return e.text !== void 0;
  }
  /**
   * Normalize input data
   *
   * @param {HeaderData} data - saved data to process
   *
   * @returns {HeaderData}
   * @private
   */
  normalizeData(e) {
    const t = { text: "", level: this.defaultLevel.number };
    return this.isHeaderData(e) && (t.text = e.text || "", e.level !== void 0 && !isNaN(parseInt(e.level.toString())) && (t.level = parseInt(e.level.toString()))), t;
  }
  /**
   * Return Tool's view
   *
   * @returns {HTMLHeadingElement}
   * @public
   */
  render() {
    return this._element;
  }
  /**
   * Returns header block tunes config
   *
   * @returns {Array}
   */
  renderSettings() {
    return this.levels.map((e) => ({
      icon: e.svg,
      label: this.api.i18n.t(`Heading ${e.number}`),
      onActivate: () => this.setLevel(e.number),
      closeOnActivate: !0,
      isActive: this.currentLevel.number === e.number,
      render: () => document.createElement("div")
    }));
  }
  /**
   * Callback for Block's settings buttons
   *
   * @param {number} level - level to set
   */
  setLevel(e) {
    this.data = {
      level: e,
      text: this.data.text
    };
  }
  /**
   * Method that specified how to merge two Text blocks.
   * Called by Editor.js by backspace at the beginning of the Block
   *
   * @param {HeaderData} data - saved data to merger with current block
   * @public
   */
  merge(e) {
    this._element.insertAdjacentHTML("beforeend", e.text);
  }
  /**
   * Validate Text block data:
   * - check for emptiness
   *
   * @param {HeaderData} blockData — data received after saving
   * @returns {boolean} false if saved data is not correct, otherwise true
   * @public
   */
  validate(e) {
    return e.text.trim() !== "";
  }
  /**
   * Extract Tool's data from the view
   *
   * @param {HTMLHeadingElement} toolsContent - Text tools rendered view
   * @returns {HeaderData} - saved data
   * @public
   */
  save(e) {
    return {
      text: e.innerHTML,
      level: this.currentLevel.number
    };
  }
  /**
   * Allow Header to be converted to/from other blocks
   */
  static get conversionConfig() {
    return {
      export: "text",
      // use 'text' property for other blocks
      import: "text"
      // fill 'text' property from other block's export string
    };
  }
  /**
   * Sanitizer Rules
   */
  static get sanitize() {
    return {
      level: !1,
      text: {}
    };
  }
  /**
   * Returns true to notify core that read-only is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Get current Tools`s data
   *
   * @returns {HeaderData} Current data
   * @private
   */
  get data() {
    return this._data.text = this._element.innerHTML, this._data.level = this.currentLevel.number, this._data;
  }
  /**
   * Store data in plugin:
   * - at the this._data property
   * - at the HTML
   *
   * @param {HeaderData} data — data to set
   * @private
   */
  set data(e) {
    if (this._data = this.normalizeData(e), e.level !== void 0 && this._element.parentNode) {
      const t = this.getTag();
      t.innerHTML = this._element.innerHTML, this._element.parentNode.replaceChild(t, this._element), this._element = t;
    }
    e.text !== void 0 && (this._element.innerHTML = this._data.text || "");
  }
  /**
   * Get tag for target level
   * By default returns second-leveled header
   *
   * @returns {HTMLElement}
   */
  getTag() {
    var e;
    const t = document.createElement(this.currentLevel.tag);
    return t.innerHTML = this._data.text || "", t.classList.add(this._CSS.wrapper), t.contentEditable = this.readOnly ? "false" : "true", t.dataset.placeholder = this.api.i18n.t(((e = this._config) == null ? void 0 : e.placeholder) || ""), t;
  }
  /**
   * Get current level
   *
   * @returns {level}
   */
  get currentLevel() {
    let e = this.levels.find((t) => t.number === this._data.level);
    return e || (e = this.defaultLevel), e;
  }
  /**
   * Return default level
   *
   * @returns {level}
   */
  get defaultLevel() {
    var e;
    if ((e = this._config) != null && e.defaultLevel) {
      const t = this.levels.find((r) => {
        var n;
        return r.number === ((n = this._config) == null ? void 0 : n.defaultLevel);
      });
      if (t)
        return t;
      console.warn("(ง'̀-'́)ง Heading Tool: the default level specified was not found in available levels");
    }
    return this.levels[1];
  }
  /**
   * @typedef {object} level
   * @property {number} number - level number
   * @property {string} tag - tag corresponds with level number
   * @property {string} svg - icon
   */
  /**
   * Available header levels
   *
   * @returns {level[]}
   */
  get levels() {
    var e;
    const t = [
      {
        number: 1,
        tag: "H1",
        svg: $c
      },
      {
        number: 2,
        tag: "H2",
        svg: Uc
      },
      {
        number: 3,
        tag: "H3",
        svg: zc
      },
      {
        number: 4,
        tag: "H4",
        svg: Vc
      },
      {
        number: 5,
        tag: "H5",
        svg: Wc
      },
      {
        number: 6,
        tag: "H6",
        svg: qc
      }
    ];
    return (e = this._config) != null && e.levels ? t.filter(
      (r) => {
        var n;
        return (n = this._config) == null ? void 0 : n.levels.includes(r.number);
      }
    ) : t;
  }
  /**
   * Handle H1-H6 tags on paste to substitute it with header Tool
   *
   * @param {PasteEvent} event - event with pasted content
   */
  onPaste(e) {
    var t, r;
    const n = e.detail;
    if ("data" in n) {
      const i = n.data;
      let s = this.defaultLevel.number;
      switch (i.tagName) {
        case "H1":
          s = 1;
          break;
        case "H2":
          s = 2;
          break;
        case "H3":
          s = 3;
          break;
        case "H4":
          s = 4;
          break;
        case "H5":
          s = 5;
          break;
        case "H6":
          s = 6;
          break;
      }
      (t = this._config) != null && t.levels && (s = (r = this._config) == null ? void 0 : r.levels.reduce((a, l) => Math.abs(l - s) < Math.abs(a - s) ? l : a)), this.data = {
        level: s,
        text: i.innerHTML
      };
    }
  }
  /**
   * Used by Editor.js paste handling API.
   * Provides configuration to handle H1-H6 tags.
   *
   * @returns {{handler: (function(HTMLElement): {text: string}), tags: string[]}}
   */
  static get pasteConfig() {
    return {
      tags: ["H1", "H2", "H3", "H4", "H5", "H6"]
    };
  }
  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   *
   * @returns {{icon: string, title: string}}
   */
  static get toolbox() {
    return {
      icon: Kc,
      title: "Heading"
    };
  }
};
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".ce-rawtool__textarea{min-height:200px;resize:vertical;border-radius:8px;border:0;background-color:#1e2128;font-family:Menlo,Monaco,Consolas,Courier New,monospace;font-size:12px;line-height:1.6;letter-spacing:-.2px;color:#a1a7b6;overscroll-behavior:contain}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const Xc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16.6954 5C17.912 5 18.8468 6.07716 18.6755 7.28165L17.426 16.0659C17.3183 16.8229 16.7885 17.4522 16.061 17.6873L12.6151 18.8012C12.2152 18.9304 11.7848 18.9304 11.3849 18.8012L7.93898 17.6873C7.21148 17.4522 6.6817 16.8229 6.57403 16.0659L5.32454 7.28165C5.15322 6.07716 6.088 5 7.30461 5H16.6954Z"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 8.4H9L9.42857 11.7939H14.5714L14.3571 13.2788L14.1429 14.7636L12 15.4L9.85714 14.7636L9.77143 14.3394"/></svg>';
/**
 * Raw HTML Tool for CodeX Editor
 *
 * @author CodeX (team@codex.so)
 * @copyright CodeX 2018
 * @license The MIT License (MIT)
 */
class er {
  /**
   * Notify core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Should this tool be displayed at the Editor's Toolbox
   *
   * @returns {boolean}
   * @public
   */
  static get displayInToolbox() {
    return !0;
  }
  /**
   * Allow to press Enter inside the RawTool textarea
   *
   * @returns {boolean}
   * @public
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   *
   * @returns {{icon: string, title: string}}
   */
  static get toolbox() {
    return {
      icon: Xc,
      title: "Raw HTML"
    };
  }
  /**
   * @typedef {object} RawData — plugin saved data
   * @param {string} html - previously saved HTML code
   * @property
   */
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {RawData} data — previously saved HTML data
   * @param {object} config - user config for Tool
   * @param {object} api - CodeX Editor API
   * @param {boolean} readOnly - read-only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this.api = r, this.readOnly = n, this.placeholder = r.i18n.t(t.placeholder || er.DEFAULT_PLACEHOLDER), this.CSS = {
      baseClass: this.api.styles.block,
      input: this.api.styles.input,
      wrapper: "ce-rawtool",
      textarea: "ce-rawtool__textarea"
    }, this.data = {
      html: e.html || ""
    }, this.textarea = null, this.resizeDebounce = null;
  }
  /**
   * Return Tool's view
   *
   * @returns {HTMLDivElement} this.element - RawTool's wrapper
   * @public
   */
  render() {
    const e = document.createElement("div"), t = 100;
    return this.textarea = document.createElement("textarea"), e.classList.add(this.CSS.baseClass, this.CSS.wrapper), this.textarea.classList.add(this.CSS.textarea, this.CSS.input), this.textarea.textContent = this.data.html, this.textarea.placeholder = this.placeholder, this.readOnly ? this.textarea.disabled = !0 : this.textarea.addEventListener("input", () => {
      this.onInput();
    }), e.appendChild(this.textarea), setTimeout(() => {
      this.resize();
    }, t), e;
  }
  /**
   * Extract Tool's data from the view
   *
   * @param {HTMLDivElement} rawToolsWrapper - RawTool's wrapper, containing textarea with raw HTML code
   * @returns {RawData} - raw HTML code
   * @public
   */
  save(e) {
    return {
      html: e.querySelector("textarea").value
    };
  }
  /**
   * Default placeholder for RawTool's textarea
   *
   * @public
   * @returns {string}
   */
  static get DEFAULT_PLACEHOLDER() {
    return "Enter HTML code";
  }
  /**
   * Automatic sanitize config
   */
  static get sanitize() {
    return {
      html: !0
      // Allow HTML tags
    };
  }
  /**
   * Textarea change event
   *
   * @returns {void}
   */
  onInput() {
    this.resizeDebounce && clearTimeout(this.resizeDebounce), this.resizeDebounce = setTimeout(() => {
      this.resize();
    }, 200);
  }
  /**
   * Resize textarea to fit whole height
   *
   * @returns {void}
   */
  resize() {
    this.textarea.style.height = "auto", this.textarea.style.height = this.textarea.scrollHeight + "px";
  }
}
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode('.cdx-checklist{gap:6px;display:flex;flex-direction:column}.cdx-checklist__item{display:flex;box-sizing:content-box;align-items:flex-start}.cdx-checklist__item-text{outline:none;flex-grow:1;line-height:1.57em}.cdx-checklist__item-checkbox{width:22px;height:22px;display:flex;align-items:center;margin-right:8px;margin-top:calc(.785em - 11px);cursor:pointer}.cdx-checklist__item-checkbox svg{opacity:0;height:20px;width:20px;position:absolute;left:-1px;top:-1px;max-height:20px}@media (hover: hover){.cdx-checklist__item-checkbox:not(.cdx-checklist__item-checkbox--no-hover):hover .cdx-checklist__item-checkbox-check svg{opacity:1}}.cdx-checklist__item-checkbox-check{cursor:pointer;display:inline-block;flex-shrink:0;position:relative;width:20px;height:20px;box-sizing:border-box;margin-left:0;border-radius:5px;border:1px solid #C9C9C9;background:#fff}.cdx-checklist__item-checkbox-check:before{content:"";position:absolute;top:0;right:0;bottom:0;left:0;border-radius:100%;background-color:#369fff;visibility:hidden;pointer-events:none;transform:scale(1);transition:transform .4s ease-out,opacity .4s}@media (hover: hover){.cdx-checklist__item--checked .cdx-checklist__item-checkbox:not(.cdx-checklist__item--checked .cdx-checklist__item-checkbox--no-hover):hover .cdx-checklist__item-checkbox-check{background:#0059AB;border-color:#0059ab}}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check{background:#369FFF;border-color:#369fff}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check svg{opacity:1}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check svg path{stroke:#fff}.cdx-checklist__item--checked .cdx-checklist__item-checkbox-check:before{opacity:0;visibility:visible;transform:scale(2.5)}')), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const Zc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 12L10.4884 15.8372C10.5677 15.9245 10.705 15.9245 10.7844 15.8372L17 9"/></svg>', Gc = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9.2 12L11.0586 13.8586C11.1367 13.9367 11.2633 13.9367 11.3414 13.8586L14.7 10.5"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>';
function Qr() {
  const o = document.activeElement, e = window.getSelection().getRangeAt(0), t = e.cloneRange();
  return t.selectNodeContents(o), t.setStart(e.endContainer, e.endOffset), t.extractContents();
}
function Jc(o) {
  const e = document.createElement("div");
  return e.appendChild(o), e.innerHTML;
}
function Ne(o, e = null, t = {}) {
  const r = document.createElement(o);
  Array.isArray(e) ? r.classList.add(...e) : e && r.classList.add(e);
  for (const n in t)
    r[n] = t[n];
  return r;
}
function en(o) {
  return o.innerHTML.replace("<br>", " ").trim();
}
function tn(o, e = !1, t = void 0) {
  const r = document.createRange(), n = window.getSelection();
  r.selectNodeContents(o), t !== void 0 && (r.setStart(o, t), r.setEnd(o, t)), r.collapse(e), n.removeAllRanges(), n.addRange(r);
}
Element.prototype.matches || (Element.prototype.matches = Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector);
Element.prototype.closest || (Element.prototype.closest = function(o) {
  let e = this;
  if (!document.documentElement.contains(e))
    return null;
  do {
    if (e.matches(o))
      return e;
    e = e.parentElement || e.parentNode;
  } while (e !== null && e.nodeType === 1);
  return null;
});
class Qc {
  /**
   * Notify core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Allow to use native Enter behaviour
   *
   * @returns {boolean}
   * @public
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   *
   * @returns {{icon: string, title: string}}
   */
  static get toolbox() {
    return {
      icon: Gc,
      title: "Checklist"
    };
  }
  /**
   * Allow Checkbox Tool to be converted to/from other block
   *
   * @returns {{export: Function, import: Function}}
   */
  static get conversionConfig() {
    return {
      /**
       * To create exported string from the checkbox, concatenate items by dot-symbol.
       *
       * @param {ChecklistData} data - checklist data to create a string from that
       * @returns {string}
       */
      export: (e) => e.items.map(({ text: t }) => t).join(". "),
      /**
       * To create a checklist from other block's string, just put it at the first item
       *
       * @param {string} string - string to create list tool data from that
       * @returns {ChecklistData}
       */
      import: (e) => ({
        items: [
          {
            text: e,
            checked: !1
          }
        ]
      })
    };
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} options - block constructor options
   * @param {ChecklistData} options.data - previously saved data
   * @param {object} options.config - user config for Tool
   * @param {object} options.api - Editor.js API
   * @param {boolean} options.readOnly - read only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this._elements = {
      wrapper: null,
      items: []
    }, this.readOnly = n, this.api = r, this.data = e || {};
  }
  /**
   * Returns checklist tag with items
   *
   * @returns {Element}
   */
  render() {
    return this._elements.wrapper = Ne("div", [this.CSS.baseBlock, this.CSS.wrapper]), this.data.items || (this.data.items = [
      {
        text: "",
        checked: !1
      }
    ]), this.data.items.forEach((e) => {
      const t = this.createChecklistItem(e);
      this._elements.wrapper.appendChild(t);
    }), this.readOnly ? this._elements.wrapper : (this._elements.wrapper.addEventListener("keydown", (e) => {
      const [t, r] = [13, 8];
      switch (e.keyCode) {
        case t:
          this.enterPressed(e);
          break;
        case r:
          this.backspace(e);
          break;
      }
    }, !1), this._elements.wrapper.addEventListener("click", (e) => {
      this.toggleCheckbox(e);
    }), this._elements.wrapper);
  }
  /**
   * Return Checklist data
   *
   * @returns {ChecklistData}
   */
  save() {
    let e = this.items.map((t) => {
      const r = this.getItemInput(t);
      return {
        text: en(r),
        checked: t.classList.contains(this.CSS.itemChecked)
      };
    });
    return e = e.filter((t) => t.text.trim().length !== 0), {
      items: e
    };
  }
  /**
   * Validate data: check if Checklist has items
   *
   * @param {ChecklistData} savedData — data received after saving
   * @returns {boolean} false if saved data is not correct, otherwise true
   * @public
   */
  validate(e) {
    return !!e.items.length;
  }
  /**
   * Toggle checklist item state
   *
   * @param {MouseEvent} event - click
   * @returns {void}
   */
  toggleCheckbox(e) {
    const t = e.target.closest(`.${this.CSS.item}`), r = t.querySelector(`.${this.CSS.checkboxContainer}`);
    r.contains(e.target) && (t.classList.toggle(this.CSS.itemChecked), r.classList.add(this.CSS.noHover), r.addEventListener("mouseleave", () => this.removeSpecialHoverBehavior(r), { once: !0 }));
  }
  /**
   * Create Checklist items
   *
   * @param {ChecklistItem} item - data.item
   * @returns {Element} checkListItem - new element of checklist
   */
  createChecklistItem(e = {}) {
    const t = Ne("div", this.CSS.item), r = Ne("span", this.CSS.checkbox), n = Ne("div", this.CSS.checkboxContainer), i = Ne("div", this.CSS.textField, {
      innerHTML: e.text ? e.text : "",
      contentEditable: !this.readOnly
    });
    return e.checked && t.classList.add(this.CSS.itemChecked), r.innerHTML = Zc, n.appendChild(r), t.appendChild(n), t.appendChild(i), t;
  }
  /**
   * Append new elements to the list by pressing Enter
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  enterPressed(e) {
    e.preventDefault();
    const t = this.items, r = document.activeElement.closest(`.${this.CSS.item}`);
    if (t.indexOf(r) === t.length - 1 && en(this.getItemInput(r)).length === 0) {
      const a = this.api.blocks.getCurrentBlockIndex();
      r.remove(), this.api.blocks.insert(), this.api.caret.setToBlock(a + 1);
      return;
    }
    const n = Qr(), i = Jc(n), s = this.createChecklistItem({
      text: i,
      checked: !1
    });
    this._elements.wrapper.insertBefore(s, r.nextSibling), tn(this.getItemInput(s), !0);
  }
  /**
   * Handle backspace
   *
   * @param {KeyboardEvent} event - keyboard event
   */
  backspace(e) {
    const t = e.target.closest(`.${this.CSS.item}`), r = this.items.indexOf(t), n = this.items[r - 1];
    if (!n || window.getSelection().focusOffset !== 0)
      return;
    e.preventDefault();
    const i = Qr(), s = this.getItemInput(n), a = s.childNodes.length;
    s.appendChild(i), tn(s, void 0, a), t.remove();
  }
  /**
   * Styles
   *
   * @private
   * @returns {object<string>}
   */
  get CSS() {
    return {
      baseBlock: this.api.styles.block,
      wrapper: "cdx-checklist",
      item: "cdx-checklist__item",
      itemChecked: "cdx-checklist__item--checked",
      noHover: "cdx-checklist__item-checkbox--no-hover",
      checkbox: "cdx-checklist__item-checkbox-check",
      textField: "cdx-checklist__item-text",
      checkboxContainer: "cdx-checklist__item-checkbox"
    };
  }
  /**
   * Return all items elements
   *
   * @returns {Element[]}
   */
  get items() {
    return Array.from(this._elements.wrapper.querySelectorAll(`.${this.CSS.item}`));
  }
  /**
   * Removes class responsible for special hover behavior on an item
   * 
   * @private
   * @param {Element} el - item wrapper
   * @returns {Element}
   */
  removeSpecialHoverBehavior(e) {
    e.classList.remove(this.CSS.noHover);
  }
  /**
   * Find and return item's content editable element
   *
   * @private
   * @param {Element} el - item wrapper
   * @returns {Element}
   */
  getItemInput(e) {
    return e.querySelector(`.${this.CSS.textField}`);
  }
}
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".cdx-list{margin:0;padding-left:40px;outline:none}.cdx-list__item{padding:5.5px 0 5.5px 3px;line-height:1.6em}.cdx-list--unordered{list-style:disc}.cdx-list--ordered{list-style:decimal}.cdx-list-settings{display:flex}.cdx-list-settings .cdx-settings-button{width:50%}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const on = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="9" x2="19" y1="7" y2="7" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="9" x2="19" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="9" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 17H4.99002"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 12H4.99002"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 7H4.99002"/></svg>', ed = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="12" x2="19" y1="7" y2="7" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="12" x2="19" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="12" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.79999 14L7.79999 7.2135C7.79999 7.12872 7.7011 7.0824 7.63597 7.13668L4.79999 9.5"/></svg>';
let td = class {
  /**
   * Notify core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Allow to use native Enter behaviour
   *
   * @returns {boolean}
   * @public
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   *
   * @returns {{icon: string, title: string}}
   */
  static get toolbox() {
    return {
      icon: on,
      title: "List"
    };
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} params - tool constructor options
   * @param {ListData} params.data - previously saved data
   * @param {object} params.config - user config for Tool
   * @param {object} params.api - Editor.js API
   * @param {boolean} params.readOnly - read-only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this._elements = {
      wrapper: null
    }, this.api = r, this.readOnly = n, this.settings = [
      {
        name: "unordered",
        label: this.api.i18n.t("Unordered"),
        icon: on,
        default: t.defaultStyle === "unordered" || !1
      },
      {
        name: "ordered",
        label: this.api.i18n.t("Ordered"),
        icon: ed,
        default: t.defaultStyle === "ordered" || !0
      }
    ], this._data = {
      style: this.settings.find((i) => i.default === !0).name,
      items: []
    }, this.data = e;
  }
  /**
   * Returns list tag with items
   *
   * @returns {Element}
   * @public
   */
  render() {
    return this._elements.wrapper = this.makeMainTag(this._data.style), this._data.items.length ? this._data.items.forEach((e) => {
      this._elements.wrapper.appendChild(this._make("li", this.CSS.item, {
        innerHTML: e
      }));
    }) : this._elements.wrapper.appendChild(this._make("li", this.CSS.item)), this.readOnly || this._elements.wrapper.addEventListener("keydown", (e) => {
      const [t, r] = [13, 8];
      switch (e.keyCode) {
        case t:
          this.getOutofList(e);
          break;
        case r:
          this.backspace(e);
          break;
      }
    }, !1), this._elements.wrapper;
  }
  /**
   * @returns {ListData}
   * @public
   */
  save() {
    return this.data;
  }
  /**
   * Allow List Tool to be converted to/from other block
   *
   * @returns {{export: Function, import: Function}}
   */
  static get conversionConfig() {
    return {
      /**
       * To create exported string from list, concatenate items by dot-symbol.
       *
       * @param {ListData} data - list data to create a string from thats
       * @returns {string}
       */
      export: (e) => e.items.join(". "),
      /**
       * To create a list from other block's string, just put it at the first item
       *
       * @param {string} string - string to create list tool data from that
       * @returns {ListData}
       */
      import: (e) => ({
        items: [e],
        style: "unordered"
      })
    };
  }
  /**
   * Sanitizer rules
   *
   * @returns {object}
   */
  static get sanitize() {
    return {
      style: {},
      items: {
        br: !0
      }
    };
  }
  /**
   * Settings
   *
   * @public
   * @returns {Array}
   */
  renderSettings() {
    return this.settings.map((e) => ({
      ...e,
      isActive: this._data.style === e.name,
      closeOnActivate: !0,
      onActivate: () => this.toggleTune(e.name)
    }));
  }
  /**
   * On paste callback that is fired from Editor
   *
   * @param {PasteEvent} event - event with pasted data
   */
  onPaste(e) {
    const t = e.detail.data;
    this.data = this.pasteHandler(t);
  }
  /**
   * List Tool on paste configuration
   *
   * @public
   */
  static get pasteConfig() {
    return {
      tags: ["OL", "UL", "LI"]
    };
  }
  /**
   * Creates main <ul> or <ol> tag depended on style
   *
   * @param {string} style - 'ordered' or 'unordered'
   * @returns {HTMLOListElement|HTMLUListElement}
   */
  makeMainTag(e) {
    const t = e === "ordered" ? this.CSS.wrapperOrdered : this.CSS.wrapperUnordered, r = e === "ordered" ? "ol" : "ul";
    return this._make(r, [this.CSS.baseBlock, this.CSS.wrapper, t], {
      contentEditable: !this.readOnly
    });
  }
  /**
   * Toggles List style
   *
   * @param {string} style - 'ordered'|'unordered'
   */
  toggleTune(e) {
    const t = this.makeMainTag(e);
    for (; this._elements.wrapper.hasChildNodes(); )
      t.appendChild(this._elements.wrapper.firstChild);
    this._elements.wrapper.replaceWith(t), this._elements.wrapper = t, this._data.style = e;
  }
  /**
   * Styles
   *
   * @private
   */
  get CSS() {
    return {
      baseBlock: this.api.styles.block,
      wrapper: "cdx-list",
      wrapperOrdered: "cdx-list--ordered",
      wrapperUnordered: "cdx-list--unordered",
      item: "cdx-list__item"
    };
  }
  /**
   * List data setter
   *
   * @param {ListData} listData
   */
  set data(e) {
    e || (e = {}), this._data.style = e.style || this.settings.find((r) => r.default === !0).name, this._data.items = e.items || [];
    const t = this._elements.wrapper;
    t && t.parentNode.replaceChild(this.render(), t);
  }
  /**
   * Return List data
   *
   * @returns {ListData}
   */
  get data() {
    this._data.items = [];
    const e = this._elements.wrapper.querySelectorAll(`.${this.CSS.item}`);
    for (let t = 0; t < e.length; t++)
      e[t].innerHTML.replace("<br>", " ").trim() && this._data.items.push(e[t].innerHTML);
    return this._data;
  }
  /**
   * Helper for making Elements with attributes
   *
   * @param  {string} tagName           - new Element tag name
   * @param  {Array|string} classNames  - list or name of CSS classname(s)
   * @param  {object} attributes        - any attributes
   * @returns {Element}
   */
  _make(e, t = null, r = {}) {
    const n = document.createElement(e);
    Array.isArray(t) ? n.classList.add(...t) : t && n.classList.add(t);
    for (const i in r)
      n[i] = r[i];
    return n;
  }
  /**
   * Returns current List item by the caret position
   *
   * @returns {Element}
   */
  get currentItem() {
    let e = window.getSelection().anchorNode;
    return e.nodeType !== Node.ELEMENT_NODE && (e = e.parentNode), e.closest(`.${this.CSS.item}`);
  }
  /**
   * Get out from List Tool
   * by Enter on the empty last item
   *
   * @param {KeyboardEvent} event
   */
  getOutofList(e) {
    const t = this._elements.wrapper.querySelectorAll("." + this.CSS.item);
    if (t.length < 2)
      return;
    const r = t[t.length - 1], n = this.currentItem;
    n === r && !r.textContent.trim().length && (n.parentElement.removeChild(n), this.api.blocks.insert(), this.api.caret.setToBlock(this.api.blocks.getCurrentBlockIndex()), e.preventDefault(), e.stopPropagation());
  }
  /**
   * Handle backspace
   *
   * @param {KeyboardEvent} event
   */
  backspace(e) {
    const t = this._elements.wrapper.querySelectorAll("." + this.CSS.item), r = t[0];
    r && t.length < 2 && !r.innerHTML.replace("<br>", " ").trim() && e.preventDefault();
  }
  /**
   * Select LI content by CMD+A
   *
   * @param {KeyboardEvent} event
   */
  selectItem(e) {
    e.preventDefault();
    const t = window.getSelection(), r = t.anchorNode.parentNode, n = r.closest("." + this.CSS.item), i = new Range();
    i.selectNodeContents(n), t.removeAllRanges(), t.addRange(i);
  }
  /**
   * Handle UL, OL and LI tags paste and returns List data
   *
   * @param {HTMLUListElement|HTMLOListElement|HTMLLIElement} element
   * @returns {ListData}
   */
  pasteHandler(e) {
    const { tagName: t } = e;
    let r;
    switch (t) {
      case "OL":
        r = "ordered";
        break;
      case "UL":
      case "LI":
        r = "unordered";
    }
    const n = {
      style: r,
      items: []
    };
    if (t === "LI")
      n.items = [e.innerHTML];
    else {
      const i = Array.from(e.querySelectorAll("LI"));
      n.items = i.map((s) => s.innerHTML).filter((s) => !!s.trim());
    }
    return n;
  }
};
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode('.embed-tool--loading .embed-tool__caption{display:none}.embed-tool--loading .embed-tool__preloader{display:block}.embed-tool--loading .embed-tool__content{display:none}.embed-tool__preloader{display:none;position:relative;height:200px;box-sizing:border-box;border-radius:5px;border:1px solid #e6e9eb}.embed-tool__preloader:before{content:"";position:absolute;z-index:3;left:50%;top:50%;width:30px;height:30px;margin-top:-25px;margin-left:-15px;border-radius:50%;border:2px solid #cdd1e0;border-top-color:#388ae5;box-sizing:border-box;animation:embed-preloader-spin 2s infinite linear}.embed-tool__url{position:absolute;bottom:20px;left:50%;transform:translate(-50%);max-width:250px;color:#7b7e89;font-size:11px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}.embed-tool__content{width:100%}.embed-tool__caption{margin-top:7px}.embed-tool__caption[contentEditable=true][data-placeholder]:before{position:absolute;content:attr(data-placeholder);color:#707684;font-weight:400;opacity:0}.embed-tool__caption[contentEditable=true][data-placeholder]:empty:before{opacity:1}.embed-tool__caption[contentEditable=true][data-placeholder]:empty:focus:before{opacity:0}@keyframes embed-preloader-spin{0%{transform:rotate(0)}to{transform:rotate(360deg)}}')), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const od = {
  vimeo: {
    regex: /(?:http[s]?:\/\/)?(?:www.)?(?:player.)?vimeo\.co(?:.+\/([^\/]\d+)(?:#t=[\d]+)?s?$)/,
    embedUrl: "https://player.vimeo.com/video/<%= remote_id %>?title=0&byline=0",
    html: '<iframe style="width:100%;" height="320" frameborder="0"></iframe>',
    height: 320,
    width: 580
  },
  youtube: {
    regex: /(?:https?:\/\/)?(?:www\.)?(?:(?:youtu\.be\/)|(?:youtube\.com)\/(?:v\/|u\/\w\/|embed\/|watch))(?:(?:\?v=)?([^#&?=]*))?((?:[?&]\w*=\w*)*)/,
    embedUrl: "https://www.youtube.com/embed/<%= remote_id %>",
    html: '<iframe style="width:100%;" height="320" frameborder="0" allowfullscreen></iframe>',
    height: 320,
    width: 580,
    id: ([o, e]) => {
      if (!e && o)
        return o;
      const t = {
        start: "start",
        end: "end",
        t: "start",
        // eslint-disable-next-line camelcase
        time_continue: "start",
        list: "list"
      };
      let r = e.slice(1).split("&").map((n) => {
        const [i, s] = n.split("=");
        return !o && i === "v" ? (o = s, null) : !t[i] || s === "LL" || s.startsWith("RDMM") || s.startsWith("FL") ? null : `${t[i]}=${s}`;
      }).filter((n) => !!n);
      return o + "?" + r.join("&");
    }
  },
  coub: {
    regex: /https?:\/\/coub\.com\/view\/([^\/\?\&]+)/,
    embedUrl: "https://coub.com/embed/<%= remote_id %>",
    html: '<iframe style="width:100%;" height="320" frameborder="0" allowfullscreen></iframe>',
    height: 320,
    width: 580
  },
  vine: {
    regex: /https?:\/\/vine\.co\/v\/([^\/\?\&]+)/,
    embedUrl: "https://vine.co/v/<%= remote_id %>/embed/simple/",
    html: '<iframe style="width:100%;" height="320" frameborder="0" allowfullscreen></iframe>',
    height: 320,
    width: 580
  },
  imgur: {
    regex: /https?:\/\/(?:i\.)?imgur\.com.*\/([a-zA-Z0-9]+)(?:\.gifv)?/,
    embedUrl: "http://imgur.com/<%= remote_id %>/embed",
    html: '<iframe allowfullscreen="true" scrolling="no" id="imgur-embed-iframe-pub-<%= remote_id %>" class="imgur-embed-iframe-pub" style="height: 500px; width: 100%; border: 1px solid #000"></iframe>',
    height: 500,
    width: 540
  },
  gfycat: {
    regex: /https?:\/\/gfycat\.com(?:\/detail)?\/([a-zA-Z]+)/,
    embedUrl: "https://gfycat.com/ifr/<%= remote_id %>",
    html: `<iframe frameborder='0' scrolling='no' style="width:100%;" height='436' allowfullscreen ></iframe>`,
    height: 436,
    width: 580
  },
  "twitch-channel": {
    regex: /https?:\/\/www\.twitch\.tv\/([^\/\?\&]*)\/?$/,
    embedUrl: "https://player.twitch.tv/?channel=<%= remote_id %>",
    html: '<iframe frameborder="0" allowfullscreen="true" scrolling="no" height="366" style="width:100%;"></iframe>',
    height: 366,
    width: 600
  },
  "twitch-video": {
    regex: /https?:\/\/www\.twitch\.tv\/(?:[^\/\?\&]*\/v|videos)\/([0-9]*)/,
    embedUrl: "https://player.twitch.tv/?video=v<%= remote_id %>",
    html: '<iframe frameborder="0" allowfullscreen="true" scrolling="no" height="366" style="width:100%;"></iframe>',
    height: 366,
    width: 600
  },
  "yandex-music-album": {
    regex: /https?:\/\/music\.yandex\.ru\/album\/([0-9]*)\/?$/,
    embedUrl: "https://music.yandex.ru/iframe/#album/<%= remote_id %>/",
    html: '<iframe frameborder="0" style="border:none;width:540px;height:400px;" style="width:100%;" height="400"></iframe>',
    height: 400,
    width: 540
  },
  "yandex-music-track": {
    regex: /https?:\/\/music\.yandex\.ru\/album\/([0-9]*)\/track\/([0-9]*)/,
    embedUrl: "https://music.yandex.ru/iframe/#track/<%= remote_id %>/",
    html: '<iframe frameborder="0" style="border:none;width:540px;height:100px;" style="width:100%;" height="100"></iframe>',
    height: 100,
    width: 540,
    id: (o) => o.join("/")
  },
  "yandex-music-playlist": {
    regex: /https?:\/\/music\.yandex\.ru\/users\/([^\/\?\&]*)\/playlists\/([0-9]*)/,
    embedUrl: "https://music.yandex.ru/iframe/#playlist/<%= remote_id %>/show/cover/description/",
    html: '<iframe frameborder="0" style="border:none;width:540px;height:400px;" width="540" height="400"></iframe>',
    height: 400,
    width: 540,
    id: (o) => o.join("/")
  },
  codepen: {
    regex: /https?:\/\/codepen\.io\/([^\/\?\&]*)\/pen\/([^\/\?\&]*)/,
    embedUrl: "https://codepen.io/<%= remote_id %>?height=300&theme-id=0&default-tab=css,result&embed-version=2",
    html: "<iframe height='300' scrolling='no' frameborder='no' allowtransparency='true' allowfullscreen='true' style='width: 100%;'></iframe>",
    height: 300,
    width: 600,
    id: (o) => o.join("/embed/")
  },
  instagram: {
    //it support both reel and post
    regex: /^https:\/\/(?:www\.)?instagram\.com\/(?:reel|p)\/(.*)/,
    embedUrl: "https://www.instagram.com/p/<%= remote_id %>/embed",
    html: '<iframe width="400" height="505" style="margin: 0 auto;" frameborder="0" scrolling="no" allowtransparency="true"></iframe>',
    height: 505,
    width: 400,
    id: (o) => {
      var e;
      return (e = o == null ? void 0 : o[0]) == null ? void 0 : e.split("/")[0];
    }
  },
  twitter: {
    regex: /^https?:\/\/(www\.)?(?:twitter\.com|x\.com)\/.+\/status\/(\d+)/,
    embedUrl: "https://platform.twitter.com/embed/Tweet.html?id=<%= remote_id %>",
    html: '<iframe width="600" height="600" style="margin: 0 auto;" frameborder="0" scrolling="no" allowtransparency="true"></iframe>',
    height: 300,
    width: 600,
    id: (o) => o[1]
  },
  reddit: {
    regex: /https:\/\/www\.reddit\.com\/(.*)/,
    embedUrl: "https://www.redditmedia.com/<%= remote_id %>?ref_source=embed&ref=share&embed=true",
    html: "<iframe height='300' width='100%' scrolling='no' frameborder='no' allowtransparency='true' allowfullscreen='true' style='width: 100%;'></iframe>",
    width: 600,
    height: 300,
    id: (o) => o[0]
  },
  pinterest: {
    regex: /https?:\/\/([^\/\?\&]*).pinterest.com\/pin\/([^\/\?\&]*)\/?$/,
    embedUrl: "https://assets.pinterest.com/ext/embed.html?id=<%= remote_id %>",
    html: "<iframe scrolling='no' frameborder='no' allowtransparency='true' allowfullscreen='true' style='width: 100%; min-height: 400px; max-height: 1000px;'></iframe>",
    id: (o) => o[1]
  },
  facebook: {
    regex: /https?:\/\/www.facebook.com\/([^\/\?\&]*)\/(.*)/,
    embedUrl: "https://www.facebook.com/plugins/post.php?href=https://www.facebook.com/<%= remote_id %>&width=500",
    html: "<iframe scrolling='no' frameborder='no' allowtransparency='true' allowfullscreen='true' style='width: 100%; min-height: 500px; max-height: 1000px;'></iframe>",
    id: (o) => o.join("/")
  },
  aparat: {
    regex: /(?:http[s]?:\/\/)?(?:www.)?aparat\.com\/v\/([^\/\?\&]+)\/?/,
    embedUrl: "https://www.aparat.com/video/video/embed/videohash/<%= remote_id %>/vt/frame",
    html: '<iframe width="600" height="300" style="margin: 0 auto;" frameborder="0" scrolling="no" allowtransparency="true"></iframe>',
    height: 300,
    width: 600
  },
  miro: {
    regex: /https:\/\/miro.com\/\S+(\S{12})\/(\S+)?/,
    embedUrl: "https://miro.com/app/live-embed/<%= remote_id %>",
    html: '<iframe width="700" height="500" style="margin: 0 auto;" allowFullScreen frameBorder="0" scrolling="no"></iframe>'
  },
  github: {
    regex: /https?:\/\/gist.github.com\/([^\/\?\&]*)\/([^\/\?\&]*)/,
    embedUrl: 'data:text/html;charset=utf-8,<head><base target="_blank" /></head><body><script src="https://gist.github.com/<%= remote_id %>" ><\/script></body>',
    html: '<iframe width="100%" height="350" frameborder="0" style="margin: 0 auto;"></iframe>',
    height: 300,
    width: 600,
    id: (o) => `${o.join("/")}.js`
  }
};
function Gt(o, e, t) {
  var r, n, i, s, a;
  e == null && (e = 100);
  function l() {
    var d = Date.now() - s;
    d < e && d >= 0 ? r = setTimeout(l, e - d) : (r = null, t || (a = o.apply(i, n), i = n = null));
  }
  var c = function() {
    i = this, n = arguments, s = Date.now();
    var d = t && !r;
    return r || (r = setTimeout(l, e)), d && (a = o.apply(i, n), i = n = null), a;
  };
  return c.clear = function() {
    r && (clearTimeout(r), r = null);
  }, c.flush = function() {
    r && (a = o.apply(i, n), i = n = null, clearTimeout(r), r = null);
  }, c;
}
Gt.debounce = Gt;
var rd = Gt;
let rn = class pe {
  /**
   * @param {{data: EmbedData, config: EmbedConfig, api: object}}
   *   data — previously saved data
   *   config - user config for Tool
   *   api - Editor.js API
   *   readOnly - read-only mode flag
   */
  constructor({ data: e, api: t, readOnly: r }) {
    this.api = t, this._data = {}, this.element = null, this.readOnly = r, this.data = e;
  }
  /**
   * @param {EmbedData} data - embed data
   * @param {RegExp} [data.regex] - pattern of source URLs
   * @param {string} [data.embedUrl] - URL scheme to embedded page. Use '<%= remote_id %>' to define a place to insert resource id
   * @param {string} [data.html] - iframe which contains embedded content
   * @param {number} [data.height] - iframe height
   * @param {number} [data.width] - iframe width
   * @param {string} [data.caption] - caption
   */
  set data(e) {
    var t;
    if (!(e instanceof Object))
      throw Error("Embed Tool data should be object");
    const { service: r, source: n, embed: i, width: s, height: a, caption: l = "" } = e;
    this._data = {
      service: r || this.data.service,
      source: n || this.data.source,
      embed: i || this.data.embed,
      width: s || this.data.width,
      height: a || this.data.height,
      caption: l || this.data.caption || ""
    };
    const c = this.element;
    c && ((t = c.parentNode) == null || t.replaceChild(this.render(), c));
  }
  /**
   * @returns {EmbedData}
   */
  get data() {
    if (this.element) {
      const e = this.element.querySelector(`.${this.api.styles.input}`);
      this._data.caption = e ? e.innerHTML : "";
    }
    return this._data;
  }
  /**
   * Get plugin styles
   *
   * @returns {object}
   */
  get CSS() {
    return {
      baseClass: this.api.styles.block,
      input: this.api.styles.input,
      container: "embed-tool",
      containerLoading: "embed-tool--loading",
      preloader: "embed-tool__preloader",
      caption: "embed-tool__caption",
      url: "embed-tool__url",
      content: "embed-tool__content"
    };
  }
  /**
   * Render Embed tool content
   *
   * @returns {HTMLElement}
   */
  render() {
    if (!this.data.service) {
      const a = document.createElement("div");
      return this.element = a, a;
    }
    const { html: e } = pe.services[this.data.service], t = document.createElement("div"), r = document.createElement("div"), n = document.createElement("template"), i = this.createPreloader();
    t.classList.add(this.CSS.baseClass, this.CSS.container, this.CSS.containerLoading), r.classList.add(this.CSS.input, this.CSS.caption), t.appendChild(i), r.contentEditable = (!this.readOnly).toString(), r.dataset.placeholder = this.api.i18n.t("Enter a caption"), r.innerHTML = this.data.caption || "", n.innerHTML = e, n.content.firstChild.setAttribute("src", this.data.embed), n.content.firstChild.classList.add(this.CSS.content);
    const s = this.embedIsReady(t);
    return n.content.firstChild && t.appendChild(n.content.firstChild), t.appendChild(r), s.then(() => {
      t.classList.remove(this.CSS.containerLoading);
    }), this.element = t, t;
  }
  /**
   * Creates preloader to append to container while data is loading
   *
   * @returns {HTMLElement}
   */
  createPreloader() {
    const e = document.createElement("preloader"), t = document.createElement("div");
    return t.textContent = this.data.source, e.classList.add(this.CSS.preloader), t.classList.add(this.CSS.url), e.appendChild(t), e;
  }
  /**
   * Save current content and return EmbedData object
   *
   * @returns {EmbedData}
   */
  save() {
    return this.data;
  }
  /**
   * Handle pasted url and return Service object
   *
   * @param {PasteEvent} event - event with pasted data
   */
  onPaste(e) {
    var t;
    const { key: r, data: n } = e.detail, { regex: i, embedUrl: s, width: a, height: l, id: c = (u) => u.shift() || "" } = pe.services[r], d = (t = i.exec(n)) == null ? void 0 : t.slice(1), h = d ? s.replace(/<%= remote_id %>/g, c(d)) : "";
    this.data = {
      service: r,
      source: n,
      embed: h,
      width: a,
      height: l
    };
  }
  /**
   * Analyze provided config and make object with services to use
   *
   * @param {EmbedConfig} config - configuration of embed block element
   */
  static prepare({ config: e = {} }) {
    const { services: t = {} } = e;
    let r = Object.entries(od);
    const n = Object.entries(t).filter(([s, a]) => typeof a == "boolean" && a === !0).map(([s]) => s), i = Object.entries(t).filter(([s, a]) => typeof a == "object").filter(([s, a]) => pe.checkServiceConfig(a)).map(([s, a]) => {
      const { regex: l, embedUrl: c, html: d, height: h, width: u, id: f } = a;
      return [s, {
        regex: l,
        embedUrl: c,
        html: d,
        height: h,
        width: u,
        id: f
      }];
    });
    n.length && (r = r.filter(([s]) => n.includes(s))), r = r.concat(i), pe.services = r.reduce((s, [a, l]) => a in s ? (s[a] = Object.assign({}, s[a], l), s) : (s[a] = l, s), {}), pe.patterns = r.reduce((s, [a, l]) => (l && typeof l != "boolean" && (s[a] = l.regex), s), {});
  }
  /**
   * Check if Service config is valid
   *
   * @param {Service} config - configuration of embed block element
   * @returns {boolean}
   */
  static checkServiceConfig(e) {
    const { regex: t, embedUrl: r, html: n, height: i, width: s, id: a } = e;
    let l = !!(t && t instanceof RegExp) && !!(r && typeof r == "string") && !!(n && typeof n == "string");
    return l = l && (a !== void 0 ? a instanceof Function : !0), l = l && (i !== void 0 ? Number.isFinite(i) : !0), l = l && (s !== void 0 ? Number.isFinite(s) : !0), l;
  }
  /**
   * Paste configuration to enable pasted URLs processing by Editor
   *
   * @returns {object} - object of patterns which contain regx for pasteConfig
   */
  static get pasteConfig() {
    return {
      patterns: pe.patterns
    };
  }
  /**
   * Notify core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Checks that mutations in DOM have finished after appending iframe content
   *
   * @param {HTMLElement} targetNode - HTML-element mutations of which to listen
   * @returns {Promise<any>} - result that all mutations have finished
   */
  embedIsReady(e) {
    let t;
    return new Promise((r, n) => {
      t = new MutationObserver(rd.debounce(r, 450)), t.observe(e, {
        childList: !0,
        subtree: !0
      });
    }).then(() => {
      t.disconnect();
    });
  }
};
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".cdx-quote-icon svg{transform:rotate(180deg)}.cdx-quote{margin:0}.cdx-quote__text{min-height:158px;margin-bottom:10px}.cdx-quote [contentEditable=true][data-placeholder]:before{position:absolute;content:attr(data-placeholder);color:#707684;font-weight:400;opacity:0}.cdx-quote [contentEditable=true][data-placeholder]:empty:before{opacity:1}.cdx-quote [contentEditable=true][data-placeholder]:empty:focus:before{opacity:0}.cdx-quote-settings{display:flex}.cdx-quote-settings .cdx-settings-button{width:50%}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const nd = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 7L6 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 17H6"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 12L8 12"/></svg>', id = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17 7L5 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17 17H5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M13 12L5 12"/></svg>', sd = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 10.8182L9 10.8182C8.80222 10.8182 8.60888 10.7649 8.44443 10.665C8.27998 10.5651 8.15181 10.4231 8.07612 10.257C8.00043 10.0909 7.98063 9.90808 8.01922 9.73174C8.0578 9.55539 8.15304 9.39341 8.29289 9.26627C8.43275 9.13913 8.61093 9.05255 8.80491 9.01747C8.99889 8.98239 9.19996 9.00039 9.38268 9.0692C9.56541 9.13801 9.72159 9.25453 9.83147 9.40403C9.94135 9.55353 10 9.72929 10 9.90909L10 12.1818C10 12.664 9.78929 13.1265 9.41421 13.4675C9.03914 13.8084 8.53043 14 8 14"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 10.8182L15 10.8182C14.8022 10.8182 14.6089 10.7649 14.4444 10.665C14.28 10.5651 14.1518 10.4231 14.0761 10.257C14.0004 10.0909 13.9806 9.90808 14.0192 9.73174C14.0578 9.55539 14.153 9.39341 14.2929 9.26627C14.4327 9.13913 14.6109 9.05255 14.8049 9.01747C14.9989 8.98239 15.2 9.00039 15.3827 9.0692C15.5654 9.13801 15.7216 9.25453 15.8315 9.40403C15.9414 9.55353 16 9.72929 16 9.90909L16 12.1818C16 12.664 15.7893 13.1265 15.4142 13.4675C15.0391 13.8084 14.5304 14 14 14"/></svg>';
var gt = typeof globalThis < "u" ? globalThis : typeof window < "u" ? window : typeof global < "u" ? global : typeof self < "u" ? self : {};
function ad(o) {
  if (o.__esModule)
    return o;
  var e = o.default;
  if (typeof e == "function") {
    var t = function r() {
      return this instanceof r ? Reflect.construct(e, arguments, this.constructor) : e.apply(this, arguments);
    };
    t.prototype = e.prototype;
  } else
    t = {};
  return Object.defineProperty(t, "__esModule", { value: !0 }), Object.keys(o).forEach(function(r) {
    var n = Object.getOwnPropertyDescriptor(o, r);
    Object.defineProperty(t, r, n.get ? n : {
      enumerable: !0,
      get: function() {
        return o[r];
      }
    });
  }), t;
}
var nt = {}, tr = {}, or = {};
Object.defineProperty(or, "__esModule", { value: !0 });
or.allInputsSelector = ld;
function ld() {
  var o = ["text", "password", "email", "number", "search", "tel", "url"];
  return "[contenteditable=true], textarea, input:not([type]), " + o.map(function(e) {
    return 'input[type="'.concat(e, '"]');
  }).join(", ");
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.allInputsSelector = void 0;
  var e = or;
  Object.defineProperty(o, "allInputsSelector", { enumerable: !0, get: function() {
    return e.allInputsSelector;
  } });
})(tr);
var ke = {}, rr = {};
Object.defineProperty(rr, "__esModule", { value: !0 });
rr.isNativeInput = cd;
function cd(o) {
  var e = [
    "INPUT",
    "TEXTAREA"
  ];
  return o && o.tagName ? e.includes(o.tagName) : !1;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isNativeInput = void 0;
  var e = rr;
  Object.defineProperty(o, "isNativeInput", { enumerable: !0, get: function() {
    return e.isNativeInput;
  } });
})(ke);
var yi = {}, nr = {};
Object.defineProperty(nr, "__esModule", { value: !0 });
nr.append = dd;
function dd(o, e) {
  Array.isArray(e) ? e.forEach(function(t) {
    o.appendChild(t);
  }) : o.appendChild(e);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.append = void 0;
  var e = nr;
  Object.defineProperty(o, "append", { enumerable: !0, get: function() {
    return e.append;
  } });
})(yi);
var ir = {}, sr = {};
Object.defineProperty(sr, "__esModule", { value: !0 });
sr.blockElements = hd;
function hd() {
  return [
    "address",
    "article",
    "aside",
    "blockquote",
    "canvas",
    "div",
    "dl",
    "dt",
    "fieldset",
    "figcaption",
    "figure",
    "footer",
    "form",
    "h1",
    "h2",
    "h3",
    "h4",
    "h5",
    "h6",
    "header",
    "hgroup",
    "hr",
    "li",
    "main",
    "nav",
    "noscript",
    "ol",
    "output",
    "p",
    "pre",
    "ruby",
    "section",
    "table",
    "tbody",
    "thead",
    "tr",
    "tfoot",
    "ul",
    "video"
  ];
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.blockElements = void 0;
  var e = sr;
  Object.defineProperty(o, "blockElements", { enumerable: !0, get: function() {
    return e.blockElements;
  } });
})(ir);
var xi = {}, ar = {};
Object.defineProperty(ar, "__esModule", { value: !0 });
ar.calculateBaseline = ud;
function ud(o) {
  var e = window.getComputedStyle(o), t = parseFloat(e.fontSize), r = parseFloat(e.lineHeight) || t * 1.2, n = parseFloat(e.paddingTop), i = parseFloat(e.borderTopWidth), s = parseFloat(e.marginTop), a = t * 0.8, l = (r - t) / 2, c = s + i + n + l + a;
  return c;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.calculateBaseline = void 0;
  var e = ar;
  Object.defineProperty(o, "calculateBaseline", { enumerable: !0, get: function() {
    return e.calculateBaseline;
  } });
})(xi);
var Ci = {}, lr = {}, cr = {}, dr = {};
Object.defineProperty(dr, "__esModule", { value: !0 });
dr.isContentEditable = pd;
function pd(o) {
  return o.contentEditable === "true";
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isContentEditable = void 0;
  var e = dr;
  Object.defineProperty(o, "isContentEditable", { enumerable: !0, get: function() {
    return e.isContentEditable;
  } });
})(cr);
Object.defineProperty(lr, "__esModule", { value: !0 });
lr.canSetCaret = md;
var fd = ke, gd = cr;
function md(o) {
  var e = !0;
  if ((0, fd.isNativeInput)(o))
    switch (o.type) {
      case "file":
      case "checkbox":
      case "radio":
      case "hidden":
      case "submit":
      case "button":
      case "image":
      case "reset":
        e = !1;
        break;
    }
  else
    e = (0, gd.isContentEditable)(o);
  return e;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.canSetCaret = void 0;
  var e = lr;
  Object.defineProperty(o, "canSetCaret", { enumerable: !0, get: function() {
    return e.canSetCaret;
  } });
})(Ci);
var St = {}, hr = {};
function vd(o, e, t) {
  const r = t.value !== void 0 ? "value" : "get", n = t[r], i = `#${e}Cache`;
  if (t[r] = function(...s) {
    return this[i] === void 0 && (this[i] = n.apply(this, s)), this[i];
  }, r === "get" && t.set) {
    const s = t.set;
    t.set = function(a) {
      delete o[i], s.apply(this, a);
    };
  }
  return t;
}
function Ei() {
  const o = {
    win: !1,
    mac: !1,
    x11: !1,
    linux: !1
  }, e = Object.keys(o).find((t) => window.navigator.appVersion.toLowerCase().indexOf(t) !== -1);
  return e !== void 0 && (o[e] = !0), o;
}
function ur(o) {
  return o != null && o !== "" && (typeof o != "object" || Object.keys(o).length > 0);
}
function bd(o) {
  return !ur(o);
}
const kd = () => typeof window < "u" && window.navigator !== null && ur(window.navigator.platform) && (/iP(ad|hone|od)/.test(window.navigator.platform) || window.navigator.platform === "MacIntel" && window.navigator.maxTouchPoints > 1);
function wd(o) {
  const e = Ei();
  return o = o.replace(/shift/gi, "⇧").replace(/backspace/gi, "⌫").replace(/enter/gi, "⏎").replace(/up/gi, "↑").replace(/left/gi, "→").replace(/down/gi, "↓").replace(/right/gi, "←").replace(/escape/gi, "⎋").replace(/insert/gi, "Ins").replace(/delete/gi, "␡").replace(/\+/gi, "+"), e.mac ? o = o.replace(/ctrl|cmd/gi, "⌘").replace(/alt/gi, "⌥") : o = o.replace(/cmd/gi, "Ctrl").replace(/windows/gi, "WIN"), o;
}
function yd(o) {
  return o[0].toUpperCase() + o.slice(1);
}
function xd(o) {
  const e = document.createElement("div");
  e.style.position = "absolute", e.style.left = "-999px", e.style.bottom = "-999px", e.innerHTML = o, document.body.appendChild(e);
  const t = window.getSelection(), r = document.createRange();
  if (r.selectNode(e), t === null)
    throw new Error("Cannot copy text to clipboard");
  t.removeAllRanges(), t.addRange(r), document.execCommand("copy"), document.body.removeChild(e);
}
function Cd(o, e, t) {
  let r;
  return (...n) => {
    const i = this, s = () => {
      r = void 0, t !== !0 && o.apply(i, n);
    }, a = t === !0 && r !== void 0;
    window.clearTimeout(r), r = window.setTimeout(s, e), a && o.apply(i, n);
  };
}
function de(o) {
  return Object.prototype.toString.call(o).match(/\s([a-zA-Z]+)/)[1].toLowerCase();
}
function Ed(o) {
  return de(o) === "boolean";
}
function Ti(o) {
  return de(o) === "function" || de(o) === "asyncfunction";
}
function Td(o) {
  return Ti(o) && /^\s*class\s+/.test(o.toString());
}
function Sd(o) {
  return de(o) === "number";
}
function it(o) {
  return de(o) === "object";
}
function Bd(o) {
  return Promise.resolve(o) === o;
}
function Md(o) {
  return de(o) === "string";
}
function _d(o) {
  return de(o) === "undefined";
}
function Jt(o, ...e) {
  if (!e.length)
    return o;
  const t = e.shift();
  if (it(o) && it(t))
    for (const r in t)
      it(t[r]) ? (o[r] === void 0 && Object.assign(o, { [r]: {} }), Jt(o[r], t[r])) : Object.assign(o, { [r]: t[r] });
  return Jt(o, ...e);
}
function Ld(o, e, t) {
  const r = `«${e}» is deprecated and will be removed in the next major release. Please use the «${t}» instead.`;
  o && console.warn(r);
}
function Id(o) {
  try {
    return new URL(o).href;
  } catch {
  }
  return o.substring(0, 2) === "//" ? window.location.protocol + o : window.location.origin + o;
}
function Od(o) {
  return o > 47 && o < 58 || o === 32 || o === 13 || o === 229 || o > 64 && o < 91 || o > 95 && o < 112 || o > 185 && o < 193 || o > 218 && o < 223;
}
const Ad = {
  BACKSPACE: 8,
  TAB: 9,
  ENTER: 13,
  SHIFT: 16,
  CTRL: 17,
  ALT: 18,
  ESC: 27,
  SPACE: 32,
  LEFT: 37,
  UP: 38,
  DOWN: 40,
  RIGHT: 39,
  DELETE: 46,
  META: 91,
  SLASH: 191
}, Pd = {
  LEFT: 0,
  WHEEL: 1,
  RIGHT: 2,
  BACKWARD: 3,
  FORWARD: 4
};
class Nd {
  constructor() {
    this.completed = Promise.resolve();
  }
  /**
   * Add new promise to queue
   * @param operation - promise should be added to queue
   */
  add(e) {
    return new Promise((t, r) => {
      this.completed = this.completed.then(e).then(t).catch(r);
    });
  }
}
function jd(o, e, t = void 0) {
  let r, n, i, s = null, a = 0;
  t || (t = {});
  const l = function() {
    a = t.leading === !1 ? 0 : Date.now(), s = null, i = o.apply(r, n), s === null && (r = n = null);
  };
  return function() {
    const c = Date.now();
    !a && t.leading === !1 && (a = c);
    const d = e - (c - a);
    return r = this, n = arguments, d <= 0 || d > e ? (s && (clearTimeout(s), s = null), a = c, i = o.apply(r, n), s === null && (r = n = null)) : !s && t.trailing !== !1 && (s = setTimeout(l, d)), i;
  };
}
const Dd = /* @__PURE__ */ Object.freeze(/* @__PURE__ */ Object.defineProperty({
  __proto__: null,
  PromiseQueue: Nd,
  beautifyShortcut: wd,
  cacheable: vd,
  capitalize: yd,
  copyTextToClipboard: xd,
  debounce: Cd,
  deepMerge: Jt,
  deprecationAssert: Ld,
  getUserOS: Ei,
  getValidUrl: Id,
  isBoolean: Ed,
  isClass: Td,
  isEmpty: bd,
  isFunction: Ti,
  isIosDevice: kd,
  isNumber: Sd,
  isObject: it,
  isPrintableKey: Od,
  isPromise: Bd,
  isString: Md,
  isUndefined: _d,
  keyCodes: Ad,
  mouseButtons: Pd,
  notEmpty: ur,
  throttle: jd,
  typeOf: de
}, Symbol.toStringTag, { value: "Module" })), pr = /* @__PURE__ */ ad(Dd);
Object.defineProperty(hr, "__esModule", { value: !0 });
hr.containsOnlyInlineElements = Fd;
var Rd = pr, Hd = ir;
function Fd(o) {
  var e;
  (0, Rd.isString)(o) ? (e = document.createElement("div"), e.innerHTML = o) : e = o;
  var t = function(r) {
    return !(0, Hd.blockElements)().includes(r.tagName.toLowerCase()) && Array.from(r.children).every(t);
  };
  return Array.from(e.children).every(t);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.containsOnlyInlineElements = void 0;
  var e = hr;
  Object.defineProperty(o, "containsOnlyInlineElements", { enumerable: !0, get: function() {
    return e.containsOnlyInlineElements;
  } });
})(St);
var Si = {}, fr = {}, Bt = {}, gr = {};
Object.defineProperty(gr, "__esModule", { value: !0 });
gr.make = $d;
function $d(o, e, t) {
  var r;
  e === void 0 && (e = null), t === void 0 && (t = {});
  var n = document.createElement(o);
  if (Array.isArray(e)) {
    var i = e.filter(function(a) {
      return a !== void 0;
    });
    (r = n.classList).add.apply(r, i);
  } else
    e !== null && n.classList.add(e);
  for (var s in t)
    Object.prototype.hasOwnProperty.call(t, s) && (n[s] = t[s]);
  return n;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.make = void 0;
  var e = gr;
  Object.defineProperty(o, "make", { enumerable: !0, get: function() {
    return e.make;
  } });
})(Bt);
Object.defineProperty(fr, "__esModule", { value: !0 });
fr.fragmentToString = zd;
var Ud = Bt;
function zd(o) {
  var e = (0, Ud.make)("div");
  return e.appendChild(o), e.innerHTML;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.fragmentToString = void 0;
  var e = fr;
  Object.defineProperty(o, "fragmentToString", { enumerable: !0, get: function() {
    return e.fragmentToString;
  } });
})(Si);
var Bi = {}, mr = {};
Object.defineProperty(mr, "__esModule", { value: !0 });
mr.getContentLength = Wd;
var Vd = ke;
function Wd(o) {
  var e, t;
  return (0, Vd.isNativeInput)(o) ? o.value.length : o.nodeType === Node.TEXT_NODE ? o.length : (t = (e = o.textContent) === null || e === void 0 ? void 0 : e.length) !== null && t !== void 0 ? t : 0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getContentLength = void 0;
  var e = mr;
  Object.defineProperty(o, "getContentLength", { enumerable: !0, get: function() {
    return e.getContentLength;
  } });
})(Bi);
var vr = {}, br = {}, nn = gt && gt.__spreadArray || function(o, e, t) {
  if (t || arguments.length === 2)
    for (var r = 0, n = e.length, i; r < n; r++)
      (i || !(r in e)) && (i || (i = Array.prototype.slice.call(e, 0, r)), i[r] = e[r]);
  return o.concat(i || Array.prototype.slice.call(e));
};
Object.defineProperty(br, "__esModule", { value: !0 });
br.getDeepestBlockElements = Mi;
var qd = St;
function Mi(o) {
  return (0, qd.containsOnlyInlineElements)(o) ? [o] : Array.from(o.children).reduce(function(e, t) {
    return nn(nn([], e, !0), Mi(t), !0);
  }, []);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getDeepestBlockElements = void 0;
  var e = br;
  Object.defineProperty(o, "getDeepestBlockElements", { enumerable: !0, get: function() {
    return e.getDeepestBlockElements;
  } });
})(vr);
var _i = {}, kr = {}, Mt = {}, wr = {};
Object.defineProperty(wr, "__esModule", { value: !0 });
wr.isLineBreakTag = Kd;
function Kd(o) {
  return [
    "BR",
    "WBR"
  ].includes(o.tagName);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isLineBreakTag = void 0;
  var e = wr;
  Object.defineProperty(o, "isLineBreakTag", { enumerable: !0, get: function() {
    return e.isLineBreakTag;
  } });
})(Mt);
var _t = {}, yr = {};
Object.defineProperty(yr, "__esModule", { value: !0 });
yr.isSingleTag = Yd;
function Yd(o) {
  return [
    "AREA",
    "BASE",
    "BR",
    "COL",
    "COMMAND",
    "EMBED",
    "HR",
    "IMG",
    "INPUT",
    "KEYGEN",
    "LINK",
    "META",
    "PARAM",
    "SOURCE",
    "TRACK",
    "WBR"
  ].includes(o.tagName);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isSingleTag = void 0;
  var e = yr;
  Object.defineProperty(o, "isSingleTag", { enumerable: !0, get: function() {
    return e.isSingleTag;
  } });
})(_t);
Object.defineProperty(kr, "__esModule", { value: !0 });
kr.getDeepestNode = Li;
var Xd = ke, Zd = Mt, Gd = _t;
function Li(o, e) {
  e === void 0 && (e = !1);
  var t = e ? "lastChild" : "firstChild", r = e ? "previousSibling" : "nextSibling";
  if (o.nodeType === Node.ELEMENT_NODE && o[t]) {
    var n = o[t];
    if ((0, Gd.isSingleTag)(n) && !(0, Xd.isNativeInput)(n) && !(0, Zd.isLineBreakTag)(n))
      if (n[r])
        n = n[r];
      else if (n.parentNode !== null && n.parentNode[r])
        n = n.parentNode[r];
      else
        return n.parentNode;
    return Li(n, e);
  }
  return o;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.getDeepestNode = void 0;
  var e = kr;
  Object.defineProperty(o, "getDeepestNode", { enumerable: !0, get: function() {
    return e.getDeepestNode;
  } });
})(_i);
var Ii = {}, xr = {}, Je = gt && gt.__spreadArray || function(o, e, t) {
  if (t || arguments.length === 2)
    for (var r = 0, n = e.length, i; r < n; r++)
      (i || !(r in e)) && (i || (i = Array.prototype.slice.call(e, 0, r)), i[r] = e[r]);
  return o.concat(i || Array.prototype.slice.call(e));
};
Object.defineProperty(xr, "__esModule", { value: !0 });
xr.findAllInputs = oh;
var Jd = St, Qd = vr, eh = tr, th = ke;
function oh(o) {
  return Array.from(o.querySelectorAll((0, eh.allInputsSelector)())).reduce(function(e, t) {
    return (0, th.isNativeInput)(t) || (0, Jd.containsOnlyInlineElements)(t) ? Je(Je([], e, !0), [t], !1) : Je(Je([], e, !0), (0, Qd.getDeepestBlockElements)(t), !0);
  }, []);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.findAllInputs = void 0;
  var e = xr;
  Object.defineProperty(o, "findAllInputs", { enumerable: !0, get: function() {
    return e.findAllInputs;
  } });
})(Ii);
var Oi = {}, Cr = {};
Object.defineProperty(Cr, "__esModule", { value: !0 });
Cr.isCollapsedWhitespaces = rh;
function rh(o) {
  return !/[^\t\n\r ]/.test(o);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isCollapsedWhitespaces = void 0;
  var e = Cr;
  Object.defineProperty(o, "isCollapsedWhitespaces", { enumerable: !0, get: function() {
    return e.isCollapsedWhitespaces;
  } });
})(Oi);
var Er = {}, Tr = {};
Object.defineProperty(Tr, "__esModule", { value: !0 });
Tr.isElement = ih;
var nh = pr;
function ih(o) {
  return (0, nh.isNumber)(o) ? !1 : !!o && !!o.nodeType && o.nodeType === Node.ELEMENT_NODE;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isElement = void 0;
  var e = Tr;
  Object.defineProperty(o, "isElement", { enumerable: !0, get: function() {
    return e.isElement;
  } });
})(Er);
var Ai = {}, Sr = {}, Br = {}, Mr = {};
Object.defineProperty(Mr, "__esModule", { value: !0 });
Mr.isLeaf = sh;
function sh(o) {
  return o === null ? !1 : o.childNodes.length === 0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isLeaf = void 0;
  var e = Mr;
  Object.defineProperty(o, "isLeaf", { enumerable: !0, get: function() {
    return e.isLeaf;
  } });
})(Br);
var _r = {}, Lr = {};
Object.defineProperty(Lr, "__esModule", { value: !0 });
Lr.isNodeEmpty = hh;
var ah = Mt, lh = Er, ch = ke, dh = _t;
function hh(o, e) {
  var t = "";
  return (0, dh.isSingleTag)(o) && !(0, ah.isLineBreakTag)(o) ? !1 : ((0, lh.isElement)(o) && (0, ch.isNativeInput)(o) ? t = o.value : o.textContent !== null && (t = o.textContent.replace("​", "")), e !== void 0 && (t = t.replace(new RegExp(e, "g"), "")), t.trim().length === 0);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isNodeEmpty = void 0;
  var e = Lr;
  Object.defineProperty(o, "isNodeEmpty", { enumerable: !0, get: function() {
    return e.isNodeEmpty;
  } });
})(_r);
Object.defineProperty(Sr, "__esModule", { value: !0 });
Sr.isEmpty = fh;
var uh = Br, ph = _r;
function fh(o, e) {
  o.normalize();
  for (var t = [o]; t.length > 0; ) {
    var r = t.shift();
    if (r) {
      if (o = r, (0, uh.isLeaf)(o) && !(0, ph.isNodeEmpty)(o, e))
        return !1;
      t.push.apply(t, Array.from(o.childNodes));
    }
  }
  return !0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isEmpty = void 0;
  var e = Sr;
  Object.defineProperty(o, "isEmpty", { enumerable: !0, get: function() {
    return e.isEmpty;
  } });
})(Ai);
var Pi = {}, Ir = {};
Object.defineProperty(Ir, "__esModule", { value: !0 });
Ir.isFragment = mh;
var gh = pr;
function mh(o) {
  return (0, gh.isNumber)(o) ? !1 : !!o && !!o.nodeType && o.nodeType === Node.DOCUMENT_FRAGMENT_NODE;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isFragment = void 0;
  var e = Ir;
  Object.defineProperty(o, "isFragment", { enumerable: !0, get: function() {
    return e.isFragment;
  } });
})(Pi);
var Ni = {}, Or = {};
Object.defineProperty(Or, "__esModule", { value: !0 });
Or.isHTMLString = bh;
var vh = Bt;
function bh(o) {
  var e = (0, vh.make)("div");
  return e.innerHTML = o, e.childElementCount > 0;
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.isHTMLString = void 0;
  var e = Or;
  Object.defineProperty(o, "isHTMLString", { enumerable: !0, get: function() {
    return e.isHTMLString;
  } });
})(Ni);
var ji = {}, Ar = {};
Object.defineProperty(Ar, "__esModule", { value: !0 });
Ar.offset = kh;
function kh(o) {
  var e = o.getBoundingClientRect(), t = window.pageXOffset || document.documentElement.scrollLeft, r = window.pageYOffset || document.documentElement.scrollTop, n = e.top + r, i = e.left + t;
  return {
    top: n,
    left: i,
    bottom: n + e.height,
    right: i + e.width
  };
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.offset = void 0;
  var e = Ar;
  Object.defineProperty(o, "offset", { enumerable: !0, get: function() {
    return e.offset;
  } });
})(ji);
var Di = {}, Pr = {};
Object.defineProperty(Pr, "__esModule", { value: !0 });
Pr.prepend = wh;
function wh(o, e) {
  Array.isArray(e) ? (e = e.reverse(), e.forEach(function(t) {
    return o.prepend(t);
  })) : o.prepend(e);
}
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.prepend = void 0;
  var e = Pr;
  Object.defineProperty(o, "prepend", { enumerable: !0, get: function() {
    return e.prepend;
  } });
})(Di);
(function(o) {
  Object.defineProperty(o, "__esModule", { value: !0 }), o.prepend = o.offset = o.make = o.isLineBreakTag = o.isSingleTag = o.isNodeEmpty = o.isLeaf = o.isHTMLString = o.isFragment = o.isEmpty = o.isElement = o.isContentEditable = o.isCollapsedWhitespaces = o.findAllInputs = o.isNativeInput = o.allInputsSelector = o.getDeepestNode = o.getDeepestBlockElements = o.getContentLength = o.fragmentToString = o.containsOnlyInlineElements = o.canSetCaret = o.calculateBaseline = o.blockElements = o.append = void 0;
  var e = tr;
  Object.defineProperty(o, "allInputsSelector", { enumerable: !0, get: function() {
    return e.allInputsSelector;
  } });
  var t = ke;
  Object.defineProperty(o, "isNativeInput", { enumerable: !0, get: function() {
    return t.isNativeInput;
  } });
  var r = yi;
  Object.defineProperty(o, "append", { enumerable: !0, get: function() {
    return r.append;
  } });
  var n = ir;
  Object.defineProperty(o, "blockElements", { enumerable: !0, get: function() {
    return n.blockElements;
  } });
  var i = xi;
  Object.defineProperty(o, "calculateBaseline", { enumerable: !0, get: function() {
    return i.calculateBaseline;
  } });
  var s = Ci;
  Object.defineProperty(o, "canSetCaret", { enumerable: !0, get: function() {
    return s.canSetCaret;
  } });
  var a = St;
  Object.defineProperty(o, "containsOnlyInlineElements", { enumerable: !0, get: function() {
    return a.containsOnlyInlineElements;
  } });
  var l = Si;
  Object.defineProperty(o, "fragmentToString", { enumerable: !0, get: function() {
    return l.fragmentToString;
  } });
  var c = Bi;
  Object.defineProperty(o, "getContentLength", { enumerable: !0, get: function() {
    return c.getContentLength;
  } });
  var d = vr;
  Object.defineProperty(o, "getDeepestBlockElements", { enumerable: !0, get: function() {
    return d.getDeepestBlockElements;
  } });
  var h = _i;
  Object.defineProperty(o, "getDeepestNode", { enumerable: !0, get: function() {
    return h.getDeepestNode;
  } });
  var u = Ii;
  Object.defineProperty(o, "findAllInputs", { enumerable: !0, get: function() {
    return u.findAllInputs;
  } });
  var f = Oi;
  Object.defineProperty(o, "isCollapsedWhitespaces", { enumerable: !0, get: function() {
    return f.isCollapsedWhitespaces;
  } });
  var p = cr;
  Object.defineProperty(o, "isContentEditable", { enumerable: !0, get: function() {
    return p.isContentEditable;
  } });
  var k = Er;
  Object.defineProperty(o, "isElement", { enumerable: !0, get: function() {
    return k.isElement;
  } });
  var T = Ai;
  Object.defineProperty(o, "isEmpty", { enumerable: !0, get: function() {
    return T.isEmpty;
  } });
  var v = Pi;
  Object.defineProperty(o, "isFragment", { enumerable: !0, get: function() {
    return v.isFragment;
  } });
  var m = Ni;
  Object.defineProperty(o, "isHTMLString", { enumerable: !0, get: function() {
    return m.isHTMLString;
  } });
  var C = Br;
  Object.defineProperty(o, "isLeaf", { enumerable: !0, get: function() {
    return C.isLeaf;
  } });
  var S = _r;
  Object.defineProperty(o, "isNodeEmpty", { enumerable: !0, get: function() {
    return S.isNodeEmpty;
  } });
  var _ = Mt;
  Object.defineProperty(o, "isLineBreakTag", { enumerable: !0, get: function() {
    return _.isLineBreakTag;
  } });
  var x = _t;
  Object.defineProperty(o, "isSingleTag", { enumerable: !0, get: function() {
    return x.isSingleTag;
  } });
  var I = Bt;
  Object.defineProperty(o, "make", { enumerable: !0, get: function() {
    return I.make;
  } });
  var w = ji;
  Object.defineProperty(o, "offset", { enumerable: !0, get: function() {
    return w.offset;
  } });
  var b = Di;
  Object.defineProperty(o, "prepend", { enumerable: !0, get: function() {
    return b.prepend;
  } });
})(nt);
var Ri = /* @__PURE__ */ ((o) => (o.Left = "left", o.Center = "center", o))(Ri || {});
class ze {
  /**
   * Render plugin`s main Element and fill it with saved data
   * @param params - Quote Tool constructor params
   * @param params.data - previously saved data
   * @param params.config - user config for Tool
   * @param params.api - editor.js api
   * @param params.readOnly - read only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n, block: i }) {
    const { DEFAULT_ALIGNMENT: s } = ze;
    this.api = r, this.readOnly = n, this.quotePlaceholder = r.i18n.t((t == null ? void 0 : t.quotePlaceholder) ?? ze.DEFAULT_QUOTE_PLACEHOLDER), this.captionPlaceholder = r.i18n.t((t == null ? void 0 : t.captionPlaceholder) ?? ze.DEFAULT_CAPTION_PLACEHOLDER), this.data = {
      text: e.text || "",
      caption: e.caption || "",
      alignment: Object.values(Ri).includes(e.alignment) ? e.alignment : (t == null ? void 0 : t.defaultAlignment) ?? s
    }, this.css = {
      baseClass: this.api.styles.block,
      wrapper: "cdx-quote",
      text: "cdx-quote__text",
      input: this.api.styles.input,
      caption: "cdx-quote__caption"
    }, this.block = i;
  }
  /**
   * Notify core that read-only mode is supported
   * @returns true
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Get Tool toolbox settings
   * icon - Tool icon's SVG
   * title - title to show in toolbox
   * @returns icon and title of the toolbox
   */
  static get toolbox() {
    return {
      icon: sd,
      title: "Quote"
    };
  }
  /**
   * Empty Quote is not empty Block
   * @returns true
   */
  static get contentless() {
    return !0;
  }
  /**
   * Allow to press Enter inside the Quote
   * @returns true
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * Default placeholder for quote text
   * @returns 'Enter a quote'
   */
  static get DEFAULT_QUOTE_PLACEHOLDER() {
    return "Enter a quote";
  }
  /**
   * Default placeholder for quote caption
   * @returns 'Enter a caption'
   */
  static get DEFAULT_CAPTION_PLACEHOLDER() {
    return "Enter a caption";
  }
  /**
   * Default quote alignment
   * @returns Alignment.Left
   */
  static get DEFAULT_ALIGNMENT() {
    return "left";
  }
  /**
   * Allow Quote to be converted to/from other blocks
   * @returns conversion config object
   */
  static get conversionConfig() {
    return {
      /**
       * To create Quote data from string, simple fill 'text' property
       */
      import: "text",
      /**
       * To create string from Quote data, concatenate text and caption
       * @param quoteData - Quote data object
       * @returns string
       */
      export: function(e) {
        return e.caption ? `${e.text} — ${e.caption}` : e.text;
      }
    };
  }
  /**
   * Tool`s styles
   * @returns CSS classes names
   */
  get CSS() {
    return {
      baseClass: this.api.styles.block,
      wrapper: "cdx-quote",
      text: "cdx-quote__text",
      input: this.api.styles.input,
      caption: "cdx-quote__caption"
    };
  }
  /**
   * Tool`s settings properties
   * @returns settings properties
   */
  get settings() {
    return [
      {
        name: "left",
        icon: id
      },
      {
        name: "center",
        icon: nd
      }
    ];
  }
  /**
   * Create Quote Tool container with inputs
   * @returns blockquote DOM element - Quote Tool container
   */
  render() {
    const e = nt.make("blockquote", [
      this.css.baseClass,
      this.css.wrapper
    ]), t = nt.make("div", [this.css.input, this.css.text], {
      contentEditable: !this.readOnly,
      innerHTML: this.data.text
    }), r = nt.make("div", [this.css.input, this.css.caption], {
      contentEditable: !this.readOnly,
      innerHTML: this.data.caption
    });
    return t.dataset.placeholder = this.quotePlaceholder, r.dataset.placeholder = this.captionPlaceholder, e.appendChild(t), e.appendChild(r), e;
  }
  /**
   * Extract Quote data from Quote Tool element
   * @param quoteElement - Quote DOM element to save
   * @returns Quote data object
   */
  save(e) {
    const t = e.querySelector(`.${this.css.text}`), r = e.querySelector(`.${this.css.caption}`);
    return Object.assign(this.data, {
      text: (t == null ? void 0 : t.innerHTML) ?? "",
      caption: (r == null ? void 0 : r.innerHTML) ?? ""
    });
  }
  /**
   * Sanitizer rules
   * @returns sanitizer rules
   */
  static get sanitize() {
    return {
      text: {
        br: !0
      },
      caption: {
        br: !0
      },
      alignment: {}
    };
  }
  /**
   * Create wrapper for Tool`s settings buttons:
   * 1. Left alignment
   * 2. Center alignment
   * @returns settings menu
   */
  renderSettings() {
    const e = (t) => t && t[0].toUpperCase() + t.slice(1);
    return this.settings.map((t) => ({
      icon: t.icon,
      label: this.api.i18n.t(`Align ${e(t.name)}`),
      onActivate: () => this._toggleTune(t.name),
      isActive: this.data.alignment === t.name,
      closeOnActivate: !0
    }));
  }
  /**
   * Toggle quote`s alignment
   * @param tune - alignment
   */
  _toggleTune(e) {
    this.data.alignment = e, this.block.dispatchChange();
  }
}
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".ce-code__textarea{min-height:200px;font-family:Menlo,Monaco,Consolas,Courier New,monospace;color:#41314e;line-height:1.6em;font-size:12px;background:#f8f7fa;border:1px solid #f1f1f4;box-shadow:none;white-space:pre;word-wrap:normal;overflow-x:auto;resize:vertical}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
function yh(o, e) {
  let t = "";
  for (; t !== `
` && e > 0; )
    e = e - 1, t = o.substr(e, 1);
  return t === `
` && (e += 1), e;
}
const xh = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 8L5 12L9 16"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 8L19 12L15 16"/></svg>';
/**
 * CodeTool for Editor.js
 * @version 2.0.0
 * @license MIT
 */
class Nr {
  /**
   * Notify core that read-only mode is supported
   * @returns true if read-only mode is supported
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Allows pressing Enter key to create line breaks inside the CodeTool textarea
   * This enables multi-line input within the code editor.
   * @returns true if line breaks are allowed in the textarea
   */
  static get enableLineBreaks() {
    return !0;
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   * @param options - tool constricting options
   * @param options.data — previously saved plugin code
   * @param options.config - user config for Tool
   * @param options.api - Editor.js API
   * @param options.readOnly - read only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this.api = r, this.readOnly = n, this.placeholder = this.api.i18n.t(t.placeholder || Nr.DEFAULT_PLACEHOLDER), this.CSS = {
      baseClass: this.api.styles.block,
      input: this.api.styles.input,
      wrapper: "ce-code",
      textarea: "ce-code__textarea"
    }, this.nodes = {
      holder: null,
      textarea: null
    }, this.data = {
      code: e.code ?? ""
    }, this.nodes.holder = this.drawView();
  }
  /**
   * Return Tool's view
   * @returns this.nodes.holder - Code's wrapper
   */
  render() {
    return this.nodes.holder;
  }
  /**
   * Extract Tool's data from the view
   * @param codeWrapper - CodeTool's wrapper, containing textarea with code
   * @returns - saved plugin code
   */
  save(e) {
    return {
      code: e.querySelector("textarea").value
    };
  }
  /**
   * onPaste callback fired from Editor`s core
   * @param event - event with pasted content
   */
  onPaste(e) {
    switch (e.type) {
      case "tag": {
        const t = e.detail.data;
        this.handleHTMLPaste(t);
        break;
      }
    }
  }
  /**
   * Returns Tool`s data from private property
   * @returns
   */
  get data() {
    return this._data;
  }
  /**
   * Set Tool`s data to private property and update view
   * @param data - saved tool data
   */
  set data(e) {
    this._data = e, this.nodes.textarea && (this.nodes.textarea.value = e.code);
  }
  /**
   * Get Tool toolbox settings.
   * Provides the icon and title to display in the toolbox for the CodeTool.
   * @returns An object containing:
   * - icon: SVG representation of the Tool's icon
   * - title: Title to show in the toolbox
   */
  static get toolbox() {
    return {
      icon: xh,
      title: "Code"
    };
  }
  /**
   * Default placeholder for CodeTool's textarea
   * @returns
   */
  static get DEFAULT_PLACEHOLDER() {
    return "Enter a code";
  }
  /**
   *  Used by Editor.js paste handling API.
   *  Provides configuration to handle CODE tag.
   * @returns
   */
  static get pasteConfig() {
    return {
      tags: ["pre"]
    };
  }
  /**
   * Automatic sanitize config
   * @returns
   */
  static get sanitize() {
    return {
      code: !0
      // Allow HTML tags
    };
  }
  /**
   * Handles Tab key pressing (adds/removes indentations)
   * @param event - keydown
   */
  tabHandler(e) {
    e.stopPropagation(), e.preventDefault();
    const t = e.target, r = e.shiftKey, n = t.selectionStart, i = t.value, s = "  ";
    let a;
    if (!r)
      a = n + s.length, t.value = i.substring(0, n) + s + i.substring(n);
    else {
      const l = yh(i, n);
      if (i.substr(l, s.length) !== s)
        return;
      t.value = i.substring(0, l) + i.substring(l + s.length), a = n - s.length;
    }
    t.setSelectionRange(a, a);
  }
  /**
   * Create Tool's view
   * @returns
   */
  drawView() {
    const e = document.createElement("div"), t = document.createElement("textarea");
    return e.classList.add(this.CSS.baseClass, this.CSS.wrapper), t.classList.add(this.CSS.textarea, this.CSS.input), t.value = this.data.code, t.placeholder = this.placeholder, this.readOnly && (t.disabled = !0), e.appendChild(t), t.addEventListener("keydown", (r) => {
      switch (r.code) {
        case "Tab":
          this.tabHandler(r);
          break;
      }
    }), this.nodes.textarea = t, e;
  }
  /**
   * Extracts the code content from the pasted element's innerHTML and populates the tool's data.
   * @param element - pasted HTML element
   */
  handleHTMLPaste(e) {
    this.data = {
      code: e.innerHTML
    };
  }
}
(function() {
  try {
    if (typeof document < "u") {
      var o = document.createElement("style");
      o.appendChild(document.createTextNode(".ce-paragraph{line-height:1.6em;outline:none}.ce-block:only-of-type .ce-paragraph[data-placeholder-active]:empty:before,.ce-block:only-of-type .ce-paragraph[data-placeholder-active][data-empty=true]:before{content:attr(data-placeholder-active)}.ce-paragraph p:first-of-type{margin-top:0}.ce-paragraph p:last-of-type{margin-bottom:0}")), document.head.appendChild(o);
    }
  } catch (e) {
    console.error("vite-plugin-css-injected-by-js", e);
  }
})();
const Ch = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8 9V7.2C8 7.08954 8.08954 7 8.2 7L12 7M16 9V7.2C16 7.08954 15.9105 7 15.8 7L12 7M12 7L12 17M12 17H10M12 17H14"/></svg>';
function Eh(o) {
  const e = document.createElement("div");
  e.innerHTML = o.trim();
  const t = document.createDocumentFragment();
  return t.append(...Array.from(e.childNodes)), t;
}
/**
 * Base Paragraph Block for the Editor.js.
 * Represents a regular text block
 *
 * @author CodeX (team@codex.so)
 * @copyright CodeX 2018
 * @license The MIT License (MIT)
 */
class jr {
  /**
   * Default placeholder for Paragraph Tool
   *
   * @returns {string}
   * @class
   */
  static get DEFAULT_PLACEHOLDER() {
    return "";
  }
  /**
   * Render plugin`s main Element and fill it with saved data
   *
   * @param {object} params - constructor params
   * @param {ParagraphData} params.data - previously saved data
   * @param {ParagraphConfig} params.config - user config for Tool
   * @param {object} params.api - editor.js api
   * @param {boolean} readOnly - read only mode flag
   */
  constructor({ data: e, config: t, api: r, readOnly: n }) {
    this.api = r, this.readOnly = n, this._CSS = {
      block: this.api.styles.block,
      wrapper: "ce-paragraph"
    }, this.readOnly || (this.onKeyUp = this.onKeyUp.bind(this)), this._placeholder = t.placeholder ? t.placeholder : jr.DEFAULT_PLACEHOLDER, this._data = e ?? {}, this._element = null, this._preserveBlank = t.preserveBlank ?? !1;
  }
  /**
   * Check if text content is empty and set empty string to inner html.
   * We need this because some browsers (e.g. Safari) insert <br> into empty contenteditanle elements
   *
   * @param {KeyboardEvent} e - key up event
   */
  onKeyUp(e) {
    if (e.code !== "Backspace" && e.code !== "Delete" || !this._element)
      return;
    const { textContent: t } = this._element;
    t === "" && (this._element.innerHTML = "");
  }
  /**
   * Create Tool's view
   *
   * @returns {HTMLDivElement}
   * @private
   */
  drawView() {
    const e = document.createElement("DIV");
    return e.classList.add(this._CSS.wrapper, this._CSS.block), e.contentEditable = "false", e.dataset.placeholderActive = this.api.i18n.t(this._placeholder), this._data.text && (e.innerHTML = this._data.text), this.readOnly || (e.contentEditable = "true", e.addEventListener("keyup", this.onKeyUp)), e;
  }
  /**
   * Return Tool's view
   *
   * @returns {HTMLDivElement}
   */
  render() {
    return this._element = this.drawView(), this._element;
  }
  /**
   * Method that specified how to merge two Text blocks.
   * Called by Editor.js by backspace at the beginning of the Block
   *
   * @param {ParagraphData} data
   * @public
   */
  merge(e) {
    if (!this._element)
      return;
    this._data.text += e.text;
    const t = Eh(e.text);
    this._element.appendChild(t), this._element.normalize();
  }
  /**
   * Validate Paragraph block data:
   * - check for emptiness
   *
   * @param {ParagraphData} savedData — data received after saving
   * @returns {boolean} false if saved data is not correct, otherwise true
   * @public
   */
  validate(e) {
    return !(e.text.trim() === "" && !this._preserveBlank);
  }
  /**
   * Extract Tool's data from the view
   *
   * @param {HTMLDivElement} toolsContent - Paragraph tools rendered view
   * @returns {ParagraphData} - saved data
   * @public
   */
  save(e) {
    return {
      text: e.innerHTML
    };
  }
  /**
   * On paste callback fired from Editor.
   *
   * @param {HTMLPasteEvent} event - event with pasted data
   */
  onPaste(e) {
    const t = {
      text: e.detail.data.innerHTML
    };
    this._data = t, window.requestAnimationFrame(() => {
      this._element && (this._element.innerHTML = this._data.text || "");
    });
  }
  /**
   * Enable Conversion Toolbar. Paragraph can be converted to/from other tools
   * @returns {ConversionConfig}
   */
  static get conversionConfig() {
    return {
      export: "text",
      // to convert Paragraph to other block, use 'text' property of saved data
      import: "text"
      // to covert other block's exported string to Paragraph, fill 'text' property of tool data
    };
  }
  /**
   * Sanitizer rules
   * @returns {SanitizerConfig} - Edtior.js sanitizer config
   */
  static get sanitize() {
    return {
      text: {
        br: !0
      }
    };
  }
  /**
   * Returns true to notify the core that read-only mode is supported
   *
   * @returns {boolean}
   */
  static get isReadOnlySupported() {
    return !0;
  }
  /**
   * Used by Editor paste handling API.
   * Provides configuration to handle P tags.
   *
   * @returns {PasteConfig} - Paragraph Paste Setting
   */
  static get pasteConfig() {
    return {
      tags: ["P"]
    };
  }
  /**
   * Icon and title for displaying at the Toolbox
   *
   * @returns {ToolboxConfig} - Paragraph Toolbox Setting
   */
  static get toolbox() {
    return {
      icon: Ch,
      title: "Text"
    };
  }
}
function Hi(o) {
  return o && o.__esModule && Object.prototype.hasOwnProperty.call(o, "default") ? o.default : o;
}
var Rt = { exports: {} }, sn;
function Th() {
  return sn || (sn = 1, (function(o, e) {
    (function(t, r) {
      o.exports = r();
    })(self, (() => (() => {
      var t = { 523: (s, a, l) => {
        l.d(a, { A: () => f });
        var c = l(601), d = l.n(c), h = l(314), u = l.n(h)()(d());
        u.push([s.id, `.ce-block--drop-target .ce-block__content:before {
  content: "";
  position: absolute;
  top: 50%;
  left: -20px;
  margin-top: -1px;
  height: 8px;
  width: 8px;
  border: solid #a0a0a0;
  border-width: 1px 1px 0 0;
  -webkit-transform-origin: right;
  transform-origin: right;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
}

.ce-block--drop-target .ce-block__content:after {
  background: none;
}
`, ""]);
        const f = u;
      }, 314: (s) => {
        s.exports = function(a) {
          var l = [];
          return l.toString = function() {
            return this.map((function(c) {
              var d = "", h = c[5] !== void 0;
              return c[4] && (d += "@supports (".concat(c[4], ") {")), c[2] && (d += "@media ".concat(c[2], " {")), h && (d += "@layer".concat(c[5].length > 0 ? " ".concat(c[5]) : "", " {")), d += a(c), h && (d += "}"), c[2] && (d += "}"), c[4] && (d += "}"), d;
            })).join("");
          }, l.i = function(c, d, h, u, f) {
            typeof c == "string" && (c = [[null, c, void 0]]);
            var p = {};
            if (h) for (var k = 0; k < this.length; k++) {
              var T = this[k][0];
              T != null && (p[T] = !0);
            }
            for (var v = 0; v < c.length; v++) {
              var m = [].concat(c[v]);
              h && p[m[0]] || (f !== void 0 && (m[5] === void 0 || (m[1] = "@layer".concat(m[5].length > 0 ? " ".concat(m[5]) : "", " {").concat(m[1], "}")), m[5] = f), d && (m[2] && (m[1] = "@media ".concat(m[2], " {").concat(m[1], "}")), m[2] = d), u && (m[4] ? (m[1] = "@supports (".concat(m[4], ") {").concat(m[1], "}"), m[4] = u) : m[4] = "".concat(u)), l.push(m));
            }
          }, l;
        };
      }, 601: (s) => {
        s.exports = function(a) {
          return a[1];
        };
      }, 72: (s) => {
        var a = [];
        function l(h) {
          for (var u = -1, f = 0; f < a.length; f++) if (a[f].identifier === h) {
            u = f;
            break;
          }
          return u;
        }
        function c(h, u) {
          for (var f = {}, p = [], k = 0; k < h.length; k++) {
            var T = h[k], v = u.base ? T[0] + u.base : T[0], m = f[v] || 0, C = "".concat(v, " ").concat(m);
            f[v] = m + 1;
            var S = l(C), _ = { css: T[1], media: T[2], sourceMap: T[3], supports: T[4], layer: T[5] };
            if (S !== -1) a[S].references++, a[S].updater(_);
            else {
              var x = d(_, u);
              u.byIndex = k, a.splice(k, 0, { identifier: C, updater: x, references: 1 });
            }
            p.push(C);
          }
          return p;
        }
        function d(h, u) {
          var f = u.domAPI(u);
          return f.update(h), function(p) {
            if (p) {
              if (p.css === h.css && p.media === h.media && p.sourceMap === h.sourceMap && p.supports === h.supports && p.layer === h.layer) return;
              f.update(h = p);
            } else f.remove();
          };
        }
        s.exports = function(h, u) {
          var f = c(h = h || [], u = u || {});
          return function(p) {
            p = p || [];
            for (var k = 0; k < f.length; k++) {
              var T = l(f[k]);
              a[T].references--;
            }
            for (var v = c(p, u), m = 0; m < f.length; m++) {
              var C = l(f[m]);
              a[C].references === 0 && (a[C].updater(), a.splice(C, 1));
            }
            f = v;
          };
        };
      }, 659: (s) => {
        var a = {};
        s.exports = function(l, c) {
          var d = (function(h) {
            if (a[h] === void 0) {
              var u = document.querySelector(h);
              if (window.HTMLIFrameElement && u instanceof window.HTMLIFrameElement) try {
                u = u.contentDocument.head;
              } catch {
                u = null;
              }
              a[h] = u;
            }
            return a[h];
          })(l);
          if (!d) throw new Error("Couldn't find a style target. This probably means that the value for the 'insert' parameter is invalid.");
          d.appendChild(c);
        };
      }, 540: (s) => {
        s.exports = function(a) {
          var l = document.createElement("style");
          return a.setAttributes(l, a.attributes), a.insert(l, a.options), l;
        };
      }, 56: (s, a, l) => {
        s.exports = function(c) {
          var d = l.nc;
          d && c.setAttribute("nonce", d);
        };
      }, 825: (s) => {
        s.exports = function(a) {
          if (typeof document > "u") return { update: function() {
          }, remove: function() {
          } };
          var l = a.insertStyleElement(a);
          return { update: function(c) {
            (function(d, h, u) {
              var f = "";
              u.supports && (f += "@supports (".concat(u.supports, ") {")), u.media && (f += "@media ".concat(u.media, " {"));
              var p = u.layer !== void 0;
              p && (f += "@layer".concat(u.layer.length > 0 ? " ".concat(u.layer) : "", " {")), f += u.css, p && (f += "}"), u.media && (f += "}"), u.supports && (f += "}");
              var k = u.sourceMap;
              k && typeof btoa < "u" && (f += `
/*# sourceMappingURL=data:application/json;base64,`.concat(btoa(unescape(encodeURIComponent(JSON.stringify(k)))), " */")), h.styleTagTransform(f, d, h.options);
            })(l, a, c);
          }, remove: function() {
            (function(c) {
              if (c.parentNode === null) return !1;
              c.parentNode.removeChild(c);
            })(l);
          } };
        };
      }, 113: (s) => {
        s.exports = function(a, l) {
          if (l.styleSheet) l.styleSheet.cssText = a;
          else {
            for (; l.firstChild; ) l.removeChild(l.firstChild);
            l.appendChild(document.createTextNode(a));
          }
        };
      } }, r = {};
      function n(s) {
        var a = r[s];
        if (a !== void 0) return a.exports;
        var l = r[s] = { id: s, exports: {} };
        return t[s](l, l.exports, n), l.exports;
      }
      n.n = (s) => {
        var a = s && s.__esModule ? () => s.default : () => s;
        return n.d(a, { a }), a;
      }, n.d = (s, a) => {
        for (var l in a) n.o(a, l) && !n.o(s, l) && Object.defineProperty(s, l, { enumerable: !0, get: a[l] });
      }, n.o = (s, a) => Object.prototype.hasOwnProperty.call(s, a), n.nc = void 0;
      var i = {};
      return (() => {
        n.d(i, { default: () => I });
        var s = n(72), a = n.n(s), l = n(825), c = n.n(l), d = n(659), h = n.n(d), u = n(56), f = n.n(u), p = n(540), k = n.n(p), T = n(113), v = n.n(T), m = n(523), C = {};
        function S(w) {
          return S = typeof Symbol == "function" && typeof Symbol.iterator == "symbol" ? function(b) {
            return typeof b;
          } : function(b) {
            return b && typeof Symbol == "function" && b.constructor === Symbol && b !== Symbol.prototype ? "symbol" : typeof b;
          }, S(w);
        }
        function _(w, b) {
          for (var E = 0; E < b.length; E++) {
            var y = b[E];
            y.enumerable = y.enumerable || !1, y.configurable = !0, "value" in y && (y.writable = !0), Object.defineProperty(w, x(y.key), y);
          }
        }
        function x(w) {
          var b = (function(E, y) {
            if (S(E) != "object" || !E) return E;
            var B = E[Symbol.toPrimitive];
            if (B !== void 0) {
              var M = B.call(E, "string");
              if (S(M) != "object") return M;
              throw new TypeError("@@toPrimitive must return a primitive value.");
            }
            return String(E);
          })(w);
          return S(b) == "symbol" ? b : b + "";
        }
        C.styleTagTransform = v(), C.setAttributes = f(), C.insert = h().bind(null, "head"), C.domAPI = c(), C.insertStyleElement = k(), a()(m.A, C), m.A && m.A.locals && m.A.locals;
        var I = (function() {
          return w = function y(B, M) {
            var P = B.configuration, O = B.blocks, $ = B.toolbar, ue = B.save;
            (function(ee, Le) {
              if (!(ee instanceof Le)) throw new TypeError("Cannot call a class as a function");
            })(this, y), this.toolbar = $, this.borderStyle = M || "1px dashed #aaa", this.api = O, this.holder = typeof P.holder == "string" ? document.getElementById(P.holder) : P.holder, this.readOnly = P.readOnly, this.startBlock = null, this.endBlock = null, this.save = ue, this.setDragListener(), this.setDropListener();
          }, E = [{ key: "isReadOnlySupported", get: function() {
            return !0;
          } }], (b = [{ key: "setElementCursor", value: function(y) {
            if (y) {
              var B = document.createRange(), M = window.getSelection();
              B.setStart(y.childNodes[0], 0), B.collapse(!0), M.removeAllRanges(), M.addRange(B), y.focus();
            }
          } }, { key: "setDragListener", value: function() {
            var y = this;
            if (!this.readOnly) {
              var B = this.holder.querySelector(".ce-toolbar__settings-btn");
              if (B) this.initializeDragListener(B);
              else {
                var M = new MutationObserver((function(P, O) {
                  var $ = y.holder.querySelector(".ce-toolbar__settings-btn");
                  $ && (y.initializeDragListener($), O.disconnect());
                }));
                M.observe(this.holder, { childList: !0, subtree: !0 });
              }
            }
          } }, { key: "initializeDragListener", value: function(y) {
            var B = this;
            y.setAttribute("draggable", "true"), y.addEventListener("dragstart", (function() {
              B.startBlock = B.api.getCurrentBlockIndex();
            })), y.addEventListener("drag", (function() {
              if (B.toolbar.close(), !B.isTheOnlyBlock()) {
                var M = B.holder.querySelectorAll(".ce-block"), P = B.holder.querySelector(".ce-block--drop-target");
                B.setElementCursor(P), B.setBorderBlocks(M, P);
              }
            }));
          } }, { key: "setBorderBlocks", value: function(y, B) {
            var M = this;
            Object.values(y).forEach((function(P) {
              var O = P.querySelector(".ce-block__content");
              P !== B ? (O.style.removeProperty("border-top"), O.style.removeProperty("border-bottom")) : Object.keys(y).find((function($) {
                return y[$] === B;
              })) > M.startBlock ? O.style.borderBottom = M.borderStyle : O.style.borderTop = M.borderStyle;
            }));
          } }, { key: "setDropListener", value: function() {
            var y = this;
            document.addEventListener("drop", (function(B) {
              var M = B.target;
              if (y.holder.contains(M) && y.startBlock !== null) {
                var P = y.getDropTarget(M);
                if (P) {
                  var O = P.querySelector(".ce-block__content");
                  O.style.removeProperty("border-top"), O.style.removeProperty("border-bottom"), y.endBlock = y.getTargetPosition(P), y.moveBlocks();
                }
              }
              y.startBlock = null;
            }));
          } }, { key: "getDropTarget", value: function(y) {
            return y.classList.contains("ce-block") ? y : y.closest(".ce-block");
          } }, { key: "getTargetPosition", value: function(y) {
            return Array.from(y.parentNode.children).indexOf(y);
          } }, { key: "isTheOnlyBlock", value: function() {
            return this.api.getBlocksCount() === 1;
          } }, { key: "moveBlocks", value: function() {
            this.isTheOnlyBlock() || this.api.move(this.endBlock, this.startBlock);
          } }]) && _(w.prototype, b), E && _(w, E), Object.defineProperty(w, "prototype", { writable: !1 }), w;
          var w, b, E;
        })();
      })(), i.default;
    })()));
  })(Rt)), Rt.exports;
}
var Sh = Th();
const Bh = /* @__PURE__ */ Hi(Sh);
var Ht = { exports: {} }, an;
function Mh() {
  return an || (an = 1, (function(o, e) {
    (function(t, r) {
      o.exports = r();
    })(self, (() => (() => {
      var t = { 31: function(i, s) {
        (function(a) {
          a.IconAddBackground = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 19V19C9.13623 19 8.20435 19 7.46927 18.6955C6.48915 18.2895 5.71046 17.5108 5.30448 16.5307C5 15.7956 5 14.8638 5 13V12C5 9.19108 5 7.78661 5.67412 6.77772C5.96596 6.34096 6.34096 5.96596 6.77772 5.67412C7.78661 5 9.19108 5 12 5H13.5C14.8956 5 15.5933 5 16.1611 5.17224C17.4395 5.56004 18.44 6.56046 18.8278 7.83886C19 8.40666 19 9.10444 19 10.5V10.5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 13V16M16 19V16M19 16H16M16 16H13"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6.5 17.5L17.5 6.5"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.9919 10.5H19.0015"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.9919 19H11.0015"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13L13 5"/></svg>', a.IconAddBorder = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.9919 9.5H19.0015"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.5 5H14.5096"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M14.625 5H15C17.2091 5 19 6.79086 19 9V9.375"/><path stroke="currentColor" stroke-width="2" d="M9.375 5L9 5C6.79086 5 5 6.79086 5 9V9.375"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.3725 5H9.38207"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 9.5H5.00957"/><path stroke="currentColor" stroke-width="2" d="M9.375 19H9C6.79086 19 5 17.2091 5 15V14.625"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.3725 19H9.38207"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 14.55H5.00957"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 13V16M16 19V16M19 16H16M16 16H13"/></svg>', a.IconAlignCenter = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 7L6 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 17H6"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 12L8 12"/></svg>', a.IconAlignJustify = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 7L6 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 17H6"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 12L6 12"/></svg>', a.IconAlignLeft = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17 7L5 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17 17H5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M13 12L5 12"/></svg>', a.IconAlignRight = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19 7L7 7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19 17H7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19 12L11 12"/></svg>', a.IconBold = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9 12L9 7.1C9 7.04477 9.04477 7 9.1 7H10.4C11.5 7 14 7.1 14 9.5C14 9.5 14 12 11 12M9 12V16.8C9 16.9105 9.08954 17 9.2 17H12.5C14 17 15 16 15 14.5C15 11.7046 11 12 11 12M9 12H11"/></svg>', a.IconBrackets = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 8L5 12L9 16"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 8L19 12L15 16"/></svg>', a.IconCheck = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 12L10.4884 15.8372C10.5677 15.9245 10.705 15.9245 10.7844 15.8372L17 9"/></svg>', a.IconChecklist = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9.2 12L11.0586 13.8586C11.1367 13.9367 11.2633 13.9367 11.3414 13.8586L14.7 10.5"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>', a.IconChevronDown = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 10L11.8586 14.8586C11.9367 14.9367 12.0633 14.9367 12.1414 14.8586L17 10"/></svg>', a.IconChevronLeft = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M14.5 17.5L9.64142 12.6414C9.56331 12.5633 9.56331 12.4367 9.64142 12.3586L14.5 7.5"/></svg>', a.IconChevronRight = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9.58284 17.5L14.4414 12.6414C14.5195 12.5633 14.5195 12.4367 14.4414 12.3586L9.58284 7.5"/></svg>', a.IconChevronUp = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 15L11.8586 10.1414C11.9367 10.0633 12.0633 10.0633 12.1414 10.1414L17 15"/></svg>', a.IconClipboard = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.42857 7H7.71429C7.25963 7 6.82359 7.15804 6.5021 7.43934C6.18061 7.72064 6 8.10218 6 8.5V17.5C6 17.8978 6.18061 18.2794 6.5021 18.5607C6.82359 18.842 7.25963 19 7.71429 19H16.2857C16.7404 19 17.1764 18.842 17.4979 18.5607C17.8194 18.2794 18 17.8978 18 17.5V8.5C18 8.10218 17.8194 7.72064 17.4979 7.43934C17.1764 7.15804 16.7404 7 16.2857 7H14.5714"/><rect width="5.15789" height="3.36842" x="9.42105" y="5" stroke="currentColor" stroke-width="2" rx="1.5"/></svg>', a.IconCollapse = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 9L10 12M10 12L7 15M10 12H4"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 9L14 12M14 12L17 15M14 12H20"/></svg>', a.IconColor = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M5.24296 11.4075C5.23167 10.6253 5.52446 9.8395 6.12132 9.24264L9.65686 5.70711C10.0474 5.31658 10.6809 5.31693 11.0714 5.70745L16.0205 10.6565C16.2268 10.8629 16.3243 11.1371 16.3126 11.4075M5.24296 11.4075C5.25382 12.1607 5.54661 12.9106 6.12132 13.4853L8 15.364M5.24296 11.4075H11.9565M16.3126 11.4075C16.3022 11.6487 16.205 11.8869 16.0208 12.0711L12.4853 15.6066C11.3137 16.7782 9.41421 16.7782 8.24264 15.6066L8 15.364M16.3126 11.4075H11.9565M8 15.364L11.9565 11.4075"/><path stroke="currentColor" stroke-width="2" d="M20 17.4615C20 18.3112 19.3284 19 18.5 19C17.6716 19 17 18.3112 17 17.4615C17 16.6119 17.9 15.6154 18.5 15C19.1 15.6154 20 16.6119 20 17.4615Z"/></svg>', a.IconCopy = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.25 8.5H10.25C9.2835 8.5 8.5 9.2835 8.5 10.25V17.25C8.5 18.2165 9.2835 19 10.25 19H17.25C18.2165 19 19 18.2165 19 17.25V10.25C19 9.2835 18.2165 8.5 17.25 8.5Z"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.5 8.5V6.75C15.5 6.28587 15.3156 5.84075 14.9874 5.51256C14.6592 5.18437 14.2141 5 13.75 5H6.75C6.28587 5 5.84075 5.18437 5.51256 5.51256C5.18437 5.84075 5 6.28587 5 6.75V13.75C5 14.2141 5.18437 14.6592 5.51256 14.9874C5.84075 15.3156 6.28587 15.5 6.75 15.5H8.5"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 12L15.5 12"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15.5L15.5 15.5"/></svg>', a.IconCross = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8 8L12 12M12 12L16 16M12 12L16 8M12 12L8 16"/></svg>', a.IconCurlyBrackets = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 17C7 17 7 15.2536 7 13.5L5.5 12L7 10.5C7 8.74644 7 7 9 7"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 17C17 17 17 15.2536 17 13.5L18.5 12L17 10.5C17 8.74644 17 7 15 7"/></svg>', a.IconDelimiter = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="6" x2="10" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="14" x2="18" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>', a.IconDirectionDownRight = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.8833 9.16666L18.2167 12.5M18.2167 12.5L14.8833 15.8333M18.2167 12.5H10.05C9.16594 12.5 8.31809 12.1488 7.69297 11.5237C7.06785 10.8986 6.71666 10.0507 6.71666 9.16666"/></svg>', a.IconDirectionLeftDown = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.9167 14.9167L11.5833 18.25M11.5833 18.25L8.25 14.9167M11.5833 18.25L11.5833 10.0833C11.5833 9.19928 11.9345 8.35143 12.5596 7.72631C13.1848 7.10119 14.0326 6.75 14.9167 6.75"/></svg>', a.IconDirectionRightDown = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.13333 14.9167L12.4667 18.25M12.4667 18.25L15.8 14.9167M12.4667 18.25L12.4667 10.0833C12.4667 9.19928 12.1155 8.35143 11.4904 7.72631C10.8652 7.10119 10.0174 6.75 9.13333 6.75"/></svg>', a.IconDirectionUpRight = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.8833 15.8333L18.2167 12.5M18.2167 12.5L14.8833 9.16667M18.2167 12.5L10.05 12.5C9.16595 12.5 8.31811 12.8512 7.69299 13.4763C7.06787 14.1014 6.71667 14.9493 6.71667 15.8333"/></svg>', a.IconDotCircle = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><circle cx="12" cy="12" r="4" stroke="currentColor" stroke-width="2"/></svg>', a.IconEtcHorisontal = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M7.30499 11.995L7.30499 12.005M12.005 11.995V12.005M16.705 11.995L16.705 12.005"/></svg>', a.IconEtcVertical = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M12.01 7.29999H12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M12.01 12H12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M12.01 16.7H12"/></svg>', a.IconFile = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.3236 8.43554L9.49533 12.1908C9.13119 12.5505 8.93118 13.043 8.9393 13.5598C8.94741 14.0767 9.163 14.5757 9.53862 14.947C9.91424 15.3182 10.4191 15.5314 10.9422 15.5397C11.4653 15.5479 11.9637 15.3504 12.3279 14.9908L16.1562 11.2355C16.8845 10.5161 17.2845 9.53123 17.2682 8.4975C17.252 7.46376 16.8208 6.46583 16.0696 5.72324C15.3184 4.98066 14.3086 4.55425 13.2624 4.53782C12.2162 4.52138 11.2193 4.91627 10.4911 5.63562L6.66277 9.39093C5.57035 10.4699 4.97032 11.9473 4.99467 13.4979C5.01903 15.0485 5.66578 16.5454 6.79264 17.6592C7.9195 18.7731 9.43417 19.4127 11.0034 19.4374C12.5727 19.462 14.068 18.8697 15.1604 17.7907L18.9887 14.0354"/></svg>', a.IconGift = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="12" height="6" x="6" y="13" stroke="currentColor" stroke-width="2" rx="2"/><line x1="12" x2="12" y1="9" y2="19" stroke="currentColor" stroke-width="2"/><path stroke="currentColor" stroke-width="2" d="M5 11C5 9.89543 5.89543 9 7 9H17C18.1046 9 19 9.89543 19 11V11C19 12.1046 18.1046 13 17 13H7C5.89543 13 5 12.1046 5 11V11Z"/><path stroke="currentColor" stroke-width="2" d="M16 9C16 7.89543 16 6 14 6C12 6 12 7.89543 12 9C12 7.89543 12 6 10 6C8 6 8 7.89543 8 9"/></svg>', a.IconGlobe = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M18 12C18 15.3137 15.3137 18 12 18C8.68629 18 6 15.3137 6 12M18 12C18 8.68629 15.3137 6 12 6C8.68629 6 6 8.68629 6 12M18 12H6M11.7 6C11.7 6 9.7 7.63811 9.7 12C9.7 16.9 11.7 18 11.7 18M12.3 6C12.3 6 14.3 7.63811 14.3 12C14.3 16.9 12.3 18 12.3 18"/></svg>', a.IconH1 = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19 17V10.2135C19 10.1287 18.9011 10.0824 18.836 10.1367L16 12.5"/></svg>', a.IconH2 = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 11C16 10 19 9.5 19 12C19 13.9771 16.0684 13.9997 16.0012 16.8981C15.9999 16.9533 16.0448 17 16.1 17L19.3 17"/></svg>', a.IconH3 = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 11C16 10.5 16.8323 10 17.6 10C18.3677 10 19.5 10.311 19.5 11.5C19.5 12.5315 18.7474 12.9022 18.548 12.9823C18.5378 12.9864 18.5395 13.0047 18.5503 13.0063C18.8115 13.0456 20 13.3065 20 14.8C20 16 19.5 17 17.8 17C17.8 17 16 17 16 16.3"/></svg>', a.IconH4 = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M18 10L15.2834 14.8511C15.246 14.9178 15.294 15 15.3704 15C16.8489 15 18.7561 15 20.2 15M19 17C19 15.7187 19 14.8813 19 13.6"/></svg>', a.IconH5 = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16 15.9C16 15.9 16.3768 17 17.8 17C19.5 17 20 15.6199 20 14.7C20 12.7323 17.6745 12.0486 16.1635 12.9894C16.094 13.0327 16 12.9846 16 12.9027V10.1C16 10.0448 16.0448 10 16.1 10H19.8"/></svg>', a.IconH6 = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7L6 12M6 17L6 12M6 12L12 12M12 7V12M12 17L12 12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19.5 10C16.5 10.5 16 13.3285 16 15M16 15V15C16 16.1046 16.8954 17 18 17H18.3246C19.3251 17 20.3191 16.3492 20.2522 15.3509C20.0612 12.4958 16 12.6611 16 15Z"/></svg>', a.IconHeading = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9 7L9 12M9 17V12M9 12L15 12M15 7V12M15 17L15 12"/></svg>', a.IconHeart = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M6.6 7.50001C5.27451 8.82549 5.19999 10.6 6.59999 12.3C8 14 12.2 17.9 12.2 17.9C12.2 17.9 16.5 14 17.8 12.3C19.1 10.6 19.1255 8.82549 17.8 7.5C16.4745 6.17452 14.3255 6.17452 13 7.5L12.2 8.30001L11.4 7.50001C10.0745 6.17453 7.92548 6.17453 6.6 7.50001Z"/></svg>', a.IconHidden = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6.77778 6L18.5 17.7222"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.687 10C10.2473 10.4392 10.0002 11.035 10 11.6564C9.99978 12.2777 10.2465 12.8737 10.6858 13.3132C11.1251 13.7527 11.7211 13.9998 12.3427 14C12.9642 14.0002 13.5604 13.7536 14 13.3144"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 7C17 11.1666 20 11.17 20 11.67C20 12.17 19 13.17 19 13.17M8.2424 8.80936C7.59317 9.22876 6.97961 9.76732 6.4017 10.4251C5.70398 11.2193 5.35512 11.6164 5.35513 12.3702C5.35514 13.124 5.70406 13.5211 6.40191 14.3154C7.99587 16.1297 9.8618 17.0367 12 17.0367C13.1102 17.0367 14.1466 16.7917 15.1111 16.3024"/></svg>', a.IconHtml = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16.6954 5C17.912 5 18.8468 6.07716 18.6755 7.28165L17.426 16.0659C17.3183 16.8229 16.7885 17.4522 16.061 17.6873L12.6151 18.8012C12.2152 18.9304 11.7848 18.9304 11.3849 18.8012L7.93898 17.6873C7.21148 17.4522 6.6817 16.8229 6.57403 16.0659L5.32454 7.28165C5.15322 6.07716 6.088 5 7.30461 5H16.6954Z"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 8.4H9L9.42857 11.7939H14.5714L14.3571 13.2788L14.1429 14.7636L12 15.4L9.85714 14.7636L9.77143 14.3394"/></svg>', a.IconInstagram = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M15.9 8.1V8.11"/><circle cx="12" cy="12" r="3" stroke="currentColor" stroke-width="2"/></svg>', a.IconItalic = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M13.34 10C12.4223 12.7337 11 17 11 17"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M14.21 7H14.2"/></svg>', a.IconLink = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.69998 12.6L7.67896 12.62C6.53993 13.7048 6.52012 15.5155 7.63516 16.625V16.625C8.72293 17.7073 10.4799 17.7102 11.5712 16.6314L13.0263 15.193C14.0703 14.1609 14.2141 12.525 13.3662 11.3266L13.22 11.12"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16.22 11.12L16.3564 10.9805C17.2895 10.0265 17.3478 8.5207 16.4914 7.49733V7.49733C15.5691 6.39509 13.9269 6.25143 12.8271 7.17675L11.3901 8.38588C10.0935 9.47674 9.95706 11.4241 11.0888 12.6852L11.12 12.72"/></svg>', a.IconLinkedin = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/><line x1="9" x2="9" y1="11.4" y2="15.4" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M9 8.7V8.71"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 11.4V12M12 15.4V12M12 12C14 11.5 15 11.3611 15 12.5C15 13.5 15 15.4 15 15.4"/></svg>', a.IconListBulleted = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="9" x2="19" y1="7" y2="7" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="9" x2="19" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="9" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 17H4.99002"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 12H4.99002"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M5.00001 7H4.99002"/></svg>', a.IconListNumbered = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="12" x2="19" y1="7" y2="7" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="12" x2="19" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="12" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.79999 14L7.79999 7.2135C7.79999 7.12872 7.7011 7.0824 7.63597 7.13668L4.79999 9.5"/></svg>', a.IconLoader = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 6.99998C9.1747 6.99987 6.99997 9.24998 7 12C7.00003 14.55 9.02119 17 12 17C14.7712 17 17 14.75 17 12"><animateTransform attributeName="transform" attributeType="XML" dur="560ms" from="0,12,12" repeatCount="indefinite" to="360,12,12" type="rotate"/></path></svg>', a.IconMarker = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M11.3535 9.31802L12.7678 7.90381C13.5488 7.12276 14.8151 7.12276 15.5962 7.90381C16.3772 8.68486 16.3772 9.95119 15.5962 10.7322L14.182 12.1464M11.3535 9.31802L7.96729 12.7043C7.40889 13.2627 7.02826 13.9739 6.87339 14.7482L6.69798 15.6253C6.55803 16.325 7.17495 16.942 7.87467 16.802L8.75175 16.6266C9.52612 16.4717 10.2373 16.0911 10.7957 15.5327L14.182 12.1464M11.3535 9.31802L14.182 12.1464"/><line x1="15" x2="19" y1="17" y2="17" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>', a.IconMenu = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.40999 7.29999H9.4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 7.29999H14.59"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.30999 12H9.3"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 12H14.59"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.40999 16.7H9.4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 16.7H14.59"/></svg>', a.IconMenuSmall = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.41 9.66H9.4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 9.66H14.59"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M9.31 14.36H9.3"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2.6" d="M14.6 14.36H14.59"/></svg>', a.IconPicture = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5.13968 15.32L8.69058 11.5661C9.02934 11.2036 9.48873 11 9.96774 11C10.4467 11 10.9061 11.2036 11.2449 11.5661L15.3871 16M13.5806 14.0664L15.0132 12.533C15.3519 12.1705 15.8113 11.9668 16.2903 11.9668C16.7693 11.9668 17.2287 12.1705 17.5675 12.533L18.841 13.9634"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.7778 9.33331H13.7867"/></svg>', a.IconPlay = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M10 10.5606V13.4394C10 14.4777 11.1572 15.0971 12.0211 14.5211L14.1803 13.0817C14.9536 12.5661 14.9503 11.4317 14.18 10.9181L12.0214 9.47907C11.1591 8.9042 10 9.5203 10 10.5606Z"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>', a.IconPlus = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 7V12M12 17V12M17 12H12M12 12H7"/></svg>', a.IconQuestion = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 15.52V15.51"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M10.0024 9.97655C10.1567 9.01858 11 8.5 12 8.5C13 8.5 13.6857 9.17188 13.8693 9.70703C14.0529 10.2422 14.0135 11.0514 13.5067 11.5159C13 11.9805 12.7344 11.832 12.2784 12.3168C12.1134 12.4923 12 12.7476 12 12.7476"/></svg>', a.IconQuote = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 10.8182L9 10.8182C8.80222 10.8182 8.60888 10.7649 8.44443 10.665C8.27998 10.5651 8.15181 10.4231 8.07612 10.257C8.00043 10.0909 7.98063 9.90808 8.01922 9.73174C8.0578 9.55539 8.15304 9.39341 8.29289 9.26627C8.43275 9.13913 8.61093 9.05255 8.80491 9.01747C8.99889 8.98239 9.19996 9.00039 9.38268 9.0692C9.56541 9.13801 9.72159 9.25453 9.83147 9.40403C9.94135 9.55353 10 9.72929 10 9.90909L10 12.1818C10 12.664 9.78929 13.1265 9.41421 13.4675C9.03914 13.8084 8.53043 14 8 14"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 10.8182L15 10.8182C14.8022 10.8182 14.6089 10.7649 14.4444 10.665C14.28 10.5651 14.1518 10.4231 14.0761 10.257C14.0004 10.0909 13.9806 9.90808 14.0192 9.73174C14.0578 9.55539 14.153 9.39341 14.2929 9.26627C14.4327 9.13913 14.6109 9.05255 14.8049 9.01747C14.9989 8.98239 15.2 9.00039 15.3827 9.0692C15.5654 9.13801 15.7216 9.25453 15.8315 9.40403C15.9414 9.55353 16 9.72929 16 9.90909L16 12.1818C16 12.664 15.7893 13.1265 15.4142 13.4675C15.0391 13.8084 14.5304 14 14 14"/></svg>', a.IconRedo = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.6667 13.6667L18 10.3333L14.6667 7M18 10.3333H8.83333C7.94928 10.3333 7.10143 10.6845 6.47631 11.3096C5.85119 11.9348 5.5 12.7826 5.5 13.6667C5.5 14.5507 5.85119 15.3986 6.47631 16.0237C7.10143 16.6488 7.94928 17 8.83333 17H9.66667"/></svg>', a.IconRemoveBackground = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 19V19C9.13623 19 8.20435 19 7.46927 18.6955C6.48915 18.2895 5.71046 17.5108 5.30448 16.5307C5 15.7956 5 14.8638 5 13V12C5 9.19108 5 7.78661 5.67412 6.77772C5.96596 6.34096 6.34096 5.96596 6.77772 5.67412C7.78661 5 9.19108 5 12 5H13.5C14.8956 5 15.5933 5 16.1611 5.17224C17.4395 5.56004 18.44 6.56046 18.8278 7.83886C19 8.40666 19 9.10444 19 10.5V10.5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M19.1187 14.8787L16.9974 17M14.876 19.1213L16.9974 17M19.1187 19.1213L16.9974 17M16.9974 17L14.876 14.8787"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6.5 17.5L17.5 6.5"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.9919 10.5H19.0015"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.9919 19H11.0015"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13L13 5"/></svg>', a.IconReplace = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M11.5 17.5L5 11M5 11V15.5M5 11H9.5"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12.5 6.5L19 13M19 13V8.5M19 13H14.5"/></svg>', a.IconSave = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M15.078 5.62637L15.6153 4.78296L15.078 5.62637C15.4261 5.84808 15.7393 6.15354 16.5711 6.98528L17.2782 6.27817L16.5711 6.98528L17.5251 7.93934C17.8347 8.2489 17.9496 8.36494 18.0489 8.48177C18.5907 9.11982 18.9188 9.91178 18.9868 10.7461C18.9992 10.8989 19 11.0622 19 11.5V12C19 13.4166 18.9992 14.419 18.9352 15.2026C18.8721 15.9745 18.7527 16.4457 18.564 16.816C18.1805 17.5686 17.5686 18.1805 16.816 18.564C16.4457 18.7527 15.9745 18.8721 15.2026 18.9352C14.419 18.9992 13.4166 19 12 19C10.5834 19 9.58104 18.9992 8.79744 18.9352C8.02552 18.8721 7.55435 18.7527 7.18404 18.564C6.43139 18.1805 5.81947 17.5686 5.43597 16.816C5.24729 16.4457 5.12787 15.9745 5.0648 15.2026C5.00078 14.419 5 13.4166 5 12V11.7782C5 10.4673 5.00067 9.53987 5.05572 8.81299C5.10998 8.09655 5.21284 7.65673 5.37487 7.3093C5.77229 6.45718 6.45718 5.77229 7.3093 5.37487C7.65673 5.21284 8.09655 5.10998 8.81299 5.05572C9.53986 5.00067 10.4673 5 11.7782 5C12.9544 5 13.3919 5.00552 13.7948 5.09484C14.2503 5.19583 14.6846 5.37572 15.078 5.62637Z"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15C13.1046 15 14 14.1046 14 13C14 11.8954 13.1046 11 12 11C10.8954 11 10 11.8954 10 13C10 14.1046 10.8954 15 12 15Z"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5.5V7C14 7.55228 13.5523 8 13 8H11C10.4477 8 10 7.55228 10 7V5.2"/></svg>', a.IconSearch = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><circle cx="10.5" cy="10.5" r="5.5" stroke="currentColor" stroke-width="2"/><line x1="15.4142" x2="19" y1="15" y2="18.5858" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>', a.IconStar = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M11.8197 6.04369C11.8924 5.8925 12.1076 5.8925 12.1803 6.04369L13.9776 9.78496C14.0068 9.84564 14.0645 9.88759 14.1312 9.89657L18.2448 10.4498C18.411 10.4722 18.4776 10.6769 18.3562 10.7927L15.3535 13.6582C15.3048 13.7047 15.2827 13.7726 15.2948 13.8388L16.0398 17.922C16.0699 18.087 15.8957 18.2136 15.7481 18.1339L12 16.1124L8.25192 18.1339C8.10429 18.2136 7.93012 18.087 7.96022 17.922L8.7052 13.8388C8.71728 13.7726 8.69523 13.7047 8.64652 13.6582L5.64378 10.7927C5.52244 10.6769 5.58896 10.4722 5.7552 10.4498L9.86876 9.89657C9.93549 9.88759 9.99322 9.84564 10.0224 9.78496L11.8197 6.04369Z"/></svg>', a.IconStretch = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 9L20 12L17 15"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 12H20"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 9L4 12L7 15"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 12H10"/></svg>', a.IconStrikethrough = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14.5 8.50001C13.5 7 10.935 6.66476 9.75315 7.79706C9.27092 8.25909 9 8.88574 9 9.53915C9 10.1926 9.27092 10.8192 9.75315 11.2812C10.9835 12.46 13.0165 11.5457 14.2468 12.7244C14.7291 13.1865 15 13.8131 15 14.4665C15 15.1199 14.7291 15.7466 14.2468 16.2086C12.8659 17.5317 10 17.5 9 16"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 12H18"/></svg>', a.IconTable = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M10 5V18.5"/><path stroke="currentColor" stroke-width="2" d="M5 10H19"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>', a.IconTableWithHeadings = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M5 10H19"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>', a.IconTableWithoutHeadings = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M10 5V18.5"/><path stroke="currentColor" stroke-width="2" d="M14 5V18.5"/><path stroke="currentColor" stroke-width="2" d="M5 10H19"/><path stroke="currentColor" stroke-width="2" d="M5 14H19"/><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/></svg>', a.IconText = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8 9V7.2C8 7.08954 8.08954 7 8.2 7L12 7M16 9V7.2C16 7.08954 15.9105 7 15.8 7L12 7M12 7L12 17M12 17H10M12 17H14"/></svg>', a.IconTranslate = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7 17C8 14.5 12 12 13 9"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8.5 11C8.5 11 10 14 12.5 15"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6 7.7H16M11 7.7V5.7"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M14.5 18L15.2143 16M15.2143 16L16.9159 11.2354C16.9663 11.0942 17.1001 11 17.25 11C17.3999 11 17.5337 11.0942 17.5841 11.2354L19.2857 16M15.2143 16H19.2857M20 18L19.2857 16"/></svg>', a.IconTrash = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.1328 7.7234C18.423 7.7634 18.7115 7.80571 19 7.85109M18.1328 7.7234L17.2267 17.4023C17.1897 17.8371 16.973 18.2432 16.62 18.5394C16.267 18.8356 15.8037 19.0001 15.3227 19H8.67733C8.19632 19.0001 7.73299 18.8356 7.37998 18.5394C7.02698 18.2432 6.81032 17.8371 6.77333 17.4023L5.86715 7.7234M18.1328 7.7234C17.1536 7.58919 16.1693 7.48733 15.1818 7.41803M5.86715 7.7234C5.57697 7.76263 5.28848 7.80494 5 7.85032M5.86715 7.7234C6.84642 7.58919 7.83074 7.48733 8.81818 7.41803M15.1818 7.41803C13.0638 7.26963 10.9362 7.26963 8.81818 7.41803M15.1818 7.41803C15.1818 5.30368 13.7266 4.34834 12 4.34834C10.2734 4.34834 8.81818 5.43945 8.81818 7.41803"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.5 15.5L10 11"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 11L13.5 15.5"/></svg>', a.IconTwitter = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linejoin="round" stroke-width="2" d="M16.7893 7.87697C17.5 8 18.5 8 18.5 8C18.5 8 17.5 9.5 17.5 10C18.5 18.5 11.5 20.5 5.5 16.5C6.99996 16.6712 8.04617 16.5163 9.25234 15.6024C7.99546 15.58 5.36548 13.6033 5 12.5C6.5 13 8 12 8 12C6.52134 11.0446 4.93005 9.24114 5.97461 7.50832C7.39125 9.18838 9.50766 10.2939 11.8948 10.4097C11.2198 7.60755 14.9218 5.95341 16.7893 7.87697Z"/></svg>', a.IconUnderline = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 7.5V11.5C9 12.2956 9.31607 13.0587 9.87868 13.6213C10.4413 14.1839 11.2044 14.5 12 14.5C12.7956 14.5 13.5587 14.1839 14.1213 13.6213C14.6839 13.0587 15 12.2956 15 11.5V7.5"/><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7.71429 18H16.2857"/></svg>', a.IconUndo = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.33333 13.6667L6 10.3333L9.33333 7M6 10.3333H15.1667C16.0507 10.3333 16.8986 10.6845 17.5237 11.3096C18.1488 11.9348 18.5 12.7826 18.5 13.6667C18.5 14.5507 18.1488 15.3986 17.5237 16.0237C16.8986 16.6488 16.0507 17 15.1667 17H14.3333"/></svg>', a.IconUnlink = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M15.7795 11.5C15.7795 11.5 16.053 11.1962 16.5497 10.6722C17.4442 9.72856 17.4701 8.2475 16.5781 7.30145V7.30145C15.6482 6.31522 14.0873 6.29227 13.1288 7.25073L11.8796 8.49999"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M8.24517 12.3883C8.24517 12.3883 7.97171 12.6922 7.47504 13.2161C6.58051 14.1598 6.55467 15.6408 7.44666 16.5869V16.5869C8.37653 17.5731 9.93744 17.5961 10.8959 16.6376L12.1452 15.3883"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17.7802 15.1032L16.597 14.9422C16.0109 14.8624 15.4841 15.3059 15.4627 15.8969L15.4199 17.0818"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M6.39064 9.03238L7.58432 9.06668C8.17551 9.08366 8.6522 8.58665 8.61056 7.99669L8.5271 6.81397"/><line x1="12.1142" x2="11.7" y1="12.2" y2="11.7858" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>', a.IconUser = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M12 10C12.7145 10 13.239 9.56559 13.5392 9.11536C13.844 8.65814 14 8.0841 14 7.5C14 6.9159 13.844 6.34186 13.5392 5.88464C13.239 5.43441 12.7145 5 12 5C11.2855 5 10.761 5.43441 10.4608 5.88464C10.156 6.34186 10 6.9159 10 7.5C10 8.0841 10.156 8.65814 10.4608 9.11536C10.761 9.56559 11.2855 10 12 10Z"/><ellipse cx="12" cy="16" stroke="currentColor" stroke-width="2" rx="3" ry="5" transform="rotate(-90 12 16)"/></svg>', a.IconUsersGroup = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-width="2" d="M10 10C10.7145 10 11.239 9.56559 11.5392 9.11536C11.844 8.65814 12 8.0841 12 7.5C12 6.9159 11.844 6.34186 11.5392 5.88464C11.239 5.43441 10.7145 5 10 5C9.28547 5 8.761 5.43441 8.46084 5.88464C8.15603 6.34186 8 6.9159 8 7.5C8 8.0841 8.15603 8.65814 8.46084 9.11536C8.761 9.56559 9.28547 10 10 10Z"/><ellipse cx="10" cy="16" stroke="currentColor" stroke-width="2" rx="3" ry="5" transform="rotate(-90 10 16)"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M15.5555 10.2222C16.5374 10.2222 17.3333 9.42629 17.3333 8.44445C17.3333 7.46261 16.5374 6.66667 15.5555 6.66667"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M17.5 13C21 14.5 20.5 18 18 18.5"/></svg>', a.IconWarning = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><rect width="14" height="14" x="5" y="5" stroke="currentColor" stroke-width="2" rx="4"/><line x1="12" x2="12" y1="9" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M12 15.02V15.01"/></svg>', Object.defineProperties(a, { __esModule: { value: !0 }, [Symbol.toStringTag]: { value: "Module" } });
        })(s);
      }, 664: (i, s, a) => {
        a.d(s, { A: () => u });
        var l = a(601), c = a.n(l), d = a(314), h = a.n(d)()(c());
        h.push([i.id, ".cdx-alignment-tune.cdx-alignment-tune--left{text-align:left}.cdx-alignment-tune.cdx-alignment-tune--right{text-align:right}.cdx-alignment-tune.cdx-alignment-tune--center{text-align:center}.cdx-alignment-tune.cdx-alignment-tune--justify{text-align:justify}", ""]);
        const u = h;
      }, 314: (i) => {
        i.exports = function(s) {
          var a = [];
          return a.toString = function() {
            return this.map((function(l) {
              var c = "", d = l[5] !== void 0;
              return l[4] && (c += "@supports (".concat(l[4], ") {")), l[2] && (c += "@media ".concat(l[2], " {")), d && (c += "@layer".concat(l[5].length > 0 ? " ".concat(l[5]) : "", " {")), c += s(l), d && (c += "}"), l[2] && (c += "}"), l[4] && (c += "}"), c;
            })).join("");
          }, a.i = function(l, c, d, h, u) {
            typeof l == "string" && (l = [[null, l, void 0]]);
            var f = {};
            if (d) for (var p = 0; p < this.length; p++) {
              var k = this[p][0];
              k != null && (f[k] = !0);
            }
            for (var T = 0; T < l.length; T++) {
              var v = [].concat(l[T]);
              d && f[v[0]] || (u !== void 0 && (v[5] === void 0 || (v[1] = "@layer".concat(v[5].length > 0 ? " ".concat(v[5]) : "", " {").concat(v[1], "}")), v[5] = u), c && (v[2] && (v[1] = "@media ".concat(v[2], " {").concat(v[1], "}")), v[2] = c), h && (v[4] ? (v[1] = "@supports (".concat(v[4], ") {").concat(v[1], "}"), v[4] = h) : v[4] = "".concat(h)), a.push(v));
            }
          }, a;
        };
      }, 601: (i) => {
        i.exports = function(s) {
          return s[1];
        };
      }, 259: (i, s, a) => {
        a.r(s), a.d(s, { default: () => x });
        var l = a(72), c = a.n(l), d = a(825), h = a.n(d), u = a(659), f = a.n(u), p = a(56), k = a.n(p), T = a(540), v = a.n(T), m = a(113), C = a.n(m), S = a(664), _ = {};
        _.styleTagTransform = C(), _.setAttributes = k(), _.insert = f().bind(null, "head"), _.domAPI = h(), _.insertStyleElement = v(), c()(S.A, _);
        const x = S.A && S.A.locals ? S.A.locals : void 0;
      }, 72: (i) => {
        var s = [];
        function a(d) {
          for (var h = -1, u = 0; u < s.length; u++) if (s[u].identifier === d) {
            h = u;
            break;
          }
          return h;
        }
        function l(d, h) {
          for (var u = {}, f = [], p = 0; p < d.length; p++) {
            var k = d[p], T = h.base ? k[0] + h.base : k[0], v = u[T] || 0, m = "".concat(T, " ").concat(v);
            u[T] = v + 1;
            var C = a(m), S = { css: k[1], media: k[2], sourceMap: k[3], supports: k[4], layer: k[5] };
            if (C !== -1) s[C].references++, s[C].updater(S);
            else {
              var _ = c(S, h);
              h.byIndex = p, s.splice(p, 0, { identifier: m, updater: _, references: 1 });
            }
            f.push(m);
          }
          return f;
        }
        function c(d, h) {
          var u = h.domAPI(h);
          return u.update(d), function(f) {
            if (f) {
              if (f.css === d.css && f.media === d.media && f.sourceMap === d.sourceMap && f.supports === d.supports && f.layer === d.layer) return;
              u.update(d = f);
            } else u.remove();
          };
        }
        i.exports = function(d, h) {
          var u = l(d = d || [], h = h || {});
          return function(f) {
            f = f || [];
            for (var p = 0; p < u.length; p++) {
              var k = a(u[p]);
              s[k].references--;
            }
            for (var T = l(f, h), v = 0; v < u.length; v++) {
              var m = a(u[v]);
              s[m].references === 0 && (s[m].updater(), s.splice(m, 1));
            }
            u = T;
          };
        };
      }, 659: (i) => {
        var s = {};
        i.exports = function(a, l) {
          var c = (function(d) {
            if (s[d] === void 0) {
              var h = document.querySelector(d);
              if (window.HTMLIFrameElement && h instanceof window.HTMLIFrameElement) try {
                h = h.contentDocument.head;
              } catch {
                h = null;
              }
              s[d] = h;
            }
            return s[d];
          })(a);
          if (!c) throw new Error("Couldn't find a style target. This probably means that the value for the 'insert' parameter is invalid.");
          c.appendChild(l);
        };
      }, 540: (i) => {
        i.exports = function(s) {
          var a = document.createElement("style");
          return s.setAttributes(a, s.attributes), s.insert(a, s.options), a;
        };
      }, 56: (i, s, a) => {
        i.exports = function(l) {
          var c = a.nc;
          c && l.setAttribute("nonce", c);
        };
      }, 825: (i) => {
        i.exports = function(s) {
          if (typeof document > "u") return { update: function() {
          }, remove: function() {
          } };
          var a = s.insertStyleElement(s);
          return { update: function(l) {
            (function(c, d, h) {
              var u = "";
              h.supports && (u += "@supports (".concat(h.supports, ") {")), h.media && (u += "@media ".concat(h.media, " {"));
              var f = h.layer !== void 0;
              f && (u += "@layer".concat(h.layer.length > 0 ? " ".concat(h.layer) : "", " {")), u += h.css, f && (u += "}"), h.media && (u += "}"), h.supports && (u += "}");
              var p = h.sourceMap;
              p && typeof btoa < "u" && (u += `
/*# sourceMappingURL=data:application/json;base64,`.concat(btoa(unescape(encodeURIComponent(JSON.stringify(p)))), " */")), d.styleTagTransform(u, c, d.options);
            })(a, s, l);
          }, remove: function() {
            (function(l) {
              if (l.parentNode === null) return !1;
              l.parentNode.removeChild(l);
            })(a);
          } };
        };
      }, 113: (i) => {
        i.exports = function(s, a) {
          if (a.styleSheet) a.styleSheet.cssText = s;
          else {
            for (; a.firstChild; ) a.removeChild(a.firstChild);
            a.appendChild(document.createTextNode(s));
          }
        };
      }, 156: function(i, s, a) {
        var l, c, d = this && this.__classPrivateFieldGet || function(T, v, m, C) {
          if (m === "a" && !C) throw new TypeError("Private accessor was defined without a getter");
          if (typeof v == "function" ? T !== v || !C : !v.has(T)) throw new TypeError("Cannot read private member from an object whose class did not declare it");
          return m === "m" ? C : m === "a" ? C.call(T) : C ? C.value : v.get(T);
        };
        Object.defineProperty(s, "__esModule", { value: !0 });
        const h = a(31), u = a(31), f = a(31), p = a(31);
        a(259);
        class k {
          static get isTune() {
            return !0;
          }
          getDefaultAlignment() {
            const v = this.getCurrentBlockSettings();
            return v && v.default && d(this, l, "m", c).call(this, v.default) ? v.default : this.config.default && d(this, l, "m", c).call(this, this.config.default) ? this.config.default : k.DEFAULT_ALIGNMENT;
          }
          getAvailableBlockAlignments() {
            const v = [{ name: "left", icon: h.IconAlignLeft }, { name: "center", icon: u.IconAlignCenter }, { name: "right", icon: f.IconAlignRight }, { name: "justify", icon: p.IconAlignJustify }], m = this.getCurrentBlockSettings();
            if (m && m.availableAlignments) {
              const C = [];
              return m.availableAlignments.forEach(((S) => {
                if (d(this, l, "m", c).call(this, S)) {
                  const _ = v.find(((x) => x.name === S));
                  _ && C.push(_);
                }
              })), C.length > 0 ? C : v;
            }
            return v;
          }
          getCurrentBlockSettings() {
            if (this.config.blocks) {
              const v = this.config.blocks[this.block.name];
              if (v) return v;
            }
            return null;
          }
          constructor({ api: v, data: m, config: C, block: S }) {
            l.add(this), this.api = v, this.block = S, this.config = C, this.data = m ?? { alignment: this.getDefaultAlignment() }, this.alignments = this.getAvailableBlockAlignments(), this.styles = { base: "cdx-alignment-tune", alignment: { left: "cdx-alignment-tune--left", center: "cdx-alignment-tune--center", right: "cdx-alignment-tune--right", justify: "cdx-alignment-tune--justify" } }, this.wrapper = document.createElement("div");
          }
          wrap(v) {
            return this.wrapper.classList.add(this.styles.base), this.wrapper.classList.add(this.styles.alignment[this.data.alignment]), this.wrapper.append(v), this.wrapper;
          }
          render() {
            const v = document.createElement("div");
            return this.alignments.forEach(((m, C) => {
              const S = document.createElement("button");
              S.classList.add(this.api.styles.settingsButton), S.innerHTML = m.icon, S.type = "button", m.name === this.data.alignment && S.classList.add(this.api.styles.settingsButtonActive), S.addEventListener("click", (() => {
                var _;
                const x = this.data.alignment, I = this.alignments[C].name;
                this.data = { alignment: I }, this.block.dispatchChange(), (_ = v.querySelector(`button.${this.api.styles.settingsButton}.${this.api.styles.settingsButtonActive}`)) === null || _ === void 0 || _.classList.remove(this.api.styles.settingsButtonActive), S.classList.add(this.api.styles.settingsButtonActive), this.wrapper.classList.remove(this.styles.alignment[x]), this.wrapper.classList.add(this.styles.alignment[I]);
              })), v.appendChild(S);
            })), v;
          }
          save() {
            return this.data;
          }
        }
        l = /* @__PURE__ */ new WeakSet(), c = function(T) {
          const v = ["left", "center", "right", "justify"];
          return !!v.includes(T) || (console.error(`Package "editor-js-alignment-tune" error: Invalid alignment "${T}" provided. The available values are "${v.join('" | "')}".`), !1);
        }, k.DEFAULT_ALIGNMENT = "left", s.default = k;
      } }, r = {};
      function n(i) {
        var s = r[i];
        if (s !== void 0) return s.exports;
        var a = r[i] = { id: i, exports: {} };
        return t[i].call(a.exports, a, a.exports, n), a.exports;
      }
      return n.n = (i) => {
        var s = i && i.__esModule ? () => i.default : () => i;
        return n.d(s, { a: s }), s;
      }, n.d = (i, s) => {
        for (var a in s) n.o(s, a) && !n.o(i, a) && Object.defineProperty(i, a, { enumerable: !0, get: s[a] });
      }, n.o = (i, s) => Object.prototype.hasOwnProperty.call(i, s), n.r = (i) => {
        typeof Symbol < "u" && Symbol.toStringTag && Object.defineProperty(i, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(i, "__esModule", { value: !0 });
      }, n.nc = void 0, n(156);
    })()));
  })(Ht)), Ht.exports;
}
var _h = Mh();
const Lh = /* @__PURE__ */ Hi(_h);
function Fi() {
  return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(o) {
    const e = Math.random() * 16 | 0;
    return (o === "x" ? e : e & 3 | 8).toString(16);
  });
}
class je {
  static randomUUID() {
    return Fi();
  }
  static createLabel(e, t, r) {
    const n = document.createElement("label");
    return n.innerHTML = r, n.classList.add(t), n.setAttribute("for", e), n;
  }
  static createInput(e, t, r, n) {
    const i = document.createElement("input");
    return i.setAttribute("type", n), t && i.setAttribute("value", t), r && i.setAttribute("placeholder", r), i.setAttribute("id", e), i.classList.add("cdx-input"), i;
  }
}
function Ih({ value: o, bridge: e, onChange: t }) {
  const r = we(null), n = we(null), i = we(o), s = we(t), a = we(e);
  i.current = o, s.current = t, a.current = e;
  const l = we("skrivlet-editor-" + Fi());
  Yi(() => {
    var v;
    const d = (v = r.current) == null ? void 0 : v.querySelector("#" + l.current);
    if (!d)
      return;
    const h = () => {
      const m = i.current;
      if (m)
        if (typeof m == "string")
          try {
            return JSON.parse(m);
          } catch (C) {
            return console.error("Error parsing SkrivLet initial data JSON:", C), {};
          }
        else
          return m ?? {};
      else return {};
    }, u = () => {
      const m = r.current;
      if (!m)
        return;
      const C = m.querySelectorAll(
        '.cdx-block:not([disable-hotkeys="true"]),.ce-header:not([disable-hotkeys="true"]),.cdx-input:not([disable-hotkeys="true"]),.cdx-checklist__item-text:not([disable-hotkeys="true"])'
      );
      for (let S = 0; S < C.length; S++)
        C[S].setAttribute("disable-hotkeys", "true");
    }, f = async (m) => {
      const { host: C, modalManager: S } = a.current;
      if (!S)
        return;
      const _ = S.open(C, Gi, {
        data: {
          multiple: !1
        }
      });
      try {
        const x = await _.onSubmit(), I = (x == null ? void 0 : x.selection) ?? [];
        I.length && m.applyMediaSelection(I[0]);
      } catch {
      }
    }, p = async (m, C) => {
      const { host: S, modalManager: _ } = a.current;
      if (!_)
        return;
      const x = _.open(S, Ji, {
        data: {
          config: {}
        }
      });
      try {
        const I = await x.onSubmit(), w = I == null ? void 0 : I.link;
        w && m.wrap(C, w.url ?? w.unique ?? "");
      } catch {
      }
    }, k = () => {
      class m {
        constructor({ api: x }) {
          this.button = null, this._state = !1, this.element = null, this.tag = "A", this.class = "cdx-link", this.api = x;
        }
        static get isInline() {
          return !0;
        }
        get state() {
          return this._state;
        }
        set state(x) {
          var I;
          this._state = x, (I = this.button) == null || I.classList.toggle(this.api.styles.inlineToolButtonActive, x);
        }
        static get sanitize() {
          return {
            a: {
              href: !0
            }
          };
        }
        render() {
          return this.button = document.createElement("button"), this.button.type = "button", this.button.innerHTML = '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M7.69998 12.6L7.67896 12.62C6.53993 13.7048 6.52012 15.5155 7.63516 16.625V16.625C8.72293 17.7073 10.4799 17.7102 11.5712 16.6314L13.0263 15.193C14.0703 14.1609 14.2141 12.525 13.3662 11.3266L13.22 11.12"></path><path stroke="currentColor" stroke-linecap="round" stroke-width="2" d="M16.22 11.12L16.3564 10.9805C17.2895 10.0265 17.3478 8.5207 16.4914 7.49733V7.49733C15.5691 6.39509 13.9269 6.25143 12.8271 7.17675L11.3901 8.38588C10.0935 9.47674 9.95706 11.4241 11.0888 12.6852L11.12 12.72"></path></svg>', this.button.classList.add(this.api.styles.inlineToolButton), this.button;
        }
        surround(x) {
          if (this.state) {
            this.unwrap(x);
            return;
          }
          p(this, x);
        }
        wrap(x, I) {
          const w = x.extractContents(), b = document.createElement(this.tag);
          b.classList.add(this.class), b.setAttribute("href", I), b.appendChild(w), x.insertNode(b), this.api.selection.expandToTag(b), this.element = b;
        }
        unwrap(x) {
          const I = this.api.selection.findParentTag(this.tag, this.class), w = x.extractContents();
          I == null || I.remove(), x.insertNode(w);
        }
        checkState() {
          const x = this.api.selection.findParentTag(this.tag);
          this.state = !!x;
        }
      }
      class C {
        static get toolbox() {
          return {
            title: "Image",
            icon: '<svg width="17" height="15" viewBox="0 0 336 276" xmlns="http://www.w3.org/2000/svg"><path d="M291 150V79c0-19-15-34-34-34H79c-19 0-34 15-34 34v42l67-44 81 72 56-29 42 30zm0 52l-43-30-56 30-81-67-66 39v23c0 19 15 34 34 34h178c17 0 31-13 34-29zM79 0h178c44 0 79 35 79 79v118c0 44-35 79-79 79H79c-44 0-79-35-79-79V79C0 35 35 0 79 0z"/></svg>'
          };
        }
        constructor({ data: x, api: I, config: w }) {
          this.api = I, this.config = w || {}, this.data = {
            url: x.url || "",
            alt: x.alt || "",
            udi: x.udi || ""
          };
        }
        render() {
          var I, w;
          this.wrapper = document.createElement("div"), this.input = document.createElement("input"), this.input.setAttribute("type", "hidden");
          const x = je.randomUUID();
          return this.altTextLabel = je.createLabel(x, "sr-only", "Alt text"), this.altTextInput = je.createInput(x, this.data.alt, "Enter alt text", "text"), this.wrapper.classList.add("simple-image"), this._createImage(this.data.url), this.button = document.createElement("button"), this.button.type = "button", this.button.classList.add("umb-group-builder__group-add-property"), this.button.classList.add("skriv-let__add-image-button"), this.button.textContent = (I = this.data) != null && I.url ? "Change image" : "Select an image", this.button.addEventListener("click", () => {
            f(this);
          }), (w = this.image) == null || w.addEventListener("click", () => {
            f(this);
          }), this.wrapper.appendChild(this.altTextLabel), this.wrapper.appendChild(this.altTextInput), this.wrapper.appendChild(this.button), this.wrapper.appendChild(this.input), this.wrapper;
        }
        applyMediaSelection(x) {
          var b;
          const I = x.url ?? x.image ?? "", w = x.name ?? "";
          this.data.url = I, this.data.alt = w, this.data.udi = x.unique ?? x.udi ?? "", this.data.width = parseInt(String(x.width)), this.data.height = parseInt(String(x.height)), this.input && (this.input.value = I), this.image && (this.image.src = I, this.image.alt = w), this.altTextInput && (this.altTextInput.value = w), this.button && (this.button.textContent = (b = this.data) != null && b.url ? "Change image" : "Select an image"), this.save(), setTimeout(() => {
            var E;
            (E = this.image) == null || E.scrollIntoView();
          }, 200);
        }
        _createImage(x) {
          var I;
          this.image = document.createElement("img"), this.image.src = x, this.image.alt = this.data.alt, (I = this.wrapper) == null || I.appendChild(this.image);
        }
        save() {
          var x;
          return {
            url: this.data.url,
            alt: ((x = this.altTextInput) == null ? void 0 : x.value) ?? "",
            udi: this.data.udi,
            width: this.data.width,
            height: this.data.height
          };
        }
        validate(x) {
          return !(!x.url.trim() || !x.udi.trim());
        }
      }
      class S extends rn {
        static get toolbox() {
          return {
            title: "Video",
            icon: '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-youtube w-6 h-6 mx-1"><path d="M2.5 17a24.12 24.12 0 0 1 0-10 2 2 0 0 1 1.4-1.4 49.56 49.56 0 0 1 16.2 0A2 2 0 0 1 21.5 7a24.12 24.12 0 0 1 0 10 2 2 0 0 1-1.4 1.4 49.55 49.55 0 0 1-16.2 0A2 2 0 0 1 2.5 17"></path><path d="m10 15 5-3-5-3z"></path></svg>'
          };
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        render() {
          var x;
          if (!((x = this.data) != null && x.service)) {
            const I = document.createElement("div");
            this.element = I;
            const w = je.createLabel(
              "embed-input",
              "cdx-label",
              "Enter a URL to embed a video from YouTube or Vimeo"
            );
            I.appendChild(w);
            const b = je.createInput("embed-input", "", "", "url");
            return b.addEventListener("paste", (E) => {
              var P;
              const y = ((P = E.clipboardData) == null ? void 0 : P.getData("text")) ?? "", B = rn, M = Object.keys(B.services).find(
                (O) => B.services[O].regex.test(y)
              );
              M && this.onPaste({ detail: { key: M, data: y } });
            }), I.appendChild(b), I;
          }
          return super.render.call(this);
        }
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        validate(x) {
          return !!(x.service && x.source);
        }
      }
      return {
        paragraph: {
          class: jr,
          tunes: ["alignmentTune"]
        },
        header: {
          class: Yc,
          tunes: ["alignmentTune"]
        },
        image: C,
        quote: ze,
        embed: {
          class: S,
          config: {
            services: {
              youtube: !0,
              vimeo: !0
            }
          }
        },
        code: Nr,
        raw: er,
        list: {
          class: td,
          inlineToolbar: !0
        },
        checklist: Qc,
        link: m,
        // override link with Umbraco link picker
        alignmentTune: {
          class: Lh,
          config: {
            default: "left"
          }
        }
      };
    }, T = new Fc({
      holder: d,
      placeholder: "Type '/' to insert a block or just start typing something super...",
      data: h(),
      inlineToolbar: !0,
      // TODO: Not working
      sanitizer: {
        a: {}
      },
      tools: k(),
      onChange: () => {
        var m;
        u(), (m = n.current) == null || m.save().then((C) => {
          s.current(JSON.stringify(C));
        }).catch((C) => {
          console.log("Saving failed: ", C);
        });
      },
      onReady: () => {
        new Bh(n.current), u();
      }
    });
    return n.current = T, () => {
      n.current && typeof n.current.destroy == "function" && (n.current.destroy(), n.current = null);
    };
  }, []);
  const c = () => {
    var h;
    const d = (h = r.current) == null ? void 0 : h.querySelector("#" + l.current);
    !document.fullscreenElement && d ? d.requestFullscreen() : document.exitFullscreen && document.exitFullscreen();
  };
  return /* @__PURE__ */ Hr("div", { className: "skriv-let", ref: r, children: [
    /* @__PURE__ */ Ie("div", { id: l.current, className: "skriv-let__container" }),
    /* @__PURE__ */ Hr("button", { className: "skriv-let__fullscreen-button", onClick: c, type: "button", children: [
      /* @__PURE__ */ Ie(
        "svg",
        {
          xmlns: "http://www.w3.org/2000/svg",
          className: "skriv-let__fullscreen-button-icon",
          fill: "currentColor",
          width: "22",
          height: "22",
          viewBox: "0 0 512 512",
          children: /* @__PURE__ */ Ie("path", { d: "M368.432 110.765l32.367 32.362 34.896-34.898 36.539 36.539V38.783H366.256l37.075 37.077-34.899 34.905zm66.725 293.309l-34.901-34.899-32.37 32.368 34.9 34.899-36.534 36.536h105.986V366.996l-37.081 37.078zm-294.656-3.081l-32.37-32.365-34.898 34.902L36.7 366.993v105.985h105.979l-37.079-37.08 34.901-34.905zm-31.828-258.41l32.373-32.365-34.903-34.899 36.538-36.536H36.698v105.978l37.08-37.075 34.895 34.897zm278.314 157.969v-86.92c0-35.993-29.179-65.169-65.17-65.169H186.109c-35.991 0-65.171 29.177-65.171 65.169v86.92c0 35.993 29.18 65.168 65.171 65.168h135.708c35.992 0 65.17-29.175 65.17-65.168z" })
        }
      ),
      /* @__PURE__ */ Ie("span", { className: "sr-only", children: "Open editor in fullscreen" })
    ] }),
    /* @__PURE__ */ Ie("style", { children: Qi })
  ] });
}
var Oh = Object.getOwnPropertyDescriptor, $i = (o) => {
  throw TypeError(o);
}, Ah = (o, e, t, r) => {
  for (var n = r > 1 ? void 0 : r ? Oh(e, t) : e, i = o.length - 1, s; i >= 0; i--)
    (s = o[i]) && (n = s(n) || n);
  return n;
}, Dr = (o, e, t) => e.has(o) || $i("Cannot " + t), re = (o, e, t) => (Dr(o, e, "read from private field"), t ? t.call(o) : e.get(o)), De = (o, e, t) => e.has(o) ? $i("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(o) : e.set(o, t), Ee = (o, e, t, r) => (Dr(o, e, "write to private field"), e.set(o, t), t), Ft = (o, e, t) => (Dr(o, e, "access private method"), t), le, Fe, Se, mt, $e, st;
const Ph = "n3o-editor-js";
let Qt = class extends Wi(HTMLElement) {
  constructor() {
    super(), De(this, $e), De(this, le), De(this, Fe), De(this, Se), De(this, mt);
    const o = this.attachShadow({ mode: "open" });
    Ee(this, Fe, document.createElement("div")), o.appendChild(re(this, Fe)), this.consumeContext(Ki, (e) => {
      Ee(this, mt, e), Ft(this, $e, st).call(this);
    });
  }
  get value() {
    return re(this, Se);
  }
  set value(o) {
    Ee(this, Se, o), Ft(this, $e, st).call(this);
  }
  connectedCallback() {
    super.connectedCallback(), re(this, le) ?? Ee(this, le, Zi(re(this, Fe))), Ft(this, $e, st).call(this);
  }
  disconnectedCallback() {
    var o;
    super.disconnectedCallback(), (o = re(this, le)) == null || o.unmount(), Ee(this, le, void 0);
  }
};
le = /* @__PURE__ */ new WeakMap();
Fe = /* @__PURE__ */ new WeakMap();
Se = /* @__PURE__ */ new WeakMap();
mt = /* @__PURE__ */ new WeakMap();
$e = /* @__PURE__ */ new WeakSet();
st = function() {
  if (!re(this, le))
    return;
  const o = {
    host: this,
    modalManager: re(this, mt)
  };
  re(this, le).render(
    Xi(Ih, {
      value: re(this, Se),
      bridge: o,
      onChange: (e) => {
        Ee(this, Se, e), this.dispatchEvent(new qi());
      }
    })
  );
};
Qt = Ah([
  Vi(Ph)
], Qt);
const Gh = Qt;
export {
  Qt as N3oEditorJsElement,
  Gh as default
};
//# sourceMappingURL=editor-js.js.map
