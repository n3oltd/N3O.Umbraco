import { LitElement as B, css as R, state as u, customElement as V, nothing as g, html as n } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as W } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as J } from "@umbraco-cms/backoffice/document";
var K = Object.defineProperty, Z = Object.getOwnPropertyDescriptor, S = (e) => {
  throw TypeError(e);
}, l = (e, t, s, c) => {
  for (var r = c > 1 ? void 0 : c ? Z(t, s) : t, h = e.length - 1, d; h >= 0; h--)
    (d = e[h]) && (r = (c ? d(t, s, r) : d(r)) || r);
  return c && r && K(t, s, r), r;
}, T = (e, t, s) => t.has(e) || S("Cannot " + s), w = (e, t, s) => (T(e, t, "read from private field"), t.get(e)), k = (e, t, s) => t.has(e) ? S("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, s), G = (e, t, s, c) => (T(e, t, "write to private field"), t.set(e, s), s), i = (e, t, s) => (T(e, t, "access private method"), s), _, o, C, U, A, O, z, E, I, x, F, L, M, D, y, f, j, q, N;
const Q = "n3o-data-import";
let a = class extends W(B) {
  constructor() {
    super(), k(this, o), this._show = "form", this._processing = !1, this._contentTypes = [], this._contentType = null, this._datePatterns = [], this._datePattern = null, this._moveUpdatedContentToCurrentLocation = !1, this._importableProperties = [], this._errorMessages = null, k(this, _, null), this.consumeContext(J, (e) => {
      e && this.observe(
        e.unique,
        (t) => {
          t && t !== w(this, _) && (G(this, _, t), i(this, o, U).call(this));
        },
        "_observeUnique"
      );
    });
  }
  render() {
    switch (this._show) {
      case "success":
        return i(this, o, q).call(this);
      case "error":
        return i(this, o, N).call(this);
      default:
        return i(this, o, j).call(this);
    }
  }
};
_ = /* @__PURE__ */ new WeakMap();
o = /* @__PURE__ */ new WeakSet();
C = function() {
  this._processing = !1, this._contentType = null, this._errorMessages = null, this._importableProperties = [], this._show = "form";
};
U = async function() {
  this._contentTypes = await i(this, o, A).call(this, w(this, _));
  const t = await (await fetch("/umbraco/backoffice/api/Imports/lookups/datePatterns", {
    headers: { Accept: "application/json" }
  })).json();
  this._datePatterns = t, this._datePattern = t[0] ?? null;
};
A = async function(e) {
  return await (await fetch(`/umbraco/api/ContentTypes/${e}/relations?type=child`, {
    headers: { Accept: "application/json" }
  })).json();
};
O = async function() {
  if (!this._contentType) {
    this._importableProperties = [];
    return;
  }
  const t = await (await fetch(`/umbraco/backoffice/api/Imports/importableProperties/${this._contentType.alias}`, {
    headers: { Accept: "application/json" }
  })).json();
  for (const s of t)
    s.selected = !1;
  this._importableProperties = t;
};
z = function(e) {
  const t = e.target.value;
  this._contentType = this._contentTypes.find((s) => s.alias === t) ?? null, i(this, o, O).call(this);
};
E = function(e) {
  const t = e.target.value;
  this._datePattern = this._datePatterns.find((s) => s.id === t) ?? null;
};
I = function(e) {
  this._moveUpdatedContentToCurrentLocation = e.target.checked;
};
x = function(e, t) {
  e.selected = t.target.checked, this.requestUpdate();
};
F = function() {
  for (const e of this._importableProperties)
    e.selected = !0;
  this.requestUpdate();
};
L = function() {
  for (const e of this._importableProperties)
    e.selected = !1;
  this.requestUpdate();
};
M = async function() {
  var P, $;
  const e = this._importableProperties.filter((b) => b.selected).map((b) => b.alias);
  if (!e.length) {
    i(this, o, f).call(this, "At least one property must be selected");
    return;
  }
  const t = { properties: e }, s = await fetch(`/umbraco/backoffice/api/Imports/template/${this._contentType.alias}`, {
    method: "POST",
    headers: {
      Accept: "*/*",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(t)
  }), c = await s.blob(), d = ((P = ((s.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : P.replaceAll('"', "")) ?? "template.csv", m = new Blob([c]), v = window.URL.createObjectURL(m), p = document.createElement("a");
  p.href = v, p.setAttribute("download", d), document.body.appendChild(p), p.click(), ($ = p.parentNode) == null || $.removeChild(p), window.URL.revokeObjectURL(v);
};
D = async function() {
  var m, v, p;
  this._processing = !0;
  const e = this.renderRoot, t = e.getElementById("csvFile"), s = e.getElementById("zipFile");
  if (!t.value || ((m = t.value.split(".")[1]) == null ? void 0 : m.toLowerCase()) !== "csv") {
    i(this, o, f).call(this, "A valid CSV file must be specified");
    return;
  }
  if (s.value && ((v = s.value.split(".")[1]) == null ? void 0 : v.toLowerCase()) !== "zip") {
    i(this, o, f).call(this, "The selected file is not a valid ZIP file");
    return;
  }
  const c = await i(this, o, y).call(this, t), r = await i(this, o, y).call(this, s), h = {
    datePattern: (p = this._datePattern) == null ? void 0 : p.id,
    moveUpdatedContentToCurrentLocation: this._moveUpdatedContentToCurrentLocation,
    csvFile: c,
    zipFile: r
  }, d = await fetch(
    `/umbraco/backoffice/api/Imports/queue/${w(this, _)}/${this._contentType.alias}`,
    {
      method: "POST",
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(h)
    }
  );
  d.status === 200 ? (this._show = "success", this._processing = !1) : i(this, o, f).call(this, await d.json());
};
y = async function(e) {
  if (!e.files || e.files.length === 0)
    return null;
  const t = new FormData();
  return t.append("file", e.files[0]), await (await fetch("/umbraco/api/Storage/tempUpload", {
    method: "POST",
    body: t
  })).json();
};
f = function(e) {
  Array.isArray(e) || (e = [e]), this._processing = !1, this._errorMessages = e, this._show = "error";
};
j = function() {
  return n`
            <div class="umb-group-panel">
                <div class="umb-group-panel__header">Options</div>

                <div class="umb-group-panel__content">
                    <div class="control-group">
                        <label>Content Type <strong class="required">*</strong></label>
                        <select @change=${i(this, o, z)} ?disabled=${this._processing}>
                            <option value="" ?selected=${!this._contentType}></option>
                            ${this._contentTypes.map(
    (e) => n`<option value=${e.alias}>${e.name}</option>`
  )}
                        </select>
                    </div>

                    <div class="control-group">
                        <label>Date Pattern <strong class="required">*</strong></label>
                        <select @change=${i(this, o, E)} ?disabled=${this._processing}>
                            ${this._datePatterns.map(
    (e) => n`<option value=${e.id}>${e.name}</option>`
  )}
                        </select>
                    </div>

                    <div class="control-group">
                        <label>Move Updated Content to Current Location</label>
                        <input
                            type="checkbox"
                            .checked=${this._moveUpdatedContentToCurrentLocation}
                            @change=${i(this, o, I)}
                            ?disabled=${this._processing} />
                    </div>

                    <div class="control-group">
                        <label>CSV File <strong class="required">*</strong></label>
                        <input type="file" id="csvFile" ?disabled=${this._processing} />
                    </div>

                    <div class="control-group">
                        <label>ZIP Assets File (optional)</label>
                        <input type="file" id="zipFile" ?disabled=${this._processing} />
                    </div>
                </div>
            </div>

            ${this._contentType ? n`
                      <div class="umb-group-panel">
                          <div class="umb-group-panel__header">Properties</div>

                          <div class="umb-group-panel__content">
                              <div class="listTable">
                                  <a class="link" @click=${i(this, o, F)}>Select All</a> |
                                  <a class="link" @click=${i(this, o, L)}>Clear Selection</a>

                                  <ul class="selectionCheckBoxes">
                                      ${this._importableProperties.map(
    (e) => n`
                                              <li>
                                                  <label>
                                                      <input
                                                          type="checkbox"
                                                          .value=${e.alias}
                                                          .checked=${!!e.selected}
                                                          @change=${(t) => i(this, o, x).call(this, e, t)} />
                                                      &nbsp;${e.columnTitle}
                                                  </label>
                                              </li>
                                          `
  )}
                                  </ul>
                              </div>
                          </div>
                      </div>
                  ` : g}

            <div class="actions">
                ${this._contentType ? n`<uui-button look="secondary" label="Download Template" @click=${i(this, o, M)}>
                          Download Template
                      </uui-button>` : g}
                <uui-button
                    look="primary"
                    label="Import"
                    ?disabled=${this._processing}
                    @click=${i(this, o, D)}>
                    ${this._processing ? "Please wait..." : "Import"}
                </uui-button>
            </div>
        `;
};
q = function() {
  return n`
            <div class="umb-group-panel">
                <div class="umb-group-panel__header">Processing</div>

                <div class="umb-group-panel__content">
                    <p>CSV file is processing and will appear shortly.</p>
                    <p class="actions">
                        <uui-button look="primary" href="/umbraco#/content?dashboard=imports">
                            View Import Queue
                        </uui-button>
                        <uui-button look="secondary" label="Import Another File" @click=${() => i(this, o, C).call(this)}>
                            Import Another File
                        </uui-button>
                    </p>
                </div>
            </div>
        `;
};
N = function() {
  return n`
            <div class="umb-group-panel">
                <div class="umb-group-panel__header">Error</div>

                <div class="umb-group-panel__content">
                    ${this._errorMessages ? n`<ul>
                              ${this._errorMessages.map((e) => n`<li class="text-error">${e}</li>`)}
                          </ul>` : g}
                    <p>
                        <uui-button look="secondary" label="Start Over" @click=${() => i(this, o, C).call(this)}>
                            Start Over
                        </uui-button>
                    </p>
                </div>
            </div>
        `;
};
a.styles = R`
        :host {
            display: block;
            padding: var(--uui-size-layout-1);
        }

        .umb-group-panel {
            background: var(--uui-color-surface);
            border: 1px solid var(--uui-color-border);
            border-radius: var(--uui-border-radius);
            margin-bottom: var(--uui-size-space-5);
        }

        .umb-group-panel__header {
            padding: var(--uui-size-space-4) var(--uui-size-space-5);
            border-bottom: 1px solid var(--uui-color-border);
            font-weight: bold;
        }

        .umb-group-panel__content {
            padding: var(--uui-size-space-5);
        }

        .control-group {
            margin-bottom: var(--uui-size-space-4);
        }

        .control-group label {
            display: block;
            margin-bottom: var(--uui-size-space-2);
            font-weight: bold;
        }

        .required {
            color: var(--uui-color-danger);
        }

        select {
            min-width: 250px;
            padding: var(--uui-size-space-2);
        }

        .listTable .link {
            cursor: pointer;
            color: var(--uui-color-interactive);
        }

        .selectionCheckBoxes {
            list-style: none;
            padding: 0;
            margin-top: var(--uui-size-space-4);
        }

        .selectionCheckBoxes li {
            margin-bottom: var(--uui-size-space-2);
        }

        .actions {
            display: flex;
            gap: var(--uui-size-space-3);
            align-items: center;
        }

        .text-error {
            color: var(--uui-color-danger);
        }
    `;
l([
  u()
], a.prototype, "_show", 2);
l([
  u()
], a.prototype, "_processing", 2);
l([
  u()
], a.prototype, "_contentTypes", 2);
l([
  u()
], a.prototype, "_contentType", 2);
l([
  u()
], a.prototype, "_datePatterns", 2);
l([
  u()
], a.prototype, "_datePattern", 2);
l([
  u()
], a.prototype, "_moveUpdatedContentToCurrentLocation", 2);
l([
  u()
], a.prototype, "_importableProperties", 2);
l([
  u()
], a.prototype, "_errorMessages", 2);
a = l([
  V(Q)
], a);
const ee = a;
export {
  a as N3oDataImportElement,
  ee as default
};
//# sourceMappingURL=data-import.js.map
