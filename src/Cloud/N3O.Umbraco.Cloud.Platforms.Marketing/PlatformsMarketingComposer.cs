// TODO (BLOCKER-04): Umbraco.Engage namespaces changed in v17.
// After completing the Engage v17 namespace port, restore:
//   builder.Services.AddTransient<ICockpitSegmentRuleFactory, TelethonOnAirCockpitSegmentRuleFactory>();
//   builder.Services.AddTransient<ISegmentRuleFactory, TelethonOnAirSegmentRuleFactory>();
// using the updated namespaces from Umbraco.Engage 17.2.2
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Cloud.Platforms.Marketing;

public class PlatformsMarketingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) { }
}
