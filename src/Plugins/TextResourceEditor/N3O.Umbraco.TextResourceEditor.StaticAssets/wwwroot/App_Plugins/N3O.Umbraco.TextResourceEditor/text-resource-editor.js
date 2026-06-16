import { customElement as T } from "@umbraco-cms/backoffice/external/lit";
import { UmbPropertyValueChangeEvent as b } from "@umbraco-cms/backoffice/property-editor";
import { useState as y, createElement as M } from "react";
import { createRoot as R } from "react-dom/client";
import { jsxs as v, jsx as s, Fragment as N } from "react/jsx-runtime";
const S = ".n3o-text-resource-editor .row-wrapper{margin-bottom:40px;width:100%}.n3o-text-resource-editor .row-1,.n3o-text-resource-editor .row-2{display:block;width:90%}.n3o-text-resource-editor .delete{cursor:pointer}.n3o-text-resource-editor .text{font-weight:700}.n3o-text-resource-editor .custom{width:100%;margin-top:10px}";
function W({ value: e, onChange: t }) {
  const [r, n] = y(null);
  if (!e.length)
    return null;
  function c(o) {
    n(o);
  }
  function d(o) {
    n(null), t(e.filter((u) => u.source !== o));
  }
  function m() {
    n(null);
  }
  function D(o, u) {
    t(e.map((_) => _.source === o ? { ..._, custom: u } : _));
  }
  return /* @__PURE__ */ v("uui-box", { headline: "Text resources", children: [
    /* @__PURE__ */ s("div", { className: "n3o-text-resource-editor", children: e.map((o) => /* @__PURE__ */ v("div", { className: "row-wrapper", children: [
      /* @__PURE__ */ s("div", { className: "row-1", children: r === o.source ? /* @__PURE__ */ v(N, { children: [
        /* @__PURE__ */ s("span", { children: "Delete this entry? " }),
        /* @__PURE__ */ s(
          "button",
          {
            type: "button",
            className: "delete-confirm",
            onClick: () => d(o.source),
            children: "Yes"
          }
        ),
        " ",
        /* @__PURE__ */ s(
          "button",
          {
            type: "button",
            className: "delete-cancel",
            onClick: m,
            children: "No"
          }
        )
      ] }) : /* @__PURE__ */ v(N, { children: [
        "[",
        /* @__PURE__ */ s(
          "button",
          {
            type: "button",
            className: "delete",
            "aria-label": `Delete ${o.source}`,
            onClick: () => c(o.source),
            children: "x"
          }
        ),
        "] ",
        /* @__PURE__ */ s("span", { className: "text", children: o.source })
      ] }) }),
      /* @__PURE__ */ s("div", { className: "row-2", children: /* @__PURE__ */ s(
        "input",
        {
          type: "text",
          className: "custom",
          value: o.custom ?? "",
          onChange: (u) => D(o.source, u.currentTarget.value)
        }
      ) })
    ] }, o.source)) }),
    /* @__PURE__ */ s("style", { children: S })
  ] });
}
var A = Object.getOwnPropertyDescriptor, k = (e) => {
  throw TypeError(e);
}, P = (e, t, r, n) => {
  for (var c = n > 1 ? void 0 : n ? A(t, r) : t, d = e.length - 1, m; d >= 0; d--)
    (m = e[d]) && (c = m(c) || c);
  return c;
}, E = (e, t, r) => t.has(e) || k("Cannot " + r), i = (e, t, r) => (E(e, t, "read from private field"), r ? r.call(e) : t.get(e)), f = (e, t, r) => t.has(e) ? k("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), h = (e, t, r, n) => (E(e, t, "write to private field"), t.set(e, r), r), C = (e, t, r) => (E(e, t, "access private method"), r), a, p, l, x, w;
const O = "n3o-text-resource-editor";
let g = class extends HTMLElement {
  constructor() {
    super(), f(this, x), f(this, a), f(this, p), f(this, l, []);
    const e = this.attachShadow({ mode: "open" });
    h(this, p, document.createElement("div")), e.appendChild(i(this, p));
  }
  get value() {
    return i(this, l);
  }
  set value(e) {
    h(this, l, Array.isArray(e) ? e : []), C(this, x, w).call(this);
  }
  // config is set by Umbraco; not used by this editor but accepted to satisfy the contract.
  set config(e) {
  }
  connectedCallback() {
    i(this, a) ?? h(this, a, R(i(this, p))), C(this, x, w).call(this);
  }
  disconnectedCallback() {
    var e;
    (e = i(this, a)) == null || e.unmount(), h(this, a, void 0);
  }
};
a = /* @__PURE__ */ new WeakMap();
p = /* @__PURE__ */ new WeakMap();
l = /* @__PURE__ */ new WeakMap();
x = /* @__PURE__ */ new WeakSet();
w = function() {
  var e;
  (e = i(this, a)) == null || e.render(
    M(W, {
      value: i(this, l),
      onChange: (t) => {
        h(this, l, t), this.dispatchEvent(new b());
      }
    })
  );
};
g = P([
  T(O)
], g);
const U = g;
export {
  g as N3oTextResourceEditorElement,
  U as default
};
//# sourceMappingURL=text-resource-editor.js.map
