using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Platforms.Models.Connect.Lookups;

public class PlatformsLookups {
    [JsonProperty("Lookups")]
    public PlatformLookupsCountries Lookups { get; set; }
}