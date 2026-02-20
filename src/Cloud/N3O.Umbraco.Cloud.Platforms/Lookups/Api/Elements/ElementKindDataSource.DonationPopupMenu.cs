using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationPopupMenuElementKindDataSource : ElementKindDataSource {
    public DonationPopupMenuElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Donation Popup (Menu) Elements";
    public override  string Description => "Data source for menu donation popup element";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationPopupMenu;
}