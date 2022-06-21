using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client;

public class Link {
    [JsonProperty("rel")]
    public string Rel { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("method")]
    public string Method { get; set; }
}
