using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonMenuElementKindDataSource : ElementKindDataSource {
    public DonationButtonMenuElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Donation Button (Menu) Elements";
    public override  string Description => "Data source for menu donation button element";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationButtonMenu;
}