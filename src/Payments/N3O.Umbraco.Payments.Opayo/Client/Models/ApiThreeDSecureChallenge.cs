using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiThreeDSecureChallenge {
        [JsonProperty("cRes")]
        public string CRes { get; set; }

        // TODO Why is this JsonIgnore?
        [JsonIgnore]
        public string TransactionId { get; set; }
    }
}