import { customElement as x } from "@umbraco-cms/backoffice/external/lit";
import { createElement as k } from "react";
import { createRoot as C } from "react-dom/client";
import { jsxs as v, Fragment as f, jsx as n } from "react/jsx-runtime";
const W = ".n3o-import-errors-viewer .row-wrapper{margin-bottom:40px;width:100%}.n3o-import-errors-viewer .row{display:block;width:90%}.text-error{color:var(--uui-color-danger)}.text-warning{color:var(--uui-color-warning)}";
function y({ value: e }) {
  const r = (e == null ? void 0 : e.errors) ?? null, t = (e == null ? void 0 : e.warnings) ?? null, c = !!r && r.length > 0, o = !!t && t.length > 0;
  return /* @__PURE__ */ v("div", { className: "n3o-import-errors-viewer", children: [
    c ? /* @__PURE__ */ v(f, { children: [
      /* @__PURE__ */ n("p", { children: /* @__PURE__ */ n("em", { className: "text-error", children: "Errors" }) }),
      r.map((s, l) => /* @__PURE__ */ n("div", { className: "row-wrapper", children: /* @__PURE__ */ n("div", { className: "row", children: s }) }, `error-${s}-${l}`))
    ] }) : null,
    o ? /* @__PURE__ */ v(f, { children: [
      /* @__PURE__ */ n("p", { children: /* @__PURE__ */ n("em", { className: "text-warning", children: "Warnings" }) }),
      t.map((s, l) => /* @__PURE__ */ n("div", { className: "row-wrapper", children: /* @__PURE__ */ n("div", { className: "row", children: s }) }, `warning-${s}-${l}`))
    ] }) : null,
    !c && !o ? /* @__PURE__ */ n("div", { className: "row-wrapper", children: /* @__PURE__ */ n("div", { className: "row", children: "No warnings or errors" }) }) : null,
    /* @__PURE__ */ n("style", { children: W })
  ] });
}
var M = Object.getOwnPropertyDescriptor, E = (e) => {
  throw TypeError(e);
}, I = (e, r, t, c) => {
  for (var o = c > 1 ? void 0 : c ? M(r, t) : r, s = e.length - 1, l; s >= 0; s--)
    (l = e[s]) && (o = l(o) || o);
  return o;
}, u = (e, r, t) => r.has(e) || E("Cannot " + t), i = (e, r, t) => (u(e, r, "read from private field"), t ? t.call(e) : r.get(e)), p = (e, r, t) => r.has(e) ? E("Cannot add the same private member more than once") : r instanceof WeakSet ? r.add(e) : r.set(e, t), m = (e, r, t, c) => (u(e, r, "write to private field"), r.set(e, t), t), N = (e, r, t) => (u(e, r, "access private method"), t), a, h, d, w, _;
const S = "n3o-import-notices-viewer";
let g = class extends HTMLElement {
  constructor() {
    super(), p(this, w), p(this, a), p(this, h), p(this, d);
    const e = this.attachShadow({ mode: "open" });
    m(this, h, document.createElement("div")), e.appendChild(i(this, h));
  }
  get value() {
    return i(this, d);
  }
  set value(e) {
    m(this, d, e), N(this, w, _).call(this);
  }
  // Config is set by Umbraco for property editors; unused here but accepted to avoid warnings.
  set config(e) {
  }
  get config() {
  }
  connectedCallback() {
    i(this, a) ?? m(this, a, C(i(this, h))), N(this, w, _).call(this);
  }
  disconnectedCallback() {
    var e;
    (e = i(this, a)) == null || e.unmount(), m(this, a, void 0);
  }
};
a = /* @__PURE__ */ new WeakMap();
h = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
w = /* @__PURE__ */ new WeakSet();
_ = function() {
  var e;
  (e = i(this, a)) == null || e.render(
    k(y, {
      value: i(this, d)
    })
  );
};
g = I([
  x(S)
], g);
const D = g;
export {
  g as N3oImportNoticesViewerElement,
  D as default
};
//# sourceMappingURL=import-notices-viewer.js.map
