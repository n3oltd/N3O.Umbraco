import { customElement as A } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as O } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as T } from "@umbraco-cms/backoffice/property-editor";
import { UmbAuthFetchMixin as W } from "@n3o/backoffice-core";
import { useState as D, useEffect as F, createElement as z } from "react";
import { createRoot as P } from "react-dom/client";
import { jsxs as n, jsx as x } from "react/jsx-runtime";
const U = ".sv{display:flex;gap:40px}.sv-form{flex:0 0 30%}.sv-demo{flex:1 1 600px;max-width:600px}.sv-form input,.sv-form textarea{width:100%;box-sizing:border-box}.sv-form textarea{height:100px;margin-top:12px}.sv-form p.sv-error{color:var(--uui-color-danger, red);margin-top:3px}.sv-demo h6,.sv-demo p{font-family:Arial,Helvetica,sans-serif;padding:0;margin:0}.sv-demo h6{font-size:20px;line-height:1.3;margin-bottom:3px;color:#1a0dab;text-decoration:underline}.sv-demo p{font-size:14px;margin-bottom:3px;line-height:1.57;word-wrap:break-word}.sv-demo p.sv-url{color:#006621}";
let g;
function V({ value: e, maxCharsTitle: t, maxCharsDescription: r, authFetch: i, onChange: s }) {
  const [v, E] = D(g ?? "");
  F(() => {
    if (!i || g !== void 0)
      return;
    let p = !0;
    return i("/umbraco/backoffice/api/serpEditor/templateOptions").then((o) => {
      if (!o.ok)
        throw new Error(`templateOptions fetch failed: ${o.status}`);
      return o.json();
    }).then((o) => {
      g = o.titleSuffix ?? "", p && E(g);
    }).catch((o) => {
      console.error("[SerpEditor] Failed to load templateOptions:", o);
    }), () => {
      p = !1;
    };
  }, [i]);
  const c = e.title ?? "", f = e.description ?? "", M = `${location.protocol}//${window.location.hostname}`;
  return /* @__PURE__ */ n("uui-box", { headline: "SEO preview", children: [
    /* @__PURE__ */ n("div", { className: "sv", children: [
      /* @__PURE__ */ n("div", { className: "sv-form", children: [
        /* @__PURE__ */ x(
          "input",
          {
            type: "text",
            value: c,
            placeholder: "Enter a short but descriptive title",
            onChange: (p) => s({ title: p.target.value, description: f })
          }
        ),
        c.length > t ? /* @__PURE__ */ n("p", { className: "sv-error", children: [
          "A title should not be more than ",
          t,
          " characters."
        ] }) : null,
        /* @__PURE__ */ x(
          "textarea",
          {
            value: f,
            placeholder: "Enter a meta description",
            onChange: (p) => s({ title: c, description: p.target.value })
          }
        ),
        f.length > r ? /* @__PURE__ */ n("p", { className: "sv-error", children: [
          "A meta description should not be more than ",
          r,
          " characters."
        ] }) : null
      ] }),
      /* @__PURE__ */ n("div", { className: "sv-demo", children: [
        c.length > 0 ? /* @__PURE__ */ n("h6", { children: [
          c,
          " ",
          v
        ] }) : null,
        c.length > 0 || f.length > 0 ? /* @__PURE__ */ x("p", { className: "sv-url", children: M }) : null,
        /* @__PURE__ */ x("p", { children: f })
      ] })
    ] }),
    /* @__PURE__ */ x("style", { children: U })
  ] });
}
var $ = Object.getOwnPropertyDescriptor, y = (e) => {
  throw TypeError(e);
}, B = (e, t, r, i) => {
  for (var s = i > 1 ? void 0 : i ? $(t, r) : t, v = e.length - 1, E; v >= 0; v--)
    (E = e[v]) && (s = E(s) || s);
  return s;
}, k = (e, t, r) => t.has(e) || y("Cannot " + r), a = (e, t, r) => (k(e, t, "read from private field"), r ? r.call(e) : t.get(e)), d = (e, t, r) => t.has(e) ? y("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), l = (e, t, r, i) => (k(e, t, "write to private field"), t.set(e, r), r), C = (e, t, r) => (k(e, t, "access private method"), r), h, _, u, N, b, m, w;
const H = "n3o-serp-editor";
let S = class extends W(O(HTMLElement)) {
  constructor() {
    super(), d(this, m), d(this, h), d(this, _), d(this, u, { title: "", description: "" }), d(this, N, 60), d(this, b, 160);
    const e = this.attachShadow({ mode: "open" });
    l(this, _, document.createElement("div")), e.appendChild(a(this, _));
  }
  get value() {
    return a(this, u);
  }
  set value(e) {
    l(this, u, e ?? { title: "", description: "" }), C(this, m, w).call(this);
  }
  // Config (prevalues) arrives as UmbPropertyEditorConfigCollection.
  set config(e) {
    const t = Number.parseInt((e == null ? void 0 : e.getValueByAlias("maxCharsTitle")) ?? "", 10), r = Number.parseInt((e == null ? void 0 : e.getValueByAlias("maxCharsDescription")) ?? "", 10);
    !Number.isNaN(t) && t > 0 && l(this, N, t), !Number.isNaN(r) && r > 0 && l(this, b, r), C(this, m, w).call(this);
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(e) {
    C(this, m, w).call(this);
  }
  connectedCallback() {
    var e;
    (e = super.connectedCallback) == null || e.call(this), a(this, h) ?? l(this, h, P(a(this, _))), C(this, m, w).call(this);
  }
  disconnectedCallback() {
    var e, t;
    (e = super.disconnectedCallback) == null || e.call(this), (t = a(this, h)) == null || t.unmount(), l(this, h, void 0);
  }
};
h = /* @__PURE__ */ new WeakMap();
_ = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
N = /* @__PURE__ */ new WeakMap();
b = /* @__PURE__ */ new WeakMap();
m = /* @__PURE__ */ new WeakSet();
w = function() {
  var e;
  (e = a(this, h)) == null || e.render(
    z(V, {
      value: a(this, u),
      maxCharsTitle: a(this, N),
      maxCharsDescription: a(this, b),
      authFetch: this.authFetch,
      onChange: (t) => {
        l(this, u, t), this.dispatchEvent(new T());
      }
    })
  );
};
S = B([
  A(H)
], S);
const Q = S;
export {
  S as N3oSerpEditorElement,
  Q as default
};
//# sourceMappingURL=serp-editor.js.map
