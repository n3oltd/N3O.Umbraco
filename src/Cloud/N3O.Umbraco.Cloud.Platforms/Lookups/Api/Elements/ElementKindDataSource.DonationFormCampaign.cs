using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationFormCampaignElementKindDataSource : ElementKindDataSource {
    public DonationFormCampaignElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Donation Form (Campaign) Elements";
    public override  string Description => "Data source for campaign donation form elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationFormCampaign;
}