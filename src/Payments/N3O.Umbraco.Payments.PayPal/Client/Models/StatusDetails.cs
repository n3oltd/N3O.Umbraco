using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Client.Models {
    public class StatusDetails {
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}