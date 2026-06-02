import { LitElement as $, html as m, css as w, property as E, customElement as q } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as N } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as P } from "@umbraco-cms/backoffice/property-editor";
var U = Object.defineProperty, k = Object.getOwnPropertyDescriptor, b = (t) => {
  throw TypeError(t);
}, g = (t, e, r, s) => {
  for (var n = s > 1 ? void 0 : s ? k(e, r) : e, c = t.length - 1, v; c >= 0; c--)
    (v = t[c]) && (n = (s ? v(e, r, n) : v(n)) || n);
  return s && n && U(e, r, n), n;
}, _ = (t, e, r) => e.has(t) || b("Cannot " + r), p = (t, e, r) => (_(t, e, "read from private field"), r ? r.call(t) : e.get(t)), C = (t, e, r) => e.has(t) ? b("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, r), h = (t, e, r, s) => (_(t, e, "write to private field"), e.set(t, r), r), i = (t, e, r) => (_(t, e, "access private method"), r), l, a, u, f, y, o;
const A = "n3o-cropper-crop-definitions";
let d = class extends N($) {
  constructor() {
    super(...arguments), C(this, a), C(this, l, null);
  }
  get value() {
    return p(this, l);
  }
  set value(t) {
    const e = p(this, l);
    h(this, l, t ?? null), this.requestUpdate("value", e);
  }
  connectedCallback() {
    super.connectedCallback(), p(this, l) || (h(this, l, []), i(this, a, f).call(this));
  }
  render() {
    return m`
            ${(p(this, l) ?? []).map(
      (t, e) => m`
                    <table>
                        <tr>
                            <td>Label</td>
                            <td>
                                <input
                                    type="text"
                                    required
                                    .value=${t.label ?? ""}
                                    @input=${(r) => i(this, a, o).call(this, e, "label", r.target.value)} />
                            </td>
                        </tr>
                        <tr>
                            <td>Alias</td>
                            <td>
                                <input
                                    type="text"
                                    required
                                    .value=${t.alias ?? ""}
                                    @input=${(r) => i(this, a, o).call(this, e, "alias", r.target.value)} />
                            </td>
                        </tr>
                        <tr>
                            <td>Width</td>
                            <td>
                                <input
                                    type="number"
                                    required
                                    .value=${t.width ?? ""}
                                    @input=${(r) => {
        const s = r.target.value;
        i(this, a, o).call(this, e, "width", s === "" ? null : Number(s));
      }} />
                            </td>
                        </tr>
                        <tr>
                            <td>Height&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <input
                                    type="number"
                                    required
                                    .value=${t.height ?? ""}
                                    @input=${(r) => {
        const s = r.target.value;
        i(this, a, o).call(this, e, "height", s === "" ? null : Number(s));
      }} />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">Filters</td>
                            <td>
                                <textarea
                                    cols="20"
                                    rows="6"
                                    .value=${t.filters ?? ""}
                                    @input=${(r) => i(this, a, o).call(this, e, "filters", r.target.value)}></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <p>
                                    <a @click=${() => i(this, a, y).call(this, e)} class="cursor delete"
                                        >Delete</a
                                    >
                                </p>
                            </td>
                        </tr>
                    </table>
                    <hr />
                `
    )}

            <p>
                <br />
                <a @click=${() => i(this, a, f).call(this)} class="cursor add">Add</a>
            </p>
        `;
  }
};
l = /* @__PURE__ */ new WeakMap();
a = /* @__PURE__ */ new WeakSet();
u = function() {
  this.dispatchEvent(new P());
};
f = function() {
  h(this, l, [
    ...p(this, l) ?? [],
    {
      label: "",
      alias: "",
      width: null,
      height: null,
      filters: null
    }
  ]), this.requestUpdate(), i(this, a, u).call(this);
};
y = function(t) {
  h(this, l, (p(this, l) ?? []).filter((e, r) => r !== t)), this.requestUpdate(), i(this, a, u).call(this);
};
o = function(t, e, r) {
  (p(this, l) ?? [])[t][e] = r, i(this, a, u).call(this);
};
d.styles = w`
        :host {
            display: block;
        }

        .cursor {
            cursor: pointer;
            text-decoration: none;
        }

        .delete {
            color: #ff0000;
        }

        .add {
            font-weight: bold;
        }

        hr {
            color: #666;
        }
    `;
g([
  E({ type: Array })
], d.prototype, "value", 1);
d = g([
  q(A)
], d);
const x = d;
export {
  d as N3oCropperCropDefinitionsElement,
  x as default
};
//# sourceMappingURL=crop-definitions.js.map
