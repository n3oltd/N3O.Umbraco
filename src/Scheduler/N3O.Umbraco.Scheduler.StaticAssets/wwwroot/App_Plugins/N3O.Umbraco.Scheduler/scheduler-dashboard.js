import { customElement as v } from "@umbraco-cms/backoffice/external/lit";
import { createElement as _ } from "react";
import { createRoot as w } from "react-dom/client";
import { jsxs as g, Fragment as k, jsx as p } from "react/jsx-runtime";
const y = ":host{display:block;width:100%}iframe{display:block;width:100%;height:calc(100dvh - 200px);min-height:600px;border:0}";
function E() {
  return /* @__PURE__ */ g(k, { children: [
    /* @__PURE__ */ p(
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
    ),
    /* @__PURE__ */ p("style", { children: y })
  ] });
}
var S = Object.getOwnPropertyDescriptor, f = (e) => {
  throw TypeError(e);
}, C = (e, t, r, i) => {
  for (var n = i > 1 ? void 0 : i ? S(t, r) : t, c = e.length - 1, l; c >= 0; c--)
    (l = e[c]) && (n = l(n) || n);
  return n;
}, u = (e, t, r) => t.has(e) || f("Cannot " + r), o = (e, t, r) => (u(e, t, "read from private field"), r ? r.call(e) : t.get(e)), m = (e, t, r) => t.has(e) ? f("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), h = (e, t, r, i) => (u(e, t, "write to private field"), t.set(e, r), r), a, s;
const x = "n3o-scheduler-dashboard";
let d = class extends HTMLElement {
  constructor() {
    super(), m(this, a), m(this, s);
    const e = this.attachShadow({ mode: "open" });
    h(this, s, document.createElement("div")), e.appendChild(o(this, s));
  }
  connectedCallback() {
    o(this, a) ?? h(this, a, w(o(this, s))), o(this, a).render(_(E));
  }
  disconnectedCallback() {
    var e;
    (e = o(this, a)) == null || e.unmount(), h(this, a, void 0);
  }
};
a = /* @__PURE__ */ new WeakMap();
s = /* @__PURE__ */ new WeakMap();
d = C([
  v(x)
], d);
const N = d;
export {
  d as N3oSchedulerDashboardElement,
  N as default
};
//# sourceMappingURL=scheduler-dashboard.js.map
