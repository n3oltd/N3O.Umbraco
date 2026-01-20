using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms;

public class ZakatCalculatorFieldTypeDataSource : LookupsDataSource<ZakatCalculatorFieldType> {
    public ZakatCalculatorFieldTypeDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Zakat Calculator Field Types";
    public override string Description => "Data source for zakat calculator field types";
    public override string Icon => "icon-economy";

    protected override string GetIcon(ZakatCalculatorFieldType zakatCalculatorFieldType) => "icon-economy";
}