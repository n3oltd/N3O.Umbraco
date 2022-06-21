using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiThreeDSecure {
    [JsonProperty("status")]
    public string Status { get; set; }
}
