using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.Dashboards;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.WelcomeDashboard;

public class WelcomeDashboardComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Dashboards().Remove<ContentDashboard>();
        builder.Dashboards().Remove<RedirectUrlDashboard>();
    }
}
