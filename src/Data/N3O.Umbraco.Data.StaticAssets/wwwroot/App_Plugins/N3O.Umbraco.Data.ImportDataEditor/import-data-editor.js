import { customElement as b } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as M } from "@umbraco-cms/backoffice/element-api";
import { UmbAuthFetchMixin as D } from "@n3o/backoffice-core";
import { UmbPropertyValueChangeEvent as F } from "@umbraco-cms/backoffice/property-editor";
import { createElement as I } from "react";
import { createRoot as O } from "react-dom/client";
import { jsxs as _, jsx as p } from "react/jsx-runtime";
function W({ value: t, onTextChange: e, onFileSelected: a }) {
  const h = (t == null ? void 0 : t.fields) ?? [];
  return /* @__PURE__ */ _("div", { className: "n3o-import-fields-editor", children: [
    h.map((o, n) => /* @__PURE__ */ _("div", { className: "row-wrapper", children: [
      /* @__PURE__ */ p("div", { className: "row-1", children: /* @__PURE__ */ p("span", { className: "text", children: o.name }) }),
      /* @__PURE__ */ _("div", { className: "row-2", children: [
        /* @__PURE__ */ p(
          "input",
          {
            type: "text",
            className: "custom",
            value: o.value ?? "",
            placeholder: o.sourceValue ?? "",
            onInput: (d) => e(n, d.target.value)
          }
        ),
        o.isFile ? /* @__PURE__ */ p(
          "input",
          {
            type: "file",
            onChange: (d) => {
              var C;
              const E = (C = d.target.files) == null ? void 0 : C[0];
              E && a(n, E);
            }
          }
        ) : null
      ] })
    ] }, n)),
    /* @__PURE__ */ p("style", { children: P })
  ] });
}
const P = `
    .n3o-import-fields-editor .row-wrapper {
        margin-bottom: 40px;
        width: 100%;
    }
    .n3o-import-fields-editor .row-1 {
        display: block;
        width: 90%;
    }
    .n3o-import-fields-editor .row-2 {
        display: block;
        width: 90%;
    }
    .n3o-import-fields-editor .text {
        font-weight: bold;
    }
    .n3o-import-fields-editor .custom {
        width: 100%;
        margin-top: 10px;
    }
`;
var A = Object.getOwnPropertyDescriptor, x = (t) => {
  throw TypeError(t);
}, U = (t, e, a, h) => {
  for (var o = h > 1 ? void 0 : h ? A(e, a) : e, n = t.length - 1, d; n >= 0; n--)
    (d = t[n]) && (o = d(o) || o);
  return o;
}, k = (t, e, a) => e.has(t) || x("Cannot " + a), i = (t, e, a) => (k(t, e, "read from private field"), a ? a.call(t) : e.get(t)), m = (t, e, a) => e.has(t) ? x("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, a), f = (t, e, a, h) => (k(t, e, "write to private field"), e.set(t, a), a), c = (t, e, a) => (k(t, e, "access private method"), a), l, u, s, v, r, N, S, T, y, w;
const q = "n3o-import-data-editor";
let g = class extends D(M(HTMLElement)) {
  constructor() {
    super(), m(this, r), m(this, l), m(this, u), m(this, s), m(this, v);
    const t = this.attachShadow({ mode: "open" });
    f(this, u, document.createElement("div")), t.appendChild(i(this, u));
  }
  get value() {
    return i(this, s);
  }
  set value(t) {
    f(this, s, t), c(this, r, w).call(this);
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
s = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
r = /* @__PURE__ */ new WeakSet();
N = function(t, e) {
  i(this, s) && (i(this, s).fields[t].value = e, c(this, r, y).call(this));
};
S = async function(t, e) {
  if (!i(this, s))
    return;
  const a = i(this, s).reference, o = { file: await c(this, r, T).call(this, e) };
  (await (this.authFetch ?? fetch)(`/umbraco/backoffice/api/Imports/queued/${a}/files`, {
    method: "POST",
    headers: {
      Accept: "*/*",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(o)
  })).status === 200 ? (i(this, s).fields[t].value = e.name, c(this, r, w).call(this), c(this, r, y).call(this)) : alert("Failed to upload specified file, please contact support for assistance");
};
T = async function(t) {
  const e = new FormData();
  return e.append("file", t), await (await (this.authFetch ?? fetch)("/umbraco/api/Storage/tempUpload", {
    method: "POST",
    body: e
  })).json();
};
y = function() {
  this.dispatchEvent(new F());
};
w = function() {
  var t;
  (t = i(this, l)) == null || t.render(
    I(W, {
      value: i(this, s),
      onTextChange: (e, a) => c(this, r, N).call(this, e, a),
      onFileSelected: (e, a) => void c(this, r, S).call(this, e, a)
    })
  );
};
g = U([
  b(q)
], g);
const j = g;
export {
  g as N3oImportDataEditorElement,
  j as default
};
//# sourceMappingURL=import-data-editor.js.map
