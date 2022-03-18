using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Client {
    public class ApiStatusDetails {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}