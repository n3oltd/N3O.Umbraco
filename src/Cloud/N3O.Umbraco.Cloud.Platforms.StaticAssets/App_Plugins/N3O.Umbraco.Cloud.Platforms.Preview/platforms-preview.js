import { LitElement as M, html as I, css as N, customElement as W } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as A } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as S } from "@umbraco-cms/backoffice/document";
var q = Object.getOwnPropertyDescriptor, b = (e) => {
  throw TypeError(e);
}, x = (e, t, n, a) => {
  for (var i = a > 1 ? void 0 : a ? q(t, n) : t, c = e.length - 1, o; c >= 0; c--)
    (o = e[c]) && (i = o(i) || i);
  return i;
}, E = (e, t, n) => t.has(e) || b("Cannot " + n), p = (e, t, n) => (E(e, t, "read from private field"), t.get(e)), u = (e, t, n) => t.has(e) ? b("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, n), m = (e, t, n, a) => (E(e, t, "write to private field"), t.set(e, n), n), C = (e, t, n) => (E(e, t, "access private method"), n), f, v, r, d, k, O;
const B = "n3o-platforms-preview";
let w = class extends A(M) {
  constructor() {
    super(), u(this, d), u(this, f), u(this, v, null), u(this, r), this.consumeContext(S, (e) => {
      m(this, f, e);
    });
  }
  connectedCallback() {
    super.connectedCallback(), C(this, d, k).call(this), m(this, r, window.setInterval(() => {
      C(this, d, k).call(this);
    }, 1e4));
  }
  disconnectedCallback() {
    super.disconnectedCallback(), p(this, r) !== void 0 && (window.clearInterval(p(this, r)), m(this, r, void 0));
  }
  render() {
    return I`<div id="platformsPreviewContainer" style="display: none;"></div>`;
  }
};
f = /* @__PURE__ */ new WeakMap();
v = /* @__PURE__ */ new WeakMap();
r = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakSet();
k = async function() {
  var g, T;
  if (!p(this, f))
    return;
  const e = p(this, f).getData();
  if (!e)
    return;
  const t = e, n = ((g = t.contentType) == null ? void 0 : g.alias) ?? t.contentTypeAlias, a = e.variants ?? [], i = a.find((P) => P.culture == null || P.segment == null) ?? a[0], c = e.values ?? [], o = C(this, d, O).call(this, c, n);
  o.name = i == null ? void 0 : i.name, o.key = e.unique, o.parentId = ((T = t.parent) == null ? void 0 : T.unique) ?? t.parentId;
  const R = await (await fetch("/umbraco/backoffice/api/cloudBackOffice/subscription/code")).json(), y = await (await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${n}`, {
    method: "POST",
    headers: {
      accept: "application/json",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(o)
  })).json();
  if (y.eTag === p(this, v))
    return;
  m(this, v, y.eTag);
  const h = this.renderRoot.getElementById("platformsPreviewContainer");
  if (!h)
    return;
  h.innerHTML = "";
  const s = document.createElement("iframe");
  s.style.width = "100%", s.style.aspectRatio = "16 / 9", s.style.border = "0", s.style.transform = "scale(0.9)", s.style.transformOrigin = "0 0", s.style.display = "none", h.appendChild(s);
  const l = s.contentWindow.document;
  l.open(), l.write(y.html), l.close();
  const _ = l.createElement("script");
  _.src = `https://cdn.n3o.cloud/connect-${R}/platforms-js/platforms.js`, _.type = "module", l.body.appendChild(_), window.setInterval(() => {
    s.style.display = "block", h.style.display = "block";
  }, 2e3);
};
O = function(e, t) {
  const n = {};
  return e.forEach((a) => {
    n[a.alias] = a.value;
  }), n.contentTypeAlias = t, n;
};
w.styles = N`
        :host {
            display: block;
            height: 100%;
            padding: var(--uui-size-layout-1);
        }
    `;
w = x([
  W(B)
], w);
const $ = w;
export {
  w as N3oPlatformsPreviewElement,
  $ as default
};
//# sourceMappingURL=platforms-preview.js.map
