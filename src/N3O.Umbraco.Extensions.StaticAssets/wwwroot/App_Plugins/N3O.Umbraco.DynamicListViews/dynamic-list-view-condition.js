var m = (t) => {
  throw TypeError(t);
};
var r = (t, e, i) => e.has(t) || m("Cannot " + i);
var p = (t, e, i) => (r(t, e, "read from private field"), i ? i.call(t) : e.get(t)), c = (t, e, i) => e.has(t) ? m("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), d = (t, e, i, s) => (r(t, e, "write to private field"), s ? s.call(t, i) : e.set(t, i), i), f = (t, e, i) => (r(t, e, "access private method"), i);
import { UmbConditionBase as u } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as b } from "@umbraco-cms/backoffice/document";
var o, n, C;
class k extends u {
  constructor(i, s) {
    super(i, s);
    c(this, n);
    c(this, o);
    d(this, o, s), this.consumeContext(b, (a) => {
      a && this.observe(a.unique, (h) => {
        h && f(this, n, C).call(this, h);
      });
    });
  }
}
o = new WeakMap(), n = new WeakSet(), C = async function(i) {
  try {
    const s = await fetch(`/umbraco/backoffice/api/DynamicListViewApi/${i}`), a = await s.json();
    this.permitted = s.ok && a.enabled;
  } catch {
    this.permitted = !1;
  }
  p(this, o).onChange(this.permitted);
};
export {
  k as DynamicListViewCondition,
  k as default
};
//# sourceMappingURL=dynamic-list-view-condition.js.map
