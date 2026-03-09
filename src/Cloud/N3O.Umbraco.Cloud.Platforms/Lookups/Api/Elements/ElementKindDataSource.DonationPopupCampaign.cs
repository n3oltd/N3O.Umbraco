using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationPopupCampaignElementKindDataSource : ElementKindDataSource {
    public DonationPopupCampaignElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Popup (Campaign) Elements";
    public override string Description => "Data source for campaign donation popup elements";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationPopupCampaign;
}