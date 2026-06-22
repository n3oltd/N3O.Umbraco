import { customElement as F } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as M } from "@umbraco-cms/backoffice/element-api";
import { UmbAuthFetchMixin as D } from "@n3oltd/backoffice-core";
import "@umbraco-cms/backoffice/property-editor";
import { UmbChangeEvent as b } from "@umbraco-cms/backoffice/event";
import { createElement as A } from "react";
import { createRoot as O } from "react-dom/client";
import { jsxs as _, jsx as p } from "react/jsx-runtime";
const W = ".n3o-import-fields-editor .row-wrapper{margin-bottom:40px;width:100%}.n3o-import-fields-editor .row-1,.n3o-import-fields-editor .row-2{display:block;width:90%}.n3o-import-fields-editor .text{font-weight:700}.n3o-import-fields-editor .custom{width:100%;margin-top:10px}";
function I({ value: t, onTextChange: e, onFileSelected: a }) {
  const h = (t == null ? void 0 : t.fields) ?? [];
  return /* @__PURE__ */ _("div", { className: "n3o-import-fields-editor", children: [
    h.map((o, c) => /* @__PURE__ */ _("div", { className: "row-wrapper", children: [
      /* @__PURE__ */ p("div", { className: "row-1", children: /* @__PURE__ */ p("span", { className: "text", children: o.name }) }),
      /* @__PURE__ */ _("div", { className: "row-2", children: [
        /* @__PURE__ */ p(
          "input",
          {
            type: "text",
            className: "custom",
            value: o.value ?? "",
            placeholder: o.sourceValue ?? "",
            onChange: (d) => e(c, d.currentTarget.value)
          }
        ),
        o.isFile ? /* @__PURE__ */ p(
          "input",
          {
            type: "file",
            onChange: (d) => {
              var k;
              const C = (k = d.target.files) == null ? void 0 : k[0];
              C && a(c, C);
            }
          }
        ) : null
      ] })
    ] }, o.name)),
    /* @__PURE__ */ p("style", { children: W })
  ] });
}
var P = Object.getOwnPropertyDescriptor, x = (t) => {
  throw TypeError(t);
}, U = (t, e, a, h) => {
  for (var o = h > 1 ? void 0 : h ? P(e, a) : e, c = t.length - 1, d; c >= 0; c--)
    (d = t[c]) && (o = d(o) || o);
  return o;
}, y = (t, e, a) => e.has(t) || x("Cannot " + a), i = (t, e, a) => (y(t, e, "read from private field"), a ? a.call(t) : e.get(t)), m = (t, e, a) => e.has(t) ? x("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, a), f = (t, e, a, h) => (y(t, e, "write to private field"), e.set(t, a), a), n = (t, e, a) => (y(t, e, "access private method"), a), l, u, r, w, s, N, T, S, E, v;
const q = "n3o-import-data-editor";
let g = class extends D(M(HTMLElement)) {
  constructor() {
    super(), m(this, s), m(this, l), m(this, u), m(this, r), m(this, w);
    const t = this.attachShadow({ mode: "open" });
    f(this, u, document.createElement("div")), t.appendChild(i(this, u));
  }
  get value() {
    return i(this, r);
  }
  set value(t) {
    f(this, r, t), n(this, s, v).call(this);
  }
  set config(t) {
    f(this, w, t);
  }
  get config() {
    return i(this, w);
  }
  connectedCallback() {
    var t;
    (t = super.connectedCallback) == null || t.call(this), i(this, l) ?? f(this, l, O(i(this, u))), n(this, s, v).call(this);
  }
  disconnectedCallback() {
    var t, e;
    (t = super.disconnectedCallback) == null || t.call(this), (e = i(this, l)) == null || e.unmount(), f(this, l, void 0);
  }
};
l = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
r = /* @__PURE__ */ new WeakMap();
w = /* @__PURE__ */ new WeakMap();
s = /* @__PURE__ */ new WeakSet();
N = function(t, e) {
  i(this, r) && (i(this, r).fields[t].value = e, n(this, s, v).call(this), n(this, s, E).call(this));
};
T = async function(t, e) {
  if (!i(this, r))
    return;
  const a = i(this, r).reference, o = { file: await n(this, s, S).call(this, e) };
  if (!this.authFetch) {
    alert("Authentication context not ready, please try again");
    return;
  }
  (await this.authFetch(`/umbraco/backoffice/api/Imports/queued/${a}/files`, {
    method: "POST",
    headers: {
      Accept: "*/*",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(o)
  })).status === 200 ? (i(this, r).fields[t].value = e.name, n(this, s, v).call(this), n(this, s, E).call(this)) : alert("Failed to upload specified file, please contact support for assistance");
};
S = async function(t) {
  if (!this.authFetch)
    throw new Error("Authentication context not ready");
  const e = new FormData();
  return e.append("file", t), await (await this.authFetch("/umbraco/api/Storage/tempUpload", {
    method: "POST",
    body: e
  })).json();
};
E = function() {
  this.dispatchEvent(new b());
};
v = function() {
  var t;
  (t = i(this, l)) == null || t.render(
    A(I, {
      value: i(this, r),
      onTextChange: (e, a) => n(this, s, N).call(this, e, a),
      onFileSelected: (e, a) => void n(this, s, T).call(this, e, a)
    })
  );
};
g = U([
  F(q)
], g);
const z = g;
export {
  g as N3oImportDataEditorElement,
  z as default
};
//# sourceMappingURL=import-data-editor.js.map
