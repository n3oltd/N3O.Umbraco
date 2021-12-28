using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.TaxRelief.Lookups;

public class TaxStatus : NamedLookup {
    public TaxStatus(string id, string name) : base(id, name) { }
}

public class TaxStatuses : StaticLookupsCollection<TaxStatus> {
    public static readonly TaxStatus Payer = new("payer", "Payer");
    public static readonly TaxStatus NonPayer = new("nonPayer", "Non-Payer");
    public static readonly TaxStatus NotSpecified = new("notSpecified", "Not Specified");
}
