using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ContentFeedCustomElementKindDataSource : ElementKindDataSource {
    public ContentFeedCustomElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Content Feed (Custom) Elements";
    public override  string Description => "Data source for custom content feed elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.ContentFeedCustom;
}