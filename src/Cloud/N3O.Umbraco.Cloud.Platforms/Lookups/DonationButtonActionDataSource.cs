
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonActionDataSource : LookupsDataSource<DonationButtonAction> {
    public DonationButtonActionDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Button Actions";
    public override string Description => "Data source for donation button actions";
    public override string Icon => "icon-shopping-basket";

    protected override string GetIcon(DonationButtonAction _) => "icon-shopping-basket";
}
