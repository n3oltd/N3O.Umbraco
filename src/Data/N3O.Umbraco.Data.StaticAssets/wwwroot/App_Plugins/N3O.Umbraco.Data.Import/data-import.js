import { customElement as pe } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as ue } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as me } from "@umbraco-cms/backoffice/document";
import { UmbAuthFetchMixin as he } from "@n3o/backoffice-core";
import { useState as d, useRef as Q, useEffect as be, createElement as ve } from "react";
import { createRoot as fe } from "react-dom/client";
import { jsxs as n, jsx as t, Fragment as X } from "react/jsx-runtime";
function ge({ contentKey: o, authFetch: a }) {
  const [r, m] = d("form"), [s, p] = d(!1), [v, K] = d([]), [u, j] = d(null), [A, ee] = d([]), [f, B] = d(null), [F, te] = d(!1), [S, g] = d([]), [E, R] = d(null), V = Q(null), q = Q(null);
  be(() => {
    if (!o || !a)
      return;
    let e = !0;
    return (async () => {
      const c = await oe(o), w = await (await a("/umbraco/backoffice/api/Imports/lookups/datePatterns", {
        headers: { Accept: "application/json" }
      })).json();
      e && (K(c), ee(w), B(w[0] ?? null));
    })(), () => {
      e = !1;
    };
  }, [o, a]);
  const oe = async (e) => await (await a(`/umbraco/api/ContentTypes/${e}/relations?type=child`, {
    headers: { Accept: "application/json" }
  })).json(), ae = async (e) => {
    if (!e) {
      g([]);
      return;
    }
    const c = await (await a(`/umbraco/backoffice/api/Imports/importableProperties/${e.alias}`, {
      headers: { Accept: "application/json" }
    })).json();
    for (const l of c)
      l.selected = !1;
    g(c);
  }, W = () => {
    p(!1), j(null), R(null), g([]), m("form");
  }, ie = (e) => {
    const i = e.target.value, c = v.find((l) => l.alias === i) ?? null;
    j(c), ae(c);
  }, re = (e) => {
    const i = e.target.value;
    B(A.find((c) => c.id === i) ?? null);
  }, ne = (e, i) => {
    g((c) => c.map((l) => l === e ? { ...l, selected: i } : l));
  }, se = () => g((e) => e.map((i) => ({ ...i, selected: !0 }))), ce = () => g((e) => e.map((i) => ({ ...i, selected: !1 }))), z = (e) => {
    const i = Array.isArray(e) ? e : [e];
    p(!1), R(i), m("error");
  }, le = async () => {
    var Z, J;
    const e = S.filter((D) => D.selected).map((D) => D.alias);
    if (!e.length) {
      z("At least one property must be selected");
      return;
    }
    const i = { properties: e }, c = await a(`/umbraco/backoffice/api/Imports/template/${u.alias}`, {
      method: "POST",
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(i)
    }), l = await c.blob(), N = ((Z = ((c.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : Z.replaceAll('"', "")) ?? "template.csv", T = new Blob([l]), G = window.URL.createObjectURL(T), y = document.createElement("a");
    y.href = G, y.setAttribute("download", N), document.body.appendChild(y), y.click(), (J = y.parentNode) == null || J.removeChild(y), window.URL.revokeObjectURL(G);
  }, de = async () => {
    var N, T;
    p(!0);
    const e = V.current, i = q.current;
    if (!e || !e.value || ((N = e.value.split(".")[1]) == null ? void 0 : N.toLowerCase()) !== "csv") {
      z("A valid CSV file must be specified");
      return;
    }
    if (i && i.value && ((T = i.value.split(".")[1]) == null ? void 0 : T.toLowerCase()) !== "zip") {
      z("The selected file is not a valid ZIP file");
      return;
    }
    const c = await $(e), l = i ? await $(i) : null, w = {
      datePattern: f == null ? void 0 : f.id,
      moveUpdatedContentToCurrentLocation: F,
      csvFile: c,
      zipFile: l
    }, I = await a(
      `/umbraco/backoffice/api/Imports/queue/${o}/${u.alias}`,
      {
        method: "POST",
        headers: {
          Accept: "*/*",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(w)
      }
    );
    I.status === 200 ? (m("success"), p(!1)) : z(await I.json());
  }, $ = async (e) => {
    if (!e.files || e.files.length === 0)
      return null;
    const i = new FormData();
    return i.append("file", e.files[0]), await (await a("/umbraco/api/Storage/tempUpload", {
      method: "POST",
      body: i
    })).json();
  }, H = S.filter((e) => e.selected).length;
  return /* @__PURE__ */ n("div", { className: "n3o-data-import", children: [
    r === "success" ? /* @__PURE__ */ n("uui-box", { headline: "Import queued", children: [
      /* @__PURE__ */ n("div", { className: "statusBox statusBox--positive", children: [
        /* @__PURE__ */ t("uui-icon", { name: "icon-check" }),
        /* @__PURE__ */ t("span", { children: "Your CSV file has been queued and will be processed shortly." })
      ] }),
      /* @__PURE__ */ n("div", { className: "actions", children: [
        /* @__PURE__ */ t("a", { className: "btn btn--primary", href: "/umbraco/section/content/dashboard/imports", children: "View import queue" }),
        /* @__PURE__ */ t("button", { type: "button", className: "btn btn--secondary", onClick: W, children: "Import another file" })
      ] })
    ] }) : r === "error" ? /* @__PURE__ */ n("uui-box", { headline: "Import failed", children: [
      /* @__PURE__ */ n("div", { className: "statusBox statusBox--danger", children: [
        /* @__PURE__ */ t("uui-icon", { name: "icon-alert" }),
        /* @__PURE__ */ t("div", { children: E && E.length > 0 ? /* @__PURE__ */ t("ul", { className: "errorList", children: E.map((e, i) => /* @__PURE__ */ t("li", { children: e }, i)) }) : /* @__PURE__ */ t("span", { children: "Something went wrong while queueing the import." }) })
      ] }),
      /* @__PURE__ */ t("div", { className: "actions", children: /* @__PURE__ */ t("button", { type: "button", className: "btn btn--secondary", onClick: W, children: "Start over" }) })
    ] }) : /* @__PURE__ */ n(X, { children: [
      /* @__PURE__ */ n("uui-box", { headline: "1. Choose what to import", children: [
        /* @__PURE__ */ t(
          "umb-property-layout",
          {
            label: "Content type",
            description: "The child type that rows in your CSV will be imported as.",
            mandatory: !0,
            children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ n(
              "select",
              {
                className: "nativeSelect",
                value: (u == null ? void 0 : u.alias) ?? "",
                onChange: ie,
                disabled: s || v.length === 0,
                children: [
                  /* @__PURE__ */ t("option", { value: "", disabled: !0, children: "Select a content type…" }),
                  v.map((e) => /* @__PURE__ */ t("option", { value: e.alias, children: e.name }, e.alias))
                ]
              }
            ) })
          }
        ),
        /* @__PURE__ */ t(
          "umb-property-layout",
          {
            label: "Date pattern",
            description: "How dates in your CSV are formatted, so they can be parsed correctly.",
            mandatory: !0,
            children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ t(
              "select",
              {
                className: "nativeSelect",
                value: (f == null ? void 0 : f.id) ?? "",
                onChange: re,
                disabled: s || A.length === 0,
                children: A.map((e) => /* @__PURE__ */ t("option", { value: e.id, children: e.name }, e.id))
              }
            ) })
          }
        ),
        /* @__PURE__ */ t(
          "umb-property-layout",
          {
            label: "Move updated content",
            description: "When enabled, existing content that is updated will be moved beneath the current item.",
            children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ n("label", { className: "toggleOption", children: [
              /* @__PURE__ */ t(
                "input",
                {
                  type: "checkbox",
                  checked: F,
                  onChange: (e) => te(e.target.checked),
                  disabled: s
                }
              ),
              /* @__PURE__ */ t("span", { children: "Move updated content to the current location" })
            ] }) })
          }
        )
      ] }),
      /* @__PURE__ */ n("uui-box", { headline: "2. Select properties", children: [
        /* @__PURE__ */ n("div", { slot: "header-actions", className: "selectionCount", children: [
          H,
          " selected"
        ] }),
        u ? S.length === 0 ? /* @__PURE__ */ t("p", { className: "emptyState", children: "This content type has no importable properties." }) : /* @__PURE__ */ n(X, { children: [
          /* @__PURE__ */ n("div", { className: "selectionActions", children: [
            /* @__PURE__ */ t(
              "button",
              {
                type: "button",
                className: "btn btn--secondary btn--compact",
                disabled: s,
                onClick: se,
                children: "Select all"
              }
            ),
            /* @__PURE__ */ t(
              "button",
              {
                type: "button",
                className: "btn btn--secondary btn--compact",
                disabled: s,
                onClick: ce,
                children: "Clear"
              }
            )
          ] }),
          /* @__PURE__ */ t("div", { className: "checkboxGrid", children: S.map((e) => /* @__PURE__ */ n("label", { className: "checkOption", children: [
            /* @__PURE__ */ t(
              "input",
              {
                type: "checkbox",
                checked: !!e.selected,
                onChange: (i) => ne(e, i.target.checked),
                disabled: s
              }
            ),
            /* @__PURE__ */ t("span", { children: e.columnTitle })
          ] }, e.alias)) })
        ] }) : /* @__PURE__ */ t("p", { className: "emptyState", children: "Select a content type above to choose which properties to import." })
      ] }),
      /* @__PURE__ */ n("uui-box", { headline: "3. Download template", children: [
        /* @__PURE__ */ t("p", { className: "boxHint", children: "Download a CSV template containing a column for each selected property, then fill it in with your data." }),
        /* @__PURE__ */ n(
          "button",
          {
            type: "button",
            className: "btn btn--secondary",
            disabled: !u || H === 0 || s,
            onClick: () => void le(),
            children: [
              /* @__PURE__ */ t("uui-icon", { name: "icon-download-alt" }),
              "Download template"
            ]
          }
        )
      ] }),
      /* @__PURE__ */ n("uui-box", { headline: "4. Upload & queue", children: [
        /* @__PURE__ */ t(
          "umb-property-layout",
          {
            label: "CSV file",
            description: "The completed CSV file containing the rows to import.",
            mandatory: !0,
            children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ t("input", { type: "file", id: "csvFile", accept: ".csv", ref: V, disabled: s }) })
          }
        ),
        /* @__PURE__ */ t(
          "umb-property-layout",
          {
            label: "ZIP assets file",
            description: "Optional. A ZIP archive of media/assets referenced by the CSV.",
            children: /* @__PURE__ */ t("div", { slot: "editor", children: /* @__PURE__ */ t("input", { type: "file", id: "zipFile", accept: ".zip", ref: q, disabled: s }) })
          }
        ),
        s ? /* @__PURE__ */ n("div", { className: "progress", children: [
          /* @__PURE__ */ t("uui-loader-bar", {}),
          /* @__PURE__ */ t("span", { children: "Queueing import…" })
        ] }) : null,
        /* @__PURE__ */ t("div", { className: "actions", children: /* @__PURE__ */ t(
          "button",
          {
            type: "button",
            className: "btn btn--primary btn--positive",
            disabled: !u || s,
            onClick: () => void de(),
            children: s ? "Importing…" : "Import"
          }
        ) })
      ] })
    ] }),
    /* @__PURE__ */ t("style", { children: ye })
  ] });
}
const ye = `
    .n3o-data-import {
        display: block;
        padding: var(--uui-size-space-4);
    }
    .n3o-data-import uui-box {
        --uui-box-default-padding: var(--uui-size-space-4);
        margin-bottom: var(--uui-size-space-3);
    }
    .n3o-data-import .nativeSelect {
        width: 100%;
        max-width: 420px;
        box-sizing: border-box;
        height: var(--uui-size-11, 36px);
        padding: 0 var(--uui-size-space-3);
        font: inherit;
        color: var(--uui-color-text);
        background: var(--uui-color-surface);
        border: 1px solid var(--uui-color-border);
        border-radius: var(--uui-border-radius);
    }
    .n3o-data-import .nativeSelect:focus {
        outline: none;
        border-color: var(--uui-color-focus);
        box-shadow: 0 0 0 1px var(--uui-color-focus);
    }
    .n3o-data-import .nativeSelect:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }
    .n3o-data-import input[type='file'] {
        font: inherit;
    }
    .n3o-data-import .toggleOption,
    .n3o-data-import .checkOption {
        display: flex;
        align-items: center;
        gap: var(--uui-size-space-2);
        cursor: pointer;
    }
    .n3o-data-import .toggleOption input,
    .n3o-data-import .checkOption input {
        cursor: pointer;
    }
    .n3o-data-import .selectionCount {
        font-size: var(--uui-type-small-size);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-import .selectionActions {
        display: flex;
        gap: var(--uui-size-space-2);
        margin-bottom: var(--uui-size-space-3);
    }
    .n3o-data-import .checkboxGrid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
        gap: var(--uui-size-space-1) var(--uui-size-space-5);
    }
    .n3o-data-import .emptyState {
        margin: 0;
        color: var(--uui-color-text-alt);
        font-style: italic;
    }
    .n3o-data-import .boxHint {
        margin: 0 0 var(--uui-size-space-4);
        color: var(--uui-color-text-alt);
    }
    .n3o-data-import .progress {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-2);
        margin: var(--uui-size-space-4) 0;
        color: var(--uui-color-text-alt);
    }
    .n3o-data-import .statusBox {
        display: flex;
        align-items: flex-start;
        gap: var(--uui-size-space-3);
        padding: var(--uui-size-space-4) var(--uui-size-space-5);
        border-radius: var(--uui-border-radius);
        margin-bottom: var(--uui-size-space-4);
    }
    .n3o-data-import .statusBox--positive {
        background: var(--uui-color-positive);
        color: var(--uui-color-positive-contrast);
    }
    .n3o-data-import .statusBox--danger {
        background: var(--uui-color-danger);
        color: var(--uui-color-danger-contrast);
    }
    .n3o-data-import .errorList {
        margin: 0;
        padding-left: var(--uui-size-space-4);
    }
    .n3o-data-import .actions {
        display: flex;
        gap: var(--uui-size-space-3);
        align-items: center;
        margin-top: var(--uui-size-space-4);
    }
    .n3o-data-import .btn {
        font: inherit;
        font-weight: 700;
        line-height: 1;
        display: inline-flex;
        align-items: center;
        gap: var(--uui-size-space-2);
        padding: 0 var(--uui-size-space-4);
        height: var(--uui-size-11, 36px);
        border: 1px solid transparent;
        border-radius: var(--uui-border-radius);
        cursor: pointer;
        box-sizing: border-box;
        text-decoration: none;
    }
    .n3o-data-import .btn--compact {
        height: var(--uui-size-9, 30px);
        padding: 0 var(--uui-size-space-3);
        font-size: var(--uui-type-small-size);
    }
    .n3o-data-import .btn--secondary {
        background: var(--uui-color-surface);
        color: var(--uui-color-text);
        border-color: var(--uui-color-border);
    }
    .n3o-data-import .btn--secondary:hover:not(:disabled) {
        background: var(--uui-color-surface-emphasis);
        border-color: var(--uui-color-border-emphasis);
    }
    .n3o-data-import .btn--primary {
        background: var(--uui-color-default);
        color: var(--uui-color-default-contrast);
    }
    .n3o-data-import .btn--primary.btn--positive {
        background: var(--uui-color-positive);
        color: var(--uui-color-positive-contrast);
    }
    .n3o-data-import .btn--primary:hover:not(:disabled) {
        background: var(--uui-color-positive-emphasis, var(--uui-color-positive));
    }
    .n3o-data-import .btn:disabled {
        opacity: 0.5;
        cursor: not-allowed;
    }
`;
var we = Object.getOwnPropertyDescriptor, Y = (o) => {
  throw TypeError(o);
}, xe = (o, a, r, m) => {
  for (var s = m > 1 ? void 0 : m ? we(a, r) : a, p = o.length - 1, v; p >= 0; p--)
    (v = o[p]) && (s = v(s) || s);
  return s;
}, L = (o, a, r) => a.has(o) || Y("Cannot " + r), h = (o, a, r) => (L(o, a, "read from private field"), r ? r.call(o) : a.get(o)), _ = (o, a, r) => a.has(o) ? Y("Cannot add the same private member more than once") : a instanceof WeakSet ? a.add(o) : a.set(o, r), P = (o, a, r, m) => (L(o, a, "write to private field"), a.set(o, r), r), M = (o, a, r) => (L(o, a, "access private method"), r), b, x, k, C, O;
const Ce = "n3o-data-import";
let U = class extends he(ue(HTMLElement)) {
  constructor() {
    super(), _(this, C), _(this, b), _(this, x), _(this, k, null);
    const o = this.attachShadow({ mode: "open" });
    P(this, x, document.createElement("div")), o.appendChild(h(this, x)), this.consumeContext(me, (a) => {
      a && this.observe(
        a.unique,
        (r) => {
          r && r !== h(this, k) && (P(this, k, r), M(this, C, O).call(this));
        },
        "_observeUnique"
      );
    });
  }
  // Re-render when the shared authenticated fetch becomes available / changes (mixin hook).
  authFetchChanged(o) {
    M(this, C, O).call(this);
  }
  connectedCallback() {
    var o;
    (o = super.connectedCallback) == null || o.call(this), h(this, b) ?? P(this, b, fe(h(this, x))), M(this, C, O).call(this);
  }
  disconnectedCallback() {
    var o, a;
    (o = super.disconnectedCallback) == null || o.call(this), (a = h(this, b)) == null || a.unmount(), P(this, b, void 0);
  }
};
b = /* @__PURE__ */ new WeakMap();
x = /* @__PURE__ */ new WeakMap();
k = /* @__PURE__ */ new WeakMap();
C = /* @__PURE__ */ new WeakSet();
O = function() {
  var o;
  (o = h(this, b)) == null || o.render(
    ve(ge, {
      contentKey: h(this, k),
      authFetch: this.authFetch
    })
  );
};
U = xe([
  pe(Ce)
], U);
const Ie = U;
export {
  U as N3oDataImportElement,
  Ie as default
};
//# sourceMappingURL=data-import.js.map
