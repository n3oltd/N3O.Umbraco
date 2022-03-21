using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class CardResponse {
        [JsonProperty(PropertyName = "last_four", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string LastFour { get; set; }

        [JsonProperty(PropertyName = "cvd_match", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? CvdMatch { get; set; }

        [JsonProperty(PropertyName = "address_match", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? AddressMatch { get; set; }

        [JsonProperty(PropertyName = "postal_result", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? PostalMatch { get; set; }

        [JsonProperty(PropertyName = "avs_result", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string AvsResult { get; set; }

        [JsonProperty(PropertyName = "cvd_result", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string CvdResult { get; set; }

        [JsonProperty(PropertyName = "card_type", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string CardType { get; set; }
    }
}