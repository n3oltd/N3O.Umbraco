import { customElement as S } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as D } from "@umbraco-cms/backoffice/element-api";
import { UMB_BLOCK_ENTRY_CONTEXT as L, UMB_BLOCK_MANAGER_CONTEXT as A } from "@umbraco-cms/backoffice/block";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as R } from "@umbraco-cms/backoffice/document";
import { createElement as $ } from "react";
import { createRoot as G } from "react-dom/client";
import { jsxs as K, Fragment as H, jsx as M } from "react/jsx-runtime";
const X = ":host{display:block}.block-preview-frame{display:block;width:100%;border:none;transform:scale(.9);transform-origin:top left}.preview-alert{background-color:#f0ac00;border:1px solid transparent;border-radius:0;margin-bottom:20px;padding:8px 35px 8px 14px;position:relative}.preview-alert uui-loader{margin-right:16px}.preview-alert,.preview-alert a,.preview-alert h4{color:#fff}.preview-alert pre{white-space:normal}.preview-alert-warning{background-color:#f0ac00;border-color:transparent;color:#fff}.preview-alert-info{background-color:#3544b1;border-color:transparent;color:#fff}.preview-alert-danger,.preview-alert-error{background-color:#d42054;border-color:transparent;color:#fff}";
function q({ loaded: e, markup: t }) {
  return /* @__PURE__ */ K(H, { children: [
    e ? /* @__PURE__ */ M("div", { className: "block-preview-frame", dangerouslySetInnerHTML: { __html: t } }) : /* @__PURE__ */ K("div", { className: "preview-alert preview-alert-info", children: [
      /* @__PURE__ */ M("uui-loader", { style: { color: "#fff" } }),
      "Loading preview..."
    ] }),
    /* @__PURE__ */ M("style", { children: X })
  ] });
}
var I = Object.getOwnPropertyDescriptor, N = (e) => {
  throw TypeError(e);
}, V = (e, t, r, n) => {
  for (var h = n > 1 ? void 0 : n ? I(t, r) : t, c = e.length - 1, _; c >= 0; c--)
    (_ = e[c]) && (h = _(h) || h);
  return h;
}, x = (e, t, r) => t.has(e) || N("Cannot " + r), i = (e, t, r) => (x(e, t, "read from private field"), r ? r.call(e) : t.get(e)), s = (e, t, r) => t.has(e) ? N("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), o = (e, t, r, n) => (x(e, t, "write to private field"), t.set(e, r), r), l = (e, t, r) => (x(e, t, "access private method"), r), w, k, u, f, m, C, b, v, g, y, d, p, a, E, W, O, P, U, B;
const F = "n3o-block-preview";
let T = class extends D(HTMLElement) {
  constructor() {
    super(), s(this, a), s(this, w), s(this, k), s(this, u), s(this, f), s(this, m, !1), s(this, C, ""), s(this, b), s(this, v), s(this, g, ""), s(this, y), s(this, d), s(this, p);
    const e = this.attachShadow({ mode: "open" });
    o(this, f, document.createElement("div")), e.appendChild(i(this, f)), this.consumeContext(R, (t) => {
      if (!t)
        return;
      const r = t;
      this.observe(r.unique, (n) => {
        o(this, b, n);
      }, "_observeUnique"), this.observe(
        r.splitView.activeVariantsInfo,
        (n) => {
          var c;
          const h = (c = n == null ? void 0 : n[0]) == null ? void 0 : c.culture;
          o(this, g, h ?? "");
        },
        "_observeCulture"
      );
    }), this.consumeContext(L, (t) => {
      t && (this.observe(t.contentKey, (r) => {
        o(this, y, r);
      }, "_observeContentKey"), this.observe(t.contentElementTypeKey, (r) => {
        o(this, v, r);
      }, "_observeContentElementTypeKey"));
    }), this.consumeContext(A, (t) => {
      o(this, p, t);
    });
  }
  get content() {
    return i(this, w);
  }
  set content(e) {
    o(this, w, e), l(this, a, E).call(this);
  }
  get settings() {
    return i(this, k);
  }
  set settings(e) {
    o(this, k, e), l(this, a, E).call(this);
  }
  connectedCallback() {
    super.connectedCallback(), i(this, u) ?? o(this, u, G(i(this, f))), l(this, a, B).call(this), l(this, a, W).call(this, 0);
  }
  disconnectedCallback() {
    var e;
    super.disconnectedCallback(), i(this, d) !== void 0 && (clearTimeout(i(this, d)), o(this, d, void 0)), (e = i(this, u)) == null || e.unmount(), o(this, u, void 0);
  }
};
w = /* @__PURE__ */ new WeakMap();
k = /* @__PURE__ */ new WeakMap();
u = /* @__PURE__ */ new WeakMap();
f = /* @__PURE__ */ new WeakMap();
m = /* @__PURE__ */ new WeakMap();
C = /* @__PURE__ */ new WeakMap();
b = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
g = /* @__PURE__ */ new WeakMap();
y = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
p = /* @__PURE__ */ new WeakMap();
a = /* @__PURE__ */ new WeakSet();
E = function() {
  i(this, m) && l(this, a, W).call(this, 500);
};
W = function(e) {
  i(this, d) !== void 0 && clearTimeout(i(this, d)), o(this, d, setTimeout(() => {
    l(this, a, U).call(this);
  }, e));
};
O = function() {
  if (!i(this, p))
    return null;
  const e = i(this, p).getLayouts(), t = i(this, p).getContents(), r = i(this, p).getSettings(), n = i(this, p).getExposes();
  return {
    layout: {
      "Umbraco.BlockGrid": e
    },
    contentData: t,
    settingsData: r,
    expose: n
  };
};
P = function(e) {
  return e ? `umb://element/${e.replace(/-/g, "")}` : "";
};
U = async function() {
  const e = l(this, a, O).call(this);
  if (!e || !i(this, v))
    return;
  const t = i(this, b) ?? "", r = l(this, a, P).call(this, i(this, y)), n = i(this, g) ?? "", h = `/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock/?nodeKey=${t}&documentTypeKey=${i(this, v)}&contentUdi=${r}&culture=${n}`, c = await fetch(h, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(e)
  });
  if (!c.ok)
    return;
  const _ = await c.json();
  o(this, C, _), o(this, m, !0), l(this, a, B).call(this);
};
B = function() {
  var e;
  (e = i(this, u)) == null || e.render(
    $(q, {
      loaded: i(this, m),
      markup: i(this, C)
    })
  );
};
T = V([
  S(F)
], T);
const te = T;
export {
  T as N3oBlockPreviewElement,
  te as default
};
//# sourceMappingURL=block-preview.js.map
