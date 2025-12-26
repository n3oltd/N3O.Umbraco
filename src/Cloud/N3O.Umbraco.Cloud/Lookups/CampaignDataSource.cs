using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class CampaignDataSource : LookupsDataSource<Campaign> {
    public CampaignDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Campaigns";
    public override string Description => "Data source for campaigns";
    public override string Icon => "icon-categories";

    protected override string GetIcon(Campaign campaign) => "icon-categories";
}
