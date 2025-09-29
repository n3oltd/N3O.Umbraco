using N3O.Umbraco.Cloud.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Templates;

public class DesignationPlatformsMergeModelProvider : PlatformsMergeModelProvider {
    public DesignationPlatformsMergeModelProvider(IPlatformsPageAccessor platformsPageAccessor)
        : base(platformsPageAccessor) { }
    
    protected override PublishedFileKind Kind => PublishedFileKinds.Designation;
}