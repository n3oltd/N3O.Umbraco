using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class ThreeDSecure {
    [JsonProperty("browser")]
    public BrowserReq Browser { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("version")]
    public int Version { get; set; }

    [JsonProperty("auth_required")]
    public bool AuthRequired { get; set; }
}
