using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ElementDataSource : LookupsDataSource<Element> {
    public ElementDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Elements";
    public override string Description => "Data source for elements";
    public override string Icon => "icon-categories";

    protected override string GetIcon(Element element) => "icon-categories";
}