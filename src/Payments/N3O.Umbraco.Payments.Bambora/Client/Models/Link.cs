using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class Link {
        [JsonProperty(PropertyName = "rel")]
        public string Rel { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }
    }
}