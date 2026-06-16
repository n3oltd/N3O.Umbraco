import { customElement as u } from "@umbraco-cms/backoffice/external/lit";
import { createElement as v } from "react";
import { createRoot as _ } from "react-dom/client";
import { jsx as S } from "react/jsx-runtime";
function w() {
  return /* @__PURE__ */ S(
    "iframe",
    {
      name: "hangfireIFrame",
      id: "hangfire",
      title: "Scheduler",
      frameBorder: "0",
      scrolling: "yes",
      src: "/umbraco/backoffice/hangfire/",
      allowFullScreen: !0
    }
  );
}
const y = ":host{display:block;width:100%}iframe{display:block;width:100%;height:calc(100dvh - 200px);min-height:600px;border:0}";
var g = Object.getOwnPropertyDescriptor, m = (e) => {
  throw TypeError(e);
}, k = (e, t, r, c) => {
  for (var n = c > 1 ? void 0 : c ? g(t, r) : t, i = e.length - 1, l; i >= 0; i--)
    (l = e[i]) && (n = l(n) || n);
  return n;
}, f = (e, t, r) => t.has(e) || m("Cannot " + r), o = (e, t, r) => (f(e, t, "read from private field"), r ? r.call(e) : t.get(e)), p = (e, t, r) => t.has(e) ? m("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), h = (e, t, r, c) => (f(e, t, "write to private field"), t.set(e, r), r), a, s;
const C = "n3o-scheduler-dashboard";
let d = class extends HTMLElement {
  constructor() {
    super(), p(this, a), p(this, s);
    const e = this.attachShadow({ mode: "open" }), t = new CSSStyleSheet();
    t.replaceSync(y), e.adoptedStyleSheets = [t], h(this, s, document.createElement("div")), e.appendChild(o(this, s));
  }
  connectedCallback() {
    o(this, a) ?? h(this, a, _(o(this, s))), o(this, a).render(v(w));
  }
  disconnectedCallback() {
    var e;
    (e = o(this, a)) == null || e.unmount(), h(this, a, void 0);
  }
};
a = /* @__PURE__ */ new WeakMap();
s = /* @__PURE__ */ new WeakMap();
d = k([
  u(C)
], d);
const M = d;
export {
  d as N3oSchedulerDashboardElement,
  M as default
};
//# sourceMappingURL=scheduler-dashboard.js.map
