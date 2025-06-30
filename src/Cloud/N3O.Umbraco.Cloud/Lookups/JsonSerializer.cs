using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class JsonSerializer : Lookup {
    public JsonSerializer(string id) : base(id) { }
}

public class JsonSerializers : StaticLookupsCollection<JsonSerializer> {
    public static readonly JsonSerializer JsonProvider = new("jsonProvider");
    public static readonly JsonSerializer Simple = new("simple");
}