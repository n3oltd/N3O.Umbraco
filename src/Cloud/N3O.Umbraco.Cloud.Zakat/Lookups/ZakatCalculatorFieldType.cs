using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Zakat;

public class ZakatCalculatorFieldType : NamedLookup {
    public ZakatCalculatorFieldType(string id, string name) : base(id, name) { }
}

public class ZakatCalculatorFieldTypes : StaticLookupsCollection<ZakatCalculatorFieldType> {
    public static readonly ZakatCalculatorFieldType Asset = new("asset", "Asset");
    public static readonly ZakatCalculatorFieldType Liability = new("liability", "Liability");
}