import { LitElement as B, html as U, nothing as $, css as H, customElement as L } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as x } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as z } from "@umbraco-cms/backoffice/document";
import { useRef as I, useEffect as G, createElement as J } from "react";
import { createRoot as K } from "react-dom/client";
import { jsx as X } from "react/jsx-runtime";
function F(e, t) {
  const n = {};
  return e.forEach((a) => {
    n[a.alias] = a.value;
  }), n.contentTypeAlias = t, n;
}
function Q({ unique: e, getContent: t }) {
  const n = I(null), a = I(null);
  return G(() => {
    let r = !0;
    const l = async () => {
      var q, N;
      const d = t();
      if (!d)
        return;
      const v = d, M = ((q = v.contentType) == null ? void 0 : q.alias) ?? v.contentTypeAlias, W = d.variants ?? [], g = W.find((A) => A.culture == null || A.segment == null) ?? W[0], j = d.values ?? [], w = F(j, M);
      w.name = g == null ? void 0 : g.name, w.key = d.unique, w.parentId = ((N = v.parent) == null ? void 0 : N.unique) ?? v.parentId;
      const D = await (await fetch("/umbraco/backoffice/api/cloudBackOffice/subscription/code")).json(), P = await (await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${M}`, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(w)
      })).json();
      if (P.eTag === a.current)
        return;
      a.current = P.eTag;
      const y = n.current;
      if (!y || !r)
        return;
      y.innerHTML = "";
      const i = document.createElement("iframe");
      i.style.width = "100%", i.style.aspectRatio = "16 / 9", i.style.border = "0", i.style.transform = "scale(0.9)", i.style.transformOrigin = "0 0", i.style.display = "none", y.appendChild(i);
      const u = i.contentWindow.document;
      u.open(), u.write(P.html), u.close();
      const T = u.createElement("script");
      T.src = `https://cdn.n3o.cloud/connect-${D}/platforms-js/platforms.js`, T.type = "module", u.body.appendChild(T), window.setInterval(() => {
        i.style.display = "block", y.style.display = "block";
      }, 2e3);
    };
    l();
    const m = window.setInterval(() => {
      l();
    }, 1e4);
    return () => {
      r = !1, window.clearInterval(m);
    };
  }, [e, t]), /* @__PURE__ */ X("div", { ref: n, id: "platformsPreviewContainer", style: { display: "none" } });
}
var V = Object.getOwnPropertyDescriptor, S = (e) => {
  throw TypeError(e);
}, Y = (e, t, n, a) => {
  for (var r = a > 1 ? void 0 : a ? V(t, n) : t, l = e.length - 1, m; l >= 0; l--)
    (m = e[l]) && (r = m(r) || r);
  return r;
}, R = (e, t, n) => t.has(e) || S("Cannot " + n), s = (e, t, n) => (R(e, t, "read from private field"), t.get(e)), p = (e, t, n) => t.has(e) ? S("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, n), f = (e, t, n, a) => (R(e, t, "write to private field"), t.set(e, n), n), b = (e, t, n) => (R(e, t, "access private method"), n), _, k, c, o, O, h, C;
const Z = "n3o-platforms-preview";
let E = class extends x(B) {
  constructor() {
    super(), p(this, h), p(this, _), p(this, k), p(this, c), p(this, o), p(this, O, () => {
      var e;
      return (e = s(this, _)) == null ? void 0 : e.getData();
    }), f(this, o, document.createElement("div")), this.consumeContext(z, (e) => {
      f(this, _, e), this.observe(e == null ? void 0 : e.unique, (t) => {
        f(this, k, t), b(this, h, C).call(this);
      });
    });
  }
  connectedCallback() {
    super.connectedCallback(), b(this, h, C).call(this);
  }
  disconnectedCallback() {
    var e;
    super.disconnectedCallback(), (e = s(this, c)) == null || e.unmount(), f(this, c, void 0);
  }
  // Lit owns the shadow root; we host a single mount div in it and let React render into that.
  render() {
    return s(this, o).parentNode == null ? U`${s(this, o)}` : $;
  }
  updated() {
    !s(this, c) && s(this, o).isConnected && (f(this, c, K(s(this, o))), b(this, h, C).call(this));
  }
};
_ = /* @__PURE__ */ new WeakMap();
k = /* @__PURE__ */ new WeakMap();
c = /* @__PURE__ */ new WeakMap();
o = /* @__PURE__ */ new WeakMap();
O = /* @__PURE__ */ new WeakMap();
h = /* @__PURE__ */ new WeakSet();
C = function() {
  var e;
  (e = s(this, c)) == null || e.render(
    J(Q, {
      unique: s(this, k),
      getContent: s(this, O)
    })
  );
};
E.styles = H`
        :host {
            display: block;
            height: 100%;
            padding: var(--uui-size-layout-1);
        }
    `;
E = Y([
  L(Z)
], E);
const ce = E;
export {
  E as N3oPlatformsPreviewElement,
  ce as default
};
//# sourceMappingURL=platforms-preview.js.map
