using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Engage.Lookups;

public class DataRegion : NamedLookup {
    public DataRegion(string id, string name) : base(id, name) { }
    
    public string Slug => Id;
}

public class DataRegions : StaticLookupsCollection<DataRegion> {
    public static readonly DataRegion EU1 = new("eu1", "Europe 1");
}