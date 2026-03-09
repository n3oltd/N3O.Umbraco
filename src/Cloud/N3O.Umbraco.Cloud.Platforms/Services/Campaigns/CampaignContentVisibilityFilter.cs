using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms;

public class CampaignContentVisibilityFilter : PlatformsPageContentVisibilityFilter {
    public CampaignContentVisibilityFilter(IContentCache contentCache,
                                           Lazy<IPlatformsPageAccessor> platformsPageAccessor)
        : base(contentCache, platformsPageAccessor) { }

    protected override SpecialContent Page => PlatformsSpecialPages.Campaign;
    protected override PublishedFileKind PublishedFileKind => PublishedFileKinds.CampaignPage;
}
