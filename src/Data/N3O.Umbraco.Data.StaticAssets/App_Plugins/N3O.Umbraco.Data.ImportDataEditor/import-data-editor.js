import { customElement as D } from "@umbraco-cms/backoffice/external/lit";
import { UmbPropertyValueChangeEvent as I } from "@umbraco-cms/backoffice/property-editor";
import { createElement as M } from "react";
import { createRoot as O } from "react-dom/client";
import { jsxs as _, jsx as p } from "react/jsx-runtime";
function W({ value: t, onTextChange: e, onFileSelected: a }) {
  const d = (t == null ? void 0 : t.fields) ?? [];
  return /* @__PURE__ */ _("div", { className: "n3o-import-fields-editor", children: [
    d.map((s, n) => /* @__PURE__ */ _("div", { className: "row-wrapper", children: [
      /* @__PURE__ */ p("div", { className: "row-1", children: /* @__PURE__ */ p("span", { className: "text", children: s.name }) }),
      /* @__PURE__ */ _("div", { className: "row-2", children: [
        /* @__PURE__ */ p(
          "input",
          {
            type: "text",
            className: "custom",
            value: s.value ?? "",
            placeholder: s.sourceValue ?? "",
            onInput: (h) => e(n, h.target.value)
          }
        ),
        s.isFile ? /* @__PURE__ */ p(
          "input",
          {
            type: "file",
            onChange: (h) => {
              var C;
              const k = (C = h.target.files) == null ? void 0 : C[0];
              k && a(n, k);
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
var b = Object.getOwnPropertyDescriptor, N = (t) => {
  throw TypeError(t);
}, F = (t, e, a, d) => {
  for (var s = d > 1 ? void 0 : d ? b(e, a) : e, n = t.length - 1, h; n >= 0; n--)
    (h = t[n]) && (s = h(s) || s);
  return s;
}, y = (t, e, a) => e.has(t) || N("Cannot " + a), i = (t, e, a) => (y(t, e, "read from private field"), a ? a.call(t) : e.get(t)), f = (t, e, a) => e.has(t) ? N("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, a), m = (t, e, a, d) => (y(t, e, "write to private field"), e.set(t, a), a), c = (t, e, a) => (y(t, e, "access private method"), a), l, u, o, v, r, S, T, x, E, w;
const A = "n3o-import-data-editor";
let g = class extends HTMLElement {
  constructor() {
    super(), f(this, r), f(this, l), f(this, u), f(this, o), f(this, v);
    const t = this.attachShadow({ mode: "open" });
    m(this, u, document.createElement("div")), t.appendChild(i(this, u));
  }
  get value() {
    return i(this, o);
  }
  set value(t) {
    m(this, o, t), c(this, r, w).call(this);
  }
  set config(t) {
    m(this, v, t);
  }
  get config() {
    return i(this, v);
  }
  connectedCallback() {
    i(this, l) ?? m(this, l, O(i(this, u))), c(this, r, w).call(this);
  }
  disconnectedCallback() {
    var t;
    (t = i(this, l)) == null || t.unmount(), m(this, l, void 0);
  }
};
l = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
o = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
r = /* @__PURE__ */ new WeakSet();
S = function(t, e) {
  i(this, o) && (i(this, o).fields[t].value = e, c(this, r, E).call(this));
};
T = async function(t, e) {
  if (!i(this, o))
    return;
  const a = i(this, o).reference, s = { file: await c(this, r, x).call(this, e) };
  (await fetch(`/umbraco/backoffice/api/Imports/queued/${a}/files`, {
    method: "POST",
    headers: {
      Accept: "*/*",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(s)
  })).status === 200 ? (i(this, o).fields[t].value = e.name, c(this, r, w).call(this), c(this, r, E).call(this)) : alert("Failed to upload specified file, please contact support for assistance");
};
x = async function(t) {
  const e = new FormData();
  return e.append("file", t), await (await fetch("/umbraco/api/Storage/tempUpload", {
    method: "POST",
    body: e
  })).json();
};
E = function() {
  this.dispatchEvent(new I());
};
w = function() {
  var t;
  (t = i(this, l)) == null || t.render(
    M(W, {
      value: i(this, o),
      onTextChange: (e, a) => c(this, r, S).call(this, e, a),
      onFileSelected: (e, a) => void c(this, r, T).call(this, e, a)
    })
  );
};
g = F([
  D(A)
], g);
const H = g;
export {
  g as N3oImportDataEditorElement,
  H as default
};
//# sourceMappingURL=import-data-editor.js.map
