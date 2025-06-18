using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.TaxRelief.Lookups;

public class TaxStatus : NamedLookup {
    private readonly bool? _boolValue;
    
    public TaxStatus(string id, string name, bool? boolValue) : base(id, name) {
        _boolValue = boolValue;
    }

    public bool? ToBool() => _boolValue;
}

public class TaxStatuses : StaticLookupsCollection<TaxStatus> {
    public static readonly TaxStatus Payer = new("payer", "Payer", true);
    public static readonly TaxStatus NonPayer = new("nonPayer", "Non-Payer", false);
    public static readonly TaxStatus NotSpecified = new("notSpecified", "Not Specified", null);
}
