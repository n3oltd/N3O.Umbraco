using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class OfferingDataSource : LookupsDataSource<Offering> {
    private readonly ILookups _lookups;
    
    public OfferingDataSource(ILookups lookups) : base(lookups) {
        _lookups = lookups;
    }
    
    public override string Name => "Offerings";
    public override string Description => "Data source for offerings";
    public override string Icon => "icon-categories";

    protected override string GetIcon(Offering offering) => "icon-categories";

    protected override string GetDescription(Offering offering) {
        var campaign = _lookups.FindById<Campaign>(offering.CampaignId);
        
        return $"{campaign.Name}: {offering.Name}";
    }
}