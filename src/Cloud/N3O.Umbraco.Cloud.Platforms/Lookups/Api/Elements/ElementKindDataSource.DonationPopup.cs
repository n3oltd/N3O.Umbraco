using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationPopupElementKindDataSource : ElementKindDataSource {
    public DonationPopupElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Popup Element";
    public override string Description => "Data source for donation popup element";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationPopup;
}