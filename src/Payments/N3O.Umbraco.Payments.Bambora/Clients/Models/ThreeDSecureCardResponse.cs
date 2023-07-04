using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Clients;

public class ThreeDSecureCardResponse {
    [JsonProperty("cres")]
    public string Cres { get; set; }
}
