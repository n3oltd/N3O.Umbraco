const e = "/_framework/blazor.server.js";
function r() {
  return Array.from(document.querySelectorAll("script")).some(
    (t) => t.getAttribute("src") === e
  );
}
if (!r()) {
  const t = document.createElement("script");
  t.src = e, t.onload = n, t.setAttribute("autostart", "false"), document.body.appendChild(t);
}
async function n() {
  Blazor.start({
    configureSignalR: function(t) {
      t.withUrl("/_blazor"), t.withAutomaticReconnect([0, 2e3, 1e4, 15e3, 2e4, 3e4, 6e4]);
      const o = t.build();
      o.serverTimeoutInMilliseconds = 3e4;
    }
  });
}
//# sourceMappingURL=N3O.Umbraco.Blazor.BackOffice.js.map
