using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ContentFeedCollectionElementKindDataSource : ElementKindDataSource {
    private readonly ILookups _lookups;
    
    public ContentFeedCollectionElementKindDataSource(ILookups lookups) : base(lookups) {
        _lookups = lookups;
    }
    
    public override  string Name => "Content Feed (Collection) Elements";
    public override  string Description => "Data source for collection content feed elements";
    public override  string Icon => "icon-thumbnails";

    protected override ElementKind Kind => ElementKind.ContentFeedCollection;
}