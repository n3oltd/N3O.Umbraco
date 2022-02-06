using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiThreeDSecureChallengeResponse {
        [JsonProperty("cRes")]
        public string CRes { get; set; }

        // Used in Refit client route 
        [JsonIgnore]
        public string TransactionId { get; set; }
    }
}