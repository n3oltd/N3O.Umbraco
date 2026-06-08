import { LitElement as E, html as u, nothing as T, css as D, state as _, customElement as C } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as P } from "@umbraco-cms/backoffice/element-api";
import { UmbDocumentTreeRepository as $, UMB_DOCUMENT_WORKSPACE_CONTEXT as x, UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN as O } from "@umbraco-cms/backoffice/document";
var N = Object.defineProperty, S = Object.getOwnPropertyDescriptor, m = (t) => {
  throw TypeError(t);
}, r = (t, e, i, o) => {
  for (var l = o > 1 ? void 0 : o ? S(e, i) : e, c = t.length - 1, h; c >= 0; c--)
    (h = t[c]) && (l = (o ? h(e, i, l) : h(l)) || l);
  return o && l && N(e, i, l), l;
}, v = (t, e, i) => e.has(t) || m("Cannot " + i), A = (t, e, i) => (v(t, e, "read from private field"), i ? i.call(t) : e.get(t)), f = (t, e, i) => e.has(t) ? m("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), n = (t, e, i) => (v(t, e, "access private method"), i), p, a, b, d, g, y, w;
const M = "n3o-dynamic-list-view", U = 100;
let s = class extends P(E) {
  constructor() {
    super(), f(this, a), this._items = [], this._total = 0, this._loading = !0, f(this, p, new $(this)), this.consumeContext(x, (t) => {
      t && this.observe(t.unique, (e) => {
        e ? n(this, a, b).call(this, e) : (this._items = [], this._total = 0);
      });
    });
  }
  render() {
    return this._loading ? u`<div class="center"><uui-loader></uui-loader></div>` : this._items.length ? u`
            <uui-box>
                <uui-table>
                    <uui-table-head>
                        <uui-table-head-cell>Name</uui-table-head-cell>
                        <uui-table-head-cell>Status</uui-table-head-cell>
                        <uui-table-head-cell>Created</uui-table-head-cell>
                    </uui-table-head>
                    ${this._items.map((t) => n(this, a, w).call(this, t))}
                </uui-table>
                ${this._total > this._items.length ? u`<div class="footnote">Showing ${this._items.length} of ${this._total} items.</div>` : T}
            </uui-box>
        ` : u`<uui-box><div class="center">There are no child items.</div></uui-box>`;
  }
};
p = /* @__PURE__ */ new WeakMap();
a = /* @__PURE__ */ new WeakSet();
b = async function(t) {
  this._loading = !0;
  const { data: e } = await A(this, p).requestTreeItemsOf({
    parent: { unique: t, entityType: "document" },
    paging: { skip: 0, take: U }
  });
  this._items = (e == null ? void 0 : e.items) ?? [], this._total = (e == null ? void 0 : e.total) ?? this._items.length, this._loading = !1;
};
d = function(t) {
  var e, i;
  return ((i = (e = t.variants) == null ? void 0 : e[0]) == null ? void 0 : i.name) ?? "(unnamed)";
};
g = function(t) {
  var e, i;
  return ((i = (e = t.variants) == null ? void 0 : e[0]) == null ? void 0 : i.state) ?? "Unknown";
};
y = function(t) {
  return O.generateAbsolute({ unique: t });
};
w = function(t) {
  const e = n(this, a, g).call(this, t), i = e === "Published" ? "positive" : e === "Draft" ? "warning" : "default";
  return u`
            <uui-table-row>
                <uui-table-cell>
                    <uui-button compact look="default" href=${n(this, a, y).call(this, t.unique)} label=${n(this, a, d).call(this, t)}>
                        <uui-icon name=${t.documentType.icon}></uui-icon>
                        <span style="margin-left: var(--uui-size-space-2)">${n(this, a, d).call(this, t)}</span>
                    </uui-button>
                </uui-table-cell>
                <uui-table-cell><uui-tag color=${i} look="secondary">${e}</uui-tag></uui-table-cell>
                <uui-table-cell>${new Date(t.createDate).toLocaleDateString()}</uui-table-cell>
            </uui-table-row>
        `;
};
s.styles = D`
        :host { display: block; padding: var(--uui-size-layout-1); }
        .center { display: flex; justify-content: center; padding: var(--uui-size-layout-1); }
        .footnote { color: var(--uui-color-text-alt); font-size: var(--uui-type-small-size); padding-top: var(--uui-size-space-4); }
        uui-table-cell uui-button { --uui-button-padding-left-factor: 0; text-align: left; }
    `;
r([
  _()
], s.prototype, "_items", 2);
r([
  _()
], s.prototype, "_total", 2);
r([
  _()
], s.prototype, "_loading", 2);
s = r([
  C(M)
], s);
const L = s;
export {
  s as N3oDynamicListViewElement,
  L as default
};
//# sourceMappingURL=dynamic-list-view.js.map
