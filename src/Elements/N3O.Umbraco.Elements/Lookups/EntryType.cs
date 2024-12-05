using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Elements.Lookups;

public class EntryType : Lookup {
    public EntryType(string id) : base(id) { }
}

public class EntryTypes : StaticLookupsCollection<EntryType> {
    public static readonly EntryType Category = new("category");
    public static readonly EntryType Option = new("option");
}
