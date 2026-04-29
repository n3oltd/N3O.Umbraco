using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Offerings.Qurbani)]
public class QurbaniOfferingContent : UmbracoContent<QurbaniOfferingContent> {
    public QurbaniItem QurbaniItem => GetValue(x => x.QurbaniItem);
}