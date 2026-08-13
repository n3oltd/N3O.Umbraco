using Newtonsoft.Json;

namespace N3O.Umbraco.Search.Typesense.Models;

public class ExportedDocument {
    [JsonProperty("id")]
    public string Id { get; set; }
}
