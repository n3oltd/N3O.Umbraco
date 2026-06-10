var f = (t) => {
  throw TypeError(t);
};
var m = (t, e, i) => e.has(t) || f("Cannot " + i);
var v = (t, e, i) => (m(t, e, "read from private field"), i ? i.call(t) : e.get(t)), b = (t, e, i) => e.has(t) ? f("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), y = (t, e, i, a) => (m(t, e, "write to private field"), a ? a.call(t, i) : e.set(t, i), i);
import { LitElement as P, html as l, nothing as $, css as x, state as _, customElement as O } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as N } from "@umbraco-cms/backoffice/element-api";
import { UmbDocumentTreeRepository as S, UMB_EDIT_DOCUMENT_WORKSPACE_PATH_PATTERN as A, UMB_DOCUMENT_WORKSPACE_CONTEXT as M } from "@umbraco-cms/backoffice/document";
const R = 100;
var u;
class U {
  constructor(e) {
    b(this, u);
    y(this, u, new S(e));
  }
  async getChildren(e) {
    const { data: i } = await v(this, u).requestTreeItemsOf({
      parent: { unique: e, entityType: "document" },
      paging: { skip: 0, take: R }
    }), a = ((i == null ? void 0 : i.items) ?? []).map(z);
    return { items: a, total: (i == null ? void 0 : i.total) ?? a.length };
  }
}
u = new WeakMap();
function z(t) {
  var e, i, a, n;
  return {
    unique: t.unique,
    name: ((i = (e = t.variants) == null ? void 0 : e[0]) == null ? void 0 : i.name) ?? "(unnamed)",
    state: ((n = (a = t.variants) == null ? void 0 : a[0]) == null ? void 0 : n.state) ?? "Unknown",
    icon: t.documentType.icon,
    createDate: t.createDate,
    editPath: A.generateAbsolute({ unique: t.unique })
  };
}
var L = Object.defineProperty, k = Object.getOwnPropertyDescriptor, w = (t) => {
  throw TypeError(t);
}, r = (t, e, i, a) => {
  for (var n = a > 1 ? void 0 : a ? k(e, i) : e, c = t.length - 1, h; c >= 0; c--)
    (h = t[c]) && (n = (a ? h(e, i, n) : h(n)) || n);
  return a && n && L(e, i, n), n;
}, E = (t, e, i) => e.has(t) || w("Cannot " + i), q = (t, e, i) => (E(t, e, "read from private field"), i ? i.call(t) : e.get(t)), g = (t, e, i) => e.has(t) ? w("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), d = (t, e, i) => (E(t, e, "access private method"), i), p, o, D, C, T;
const W = "n3o-dynamic-list-view";
let s = class extends N(P) {
  constructor() {
    super(), g(this, o), this._items = [], this._total = 0, this._loading = !0, g(this, p, new U(this)), this.consumeContext(M, (t) => {
      t && this.observe(t.unique, (e) => {
        e ? d(this, o, D).call(this, e) : d(this, o, C).call(this);
      });
    });
  }
  render() {
    return this._loading ? l`<div class="center"><uui-loader></uui-loader></div>` : this._items.length ? l`
            <uui-box>
                <uui-table>
                    <uui-table-head>
                        <uui-table-head-cell>Name</uui-table-head-cell>
                        <uui-table-head-cell>Status</uui-table-head-cell>
                        <uui-table-head-cell>Created</uui-table-head-cell>
                    </uui-table-head>
                    ${this._items.map((t) => d(this, o, T).call(this, t))}
                </uui-table>
                ${this._total > this._items.length ? l`<div class="footnote">Showing ${this._items.length} of ${this._total} items.</div>` : $}
            </uui-box>
        ` : l`<uui-box><div class="center">There are no child items.</div></uui-box>`;
  }
};
p = /* @__PURE__ */ new WeakMap();
o = /* @__PURE__ */ new WeakSet();
D = async function(t) {
  this._loading = !0;
  const { items: e, total: i } = await q(this, p).getChildren(t);
  this._items = e, this._total = i, this._loading = !1;
};
C = function() {
  this._items = [], this._total = 0;
};
T = function(t) {
  const e = t.state === "Published" ? "positive" : t.state === "Draft" ? "warning" : "default";
  return l`
            <uui-table-row>
                <uui-table-cell>
                    <uui-button compact look="default" href=${t.editPath} label=${t.name}>
                        <uui-icon name=${t.icon}></uui-icon>
                        <span style="margin-left: var(--uui-size-space-2)">${t.name}</span>
                    </uui-button>
                </uui-table-cell>
                <uui-table-cell><uui-tag color=${e} look="secondary">${t.state}</uui-tag></uui-table-cell>
                <uui-table-cell>${new Date(t.createDate).toLocaleDateString()}</uui-table-cell>
            </uui-table-row>
        `;
};
s.styles = x`
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
  O(W)
], s);
const K = s;
export {
  s as N3oDynamicListViewElement,
  K as default
};
//# sourceMappingURL=dynamic-list-view.js.map
