using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiBillingAddressReq {
        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}