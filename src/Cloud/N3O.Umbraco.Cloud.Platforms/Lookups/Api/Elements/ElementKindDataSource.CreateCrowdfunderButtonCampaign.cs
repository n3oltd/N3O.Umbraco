using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class CreateCrowdfunderButtonCampaignElementKindDataSource : ElementKindDataSource {
    public CreateCrowdfunderButtonCampaignElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Create Crowdfunder Button (Campaign) Elements";
    public override string Description => "Data source for campaign create crowdfunder button elements";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.CreateCrowdfunderButtonCampaign;
}