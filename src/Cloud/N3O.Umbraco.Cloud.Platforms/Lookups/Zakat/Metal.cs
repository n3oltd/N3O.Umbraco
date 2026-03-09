using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms;

public class Metal : NamedLookup {
    public Metal(string id, string name) : base(id, name) { }
}

public class Metals : StaticLookupsCollection<Metal> {
    public static readonly Metal Gold = new("gold", "Gold");
    public static readonly Metal Silver = new("silver", "Silver");
}