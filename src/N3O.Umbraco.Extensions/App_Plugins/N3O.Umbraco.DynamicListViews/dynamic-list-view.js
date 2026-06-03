import { LitElement as r, html as c, css as a, customElement as u } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as p } from "@umbraco-cms/backoffice/element-api";
var w = Object.getOwnPropertyDescriptor, v = (o, l, i, s) => {
  for (var e = s > 1 ? void 0 : s ? w(l, i) : l, n = o.length - 1, m; n >= 0; n--)
    (m = o[n]) && (e = m(e) || e);
  return e;
};
const d = "n3o-dynamic-list-view";
let t = class extends p(r) {
  render() {
    return c`<umb-document-workspace-view-collection></umb-document-workspace-view-collection>`;
  }
};
t.styles = a`:host { display: block; height: 100%; }`;
t = v([
  u(d)
], t);
const b = t;
export {
  t as N3oDynamicListViewElement,
  b as default
};
//# sourceMappingURL=dynamic-list-view.js.map
