using N3O.Umbraco.Extensions;
using Newtonsoft.Json;

namespace N3O.Umbraco.Localization;

public class TextResource {
    [JsonProperty("source")]
    public string Source { get; set; }
    
    [JsonProperty("custom")]
    public string Custom { get; set; }

    [JsonIgnore]
    public string Value => Custom.Or(Source);
}
