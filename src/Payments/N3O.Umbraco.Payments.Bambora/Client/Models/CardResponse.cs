using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client {
    public class CardResponse {
        [JsonProperty("last_four", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string LastFour { get; set; }

        [JsonProperty("cvd_match", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? CvdMatch { get; set; }

        [JsonProperty("address_match", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? AddressMatch { get; set; }

        [JsonProperty("postal_result", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int? PostalMatch { get; set; }

        [JsonProperty("avs_result", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string AvsResult { get; set; }

        [JsonProperty("cvd_result", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string CvdResult { get; set; }

        [JsonProperty("card_type", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string CardType { get; set; }
    }
}