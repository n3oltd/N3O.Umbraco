using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class GivingTypeDataSource : LookupsDataSource<GivingType> {
    public GivingTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Giving Types";
    public override string Description => "Data source for giving types";
    public override string Icon => "icon-donate";

    protected override string GetIcon(GivingType givingType) => givingType.Icon;
}
