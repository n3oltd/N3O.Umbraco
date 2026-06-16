var f = (t) => {
  throw TypeError(t);
};
var p = (t, e, i) => e.has(t) || f("Cannot " + i);
var o = (t, e, i) => (p(t, e, "read from private field"), i ? i.call(t) : e.get(t)), h = (t, e, i) => e.has(t) ? f("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), l = (t, e, i, s) => (p(t, e, "write to private field"), s ? s.call(t, i) : e.set(t, i), i), m = (t, e, i) => (p(t, e, "access private method"), i);
import { UmbConditionBase as A } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as T } from "@umbraco-cms/backoffice/document";
import { UMB_AUTH_CONTEXT as w } from "@umbraco-cms/backoffice/auth";
import { createAuthFetch as y } from "@n3o/backoffice-core";
var c, r, u, n, d, C;
class g extends A {
  constructor(i, s) {
    super(i, s);
    h(this, n);
    h(this, c);
    h(this, r, null);
    h(this, u, null);
    l(this, c, s), this.consumeContext(w, (a) => {
      l(this, r, a ? y(a.getOpenApiConfiguration()) : null), m(this, n, d).call(this);
    }), this.consumeContext(T, (a) => {
      a && this.observe(a.unique, (b) => {
        l(this, u, b ?? null), m(this, n, d).call(this);
      });
    });
  }
}
c = new WeakMap(), r = new WeakMap(), u = new WeakMap(), n = new WeakSet(), d = async function() {
  !o(this, u) || !o(this, r) || (this.permitted = await m(this, n, C).call(this, o(this, u)), o(this, c).onChange(this.permitted));
}, C = async function(i) {
  try {
    const s = await o(this, r).call(this, `/umbraco/backoffice/api/DynamicListViewApi/${i}`, {
      headers: { Accept: "application/json" }
    });
    return s.ok ? (await s.json()).enabled === !0 : !1;
  } catch {
    return !1;
  }
};
export {
  g as DynamicListViewCondition,
  g as default
};
//# sourceMappingURL=dynamic-list-view-condition.js.map
