import { LitElement as B, html as $, nothing as x, css as F, customElement as H } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as L } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as z } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as G } from "@n3o/backoffice-core";
import { useRef as S, useEffect as J, createElement as K } from "react";
import { createRoot as X } from "react-dom/client";
import { jsx as Q } from "react/jsx-runtime";
function V(e, t) {
  const n = {};
  return e.forEach((i) => {
    n[i.alias] = i.value;
  }), n.contentTypeAlias = t, n;
}
function Y({ unique: e, getContent: t, authFetch: n }) {
  const i = S(null), a = S(null);
  return J(() => {
    let o = !0;
    const h = async () => {
      var W, N;
      if (!n)
        return;
      const p = t();
      if (!p)
        return;
      const M = (W = p.documentType) == null ? void 0 : W.unique, O = p, q = p.variants ?? [], g = q.find((A) => A.culture == null || A.segment == null) ?? q[0], j = p.values ?? [], w = V(j, M);
      w.name = g == null ? void 0 : g.name, w.key = p.unique, w.parentId = ((N = O.parent) == null ? void 0 : N.unique) ?? O.parentId;
      const D = await (await n("/umbraco/backoffice/api/cloudBackOffice/subscription/code")).json(), b = await (await n(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${M}`, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(w)
      })).json();
      if (b.eTag === a.current)
        return;
      a.current = b.eTag;
      const _ = i.current;
      if (!_ || !o)
        return;
      _.innerHTML = "";
      const r = document.createElement("iframe");
      r.style.width = "100%", r.style.aspectRatio = "16 / 9", r.style.border = "0", r.style.transform = "scale(0.9)", r.style.transformOrigin = "0 0", r.style.display = "none", _.appendChild(r);
      const f = r.contentWindow.document;
      f.open(), f.write(b.html), f.close();
      const P = f.createElement("script");
      P.src = `https://cdn.n3o.cloud/connect-${D}/platforms-js/platforms.js`, P.type = "module", f.body.appendChild(P), window.setTimeout(() => {
        o && (r.style.display = "block", _.style.display = "block");
      }, 2e3);
    };
    h();
    const U = window.setInterval(() => {
      h();
    }, 1e4);
    return () => {
      o = !1, window.clearInterval(U);
    };
  }, [e, t, n]), /* @__PURE__ */ Q("div", { ref: i, id: "platformsPreviewContainer", style: { display: "none" } });
}
var Z = Object.getOwnPropertyDescriptor, I = (e) => {
  throw TypeError(e);
}, ee = (e, t, n, i) => {
  for (var a = i > 1 ? void 0 : i ? Z(t, n) : t, o = e.length - 1, h; o >= 0; o--)
    (h = e[o]) && (a = h(a) || a);
  return a;
}, T = (e, t, n) => t.has(e) || I("Cannot " + n), s = (e, t, n) => (T(e, t, "read from private field"), t.get(e)), u = (e, t, n) => t.has(e) ? I("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, n), m = (e, t, n, i) => (T(e, t, "write to private field"), t.set(e, n), n), y = (e, t, n) => (T(e, t, "access private method"), n), C, k, l, c, R, d, v;
const te = "n3o-platforms-preview";
let E = class extends G(L(B)) {
  constructor() {
    super(), u(this, d), u(this, C), u(this, k), u(this, l), u(this, c), u(this, R, () => {
      var e;
      return (e = s(this, C)) == null ? void 0 : e.getData();
    }), m(this, c, document.createElement("div")), this.consumeContext(z, (e) => {
      m(this, C, e), this.observe(e == null ? void 0 : e.unique, (t) => {
        m(this, k, t), y(this, d, v).call(this);
      });
    });
  }
  connectedCallback() {
    super.connectedCallback(), y(this, d, v).call(this);
  }
  disconnectedCallback() {
    var e;
    super.disconnectedCallback(), (e = s(this, l)) == null || e.unmount(), m(this, l, void 0);
  }
  // Lit owns the shadow root; we host a single mount div in it and let React render into that.
  render() {
    return s(this, c).parentNode == null ? $`${s(this, c)}` : x;
  }
  updated() {
    !s(this, l) && s(this, c).isConnected && (m(this, l, X(s(this, c))), y(this, d, v).call(this));
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(e) {
    y(this, d, v).call(this);
  }
};
C = /* @__PURE__ */ new WeakMap();
k = /* @__PURE__ */ new WeakMap();
l = /* @__PURE__ */ new WeakMap();
c = /* @__PURE__ */ new WeakMap();
R = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakSet();
v = function() {
  var e;
  (e = s(this, l)) == null || e.render(
    K(Y, {
      unique: s(this, k),
      getContent: s(this, R),
      authFetch: this.authFetch
    })
  );
};
E.styles = F`
        :host {
            display: block;
            height: 100%;
            padding: var(--uui-size-layout-1);
        }
    `;
E = ee([
  H(te)
], E);
const ue = E;
export {
  E as N3oPlatformsPreviewElement,
  ue as default
};
//# sourceMappingURL=platforms-preview.js.map
