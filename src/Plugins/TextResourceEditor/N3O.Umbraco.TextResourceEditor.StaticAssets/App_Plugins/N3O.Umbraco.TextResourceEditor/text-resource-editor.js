import { LitElement as E, nothing as g, html as f, css as C, property as A, customElement as P } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as $ } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as N } from "@umbraco-cms/backoffice/property-editor";
var O = Object.defineProperty, T = Object.getOwnPropertyDescriptor, m = (t) => {
  throw TypeError(t);
}, w = (t, e, r, i) => {
  for (var o = i > 1 ? void 0 : i ? T(e, r) : e, p = t.length - 1, u; p >= 0; p--)
    (u = t[p]) && (o = (i ? u(e, r, o) : u(o)) || o);
  return i && o && O(e, r, o), o;
}, h = (t, e, r) => e.has(t) || m("Cannot " + r), a = (t, e, r) => (h(t, e, "read from private field"), r ? r.call(t) : e.get(t)), _ = (t, e, r) => e.has(t) ? m("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, r), d = (t, e, r, i) => (h(t, e, "write to private field"), e.set(t, r), r), l = (t, e, r) => (h(t, e, "access private method"), r), s, n, x, y, v;
const U = "n3o-text-resource-editor";
let c = class extends $(E) {
  constructor() {
    super(...arguments), _(this, n), _(this, s, []);
  }
  get value() {
    return a(this, s);
  }
  set value(t) {
    const e = a(this, s);
    d(this, s, Array.isArray(t) ? t : []), this.requestUpdate("value", e);
  }
  // config is set by Umbraco; not used by this editor but accepted to satisfy the contract.
  set config(t) {
  }
  render() {
    return a(this, s).length ? f`
            <div class="n3o-text-resource-editor">
                ${a(this, s).map(
      (t, e) => f`
                        <div class="row-wrapper">
                            <div class="row-1">
                                [<a @click=${() => l(this, n, x).call(this, e)} style="cursor: pointer;">x</a>]
                                <span class="text">${t.source}</span>
                            </div>
                            <div class="row-2">
                                <input
                                    type="text"
                                    class="custom"
                                    .value=${t.custom ?? ""}
                                    @input=${(r) => l(this, n, y).call(this, e, r)} />
                            </div>
                        </div>
                    `
    )}
            </div>
        ` : g;
  }
};
s = /* @__PURE__ */ new WeakMap();
n = /* @__PURE__ */ new WeakSet();
x = function(t) {
  confirm("Are you sure you wish to delete this entry?") && (d(this, s, a(this, s).filter((e, r) => r !== t)), l(this, n, v).call(this));
};
y = function(t, e) {
  const r = e.target.value;
  d(this, s, a(this, s).map((i, o) => o === t ? { ...i, custom: r } : i)), l(this, n, v).call(this);
};
v = function() {
  this.requestUpdate(), this.dispatchEvent(new N());
};
c.styles = C`
        .n3o-text-resource-editor .row-wrapper {
            margin-bottom: 40px;
            width: 100%;
        }

        .n3o-text-resource-editor .row-1 {
            display: block;
            width: 90%;
        }

        .n3o-text-resource-editor .row-2 {
            display: block;
            width: 90%;
        }

        .n3o-text-resource-editor .text {
            font-weight: bold;
        }

        .n3o-text-resource-editor .custom {
            width: 100%;
            margin-top: 10px;
        }
    `;
w([
  A({ type: Array })
], c.prototype, "value", 1);
c = w([
  P(U)
], c);
const W = c;
export {
  c as N3oTextResourceEditorElement,
  W as default
};
//# sourceMappingURL=text-resource-editor.js.map
