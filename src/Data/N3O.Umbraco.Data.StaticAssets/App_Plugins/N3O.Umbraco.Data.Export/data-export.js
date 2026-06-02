import { LitElement as O, nothing as N, html as _, css as D, state as p, customElement as R } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as B } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as K } from "@umbraco-cms/backoffice/document";
var L = Object.defineProperty, z = Object.getOwnPropertyDescriptor, y = (e) => {
  throw TypeError(e);
}, c = (e, t, s, l) => {
  for (var n = l > 1 ? void 0 : l ? z(t, s) : t, o = e.length - 1, h; o >= 0; o--)
    (h = e[o]) && (n = (l ? h(t, s, n) : h(n)) || n);
  return l && n && L(t, s, n), n;
}, W = (e, t, s) => t.has(e) || y("Cannot " + s), F = (e, t, s) => t.has(e) ? y("Cannot add the same private member more than once") : t instanceof WeakSet ? t.add(e) : t.set(e, s), i = (e, t, s) => (W(e, t, "access private method"), s), a, g, x, v, $, T, k, u, P, E, C, w, j, A;
const G = "n3o-data-export";
let r = class extends B(O) {
  constructor() {
    super(), F(this, a), this._contentKey = null, this._contentTypes = [], this._contentType = null, this._format = "excel", this._includeUnpublished = !1, this._metadatas = [], this._exportableProperties = [], this._processing = !1, this._progress = "", this._errorMessage = null, this.consumeContext(K, (e) => {
      e && this.observe(
        e.unique,
        (t) => {
          t && t !== this._contentKey && (this._contentKey = t, i(this, a, g).call(this, t));
        },
        "_observeUnique"
      );
    });
  }
  render() {
    return _`
            <uui-box headline="Options">
                <umb-property-layout label="Content Type" mandatory>
                    <div slot="editor">
                        <select
                            ?disabled=${this._processing}
                            @change=${i(this, a, $)}>
                            <option value="" ?selected=${!this._contentType}></option>
                            ${this._contentTypes.map(
      (e) => _`<option value=${e.alias}>${e.name}</option>`
    )}
                        </select>
                    </div>
                </umb-property-layout>

                <umb-property-layout label="Format" mandatory>
                    <div slot="editor">
                        <label>
                            <input
                                type="radio"
                                name="format"
                                value="excel"
                                .checked=${this._format === "excel"}
                                ?disabled=${this._processing}
                                @change=${() => this._format = "excel"} />
                            Excel
                        </label>
                        <br />
                        <label>
                            <input
                                type="radio"
                                name="format"
                                value="csv"
                                .checked=${this._format === "csv"}
                                ?disabled=${this._processing}
                                @change=${() => this._format = "csv"} />
                            CSV
                        </label>
                    </div>
                </umb-property-layout>

                <umb-property-layout label="Include Unpublished" mandatory>
                    <div slot="editor">
                        <input
                            type="checkbox"
                            .checked=${this._includeUnpublished}
                            ?disabled=${this._processing}
                            @change=${(e) => this._includeUnpublished = e.target.checked} />
                    </div>
                </umb-property-layout>
            </uui-box>

            <uui-box headline="Metadata">
                <div class="listTable">
                    <a class="umb-outline" @click=${i(this, a, P)}>Select All</a> |
                    <a class="umb-outline" @click=${i(this, a, E)}>Clear Selection</a>

                    <br /><br />

                    <ul class="selectionCheckBoxes">
                        ${this._metadatas.map(
      (e) => _`
                                <li>
                                    <label>
                                        <input
                                            type="checkbox"
                                            .checked=${!!e.selected}
                                            @change=${(t) => i(this, a, j).call(this, e, t.target.checked)} />
                                        &nbsp;${e.name}
                                    </label>
                                </li>
                            `
    )}
                    </ul>
                </div>
            </uui-box>

            <uui-box headline="Properties">
                <div class="listTable">
                    <a class="umb-outline" @click=${i(this, a, C)}>Select All</a> |
                    <a class="umb-outline" @click=${i(this, a, w)}>Clear Selection</a>

                    <br /><br />

                    <ul class="selectionCheckBoxes">
                        ${this._exportableProperties.map(
      (e) => _`
                                <li>
                                    <label>
                                        <input
                                            type="checkbox"
                                            .checked=${!!e.selected}
                                            @change=${(t) => i(this, a, A).call(this, e, t.target.checked)} />
                                        &nbsp;${e.columnTitle}
                                    </label>
                                </li>
                            `
    )}
                    </ul>
                </div>

                ${this._errorMessage ? _`<em class="text-error">${this._errorMessage}</em>` : N}
            </uui-box>

            <div class="actions">
                <uui-button
                    look="primary"
                    ?disabled=${this._processing}
                    @click=${i(this, a, k)}
                    label=${this._processing && this._progress || "Export"}>
                </uui-button>
            </div>
        `;
  }
};
a = /* @__PURE__ */ new WeakSet();
g = async function(e) {
  this._contentTypes = await i(this, a, x).call(this, e);
  const t = await fetch("/umbraco/backoffice/api/Exports/lookups/contentMetadata", {
    headers: { Accept: "application/json" }
  }).then((s) => s.json());
  for (const s of t)
    s.selected = s.autoSelected;
  t.sort((s, l) => s.displayOrder - l.displayOrder), this._metadatas = t;
};
x = async function(e) {
  return await (await fetch(`/umbraco/api/ContentTypes/${e}/relations?type=descendant`, {
    headers: { Accept: "application/json" }
  })).json();
};
v = async function() {
  if (!this._contentType) {
    this._exportableProperties = [];
    return;
  }
  const e = await fetch(
    `/umbraco/backoffice/api/Exports/exportableProperties/${this._contentType.alias}`,
    { headers: { Accept: "application/json" } }
  ).then((t) => t.json());
  for (const t of e)
    t.selected = !1;
  this._exportableProperties = e;
};
$ = function(e) {
  const t = e.target.value;
  this._contentType = this._contentTypes.find((s) => s.alias === t) ?? null, i(this, a, v).call(this);
};
T = function(e) {
  const t = async (s, l) => {
    const n = await fetch(`/umbraco/backoffice/api/Exports/export/${e}/progress`, {
      headers: { Accept: "application/json", "Content-Type": "application/json" },
      method: "GET"
    }), o = await n.json();
    if (n.status !== 200) {
      i(this, a, u).call(this, String(o)), l(o);
      return;
    }
    o.isComplete === !0 ? s(o) : (this._progress = o.text, setTimeout(() => void t(s, l), 2500));
  };
  return new Promise(t);
};
k = async function() {
  if (this._processing = !0, this._progress = "", this._errorMessage = null, !this._contentType) {
    i(this, a, u).call(this, "Please select a content type");
    return;
  }
  const e = this._metadatas.filter((o) => o.selected).map((o) => o.id), t = this._exportableProperties.filter((o) => o.selected).map((o) => o.alias);
  if (!t.length && !e.length) {
    i(this, a, u).call(this, "At least one property or metadata field must be selected");
    return;
  }
  const s = {
    format: this._format,
    includeUnpublished: this._includeUnpublished,
    metadata: e,
    properties: t
  }, l = await fetch(
    `/umbraco/backoffice/api/Exports/export/${this._contentKey}/${this._contentType.alias}`,
    {
      headers: { Accept: "application/json", "Content-Type": "application/json" },
      method: "POST",
      body: JSON.stringify(s)
    }
  ), n = await l.json();
  if (l.status !== 200) {
    i(this, a, u).call(this, String(n));
    return;
  }
  i(this, a, T).call(this, n.id).then(async (o) => {
    var m, f;
    const h = await fetch(`/umbraco/backoffice/api/Exports/export/${o.id}/file`, {
      headers: { Accept: "application/json", "Content-Type": "application/json" },
      method: "GET"
    });
    if (h.status !== 200) {
      i(this, a, u).call(this, String(await h.json()));
      return;
    }
    const M = await h.blob(), S = ((m = ((h.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : m.replaceAll('"', "")) ?? "export", U = new Blob([M]), b = window.URL.createObjectURL(U), d = document.createElement("a");
    d.href = b, d.setAttribute("download", S), document.body.appendChild(d), d.click(), (f = d.parentNode) == null || f.removeChild(d), window.URL.revokeObjectURL(b), this._processing = !1, this._progress = "";
  }).catch(() => {
  });
};
u = function(e) {
  this._processing = !1, this._progress = "", this._errorMessage = e;
};
P = function() {
  this._metadatas = this._metadatas.map((e) => ({ ...e, selected: !0 }));
};
E = function() {
  this._metadatas = this._metadatas.map((e) => ({ ...e, selected: !1 }));
};
C = function() {
  this._exportableProperties = this._exportableProperties.map((e) => ({ ...e, selected: !0 }));
};
w = function() {
  this._exportableProperties = this._exportableProperties.map((e) => ({ ...e, selected: !1 }));
};
j = function(e, t) {
  this._metadatas = this._metadatas.map((s) => s === e ? { ...s, selected: t } : s);
};
A = function(e, t) {
  this._exportableProperties = this._exportableProperties.map(
    (s) => s === e ? { ...s, selected: t } : s
  );
};
r.styles = D`
        :host {
            display: block;
            padding: var(--uui-size-layout-1);
        }

        uui-box {
            margin-bottom: var(--uui-size-layout-1);
        }

        .listTable {
            overflow: hidden;
        }

        ul.selectionCheckBoxes {
            list-style: none;
            column-count: 4;
            column-gap: 0.5em;
            display: block;
            padding: 0;
            margin: 0;
        }

        .umb-outline {
            cursor: pointer;
            color: var(--uui-color-interactive);
        }

        .text-error {
            color: var(--uui-color-danger);
            display: block;
            margin-top: var(--uui-size-space-3);
        }

        .actions {
            margin-top: var(--uui-size-layout-1);
        }
    `;
c([
  p()
], r.prototype, "_contentKey", 2);
c([
  p()
], r.prototype, "_contentTypes", 2);
c([
  p()
], r.prototype, "_contentType", 2);
c([
  p()
], r.prototype, "_format", 2);
c([
  p()
], r.prototype, "_includeUnpublished", 2);
c([
  p()
], r.prototype, "_metadatas", 2);
c([
  p()
], r.prototype, "_exportableProperties", 2);
c([
  p()
], r.prototype, "_processing", 2);
c([
  p()
], r.prototype, "_progress", 2);
c([
  p()
], r.prototype, "_errorMessage", 2);
r = c([
  R(G)
], r);
const H = r;
export {
  r as N3oDataExportElement,
  H as default
};
//# sourceMappingURL=data-export.js.map
