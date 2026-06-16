import { customElement as S } from "@umbraco-cms/backoffice/external/lit";
import { UMB_BLOCK_ENTRY_CONTEXT as D, UMB_BLOCK_MANAGER_CONTEXT as A } from "@umbraco-cms/backoffice/block";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as F } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as L, UmbElementMixin as R } from "@n3o/backoffice-core";
import { createElement as $ } from "react";
import { createRoot as G } from "react-dom/client";
import { jsxs as K, Fragment as H, jsx as E } from "react/jsx-runtime";
const X = ":host{display:block}.block-preview-frame{display:block;width:100%;border:none;transform:scale(.9);transform-origin:top left}.preview-alert{background-color:#f0ac00;border:1px solid transparent;border-radius:0;margin-bottom:20px;padding:8px 35px 8px 14px;position:relative}.preview-alert uui-loader{margin-right:16px}.preview-alert,.preview-alert a,.preview-alert h4{color:#fff}.preview-alert pre{white-space:normal}.preview-alert-warning{background-color:#f0ac00;border-color:transparent;color:#fff}.preview-alert-info{background-color:#3544b1;border-color:transparent;color:#fff}.preview-alert-danger,.preview-alert-error{background-color:#d42054;border-color:transparent;color:#fff}";
function q({ loaded: e, markup: t }) {
  return /* @__PURE__ */ K(H, { children: [
    e ? /* @__PURE__ */ E("div", { className: "block-preview-frame", dangerouslySetInnerHTML: { __html: t } }) : /* @__PURE__ */ K("div", { className: "preview-alert preview-alert-info", children: [
      /* @__PURE__ */ E("uui-loader", { style: { color: "#fff" } }),
      "Loading preview..."
    ] }),
    /* @__PURE__ */ E("style", { children: X })
  ] });
}
var I = Object.getOwnPropertyDescriptor, N = (e) => {
  throw TypeError(e);
}, V = (e, t, r, n) => {
  for (var h = n > 1 ? void 0 : n ? I(t, r) : t, l = e.length - 1, _; l >= 0; l--)
    (_ = e[l]) && (h = _(h) || h);
  return h;
}, W = (e, t, r) => t.has(e) || N("Cannot " + r), i = (e, t, r) => (W(e, t, "read from private field"), r ? r.call(e) : t.get(e)), a = (e, t, r) => t.has(e) ? N("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, r), o = (e, t, r, n) => (W(e, t, "write to private field"), t.set(e, r), r), c = (e, t, r) => (W(e, t, "access private method"), r), w, k, u, f, m, M, b, v, g, y, d, p, s, T, C, O, U, P, B;
const J = "n3o-block-preview";
let x = class extends L(R(HTMLElement)) {
  constructor() {
    super(), a(this, s), a(this, w), a(this, k), a(this, u), a(this, f), a(this, m, !1), a(this, M, ""), a(this, b), a(this, v), a(this, g, ""), a(this, y), a(this, d), a(this, p);
    const e = this.attachShadow({ mode: "open" });
    o(this, f, document.createElement("div")), e.appendChild(i(this, f)), this.consumeContext(F, (t) => {
      if (!t)
        return;
      const r = t;
      this.observe(r.unique, (n) => {
        o(this, b, n);
      }, "_observeUnique"), this.observe(
        r.splitView.activeVariantsInfo,
        (n) => {
          var l;
          const h = (l = n == null ? void 0 : n[0]) == null ? void 0 : l.culture;
          o(this, g, h ?? "");
        },
        "_observeCulture"
      );
    }), this.consumeContext(D, (t) => {
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
    o(this, w, e), c(this, s, T).call(this);
  }
  get settings() {
    return i(this, k);
  }
  set settings(e) {
    o(this, k, e), c(this, s, T).call(this);
  }
  // Re-render the preview when the shared authenticated fetch becomes available (mixin hook).
  authFetchChanged(e) {
    c(this, s, C).call(this, 0);
  }
  connectedCallback() {
    super.connectedCallback(), i(this, u) ?? o(this, u, G(i(this, f))), c(this, s, B).call(this), c(this, s, C).call(this, 0);
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
M = /* @__PURE__ */ new WeakMap();
b = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
g = /* @__PURE__ */ new WeakMap();
y = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
p = /* @__PURE__ */ new WeakMap();
s = /* @__PURE__ */ new WeakSet();
T = function() {
  i(this, m) && c(this, s, C).call(this, 500);
};
C = function(e) {
  i(this, d) !== void 0 && clearTimeout(i(this, d)), o(this, d, setTimeout(() => {
    c(this, s, P).call(this);
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
U = function(e) {
  return e ? `umb://element/${e.replace(/-/g, "")}` : "";
};
P = async function() {
  const e = c(this, s, O).call(this);
  if (!e || !i(this, v) || !this.authFetch)
    return;
  const t = i(this, b) ?? "", r = c(this, s, U).call(this, i(this, y)), n = i(this, g) ?? "", h = `/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock/?nodeKey=${t}&documentTypeKey=${i(this, v)}&contentUdi=${r}&culture=${n}`, l = await this.authFetch(h, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(e)
  });
  if (!l.ok)
    return;
  const _ = await l.json();
  o(this, M, _), o(this, m, !0), c(this, s, B).call(this);
};
B = function() {
  var e;
  (e = i(this, u)) == null || e.render(
    $(q, {
      loaded: i(this, m),
      markup: i(this, M)
    })
  );
};
x = V([
  S(J)
], x);
const re = x;
export {
  x as N3oBlockPreviewElement,
  re as default
};
//# sourceMappingURL=block-preview.js.map
