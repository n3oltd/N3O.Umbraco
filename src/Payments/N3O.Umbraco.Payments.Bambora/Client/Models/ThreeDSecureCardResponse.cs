using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class ThreeDSecureCardResponse {
        [JsonProperty("cres")]
        public string Cres { get; set; }
    }
}