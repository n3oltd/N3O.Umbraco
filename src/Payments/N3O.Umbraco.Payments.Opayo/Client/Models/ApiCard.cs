using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiCard {
        [JsonProperty("cardIdentifier")]
        public string CardIdentifier { get; set; }

        [JsonProperty("merchantSessionKey")]
        public string MerchantSessionKey { get; set; }

        [JsonProperty("reusable")]
        public bool Reusable { get; set; }

        [JsonProperty("save")]
        public bool Save { get; set; }
    }
}