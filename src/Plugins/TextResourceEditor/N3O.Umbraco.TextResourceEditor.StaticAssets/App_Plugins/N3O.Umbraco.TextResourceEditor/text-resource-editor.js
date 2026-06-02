import { customElement as g } from "@umbraco-cms/backoffice/external/lit";
import { UmbPropertyValueChangeEvent as k } from "@umbraco-cms/backoffice/property-editor";
import { createElement as C } from "react";
import { createRoot as N } from "react-dom/client";
import { jsxs as v, jsx as c } from "react/jsx-runtime";
function T({ value: e, onChange: t }) {
  if (!e.length)
    return null;
  function r(o) {
    confirm("Are you sure you wish to delete this entry?") && t(e.filter((s, i) => i !== o));
  }
  function l(o, s) {
    t(e.map((i, y) => y === o ? { ...i, custom: s } : i));
  }
  return /* @__PURE__ */ v("uui-box", { headline: "Text resources", children: [
    /* @__PURE__ */ c("div", { className: "n3o-text-resource-editor", children: e.map((o, s) => /* @__PURE__ */ v("div", { className: "row-wrapper", children: [
      /* @__PURE__ */ v("div", { className: "row-1", children: [
        "[",
        /* @__PURE__ */ c("a", { className: "delete", onClick: () => r(s), children: "x" }),
        "] ",
        /* @__PURE__ */ c("span", { className: "text", children: o.source })
      ] }),
      /* @__PURE__ */ c("div", { className: "row-2", children: /* @__PURE__ */ c(
        "uui-input",
        {
          type: "text",
          class: "custom",
          value: o.custom ?? "",
          onInput: (i) => l(s, i.target.value)
        }
      ) })
    ] }, `${o.source}-${s}`)) }),
    /* @__PURE__ */ c("style", { children: A })
  ] });
}
const A = `
    .n3o-text-resource-editor .row-wrapper {
        margin-bottom: 40px;
        width: 100%;
    }

    .n3o-text-resource-editor .row-1 {
        display: block;
        width: 90%;
    }

    .n3o-text-resource-editor .row-2 {
        display: block;
        width: 90%;
    }

    .n3o-text-resource-editor .delete {
        cursor: pointer;
    }

    .n3o-text-resource-editor .text {
        font-weight: bold;
    }

    .n3o-text-resource-editor .custom {
        width: 100%;
        margin-top: 10px;
    }
`;
var M = Object.getOwnPropertyDescriptor, E = (e) => {
  throw TypeError(e);
}, R = (e, t, r, l) => {
  for (var o = l > 1 ? void 0 : l ? M(t, r) : t, s = e.length - 1, i; s >= 0; s--)
    (i = e[s]) && (o = i(o) || o);
  return o;
}, _ = (e, t, r) => t.has(e) || E("Cannot " + r), a = (e, t, r) => (_(e, t, "read from private field"), r ? r.call(e) : t.get(e)), p = (e, t, r) => t.has(e) ? E("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), h = (e, t, r, l) => (_(e, t, "write to private field"), t.set(e, r), r), x = (e, t, r) => (_(e, t, "access private method"), r), n, u, d, m, f;
const W = "n3o-text-resource-editor";
let w = class extends HTMLElement {
  constructor() {
    super(), p(this, m), p(this, n), p(this, u), p(this, d, []);
    const e = this.attachShadow({ mode: "open" });
    h(this, u, document.createElement("div")), e.appendChild(a(this, u));
  }
  get value() {
    return a(this, d);
  }
  set value(e) {
    h(this, d, Array.isArray(e) ? e : []), x(this, m, f).call(this);
  }
  // config is set by Umbraco; not used by this editor but accepted to satisfy the contract.
  set config(e) {
  }
  connectedCallback() {
    a(this, n) ?? h(this, n, N(a(this, u))), x(this, m, f).call(this);
  }
  disconnectedCallback() {
    var e;
    (e = a(this, n)) == null || e.unmount(), h(this, n, void 0);
  }
};
n = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
m = /* @__PURE__ */ new WeakSet();
f = function() {
  var e;
  (e = a(this, n)) == null || e.render(
    C(T, {
      value: a(this, d),
      onChange: (t) => {
        h(this, d, t), this.dispatchEvent(new k());
      }
    })
  );
};
w = R([
  g(W)
], w);
const G = w;
export {
  w as N3oTextResourceEditorElement,
  G as default
};
//# sourceMappingURL=text-resource-editor.js.map
