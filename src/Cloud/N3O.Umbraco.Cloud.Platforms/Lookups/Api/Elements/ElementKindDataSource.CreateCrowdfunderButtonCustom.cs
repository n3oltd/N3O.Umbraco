using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class CreateCrowdfunderButtonCustomElementKindDataSource : ElementKindDataSource {
    public CreateCrowdfunderButtonCustomElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Create Crowdfunder Button (Custom) Elements";
    public override  string Description => "Data source for custom create crowdfunder button elements";
    public override  string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.CreateCrowdfunderButtonCustom;
}