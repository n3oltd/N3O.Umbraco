using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonElementKindDataSource : ElementKindDataSource {
    public DonationButtonElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Button Element";
    public override string Description => "Data source for donation button element";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationButton;
}