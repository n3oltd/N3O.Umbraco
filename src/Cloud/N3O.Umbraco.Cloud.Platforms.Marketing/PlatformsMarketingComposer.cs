using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Engage.Infrastructure.Personalization.Segments.Rules;
using Umbraco.Engage.Web.Cockpit.Segments;

namespace N3O.Umbraco.Cloud.Platforms.Marketing;

public class PlatformsMarketingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ICockpitSegmentRuleFactory, TelethonOnAirCockpitSegmentRuleFactory>();
        builder.Services.AddTransient<ISegmentRuleFactory, TelethonOnAirSegmentRuleFactory>();
        
    }
}
