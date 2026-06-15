var g = (i) => {
  throw TypeError(i);
};
var m = (i, e, t) => e.has(i) || g("Cannot " + t);
var o = (i, e, t) => (m(i, e, "read from private field"), t ? t.call(i) : e.get(i)), c = (i, e, t) => e.has(i) ? g("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(i) : e.set(i, t), p = (i, e, t, s) => (m(i, e, "write to private field"), s ? s.call(i, t) : e.set(i, t), t), d = (i, e, t) => (m(i, e, "access private method"), t);
import { UmbConditionBase as A } from "@umbraco-cms/backoffice/extension-registry";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as O } from "@umbraco-cms/backoffice/document";
import { UMB_AUTH_CONTEXT as U } from "@umbraco-cms/backoffice/auth";
var h, r, u, a, C, w;
class B extends A {
  constructor(t, s) {
    super(t, s);
    c(this, a);
    c(this, h);
    c(this, r, null);
    c(this, u, null);
    p(this, h, s), this.consumeContext(U, (n) => {
      p(this, r, n ? n.getOpenApiConfiguration() : null), d(this, a, C).call(this);
    }), this.consumeContext(O, (n) => {
      n && this.observe(n.unique, (l) => {
        p(this, u, l ?? null), d(this, a, C).call(this);
      });
    });
  }
}
h = new WeakMap(), r = new WeakMap(), u = new WeakMap(), a = new WeakSet(), C = async function() {
  var s;
  const t = (s = o(this, h).config) == null ? void 0 : s.endpoint;
  !t || !o(this, u) || !o(this, r) || (this.permitted = await d(this, a, w).call(this, t, o(this, u)), o(this, h).onChange(this.permitted));
}, w = async function(t, s) {
  try {
    const n = await o(this, r).token(), l = new Headers({ Accept: "application/json" });
    n && l.set("Authorization", `Bearer ${n}`);
    const T = await fetch(`${t}/${s}`, {
      credentials: o(this, r).credentials,
      headers: l
    });
    if (!T.ok)
      return !1;
    const f = await T.json();
    return (f == null ? void 0 : f.permitted) === !0;
  } catch {
    return !1;
  }
};
export {
  B as WorkspaceVisibilityCondition,
  B as default
};
//# sourceMappingURL=workspace-visibility-condition.js.map
