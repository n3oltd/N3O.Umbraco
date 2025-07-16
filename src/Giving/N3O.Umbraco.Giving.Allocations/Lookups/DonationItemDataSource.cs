using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Allocations.Lookups;

public class DonationItemDataSource : LookupsDataSource<DonationItem> {
    public DonationItemDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Items";
    public override string Description => "Data source for donation items";
    public override string Icon => "icon-shopping-basket-alt-2";

    protected override string GetIcon(DonationItem donationItem) => "icon-shopping-basket-alt-2";
}
