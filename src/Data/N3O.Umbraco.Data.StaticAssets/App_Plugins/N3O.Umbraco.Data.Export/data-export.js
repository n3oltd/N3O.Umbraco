import { customElement as re } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin as ce } from "@umbraco-cms/backoffice/element-api";
import { UMB_DOCUMENT_WORKSPACE_CONTEXT as le } from "@umbraco-cms/backoffice/document";
import { useState as p, useEffect as ie, createElement as de } from "react";
import { createRoot as pe } from "react-dom/client";
import { jsxs as d, jsx as a } from "react/jsx-runtime";
function ue({ contentKey: t }) {
  const [s, n] = p([]), [u, f] = p(null), [h, y] = p("excel"), [U, I] = p(!1), [D, w] = p([]), [R, g] = p([]), [x, A] = p(!1), [q, _] = p(""), [B, W] = p(null);
  ie(() => {
    if (!t)
      return;
    let e = !0;
    return (async () => {
      const c = await H(t), r = await fetch("/umbraco/backoffice/api/Exports/lookups/contentMetadata", {
        headers: { Accept: "application/json" }
      }).then((i) => i.json());
      for (const i of r)
        i.selected = i.autoSelected;
      r.sort((i, l) => i.displayOrder - l.displayOrder), e && (n(c), w(r));
    })(), () => {
      e = !1;
    };
  }, [t]);
  const H = async (e) => await (await fetch(`/umbraco/api/ContentTypes/${e}/relations?type=descendant`, {
    headers: { Accept: "application/json" }
  })).json(), J = async (e) => {
    if (!e) {
      g([]);
      return;
    }
    const o = await fetch(`/umbraco/backoffice/api/Exports/exportableProperties/${e.alias}`, {
      headers: { Accept: "application/json" }
    }).then((c) => c.json());
    for (const c of o)
      c.selected = !1;
    g(o);
  }, V = (e) => {
    const o = e.target.value, c = s.find((r) => r.alias === o) ?? null;
    f(c), J(c);
  }, C = (e) => {
    A(!1), _(""), W(e);
  }, X = (e) => {
    const o = async (c, r) => {
      const i = await fetch(`/umbraco/backoffice/api/Exports/export/${e}/progress`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      }), l = await i.json();
      if (i.status !== 200) {
        C(String(l)), r(l);
        return;
      }
      l.isComplete === !0 ? c(l) : (_(l.text), setTimeout(() => void o(c, r), 2500));
    };
    return new Promise(o);
  }, K = async () => {
    if (A(!0), _(""), W(null), !u) {
      C("Please select a content type");
      return;
    }
    const e = D.filter((l) => l.selected).map((l) => l.id), o = R.filter((l) => l.selected).map((l) => l.alias);
    if (!o.length && !e.length) {
      C("At least one property or metadata field must be selected");
      return;
    }
    const c = {
      format: h,
      includeUnpublished: U,
      metadata: e,
      properties: o
    }, r = await fetch(
      `/umbraco/backoffice/api/Exports/export/${t}/${u.alias}`,
      {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "POST",
        body: JSON.stringify(c)
      }
    ), i = await r.json();
    if (r.status !== 200) {
      C(String(i));
      return;
    }
    X(i.id).then(async (l) => {
      var L, z;
      const T = await fetch(`/umbraco/backoffice/api/Exports/export/${l.id}/file`, {
        headers: { Accept: "application/json", "Content-Type": "application/json" },
        method: "GET"
      });
      if (T.status !== 200) {
        C(String(await T.json()));
        return;
      }
      const oe = await T.blob(), se = ((L = ((T.headers.get("Content-Disposition") ?? "").split(";")[1] ?? "").split("=")[1]) == null ? void 0 : L.replaceAll('"', "")) ?? "export", ne = new Blob([oe]), $ = window.URL.createObjectURL(ne), v = document.createElement("a");
      v.href = $, v.setAttribute("download", se), document.body.appendChild(v), v.click(), (z = v.parentNode) == null || z.removeChild(v), window.URL.revokeObjectURL($), A(!1), _("");
    }).catch(() => {
    });
  }, Q = () => w((e) => e.map((o) => ({ ...o, selected: !0 }))), Y = () => w((e) => e.map((o) => ({ ...o, selected: !1 }))), Z = () => g((e) => e.map((o) => ({ ...o, selected: !0 }))), ee = () => g((e) => e.map((o) => ({ ...o, selected: !1 }))), te = (e, o) => w((c) => c.map((r) => r === e ? { ...r, selected: o } : r)), ae = (e, o) => g((c) => c.map((r) => r === e ? { ...r, selected: o } : r));
  return /* @__PURE__ */ d("div", { className: "n3o-data-export", children: [
    /* @__PURE__ */ d("uui-box", { headline: "Options", children: [
      /* @__PURE__ */ a("umb-property-layout", { label: "Content Type", mandatory: !0, children: /* @__PURE__ */ a("div", { slot: "editor", children: /* @__PURE__ */ d("select", { disabled: x, onChange: V, children: [
        /* @__PURE__ */ a("option", { value: "", selected: !u }),
        s.map((e) => /* @__PURE__ */ a("option", { value: e.alias, children: e.name }, e.alias))
      ] }) }) }),
      /* @__PURE__ */ a("umb-property-layout", { label: "Format", mandatory: !0, children: /* @__PURE__ */ d("div", { slot: "editor", children: [
        /* @__PURE__ */ d("label", { children: [
          /* @__PURE__ */ a(
            "input",
            {
              type: "radio",
              name: "format",
              value: "excel",
              checked: h === "excel",
              disabled: x,
              onChange: () => y("excel")
            }
          ),
          "Excel"
        ] }),
        /* @__PURE__ */ a("br", {}),
        /* @__PURE__ */ d("label", { children: [
          /* @__PURE__ */ a(
            "input",
            {
              type: "radio",
              name: "format",
              value: "csv",
              checked: h === "csv",
              disabled: x,
              onChange: () => y("csv")
            }
          ),
          "CSV"
        ] })
      ] }) }),
      /* @__PURE__ */ a("umb-property-layout", { label: "Include Unpublished", mandatory: !0, children: /* @__PURE__ */ a("div", { slot: "editor", children: /* @__PURE__ */ a(
        "input",
        {
          type: "checkbox",
          checked: U,
          disabled: x,
          onChange: (e) => I(e.target.checked)
        }
      ) }) })
    ] }),
    /* @__PURE__ */ a("uui-box", { headline: "Metadata", children: /* @__PURE__ */ d("div", { className: "listTable", children: [
      /* @__PURE__ */ a("a", { className: "umb-outline", onClick: Q, children: "Select All" }),
      " ",
      "|",
      " ",
      /* @__PURE__ */ a("a", { className: "umb-outline", onClick: Y, children: "Clear Selection" }),
      /* @__PURE__ */ a("br", {}),
      /* @__PURE__ */ a("br", {}),
      /* @__PURE__ */ a("ul", { className: "selectionCheckBoxes", children: D.map((e) => /* @__PURE__ */ a("li", { children: /* @__PURE__ */ d("label", { children: [
        /* @__PURE__ */ a(
          "input",
          {
            type: "checkbox",
            checked: !!e.selected,
            onChange: (o) => te(e, o.target.checked)
          }
        ),
        " ",
        e.name
      ] }) }, e.id)) })
    ] }) }),
    /* @__PURE__ */ d("uui-box", { headline: "Properties", children: [
      /* @__PURE__ */ d("div", { className: "listTable", children: [
        /* @__PURE__ */ a("a", { className: "umb-outline", onClick: Z, children: "Select All" }),
        " ",
        "|",
        " ",
        /* @__PURE__ */ a("a", { className: "umb-outline", onClick: ee, children: "Clear Selection" }),
        /* @__PURE__ */ a("br", {}),
        /* @__PURE__ */ a("br", {}),
        /* @__PURE__ */ a("ul", { className: "selectionCheckBoxes", children: R.map((e) => /* @__PURE__ */ a("li", { children: /* @__PURE__ */ d("label", { children: [
          /* @__PURE__ */ a(
            "input",
            {
              type: "checkbox",
              checked: !!e.selected,
              onChange: (o) => ae(e, o.target.checked)
            }
          ),
          " ",
          e.columnTitle
        ] }) }, e.alias)) })
      ] }),
      B ? /* @__PURE__ */ a("em", { className: "text-error", children: B }) : null
    ] }),
    /* @__PURE__ */ a("div", { className: "actions", children: /* @__PURE__ */ a(
      "uui-button",
      {
        look: "primary",
        disabled: x,
        onClick: () => void K(),
        label: x && q || "Export"
      }
    ) }),
    /* @__PURE__ */ a("style", { children: he })
  ] });
}
const he = `
    .n3o-data-export {
        display: block;
        padding: var(--uui-size-layout-1);
    }
    .n3o-data-export uui-box {
        margin-bottom: var(--uui-size-layout-1);
    }
    .n3o-data-export .listTable {
        overflow: hidden;
    }
    .n3o-data-export ul.selectionCheckBoxes {
        list-style: none;
        column-count: 4;
        column-gap: 0.5em;
        display: block;
        padding: 0;
        margin: 0;
    }
    .n3o-data-export .umb-outline {
        cursor: pointer;
        color: var(--uui-color-interactive);
    }
    .n3o-data-export .text-error {
        color: var(--uui-color-danger);
        display: block;
        margin-top: var(--uui-size-space-3);
    }
    .n3o-data-export .actions {
        margin-top: var(--uui-size-layout-1);
    }
`;
var me = Object.getOwnPropertyDescriptor, G = (t) => {
  throw TypeError(t);
}, be = (t, s, n, u) => {
  for (var f = u > 1 ? void 0 : u ? me(s, n) : s, h = t.length - 1, y; h >= 0; h--)
    (y = t[h]) && (f = y(f) || f);
  return f;
}, O = (t, s, n) => s.has(t) || G("Cannot " + n), m = (t, s, n) => (O(t, s, "read from private field"), n ? n.call(t) : s.get(t)), N = (t, s, n) => s.has(t) ? G("Cannot add the same private member more than once") : s instanceof WeakSet ? s.add(t) : s.set(t, n), P = (t, s, n, u) => (O(t, s, "write to private field"), s.set(t, n), n), F = (t, s, n) => (O(t, s, "access private method"), n), b, k, E, S, M;
const fe = "n3o-data-export";
let j = class extends ce(HTMLElement) {
  constructor() {
    super(), N(this, S), N(this, b), N(this, k), N(this, E, null);
    const t = this.attachShadow({ mode: "open" });
    P(this, k, document.createElement("div")), t.appendChild(m(this, k)), this.consumeContext(le, (s) => {
      s && this.observe(
        s.unique,
        (n) => {
          n && n !== m(this, E) && (P(this, E, n), F(this, S, M).call(this));
        },
        "_observeUnique"
      );
    });
  }
  connectedCallback() {
    var t;
    (t = super.connectedCallback) == null || t.call(this), m(this, b) ?? P(this, b, pe(m(this, k))), F(this, S, M).call(this);
  }
  disconnectedCallback() {
    var t, s;
    (t = super.disconnectedCallback) == null || t.call(this), (s = m(this, b)) == null || s.unmount(), P(this, b, void 0);
  }
};
b = /* @__PURE__ */ new WeakMap();
k = /* @__PURE__ */ new WeakMap();
E = /* @__PURE__ */ new WeakMap();
S = /* @__PURE__ */ new WeakSet();
M = function() {
  var t;
  (t = m(this, b)) == null || t.render(
    de(ue, {
      contentKey: m(this, E)
    })
  );
};
j = be([
  re(fe)
], j);
const _e = j;
export {
  j as N3oDataExportElement,
  _e as default
};
//# sourceMappingURL=data-export.js.map
