using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class FundDimensionSelector : NamedLookup {
    public FundDimensionSelector(string id, string name) : base(id, name) { }
}

public class FundDimensionSelectors : StaticLookupsCollection<FundDimensionSelector> {
    public static readonly FundDimensionSelector Dropdown = new("dropdown", "Dropdown");
    public static readonly FundDimensionSelector Toggle = new("toggle", "Toggle");
}