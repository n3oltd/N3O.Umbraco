using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class CrowdfunderPlatformsMergeModelProvider : PlatformsMergeModelProvider {
    public CrowdfunderPlatformsMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor)
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.Crowdfunder;
}