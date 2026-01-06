using System.Collections.Generic;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseCollectionsOptions {
    public Dictionary<string, string> Collections { get; set; } = new();
}