using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonCustomElementKindDataSource : ElementKindDataSource {
    public DonationButtonCustomElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Donation Button (Custom) Elements";
    public override  string Description => "Data source for custom donation button elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.DonationButtonCustom;
}