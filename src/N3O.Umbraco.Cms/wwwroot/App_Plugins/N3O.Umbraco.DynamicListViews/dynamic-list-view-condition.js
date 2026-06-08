var p = (t) => {
  throw TypeError(t);
};
var c = (t, i, e) => i.has(t) || p("Cannot " + e);
var h = (t, i, e) => (c(t, i, "read from private field"), e ? e.call(t) : i.get(t)), m = (t, i, e) => i.has(t) ? p("Cannot add the same private member more than once") : i instanceof WeakSet ? i.add(t) : i.set(t, e), d = (t, i, e, s) => (c(t, i, "write to private field"), s ? s.call(t, e) : i.set(t, e), e), f = (t, i, e) => (c(t, i, "access private method"), e);
import { UmbConditionBase as l } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as b } from "@umbraco-cms/backoffice/document";
import { UMB_AUTH_CONTEXT as g } from "@umbraco-cms/backoffice/auth";
var a, r, C;
class U extends l {
  constructor(e, s) {
    super(e, s);
    m(this, r);
    m(this, a);
    d(this, a, s), this.consumeContext(b, (o) => {
      o && this.observe(o.unique, (n) => {
        n && f(this, r, C).call(this, n);
      });
    });
  }
}
a = new WeakMap(), r = new WeakSet(), C = async function(e) {
  try {
    const s = await this.getContext(g);
    if (!s) {
      this.permitted = !1, h(this, a).onChange(this.permitted);
      return;
    }
    const o = s.getOpenApiConfiguration(), n = await fetch(`${o.base}/umbraco/backoffice/api/DynamicListViewApi/${e}`, {
      credentials: o.credentials,
      headers: { Authorization: `Bearer ${await o.token()}` }
    }), u = await n.json();
    this.permitted = n.ok && u.enabled === !0;
  } catch {
    this.permitted = !1;
  }
  h(this, a).onChange(this.permitted);
};
export {
  U as DynamicListViewCondition,
  U as default
};
//# sourceMappingURL=dynamic-list-view-condition.js.map
