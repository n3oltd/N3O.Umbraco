using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class Token {
        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "function", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Function { get; set; }
    }
}