using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class Token {
    [JsonProperty("complete")]
    public bool Complete { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("3d_secure")]
    public ThreeDSecure ThreeDSecure { get; set; }
}
