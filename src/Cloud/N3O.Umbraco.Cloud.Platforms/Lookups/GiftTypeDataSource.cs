using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class GiftTypeDataSource : LookupsDataSource<GiftType> {
    public GiftTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Gift Types";
    public override string Description => "Data source for gift types";
    public override string Icon => "icon-donate";

    protected override string GetIcon(GiftType giftType) => giftType.Icon;
}
