using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class OfferingPlatformsMergeModelProvider : PlatformsMergeModelProvider {
    public OfferingPlatformsMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor)
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.OfferingPage;
}