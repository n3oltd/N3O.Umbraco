using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms;

public class ZakatCalculatorFieldClassificationDataSource :
    LookupsDataSource<ZakatCalculatorFieldClassification> {
    public ZakatCalculatorFieldClassificationDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Zakat Calculator Field Classifications";
    public override string Description => "Data source for zakat calculator field classifications";
    public override string Icon => "icon-economy";

    protected override string GetIcon(ZakatCalculatorFieldClassification zakatCalculatorFieldClassification) => "icon-economy";
}