using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms;

public class OfferingContentVisibilityFilter : PlatformsPageContentVisibilityFilter {
    public OfferingContentVisibilityFilter(IContentCache contentCache,
                                           Lazy<IPlatformsPageAccessor> platformsPageAccessor)
        : base(contentCache, platformsPageAccessor) { }

    protected override SpecialContent Page => PlatformsSpecialPages.Offering;
    protected override PublishedFileKind PublishedFileKind => PublishedFileKinds.OfferingPage;
}
