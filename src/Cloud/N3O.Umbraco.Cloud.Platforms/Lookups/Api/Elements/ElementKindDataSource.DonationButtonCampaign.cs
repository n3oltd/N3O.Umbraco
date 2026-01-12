using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonCampaignElementKindDataSource : ElementKindDataSource {
    public DonationButtonCampaignElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Button (Campaign) Elements";
    public override string Description => "Data source for campaign donation button elements";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationButtonCampaign;
}