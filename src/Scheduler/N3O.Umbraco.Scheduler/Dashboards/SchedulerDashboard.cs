// TODO Migration Review: IDashboard was removed in Umbraco 14. Dashboards are now registered via umbraco-package.json
// using the Bellissima extension system (a "dashboard" manifest entry with a web component).
// TODO: Create App_Plugins/N3O.Umbraco.Scheduler/umbraco-package.json registering the dashboard
//       as a Lit web component, replacing the legacy Angular view at N3O.Umbraco.Scheduler.html.
namespace N3O.Umbraco.Scheduler.Dashboards;
