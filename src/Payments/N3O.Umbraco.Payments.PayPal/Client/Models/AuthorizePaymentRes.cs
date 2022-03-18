using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Client.Models {
    public class AuthorizePaymentRes {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("links")]
        public IEnumerable<Link> Links { get; set; }

        [JsonProperty("status_details")]
        public StatusDetails StatusDetails { get; set; }
    }
}