using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class CrowdfunderPageMergeModelProvider : PlatformsPageMergeModelProvider {
    public CrowdfunderPageMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor)
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.CrowdfunderPage;
}