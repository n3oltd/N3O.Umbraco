using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Crowdfunding;

public class StatisticsAppComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.ContentApps().Append<StatisticsApp>();
    }
}
