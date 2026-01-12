using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationFormOfferingElementKindDataSource : ElementKindDataSource {
    private readonly ILookups _lookups;
    
    public DonationFormOfferingElementKindDataSource(ILookups lookups) : base(lookups) {
        _lookups = lookups;
    }
    
    public override  string Name => "Donation Form (Offering) Elements";
    public override  string Description => "Data source for offering donation form elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationFormOffering;

    protected override string GetDescription(Element lookup) {
        var offering = _lookups.FindById<Offering>(GetOfferingId(lookup.Id));
        var campaign = _lookups.FindById<Campaign>(offering.CampaignId);
        
        return $"Campaign: {campaign.Name}";
    }
    
    private string GetOfferingId(string id) {
        var (_, offeringId) = ElementId.Parse(id);

        return offeringId;
    }
}