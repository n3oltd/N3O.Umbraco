import { customElement as le } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as ce } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as de } from "@umbraco-cms/backoffice/document";
import { useState as p, useRef as G, useEffect as pe, createElement as ue } from "react";
import { createRoot as me } from "react-dom/client";
import { jsxs as n, jsx as a, Fragment as he } from "react/jsx-runtime";
function ve({ contentKey: t }) {
  const [r, s] = p("form"), [c, d] = p(!1), [h, k] = p([]), [f, F] = p(null), [D, X] = p([]), [A, U] = p(null), [M, K] = p(!1), [j, v] = p([]), [L, R] = p(null), q = G(null), B = G(null);
  pe(() => {
    if (!t)
      return;
    let e = !0;
    return (async () => {
      const i = await Y(t), g = await (await fetch("/umbraco/backoffice/api/Imports/lookups/datePatterns", {
        headers: { Accept: "application/json" }
      })).json();
      e && (k(i), X(g), U(g[0] ?? null));
    })(), () => {
      e = !1;
    };
  }, [t]);
  const Y = async (e) => await (await fetch(`/umbraco/api/ContentTypes/${e}/relations?type=child`, {
    headers: { Accept: "application/json" }
  })).json(), ee = async (e) => {
    if (!e) {
      v([]);
      return;
    }
    const i = await (await fetch(`/umbraco/backoffice/api/Imports/importableProperties/${e.alias}`, {
      headers: { Accept: "application/json" }
    })).json();
    for (const l of i)
      l.selected = !1;
    v(i);
  }, W = () => {
    d(!1), F(null), R(null), v([]), s("form");
  }, te = (e) => {
    const o = e.target.value, i = h.find((l) => l.alias === o) ?? null;
    F(i), ee(i);
  }, ae = (e) => {
    const o = e.target.value;
    U(D.find((i) => i.id === o) ?? null);
  }, oe = (e, o) => {
    v((i) => i.map((l) => l === e ? { ...l, selected: o } : l));
  }, re = () => v((e) => e.map((o) => ({ ...o, selected: !0 }))), ne = () => v((e) => e.map((o) => ({ ...o, selected: !1 }))), w = (e) => {
    const o = Array.isArray(e) ? e : [e];
    d(!1), R(o), s("error");
  }, se = async () => {
    var J, Z;
    const e = j.filter((z) => z.selected).map((z) => z.alias);
    if (!e.length) {
      w("At least one property must be selected");
      return;
    }
    const o = { properties: e }, i = await fetch(`/umbraco/backoffice/api/Imports/template/${f.alias}`, {
      method: "POST",
      headers: {
        Accept: "*/*",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(o)
    }), l = await i.blob(), y = ((J = ((i.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : J.replaceAll('"', "")) ?? "template.csv", N = new Blob([l]), V = window.URL.createObjectURL(N), b = document.createElement("a");
    b.href = V, b.setAttribute("download", y), document.body.appendChild(b), b.click(), (Z = b.parentNode) == null || Z.removeChild(b), window.URL.revokeObjectURL(V);
  }, ie = async () => {
    var y, N;
    d(!0);
    const e = q.current, o = B.current;
    if (!e || !e.value || ((y = e.value.split(".")[1]) == null ? void 0 : y.toLowerCase()) !== "csv") {
      w("A valid CSV file must be specified");
      return;
    }
    if (o && o.value && ((N = o.value.split(".")[1]) == null ? void 0 : N.toLowerCase()) !== "zip") {
      w("The selected file is not a valid ZIP file");
      return;
    }
    const i = await $(e), l = o ? await $(o) : null, g = {
      datePattern: A == null ? void 0 : A.id,
      moveUpdatedContentToCurrentLocation: M,
      csvFile: i,
      zipFile: l
    }, E = await fetch(
      `/umbraco/backoffice/api/Imports/queue/${t}/${f.alias}`,
      {
        method: "POST",
        headers: {
          Accept: "*/*",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(g)
      }
    );
    E.status === 200 ? (s("success"), d(!1)) : w(await E.json());
  }, $ = async (e) => {
    if (!e.files || e.files.length === 0)
      return null;
    const o = new FormData();
    return o.append("file", e.files[0]), await (await fetch("/umbraco/api/Storage/tempUpload", {
      method: "POST",
      body: o
    })).json();
  };
  return /* @__PURE__ */ n("div", { className: "n3o-data-import", children: [
    r === "success" ? /* @__PURE__ */ n("div", { className: "umb-group-panel", children: [
      /* @__PURE__ */ a("div", { className: "umb-group-panel__header", children: "Processing" }),
      /* @__PURE__ */ n("div", { className: "umb-group-panel__content", children: [
        /* @__PURE__ */ a("p", { children: "CSV file is processing and will appear shortly." }),
        /* @__PURE__ */ n("p", { className: "actions", children: [
          /* @__PURE__ */ a("uui-button", { look: "primary", href: "/umbraco#/content?dashboard=imports", children: "View Import Queue" }),
          /* @__PURE__ */ a("uui-button", { look: "secondary", label: "Import Another File", onClick: W, children: "Import Another File" })
        ] })
      ] })
    ] }) : r === "error" ? /* @__PURE__ */ n("div", { className: "umb-group-panel", children: [
      /* @__PURE__ */ a("div", { className: "umb-group-panel__header", children: "Error" }),
      /* @__PURE__ */ n("div", { className: "umb-group-panel__content", children: [
        L ? /* @__PURE__ */ a("ul", { children: L.map((e, o) => /* @__PURE__ */ a("li", { className: "text-error", children: e }, o)) }) : null,
        /* @__PURE__ */ a("p", { children: /* @__PURE__ */ a("uui-button", { look: "secondary", label: "Start Over", onClick: W, children: "Start Over" }) })
      ] })
    ] }) : /* @__PURE__ */ n(he, { children: [
      /* @__PURE__ */ n("div", { className: "umb-group-panel", children: [
        /* @__PURE__ */ a("div", { className: "umb-group-panel__header", children: "Options" }),
        /* @__PURE__ */ n("div", { className: "umb-group-panel__content", children: [
          /* @__PURE__ */ n("div", { className: "control-group", children: [
            /* @__PURE__ */ n("label", { children: [
              "Content Type ",
              /* @__PURE__ */ a("strong", { className: "required", children: "*" })
            ] }),
            /* @__PURE__ */ n("select", { onChange: te, disabled: c, children: [
              /* @__PURE__ */ a("option", { value: "", selected: !f }),
              h.map((e) => /* @__PURE__ */ a("option", { value: e.alias, children: e.name }, e.alias))
            ] })
          ] }),
          /* @__PURE__ */ n("div", { className: "control-group", children: [
            /* @__PURE__ */ n("label", { children: [
              "Date Pattern ",
              /* @__PURE__ */ a("strong", { className: "required", children: "*" })
            ] }),
            /* @__PURE__ */ a("select", { onChange: ae, disabled: c, children: D.map((e) => /* @__PURE__ */ a("option", { value: e.id, children: e.name }, e.id)) })
          ] }),
          /* @__PURE__ */ n("div", { className: "control-group", children: [
            /* @__PURE__ */ a("label", { children: "Move Updated Content to Current Location" }),
            /* @__PURE__ */ a(
              "input",
              {
                type: "checkbox",
                checked: M,
                onChange: (e) => K(e.target.checked),
                disabled: c
              }
            )
          ] }),
          /* @__PURE__ */ n("div", { className: "control-group", children: [
            /* @__PURE__ */ n("label", { children: [
              "CSV File ",
              /* @__PURE__ */ a("strong", { className: "required", children: "*" })
            ] }),
            /* @__PURE__ */ a("input", { type: "file", id: "csvFile", ref: q, disabled: c })
          ] }),
          /* @__PURE__ */ n("div", { className: "control-group", children: [
            /* @__PURE__ */ a("label", { children: "ZIP Assets File (optional)" }),
            /* @__PURE__ */ a("input", { type: "file", id: "zipFile", ref: B, disabled: c })
          ] })
        ] })
      ] }),
      f ? /* @__PURE__ */ n("div", { className: "umb-group-panel", children: [
        /* @__PURE__ */ a("div", { className: "umb-group-panel__header", children: "Properties" }),
        /* @__PURE__ */ a("div", { className: "umb-group-panel__content", children: /* @__PURE__ */ n("div", { className: "listTable", children: [
          /* @__PURE__ */ a("a", { className: "link", onClick: re, children: "Select All" }),
          " ",
          "|",
          " ",
          /* @__PURE__ */ a("a", { className: "link", onClick: ne, children: "Clear Selection" }),
          /* @__PURE__ */ a("ul", { className: "selectionCheckBoxes", children: j.map((e) => /* @__PURE__ */ a("li", { children: /* @__PURE__ */ n("label", { children: [
            /* @__PURE__ */ a(
              "input",
              {
                type: "checkbox",
                value: e.alias,
                checked: !!e.selected,
                onChange: (o) => oe(e, o.target.checked)
              }
            ),
            " ",
            e.columnTitle
          ] }) }, e.alias)) })
        ] }) })
      ] }) : null,
      /* @__PURE__ */ n("div", { className: "actions", children: [
        f ? /* @__PURE__ */ a("uui-button", { look: "secondary", label: "Download Template", onClick: () => void se(), children: "Download Template" }) : null,
        /* @__PURE__ */ a("uui-button", { look: "primary", label: "Import", disabled: c, onClick: () => void ie(), children: c ? "Please wait..." : "Import" })
      ] })
    ] }),
    /* @__PURE__ */ a("style", { children: be })
  ] });
}
const be = `
    .n3o-data-import {
        display: block;
        padding: var(--uui-size-layout-1);
    }
    .n3o-data-import .umb-group-panel {
        background: var(--uui-color-surface);
        border: 1px solid var(--uui-color-border);
        border-radius: var(--uui-border-radius);
        margin-bottom: var(--uui-size-space-5);
    }
    .n3o-data-import .umb-group-panel__header {
        padding: var(--uui-size-space-4) var(--uui-size-space-5);
        border-bottom: 1px solid var(--uui-color-border);
        font-weight: bold;
    }
    .n3o-data-import .umb-group-panel__content {
        padding: var(--uui-size-space-5);
    }
    .n3o-data-import .control-group {
        margin-bottom: var(--uui-size-space-4);
    }
    .n3o-data-import .control-group label {
        display: block;
        margin-bottom: var(--uui-size-space-2);
        font-weight: bold;
    }
    .n3o-data-import .required {
        color: var(--uui-color-danger);
    }
    .n3o-data-import select {
        min-width: 250px;
        padding: var(--uui-size-space-2);
    }
    .n3o-data-import .listTable .link {
        cursor: pointer;
        color: var(--uui-color-interactive);
    }
    .n3o-data-import .selectionCheckBoxes {
        list-style: none;
        padding: 0;
        margin-top: var(--uui-size-space-4);
    }
    .n3o-data-import .selectionCheckBoxes li {
        margin-bottom: var(--uui-size-space-2);
    }
    .n3o-data-import .actions {
        display: flex;
        gap: var(--uui-size-space-3);
        align-items: center;
    }
    .n3o-data-import .text-error {
        color: var(--uui-color-danger);
    }
`;
var fe = Object.getOwnPropertyDescriptor, Q = (t) => {
  throw TypeError(t);
}, ge = (t, r, s, c) => {
  for (var d = c > 1 ? void 0 : c ? fe(r, s) : r, h = t.length - 1, k; h >= 0; h--)
    (k = t[h]) && (d = k(d) || d);
  return d;
}, x = (t, r, s) => r.has(t) || Q("Cannot " + s), u = (t, r, s) => (x(t, r, "read from private field"), s ? s.call(t) : r.get(t)), T = (t, r, s) => r.has(t) ? Q("Cannot add the same private member more than once") : r instanceof WeakSet ? r.add(t) : r.set(t, s), P = (t, r, s, c) => (x(t, r, "write to private field"), r.set(t, s), s), H = (t, r, s) => (x(t, r, "access private method"), s), m, C, _, S, I;
const Ce = "n3o-data-import";
let O = class extends ce(HTMLElement) {
  constructor() {
    super(), T(this, S), T(this, m), T(this, C), T(this, _, null);
    const t = this.attachShadow({ mode: "open" });
    P(this, C, document.createElement("div")), t.appendChild(u(this, C)), this.consumeContext(de, (r) => {
      r && this.observe(
        r.unique,
        (s) => {
          s && s !== u(this, _) && (P(this, _, s), H(this, S, I).call(this));
        },
        "_observeUnique"
      );
    });
  }
  connectedCallback() {
    var t;
    (t = super.connectedCallback) == null || t.call(this), u(this, m) ?? P(this, m, me(u(this, C))), H(this, S, I).call(this);
  }
  disconnectedCallback() {
    var t, r;
    (t = super.disconnectedCallback) == null || t.call(this), (r = u(this, m)) == null || r.unmount(), P(this, m, void 0);
  }
};
m = /* @__PURE__ */ new WeakMap();
C = /* @__PURE__ */ new WeakMap();
_ = /* @__PURE__ */ new WeakMap();
S = /* @__PURE__ */ new WeakSet();
I = function() {
  var t;
  (t = u(this, m)) == null || t.render(
    ue(ve, {
      contentKey: u(this, _)
    })
  );
};
O = ge([
  le(Ce)
], O);
const Ee = O;
export {
  O as N3oDataImportElement,
  Ee as default
};
//# sourceMappingURL=data-import.js.map
