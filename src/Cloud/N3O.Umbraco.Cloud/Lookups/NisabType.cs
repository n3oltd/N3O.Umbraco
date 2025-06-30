using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class NisabType : NamedLookup {
    public NisabType(string id, string name) : base(id, name) { }
}

public class NisabTypes : StaticLookupsCollection<NisabType> {
    public static readonly NisabType Gold = new("gold", "Gold");
    public static readonly NisabType Silver = new("silver", "Silver");
}