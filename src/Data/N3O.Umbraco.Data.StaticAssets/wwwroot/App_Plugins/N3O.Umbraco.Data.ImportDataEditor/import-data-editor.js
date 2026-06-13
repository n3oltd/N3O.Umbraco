import { customElement as M } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as D } from "@umbraco-cms/backoffice/element-api";
import { UmbAuthFetchMixin as F } from "@n3o/backoffice-core";
import { UmbPropertyValueChangeEvent as b } from "@umbraco-cms/backoffice/property-editor";
import { createElement as I } from "react";
import { createRoot as O } from "react-dom/client";
import { jsxs as _, jsx as d } from "react/jsx-runtime";
const W = ".n3o-import-fields-editor .row-wrapper{margin-bottom:40px;width:100%}.n3o-import-fields-editor .row-1,.n3o-import-fields-editor .row-2{display:block;width:90%}.n3o-import-fields-editor .text{font-weight:700}.n3o-import-fields-editor .custom{width:100%;margin-top:10px}";
function P({ value: t, onTextChange: e, onFileSelected: a }) {
  const h = (t == null ? void 0 : t.fields) ?? [];
  return /* @__PURE__ */ _("div", { className: "n3o-import-fields-editor", children: [
    h.map((s, n) => /* @__PURE__ */ _("div", { className: "row-wrapper", children: [
      /* @__PURE__ */ d("div", { className: "row-1", children: /* @__PURE__ */ d("span", { className: "text", children: s.name }) }),
      /* @__PURE__ */ _("div", { className: "row-2", children: [
        /* @__PURE__ */ d(
          "input",
          {
            type: "text",
            className: "custom",
            value: s.value ?? "",
            placeholder: s.sourceValue ?? "",
            onInput: (p) => e(n, p.target.value)
          }
        ),
        s.isFile ? /* @__PURE__ */ d(
          "input",
          {
            type: "file",
            onChange: (p) => {
              var C;
              const y = (C = p.target.files) == null ? void 0 : C[0];
              y && a(n, y);
            }
          }
        ) : null
      ] })
    ] }, n)),
    /* @__PURE__ */ d("style", { children: W })
  ] });
}
var A = Object.getOwnPropertyDescriptor, x = (t) => {
  throw TypeError(t);
}, U = (t, e, a, h) => {
  for (var s = h > 1 ? void 0 : h ? A(e, a) : e, n = t.length - 1, p; n >= 0; n--)
    (p = t[n]) && (s = p(s) || s);
  return s;
}, E = (t, e, a) => e.has(t) || x("Cannot " + a), i = (t, e, a) => (E(t, e, "read from private field"), a ? a.call(t) : e.get(t)), m = (t, e, a) => e.has(t) ? x("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, a), f = (t, e, a, h) => (E(t, e, "write to private field"), e.set(t, a), a), c = (t, e, a) => (E(t, e, "access private method"), a), l, u, o, v, r, N, S, T, k, w;
const q = "n3o-import-data-editor";
let g = class extends F(D(HTMLElement)) {
  constructor() {
    super(), m(this, r), m(this, l), m(this, u), m(this, o), m(this, v);
    const t = this.attachShadow({ mode: "open" });
    f(this, u, document.createElement("div")), t.appendChild(i(this, u));
  }
  get value() {
    return i(this, o);
  }
  set value(t) {
    f(this, o, t), c(this, r, w).call(this);
  }
  set config(t) {
    f(this, v, t);
  }
  get config() {
    return i(this, v);
  }
  connectedCallback() {
    var t;
    (t = super.connectedCallback) == null || t.call(this), i(this, l) ?? f(this, l, O(i(this, u))), c(this, r, w).call(this);
  }
  disconnectedCallback() {
    var t, e;
    (t = super.disconnectedCallback) == null || t.call(this), (e = i(this, l)) == null || e.unmount(), f(this, l, void 0);
  }
};
l = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
o = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
r = /* @__PURE__ */ new WeakSet();
N = function(t, e) {
  i(this, o) && (i(this, o).fields[t].value = e, c(this, r, k).call(this));
};
S = async function(t, e) {
  if (!i(this, o))
    return;
  const a = i(this, o).reference, s = { file: await c(this, r, T).call(this, e) };
  (await (this.authFetch ?? fetch)(`/umbraco/backoffice/api/Imports/queued/${a}/files`, {
    method: "POST",
    headers: {
      Accept: "*/*",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(s)
  })).status === 200 ? (i(this, o).fields[t].value = e.name, c(this, r, w).call(this), c(this, r, k).call(this)) : alert("Failed to upload specified file, please contact support for assistance");
};
T = async function(t) {
  const e = new FormData();
  return e.append("file", t), await (await (this.authFetch ?? fetch)("/umbraco/api/Storage/tempUpload", {
    method: "POST",
    body: e
  })).json();
};
k = function() {
  this.dispatchEvent(new b());
};
w = function() {
  var t;
  (t = i(this, l)) == null || t.render(
    I(P, {
      value: i(this, o),
      onTextChange: (e, a) => c(this, r, N).call(this, e, a),
      onFileSelected: (e, a) => void c(this, r, S).call(this, e, a)
    })
  );
};
g = U([
  M(q)
], g);
const j = g;
export {
  g as N3oImportDataEditorElement,
  j as default
};
//# sourceMappingURL=import-data-editor.js.map
