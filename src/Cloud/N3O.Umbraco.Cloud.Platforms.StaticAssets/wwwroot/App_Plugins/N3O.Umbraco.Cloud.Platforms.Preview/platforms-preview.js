import { LitElement as U, html as B, nothing as $, css as H, customElement as L } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as x } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as z } from "@umbraco-cms/backoffice/document";
import { useRef as S, useEffect as G, createElement as J } from "react";
import { createRoot as K } from "react-dom/client";
import { jsx as X } from "react/jsx-runtime";
function F(e, t) {
  const n = {};
  return e.forEach((a) => {
    n[a.alias] = a.value;
  }), n.contentTypeAlias = t, n;
}
function Q({ unique: e, getContent: t }) {
  const n = S(null), a = S(null);
  return G(() => {
    let i = !0;
    const l = async () => {
      var W, N;
      const p = t();
      if (!p)
        return;
      const O = (W = p.documentType) == null ? void 0 : W.unique, q = p, M = p.variants ?? [], E = M.find((I) => I.culture == null || I.segment == null) ?? M[0], j = p.values ?? [], v = F(j, O);
      v.name = E == null ? void 0 : E.name, v.key = p.unique, v.parentId = ((N = q.parent) == null ? void 0 : N.unique) ?? q.parentId;
      const D = await (await fetch("/umbraco/backoffice/api/cloudBackOffice/subscription/code")).json(), g = await (await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${O}`, {
        method: "POST",
        headers: {
          accept: "application/json",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(v)
      })).json();
      if (g.eTag === a.current)
        return;
      a.current = g.eTag;
      const w = n.current;
      if (!w || !i)
        return;
      w.innerHTML = "";
      const r = document.createElement("iframe");
      r.style.width = "100%", r.style.aspectRatio = "16 / 9", r.style.border = "0", r.style.transform = "scale(0.9)", r.style.transformOrigin = "0 0", r.style.display = "none", w.appendChild(r);
      const u = r.contentWindow.document;
      u.open(), u.write(g.html), u.close();
      const P = u.createElement("script");
      P.src = `https://cdn.n3o.cloud/connect-${D}/platforms-js/platforms.js`, P.type = "module", u.body.appendChild(P), window.setInterval(() => {
        r.style.display = "block", w.style.display = "block";
      }, 2e3);
    };
    l();
    const m = window.setInterval(() => {
      l();
    }, 1e4);
    return () => {
      i = !1, window.clearInterval(m);
    };
  }, [e, t]), /* @__PURE__ */ X("div", { ref: n, id: "platformsPreviewContainer", style: { display: "none" } });
}
var V = Object.getOwnPropertyDescriptor, A = (e) => {
  throw TypeError(e);
}, Y = (e, t, n, a) => {
  for (var i = a > 1 ? void 0 : a ? V(t, n) : t, l = e.length - 1, m; l >= 0; l--)
    (m = e[l]) && (i = m(i) || i);
  return i;
}, T = (e, t, n) => t.has(e) || A("Cannot " + n), s = (e, t, n) => (T(e, t, "read from private field"), t.get(e)), d = (e, t, n) => t.has(e) ? A("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, n), f = (e, t, n, a) => (T(e, t, "write to private field"), t.set(e, n), n), b = (e, t, n) => (T(e, t, "access private method"), n), y, C, c, o, R, h, _;
const Z = "n3o-platforms-preview";
let k = class extends x(U) {
  constructor() {
    super(), d(this, h), d(this, y), d(this, C), d(this, c), d(this, o), d(this, R, () => {
      var e;
      return (e = s(this, y)) == null ? void 0 : e.getData();
    }), f(this, o, document.createElement("div")), this.consumeContext(z, (e) => {
      f(this, y, e), this.observe(e == null ? void 0 : e.unique, (t) => {
        f(this, C, t), b(this, h, _).call(this);
      });
    });
  }
  connectedCallback() {
    super.connectedCallback(), b(this, h, _).call(this);
  }
  disconnectedCallback() {
    var e;
    super.disconnectedCallback(), (e = s(this, c)) == null || e.unmount(), f(this, c, void 0);
  }
  // Lit owns the shadow root; we host a single mount div in it and let React render into that.
  render() {
    return s(this, o).parentNode == null ? B`${s(this, o)}` : $;
  }
  updated() {
    !s(this, c) && s(this, o).isConnected && (f(this, c, K(s(this, o))), b(this, h, _).call(this));
  }
};
y = /* @__PURE__ */ new WeakMap();
C = /* @__PURE__ */ new WeakMap();
c = /* @__PURE__ */ new WeakMap();
o = /* @__PURE__ */ new WeakMap();
R = /* @__PURE__ */ new WeakMap();
h = /* @__PURE__ */ new WeakSet();
_ = function() {
  var e;
  (e = s(this, c)) == null || e.render(
    J(Q, {
      unique: s(this, C),
      getContent: s(this, R)
    })
  );
};
k.styles = H`
        :host {
            display: block;
            height: 100%;
            padding: var(--uui-size-layout-1);
        }
    `;
k = Y([
  L(Z)
], k);
const ce = k;
export {
  k as N3oPlatformsPreviewElement,
  ce as default
};
//# sourceMappingURL=platforms-preview.js.map
