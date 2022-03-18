using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Client.Models {
    public class Link {
        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}