using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationFormElementKindDataSource : ElementKindDataSource {
    public DonationFormElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Form Element";
    public override string Description => "Data source for donation form element";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationForm;
}