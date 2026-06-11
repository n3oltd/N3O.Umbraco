var m = (t) => {
  throw TypeError(t);
};
var o = (t, e, i) => e.has(t) || m("Cannot " + i);
var u = (t, e, i) => (o(t, e, "read from private field"), i ? i.call(t) : e.get(t)), c = (t, e, i) => e.has(t) ? m("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), f = (t, e, i, s) => (o(t, e, "write to private field"), s ? s.call(t, i) : e.set(t, i), i), d = (t, e, i) => (o(t, e, "access private method"), i);
import { UmbConditionBase as C } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as b } from "@umbraco-cms/backoffice/document";
var n, a, p, l;
class v extends C {
  constructor(i, s) {
    super(i, s);
    c(this, a);
    c(this, n);
    f(this, n, s), this.consumeContext(b, (r) => {
      r && this.observe(r.unique, (h) => {
        h && d(this, a, p).call(this, h);
      });
    });
  }
}
n = new WeakMap(), a = new WeakSet(), p = async function(i) {
  this.permitted = await d(this, a, l).call(this, i), u(this, n).onChange(this.permitted);
}, l = async function(i) {
  try {
    const s = await fetch(`/umbraco/api/DynamicListViewApi/${i}`);
    return s.ok ? (await s.json()).enabled === !0 : !1;
  } catch {
    return !1;
  }
};
export {
  v as DynamicListViewCondition,
  v as default
};
//# sourceMappingURL=dynamic-list-view-condition.js.map
