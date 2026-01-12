using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonOfferingElementKindDataSource : ElementKindDataSource {
    public DonationButtonOfferingElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Donation Button (Offering) Elements";
    public override  string Description => "Data source for offering donation button elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationButtonOffering;
}