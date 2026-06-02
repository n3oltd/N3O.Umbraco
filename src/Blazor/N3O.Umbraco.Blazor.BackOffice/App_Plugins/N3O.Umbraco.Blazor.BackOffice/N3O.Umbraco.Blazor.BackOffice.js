const e = "/_framework/blazor.server.js";
function r() {
  return $("script").filter(function() {
    return $(this).attr("src") === e;
  }).length !== 1;
}
if (r()) {
  const t = document.createElement("script");
  t.src = e, t.onload = o, t.setAttribute("autostart", "false"), document.body.appendChild(t);
}
async function o() {
  Blazor.start({
    configureSignalR: function(t) {
      t.withUrl("/_blazor"), t.withAutomaticReconnect([0, 2e3, 1e4, 15e3, 2e4, 3e4, 6e4]);
      const n = t.build();
      n.serverTimeoutInMilliseconds = 3e4;
    }
  });
}
//# sourceMappingURL=N3O.Umbraco.Blazor.BackOffice.js.map
