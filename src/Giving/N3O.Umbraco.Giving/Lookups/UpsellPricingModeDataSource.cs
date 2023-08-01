using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class UpsellPricingModeDataSource : LookupsDataSource<UpsellPricingMode> {
    public UpsellPricingModeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Upsell Pricing Mode";
    public override string Description => "Data source for upsell pricing modes";
    public override string Icon => " icon-edit";

    protected override string GetIcon(UpsellPricingMode feedbackCustomField) => " icon-edit";
}