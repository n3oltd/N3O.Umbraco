using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLookupsRoot {
    [JsonProperty("lookups")]
    public PublishedLookups Lookups { get; set; }
}