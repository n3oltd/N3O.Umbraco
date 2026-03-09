using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms;

public class ZakatCalculatorFieldClassification : NamedLookup {
    public ZakatCalculatorFieldClassification(string id, string name) : base(id, name) { }
}

public class ZakatCalculatorFieldClassifications : StaticLookupsCollection<ZakatCalculatorFieldClassification> {
    public static readonly ZakatCalculatorFieldClassification Asset = new("asset", "Asset");
    public static readonly ZakatCalculatorFieldClassification Liability = new("liability", "Liability");
}