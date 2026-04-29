using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ECommerceStageDataSource : LookupsDataSource<ECommerceStage> {
    public ECommerceStageDataSource(ILookups lookups) : base(lookups) { }

    public override string Name => "E-Commerce Stages";
    public override string Description => "Data source for e-commerce stages";
    public override string Icon => "icon-shopping-basket";

    protected override string GetIcon(ECommerceStage eCommerceStage) => "icon-shopping-basket";
}