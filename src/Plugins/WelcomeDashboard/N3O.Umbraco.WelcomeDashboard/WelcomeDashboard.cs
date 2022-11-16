using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dashboards;

namespace N3O.Umbraco.WelcomeDashboard;

[Weight(-10)]
public class WelcomeDashboard : IDashboard {
    public string Alias => "welcomeDashboard";

    public string[] Sections => new[] {
        global::Umbraco.Cms.Core.Constants.Applications.Content
    };

    public string View => "/App_Plugins/N3O.Umbraco.WelcomeDashboard/N3O.Umbraco.WelcomeDashboard.html";

    public IAccessRule[] AccessRules => new[] {
        new AccessRule {
            Type = AccessRuleType.GrantBySection,
            Value = "content"
        }
    };
}
