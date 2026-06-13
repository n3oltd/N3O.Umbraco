import { customElement as _ } from "@umbraco-cms/backoffice/external/lit";
import { createElement as f } from "react";
import { createRoot as w } from "react-dom/client";
import { jsxs as C, jsx as i } from "react/jsx-runtime";
const y = ":host{display:block}p{margin:0 0 var(--uui-size-space-4)}p:last-of-type{margin-bottom:0}a{color:var(--uui-color-interactive)}";
function E() {
  return /* @__PURE__ */ C("uui-box", { headline: "Help & Support", children: [
    /* @__PURE__ */ i("p", { children: "Please visit the N3O Support Centre to view the latest help articles, documentation and to contact our support team with any queries." }),
    /* @__PURE__ */ i("p", { children: /* @__PURE__ */ i("a", { href: "https://support.n3o.ltd", target: "_blank", rel: "noopener", children: "Visit Support Centre →" }) }),
    /* @__PURE__ */ i("style", { children: y })
  ] });
}
var k = Object.getOwnPropertyDescriptor, m = (e) => {
  throw TypeError(e);
}, S = (e, t, r, l) => {
  for (var s = l > 1 ? void 0 : l ? k(t, r) : t, c = e.length - 1, d; c >= 0; c--)
    (d = e[c]) && (s = d(s) || s);
  return s;
}, v = (e, t, r) => t.has(e) || m("Cannot " + r), o = (e, t, r) => (v(e, t, "read from private field"), r ? r.call(e) : t.get(e)), u = (e, t, r) => t.has(e) ? m("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), p = (e, t, r, l) => (v(e, t, "write to private field"), t.set(e, r), r), a, n;
const W = "n3o-welcome-dashboard";
let h = class extends HTMLElement {
  constructor() {
    super(), u(this, a), u(this, n);
    const e = this.attachShadow({ mode: "open" });
    p(this, n, document.createElement("div")), e.appendChild(o(this, n));
  }
  connectedCallback() {
    o(this, a) ?? p(this, a, w(o(this, n))), o(this, a).render(f(E));
  }
  disconnectedCallback() {
    var e;
    (e = o(this, a)) == null || e.unmount(), p(this, a, void 0);
  }
};
a = /* @__PURE__ */ new WeakMap();
n = /* @__PURE__ */ new WeakMap();
h = S([
  _(W)
], h);
const O = h;
export {
  h as N3oWelcomeDashboardElement,
  O as default
};
//# sourceMappingURL=welcome-dashboard.js.map
