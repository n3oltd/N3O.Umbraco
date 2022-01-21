using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiThreeDSecure {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}