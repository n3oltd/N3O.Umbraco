using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Cloud.Platforms;

public class CrowdfunderContentVisibilityFilter : PlatformsPageContentVisibilityFilter {
    public CrowdfunderContentVisibilityFilter(IContentCache contentCache,
                                              Lazy<IPlatformsPageAccessor> platformsPageAccessor)
        : base(contentCache, platformsPageAccessor) { }

    protected override SpecialContent Page => PlatformsSpecialPages.Crowdfunder;
    protected override PublishedFileKind PublishedFileKind => PublishedFileKinds.CrowdfunderPage;
}
