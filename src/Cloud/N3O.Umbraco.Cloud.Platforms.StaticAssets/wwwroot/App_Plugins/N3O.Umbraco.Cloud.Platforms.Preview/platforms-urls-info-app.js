import { UmbLitElement as b } from "@umbraco-cms/backoffice/lit-element";
import { css as x, state as w, customElement as C, html as d } from "@umbraco-cms/backoffice/external/lit";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as E } from "@umbraco-cms/backoffice/document";
import { UMB_AUTH_CONTEXT as P } from "@umbraco-cms/backoffice/auth";
var O = Object.defineProperty, A = Object.getOwnPropertyDescriptor, y = (t) => {
  throw TypeError(t);
}, g = (t, r, e, o) => {
  for (var i = o > 1 ? void 0 : o ? A(r, e) : r, a = t.length - 1, h; a >= 0; a--)
    (h = t[a]) && (i = (o ? h(r, e, i) : h(i)) || i);
  return o && i && O(r, e, i), i;
}, m = (t, r, e) => r.has(t) || y("Cannot " + e), u = (t, r, e) => (m(t, r, "read from private field"), r.get(t)), f = (t, r, e) => r.has(t) ? y("Cannot add the same private member more than once") : r instanceof WeakSet ? r.add(t) : r.set(t, e), U = (t, r, e, o) => (m(t, r, "write to private field"), r.set(t, e), e), c = (t, r, e) => (m(t, r, "access private method"), e), p, n, s, _, k, v;
const T = "n3o-platforms-urls-info-app";
let l = class extends b {
  constructor() {
    super(), f(this, s), this._stagingUrl = null, this._productionUrl = null, f(this, p), f(this, n, null), this.consumeContext(P, (t) => {
      U(this, n, t ? t.getOpenApiConfiguration() : null), u(this, p) && c(this, s, _).call(this, u(this, p));
    }), this.consumeContext(E, (t) => {
      this.observe(t == null ? void 0 : t.unique, (r) => {
        U(this, p, r), r && u(this, n) && c(this, s, _).call(this, r);
      });
    });
  }
  render() {
    return !this._stagingUrl && !this._productionUrl ? d`` : d`
            <umb-workspace-info-app-layout headline="Platform URLs">
                ${this._stagingUrl ? c(this, s, v).call(this, "Staging", this._stagingUrl) : ""}
                ${this._productionUrl ? c(this, s, v).call(this, "Production", this._productionUrl) : ""}
            </umb-workspace-info-app-layout>
        `;
  }
};
p = /* @__PURE__ */ new WeakMap();
n = /* @__PURE__ */ new WeakMap();
s = /* @__PURE__ */ new WeakSet();
_ = async function(t) {
  if (!u(this, n))
    return;
  const r = u(this, n).token, e = typeof r == "function" ? await r() : r, o = { Accept: "application/json" };
  e && (o.Authorization = `Bearer ${e}`);
  try {
    const a = await (await fetch(`/umbraco/backoffice/api/PlatformsBackOffice/contentUrls/${t}`, { headers: o })).json();
    this._stagingUrl = a.permitted ? a.stagingUrl ?? null : null, this._productionUrl = a.permitted ? a.productionUrl ?? null : null;
  } catch {
    this._stagingUrl = null, this._productionUrl = null;
  }
};
k = function(t) {
  navigator.clipboard.writeText(t);
};
v = function(t, r) {
  return d`
            <div class="url-row">
                <span class="label">${t}</span>
                <a href="${r}" target="_blank" rel="noreferrer" class="url">${r}</a>
                <button class="copy" @click=${() => c(this, s, k).call(this, r)} title="Copy">Copy</button>
            </div>
        `;
};
l.styles = x`
        :host { display: block; }

        .url-row {
            display: flex;
            align-items: baseline;
            gap: var(--uui-size-space-3, 8px);
            padding: var(--uui-size-space-2, 4px) 0;
            font-size: 0.8125rem;
        }

        .label {
            flex-shrink: 0;
            width: 72px;
            color: var(--uui-color-text-alt);
            font-weight: 600;
        }

        .url {
            flex: 1;
            color: var(--uui-color-interactive);
            word-break: break-all;
            text-decoration: none;
        }

        .url:hover {
            text-decoration: underline;
        }

        .copy {
            flex-shrink: 0;
            background: none;
            border: 1px solid var(--uui-color-border);
            border-radius: 3px;
            cursor: pointer;
            color: var(--uui-color-interactive);
            padding: 2px 8px;
            font-size: 0.75rem;
        }

        .copy:hover {
            background: var(--uui-color-surface-alt);
        }
    `;
g([
  w()
], l.prototype, "_stagingUrl", 2);
g([
  w()
], l.prototype, "_productionUrl", 2);
l = g([
  C(T)
], l);
const S = l;
export {
  l as N3oPlatformsUrlsInfoAppElement,
  S as default
};
//# sourceMappingURL=platforms-urls-info-app.js.map
