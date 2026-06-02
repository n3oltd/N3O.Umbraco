import { customElement as x } from "@umbraco-cms/backoffice/external/lit";
import { createElement as k } from "react";
import { createRoot as C } from "react-dom/client";
import { jsxs as v, Fragment as f, jsx as n } from "react/jsx-runtime";
function W({ value: r }) {
  const e = (r == null ? void 0 : r.errors) ?? null, t = (r == null ? void 0 : r.warnings) ?? null, c = !!e && e.length > 0, o = !!t && t.length > 0;
  return /* @__PURE__ */ v("div", { className: "n3o-import-errors-viewer", children: [
    c ? /* @__PURE__ */ v(f, { children: [
      /* @__PURE__ */ n("p", { children: /* @__PURE__ */ n("em", { className: "text-error", children: "Errors" }) }),
      e.map((s, l) => /* @__PURE__ */ n("div", { className: "row-wrapper", children: /* @__PURE__ */ n("div", { className: "row", children: s }) }, `error-${l}`))
    ] }) : null,
    o ? /* @__PURE__ */ v(f, { children: [
      /* @__PURE__ */ n("p", { children: /* @__PURE__ */ n("em", { className: "text-warning", children: "Warnings" }) }),
      t.map((s, l) => /* @__PURE__ */ n("div", { className: "row-wrapper", children: /* @__PURE__ */ n("div", { className: "row", children: s }) }, `warning-${l}`))
    ] }) : null,
    !c && !o ? /* @__PURE__ */ n("div", { className: "row-wrapper", children: /* @__PURE__ */ n("div", { className: "row", children: "No warnings or errors" }) }) : null,
    /* @__PURE__ */ n("style", { children: y })
  ] });
}
const y = `
    .n3o-import-errors-viewer .row-wrapper {
        margin-bottom: 40px;
        width: 100%;
    }
    .n3o-import-errors-viewer .row {
        display: block;
        width: 90%;
    }
    .text-error {
        color: var(--uui-color-danger);
    }
    .text-warning {
        color: var(--uui-color-warning);
    }
`;
var M = Object.getOwnPropertyDescriptor, E = (r) => {
  throw TypeError(r);
}, I = (r, e, t, c) => {
  for (var o = c > 1 ? void 0 : c ? M(e, t) : e, s = r.length - 1, l; s >= 0; s--)
    (l = r[s]) && (o = l(o) || o);
  return o;
}, u = (r, e, t) => e.has(r) || E("Cannot " + t), i = (r, e, t) => (u(r, e, "read from private field"), t ? t.call(r) : e.get(r)), p = (r, e, t) => e.has(r) ? E("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(r) : e.set(r, t), m = (r, e, t, c) => (u(r, e, "write to private field"), e.set(r, t), t), N = (r, e, t) => (u(r, e, "access private method"), t), a, h, d, w, _;
const S = "n3o-import-notices-viewer";
let g = class extends HTMLElement {
  constructor() {
    super(), p(this, w), p(this, a), p(this, h), p(this, d);
    const r = this.attachShadow({ mode: "open" });
    m(this, h, document.createElement("div")), r.appendChild(i(this, h));
  }
  get value() {
    return i(this, d);
  }
  set value(r) {
    m(this, d, r), N(this, w, _).call(this);
  }
  // Config is set by Umbraco for property editors; unused here but accepted to avoid warnings.
  set config(r) {
  }
  get config() {
  }
  connectedCallback() {
    i(this, a) ?? m(this, a, C(i(this, h))), N(this, w, _).call(this);
  }
  disconnectedCallback() {
    var r;
    (r = i(this, a)) == null || r.unmount(), m(this, a, void 0);
  }
};
a = /* @__PURE__ */ new WeakMap();
h = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
w = /* @__PURE__ */ new WeakSet();
_ = function() {
  var r;
  (r = i(this, a)) == null || r.render(
    k(W, {
      value: i(this, d)
    })
  );
};
g = I([
  x(S)
], g);
const P = g;
export {
  g as N3oImportNoticesViewerElement,
  P as default
};
//# sourceMappingURL=import-notices-viewer.js.map
