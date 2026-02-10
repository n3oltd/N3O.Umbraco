using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class CreateCrowdfunderButtonElementKindDataSource : ElementKindDataSource {
    public CreateCrowdfunderButtonElementKindDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Create Crowdfunder Button Element";
    public override string Description => "Data source for create crowdfunder button element";
    public override string Icon => "icon-categories";

    protected override ElementKind Kind => ElementKind.CreateCrowdfunderButton;
}