using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class Link {
    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("rel")]
    public string Rel { get; set; }

    [JsonProperty("method")]
    public string Method { get; set; }

    [JsonProperty("encType")]
    public string EncType { get; set; }
}