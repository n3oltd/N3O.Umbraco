using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ContentFeedLibraryElementKindDataSource : ElementKindDataSource {
    public ContentFeedLibraryElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Content Feed (Library) Elements";
    public override  string Description => "Data source for library content feed elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.ContentFeedLibrary;
}