using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationFormCustomElementKindDataSource : ElementKindDataSource {
    public DonationFormCustomElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override  string Name => "Donation Form (Custom) Elements";
    public override  string Description => "Data source for custom donation form elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationFormCustom;
}