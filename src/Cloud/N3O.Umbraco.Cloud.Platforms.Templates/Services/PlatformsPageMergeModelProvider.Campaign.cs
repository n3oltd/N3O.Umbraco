using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class CampaignPageMergeModelProvider : PlatformsPageMergeModelProvider {
    public CampaignPageMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor)
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.CampaignPage;
}