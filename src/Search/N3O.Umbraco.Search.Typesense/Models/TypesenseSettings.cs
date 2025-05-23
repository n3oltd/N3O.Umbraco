namespace N3O.Umbraco.Search.Typesense.Models;

public class TypesenseSettings : Value {
    public TypesenseSettings(string apiKey, string node) {
        ApiKey = apiKey;
        Node = node;
    }

    public string ApiKey { get; }
    public string Node { get; }
}