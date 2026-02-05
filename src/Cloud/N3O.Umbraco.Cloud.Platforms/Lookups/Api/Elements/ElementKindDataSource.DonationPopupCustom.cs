using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationPopupCustomElementKindDataSource : ElementKindDataSource {
    public DonationPopupCustomElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Popup (Custom) Elements";
    public override  string Description => "Data source for custom donation popup elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationPopupCustom;
}