import { LitElement as $, nothing as k, html as f, css as I, customElement as S } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as T } from "@umbraco-cms/backoffice/element-api";
import { UmbPropertyValueChangeEvent as C } from "@umbraco-cms/backoffice/property-editor";
var x = Object.getOwnPropertyDescriptor, _ = (t) => {
  throw TypeError(t);
}, D = (t, e, a, i) => {
  for (var o = i > 1 ? void 0 : i ? x(e, a) : e, c = t.length - 1, l; c >= 0; c--)
    (l = t[c]) && (o = l(o) || o);
  return o;
}, m = (t, e, a) => e.has(t) || _("Cannot " + a), r = (t, e, a) => (m(t, e, "read from private field"), a ? a.call(t) : e.get(t)), u = (t, e, a) => e.has(t) ? _("Cannot add the same private member more than once") : e instanceof WeakSet ? e.add(t) : e.set(t, a), w = (t, e, a, i) => (m(t, e, "write to private field"), e.set(t, a), a), p = (t, e, a) => (m(t, e, "access private method"), a), s, d, n, g, y, E, v;
const O = "n3o-import-data-editor";
let h = class extends T($) {
  constructor() {
    super(...arguments), u(this, n), u(this, s), u(this, d);
  }
  get value() {
    return r(this, s);
  }
  set value(t) {
    const e = r(this, s);
    w(this, s, t), this.requestUpdate("value", e);
  }
  set config(t) {
    w(this, d, t);
  }
  get config() {
    return r(this, d);
  }
  render() {
    var e;
    const t = ((e = r(this, s)) == null ? void 0 : e.fields) ?? [];
    return f`
            <div class="n3o-import-fields-editor">
                ${t.map(
      (a, i) => f`
                        <div class="row-wrapper">
                            <div class="row-1">
                                <span class="text">${a.name}</span>
                            </div>

                            <div class="row-2">
                                <input
                                    type="text"
                                    class="custom"
                                    .value=${a.value ?? ""}
                                    placeholder=${a.sourceValue ?? ""}
                                    @input=${(o) => p(this, n, g).call(this, i, o)} />

                                ${a.isFile ? f`<input
                                          type="file"
                                          id=${`fileInput_${i}`}
                                          @change=${() => void p(this, n, y).call(this, r(this, s).reference, i)} />` : k}
                            </div>
                        </div>
                    `
    )}
            </div>
        `;
  }
};
s = /* @__PURE__ */ new WeakMap();
d = /* @__PURE__ */ new WeakMap();
n = /* @__PURE__ */ new WeakSet();
g = function(t, e) {
  r(this, s) && (r(this, s).fields[t].value = e.target.value, p(this, n, v).call(this));
};
y = async function(t, e) {
  var l;
  const a = (l = this.shadowRoot) == null ? void 0 : l.getElementById(`fileInput_${e}`);
  if (!a || !a.files || a.files.length === 0)
    return;
  const o = { file: await p(this, n, E).call(this, a) };
  (await fetch(`/umbraco/backoffice/api/Imports/queued/${t}/files`, {
    method: "POST",
    headers: {
      Accept: "*/*",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(o)
  })).status === 200 ? (r(this, s).fields[e].value = a.files[0].name, this.requestUpdate("value"), p(this, n, v).call(this)) : alert("Failed to upload specified file, please contact support for assistance");
};
E = async function(t) {
  const e = new FormData();
  return e.append("file", t.files[0]), await (await fetch("/umbraco/api/Storage/tempUpload", {
    method: "POST",
    body: e
  })).json();
};
v = function() {
  this.dispatchEvent(new C());
};
h.styles = I`
        .n3o-import-fields-editor .row-wrapper {
            margin-bottom: 40px;
            width: 100%;
        }

        .n3o-import-fields-editor .row-1 {
            display: block;
            width: 90%;
        }

        .n3o-import-fields-editor .row-2 {
            display: block;
            width: 90%;
        }

        .n3o-import-fields-editor .text {
            font-weight: bold;
        }

        .n3o-import-fields-editor .custom {
            width: 100%;
            margin-top: 10px;
        }
    `;
h = D([
  S(O)
], h);
const q = h;
export {
  h as N3oImportDataEditorElement,
  q as default
};
//# sourceMappingURL=import-data-editor.js.map
