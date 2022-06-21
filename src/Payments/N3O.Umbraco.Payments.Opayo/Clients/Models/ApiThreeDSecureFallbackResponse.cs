using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiThreeDSecureFallbackResponse {
    [JsonProperty("paRes")]
    public string PaRes { get; set; }

    // Used in Refit client route 
    [JsonIgnore]
    public string TransactionId { get; set; }
}
