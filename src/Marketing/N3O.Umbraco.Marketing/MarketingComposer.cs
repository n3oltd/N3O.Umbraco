using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Engage.Infrastructure.Analytics.Collection.Extractors;
using Umbraco.Engage.Infrastructure.Analytics.Processing.Extractors;
using Umbraco.Extensions;

namespace N3O.Umbraco.Marketing;

// Composers run in an unspecified order unless an attribute constrains them, and each of these two
// registers a default we replace, so without both attributes Engage can run last and win.
[ComposeAfter(typeof(AnalyticsExtractorsComposer))]
[ComposeAfter(typeof(AnalyticsProcessingExtractorsComposer))]
public class MarketingComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddUnique<IHttpContextIpAddressExtractor, EngageIpAddressExtractor>();
        builder.Services.AddUnique<IRawPageviewLocationExtractor, EngageLocationExtractor>();
    }
}
