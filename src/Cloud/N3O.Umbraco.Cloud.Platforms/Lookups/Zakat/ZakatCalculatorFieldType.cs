using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms;

public class ZakatCalculatorFieldType : NamedLookup {
    public ZakatCalculatorFieldType(string id, string name) : base(id, name) { }
}

public class ZakatCalculatorFieldTypes : StaticLookupsCollection<ZakatCalculatorFieldType> {
    public static readonly ZakatCalculatorFieldType Metal = new("metal", "Metal");
    public static readonly ZakatCalculatorFieldType Money = new("money", "Money");
}