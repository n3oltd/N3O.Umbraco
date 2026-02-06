using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationFormMenuElementKindDataSource : ElementKindDataSource {
    public DonationFormMenuElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Donation Form (Menu) Elements";
    public override  string Description => "Data source for menu donation form element";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationFormMenu;
}