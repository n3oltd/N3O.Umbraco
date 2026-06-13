import { customElement as y } from "@umbraco-cms/backoffice/external/lit";
import { UmbPropertyValueChangeEvent as A } from "@umbraco-cms/backoffice/property-editor";
import { useState as M, useEffect as W, createElement as T } from "react";
import { createRoot as D } from "react-dom/client";
import { jsxs as i, jsx as f } from "react/jsx-runtime";
const O = ".sv{display:flex;gap:40px}.sv-form{flex:0 0 30%}.sv-demo{flex:1 1 600px;max-width:600px}.sv-form input,.sv-form textarea{width:100%;box-sizing:border-box}.sv-form textarea{height:100px;margin-top:12px}.sv-form p.sv-error{color:var(--uui-color-danger, red);margin-top:3px}.sv-demo h6,.sv-demo p{font-family:Arial,Helvetica,sans-serif;padding:0;margin:0}.sv-demo h6{font-size:20px;line-height:1.3;margin-bottom:3px;color:#1a0dab;text-decoration:underline}.sv-demo p{font-size:14px;margin-bottom:3px;line-height:1.57;word-wrap:break-word}.sv-demo p.sv-url{color:#006621}";
function z({ value: e, maxCharsTitle: t, maxCharsDescription: r, onChange: l }) {
  const [h, m] = M("");
  W(() => {
    let c = !0;
    return fetch("/umbraco/backoffice/api/serpEditor/templateOptions").then((u) => u.ok ? u.json() : null).then((u) => {
      c && u && m(u.titleSuffix ?? "");
    }).catch(() => {
    }), () => {
      c = !1;
    };
  }, []);
  const a = e.title ?? "", v = e.description ?? "", k = `${location.protocol}//${window.location.hostname}`;
  return /* @__PURE__ */ i("uui-box", { headline: "SEO preview", children: [
    /* @__PURE__ */ i("div", { className: "sv", children: [
      /* @__PURE__ */ i("div", { className: "sv-form", children: [
        /* @__PURE__ */ f(
          "input",
          {
            type: "text",
            value: a,
            placeholder: "Enter a short but descriptive title",
            onChange: (c) => l({ title: c.target.value, description: v })
          }
        ),
        a.length > t ? /* @__PURE__ */ i("p", { className: "sv-error", children: [
          "A title should not be more than ",
          t,
          " characters."
        ] }) : null,
        /* @__PURE__ */ f(
          "textarea",
          {
            value: v,
            placeholder: "Enter a meta description",
            onChange: (c) => l({ title: a, description: c.target.value })
          }
        ),
        v.length > r ? /* @__PURE__ */ i("p", { className: "sv-error", children: [
          "A meta description should not be more than ",
          r,
          " characters."
        ] }) : null
      ] }),
      /* @__PURE__ */ i("div", { className: "sv-demo", children: [
        a.length > 0 ? /* @__PURE__ */ i("h6", { children: [
          a,
          " ",
          h
        ] }) : null,
        a.length > 0 || v.length > 0 ? /* @__PURE__ */ f("p", { className: "sv-url", children: k }) : null,
        /* @__PURE__ */ f("p", { children: v })
      ] })
    ] }),
    /* @__PURE__ */ f("style", { children: O })
  ] });
}
var P = Object.getOwnPropertyDescriptor, b = (e) => {
  throw TypeError(e);
}, V = (e, t, r, l) => {
  for (var h = l > 1 ? void 0 : l ? P(t, r) : t, m = e.length - 1, a; m >= 0; m--)
    (a = e[m]) && (h = a(h) || h);
  return h;
}, S = (e, t, r) => t.has(e) || b("Cannot " + r), s = (e, t, r) => (S(e, t, "read from private field"), r ? r.call(e) : t.get(e)), p = (e, t, r) => t.has(e) ? b("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), o = (e, t, r, l) => (S(e, t, "write to private field"), t.set(e, r), r), N = (e, t, r) => (S(e, t, "access private method"), r), n, x, d, E, g, _, w;
const B = "n3o-serp-editor";
let C = class extends HTMLElement {
  constructor() {
    super(), p(this, _), p(this, n), p(this, x), p(this, d, { title: "", description: "" }), p(this, E, 60), p(this, g, 160);
    const e = this.attachShadow({ mode: "open" });
    o(this, x, document.createElement("div")), e.appendChild(s(this, x));
  }
  get value() {
    return s(this, d);
  }
  set value(e) {
    o(this, d, e ?? { title: "", description: "" }), N(this, _, w).call(this);
  }
  // Config (prevalues) arrives as UmbPropertyEditorConfigCollection.
  set config(e) {
    const t = Number.parseInt((e == null ? void 0 : e.getValueByAlias("maxCharsTitle")) ?? "", 10), r = Number.parseInt((e == null ? void 0 : e.getValueByAlias("maxCharsDescription")) ?? "", 10);
    !Number.isNaN(t) && t > 0 && o(this, E, t), !Number.isNaN(r) && r > 0 && o(this, g, r), N(this, _, w).call(this);
  }
  connectedCallback() {
    s(this, n) ?? o(this, n, D(s(this, x))), N(this, _, w).call(this);
  }
  disconnectedCallback() {
    var e;
    (e = s(this, n)) == null || e.unmount(), o(this, n, void 0);
  }
};
n = /* @__PURE__ */ new WeakMap();
x = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
E = /* @__PURE__ */ new WeakMap();
g = /* @__PURE__ */ new WeakMap();
_ = /* @__PURE__ */ new WeakSet();
w = function() {
  var e;
  (e = s(this, n)) == null || e.render(
    T(z, {
      value: s(this, d),
      maxCharsTitle: s(this, E),
      maxCharsDescription: s(this, g),
      onChange: (t) => {
        o(this, d, t), this.dispatchEvent(new A());
      }
    })
  );
};
C = V([
  y(B)
], C);
const R = C;
export {
  C as N3oSerpEditorElement,
  R as default
};
//# sourceMappingURL=serp-editor.js.map
