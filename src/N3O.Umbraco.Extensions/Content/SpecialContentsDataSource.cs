using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Content;

public class SpecialContentsDataSource : LookupsDataSource<SpecialContent> {
    public SpecialContentsDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Special Contents";
    public override string Description => "Data source for special contents";
    public override string Icon => "icon-umb-content";

    protected override string GetDescription(SpecialContent specialContent) => null;
    protected override string GetIcon(SpecialContent specialContent) => Icon;
}
