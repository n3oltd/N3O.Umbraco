import { LitElement as _, nothing as p, html as o, css as f, customElement as E } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as x } from "@umbraco-cms/backoffice/element-api";
var N = Object.getOwnPropertyDescriptor, m = (r) => {
  throw TypeError(r);
}, $ = (r, e, t, c) => {
  for (var n = c > 1 ? void 0 : c ? N(e, t) : e, v = r.length - 1, w; v >= 0; v--)
    (w = r[v]) && (n = w(n) || n);
  return n;
}, u = (r, e, t) => e.has(r) || m("Cannot " + t), i = (r, e, t) => (u(r, e, "read from private field"), t ? t.call(r) : e.get(r)), d = (r, e, t) => e.has(r) ? m("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(r) : e.set(r, t), y = (r, e, t, c) => (u(r, e, "write to private field"), e.set(r, t), t), s, a, g, h;
const k = "n3o-import-notices-viewer";
let l = class extends x(_) {
  constructor() {
    super(...arguments), d(this, a), d(this, s);
  }
  get value() {
    return i(this, s);
  }
  set value(r) {
    const e = i(this, s);
    y(this, s, r), this.requestUpdate("value", e);
  }
  // Config is set by Umbraco for property editors; unused here but accepted to avoid warnings.
  set config(r) {
  }
  get config() {
  }
  render() {
    const r = i(this, a, g), e = i(this, a, h);
    return o`
            <div class="n3o-import-errors-viewer">
                ${r && r.length ? o`
                          <p><em class="text-error">Errors</em></p>
                          ${r.map(
      (t) => o`
                                  <div class="row-wrapper">
                                      <div class="row">${t}</div>
                                  </div>
                              `
    )}
                      ` : p}
                ${e && e.length ? o`
                          <p><em class="text-warning">Warnings</em></p>
                          ${e.map(
      (t) => o`
                                  <div class="row-wrapper">
                                      <div class="row">${t}</div>
                                  </div>
                              `
    )}
                      ` : p}
                ${(!r || !r.length) && (!e || !e.length) ? o`
                          <div class="row-wrapper">
                              <div class="row">No warnings or errors</div>
                          </div>
                      ` : p}
            </div>
        `;
  }
};
s = /* @__PURE__ */ new WeakMap();
a = /* @__PURE__ */ new WeakSet();
g = function() {
  var r;
  return ((r = i(this, s)) == null ? void 0 : r.errors) ?? null;
};
h = function() {
  var r;
  return ((r = i(this, s)) == null ? void 0 : r.warnings) ?? null;
};
l.styles = f`
        .n3o-import-errors-viewer .row-wrapper {
            margin-bottom: 40px;
            width: 100%;
        }

        .n3o-import-errors-viewer .row {
            display: block;
            width: 90%;
        }

        .text-error {
            color: var(--uui-color-danger);
        }

        .text-warning {
            color: var(--uui-color-warning);
        }
    `;
l = $([
  E(k)
], l);
const W = l;
export {
  l as N3oImportNoticesViewerElement,
  W as default
};
//# sourceMappingURL=import-notices-viewer.js.map
