
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonateButtonActionDataSource : LookupsDataSource<DonateButtonAction> {
    public DonateButtonActionDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donate Button Actions";
    public override string Description => "Data source for donate button actions";
    public override string Icon => "icon-shopping-basket";

    protected override string GetIcon(DonateButtonAction _) => "icon-shopping-basket";
}
