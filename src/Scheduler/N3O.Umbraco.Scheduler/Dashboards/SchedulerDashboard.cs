using Umbraco.Cms.Core.Dashboards;

namespace N3O.Umbraco.Scheduler.Dashboards {
    public class SchedulerDashboard : IDashboard {
        public string Alias => "schedulerDashboard";

        public string[] Sections => new[] {
            global::Umbraco.Cms.Core.Constants.Applications.Settings
        };

        public string View => "/App_Plugins/N3O.Umbraco.Scheduler/N3O.Umbraco.Scheduler.html";

        public IAccessRule[] AccessRules => new[] {
            new AccessRule {
                Type = AccessRuleType.Grant, Value = "admin"
            }
        };
    }
}