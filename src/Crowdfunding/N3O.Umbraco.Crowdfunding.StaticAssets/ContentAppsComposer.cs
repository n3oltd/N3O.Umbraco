using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class ContentAppsComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.ContentApps().Append<StatisticsApp>();
        builder.ContentApps().Append<RemotePagesApp>();
    }
}
