import { LitElement as C, nothing as m, html as p, css as N, property as S, state as k, customElement as A } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as D } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as M } from "@umbraco-cms/backoffice/property-editor";
var O = Object.defineProperty, P = Object.getOwnPropertyDescriptor, E = (t) => {
  throw TypeError(t);
}, x = (t, e, i, a) => {
  for (var o = a > 1 ? void 0 : a ? P(e, i) : e, u = t.length - 1, _; u >= 0; u--)
    (_ = t[u]) && (o = (a ? _(e, i, o) : _(o)) || o);
  return a && o && O(e, i, o), o;
}, g = (t, e, i) => e.has(t) || E("Cannot " + i), r = (t, e, i) => (g(t, e, "read from private field"), i ? i.call(t) : e.get(t)), f = (t, e, i) => e.has(t) ? E("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, i), v = (t, e, i, a) => (g(t, e, "write to private field"), e.set(t, i), i), d = (t, e, i) => (g(t, e, "access private method"), i), s, h, c, n, $, y, w, b;
const T = "n3o-serp-editor";
let l = class extends D(C) {
  constructor() {
    super(...arguments), f(this, n), f(this, s, { title: "", description: "" }), f(this, h, 60), f(this, c, 160), this._titleSuffix = "";
  }
  get value() {
    return r(this, s);
  }
  set value(t) {
    const e = r(this, s);
    v(this, s, t ?? { title: "", description: "" }), this.requestUpdate("value", e);
  }
  // Config (prevalues) arrives as UmbPropertyEditorConfigCollection.
  set config(t) {
    if (!t)
      return;
    const e = Number.parseInt(t.getValueByAlias("maxCharsTitle") ?? "", 10), i = Number.parseInt(t.getValueByAlias("maxCharsDescription") ?? "", 10);
    !Number.isNaN(e) && e > 0 && v(this, h, e), !Number.isNaN(i) && i > 0 && v(this, c, i);
  }
  async connectedCallback() {
    super.connectedCallback();
    try {
      const t = await fetch("/umbraco/backoffice/api/serpEditor/templateOptions");
      if (t.ok) {
        const e = await t.json();
        this._titleSuffix = e.titleSuffix ?? "";
      }
    } catch {
    }
  }
  render() {
    var i, a;
    const t = ((i = r(this, s)) == null ? void 0 : i.title) ?? "", e = ((a = r(this, s)) == null ? void 0 : a.description) ?? "";
    return p`
            <div class="sv-form">
                <input
                    type="text"
                    .value=${t}
                    placeholder="Enter a short but descriptive title"
                    @input=${d(this, n, $)} />
                ${t.length > r(this, h) ? p`<p class="sv-error">A title should not be more than ${r(this, h)} characters.</p>` : m}
                <br /><br />
                <textarea
                    .value=${e}
                    placeholder="Enter a meta description"
                    @input=${d(this, n, y)}></textarea>
                ${e.length > r(this, c) ? p`<p class="sv-error">
                          A meta description should not be more than ${r(this, c)} chars.
                      </p>` : m}
            </div>

            <div class="sv-demo">
                ${t.length > 0 ? p`<h6>${t} ${this._titleSuffix}</h6>` : m}
                ${t.length > 0 || e.length > 0 ? p`<p class="sv-url">${d(this, n, b).call(this)}</p>` : m}
                <p>${e}</p>
            </div>

            <div style="clear: both"></div>
        `;
  }
};
s = /* @__PURE__ */ new WeakMap();
h = /* @__PURE__ */ new WeakMap();
c = /* @__PURE__ */ new WeakMap();
n = /* @__PURE__ */ new WeakSet();
$ = function(t) {
  d(this, n, w).call(this, { title: t.target.value });
};
y = function(t) {
  d(this, n, w).call(this, { description: t.target.value });
};
w = function(t) {
  var e, i;
  v(this, s, {
    title: ((e = r(this, s)) == null ? void 0 : e.title) ?? "",
    description: ((i = r(this, s)) == null ? void 0 : i.description) ?? "",
    ...t
  }), this.requestUpdate(), this.dispatchEvent(new M());
};
b = function() {
  return `${location.protocol}//${window.location.hostname}`;
};
l.styles = N`
        /* containers */
        .sv-form {
            width: 30%;
            float: left;
            margin-right: 40px;
        }

        .sv-demo {
            width: 600px; /* The width of the desktop-SERP as of 2019-11-14 */
            float: left;
        }

        /* form elements */
        .sv-form input,
        .sv-form textarea {
            width: 100%;
        }

        .sv-form textarea {
            height: 100px;
        }

        /* general text formating */
        .sv-demo h6,
        .sv-demo p {
            font-family: Arial, Helvectiva, san-serif;
            padding: 0;
            margin: 0;
        }

        /* form text formating */
        .sv-form p.sv-error {
            color: red;
            margin-top: 3px;
        }

        /* demo-mode text formating */
        .sv-demo h6 {
            font-size: 20px;
            line-height: 1.3;
            margin-bottom: 3px;
            color: blue;
            text-decoration: underline;
        }

        .sv-demo p {
            font-size: 14px;
            margin-bottom: 3px;
            line-height: 1.57;
            word-wrap: break-word;
        }

        .sv-demo p.sv-url {
            color: #00802a;
        }
    `;
x([
  S({ type: Object })
], l.prototype, "value", 1);
x([
  k()
], l.prototype, "_titleSuffix", 2);
l = x([
  A(T)
], l);
const V = l;
export {
  l as N3oSerpEditorElement,
  V as default
};
//# sourceMappingURL=serp-editor.js.map
