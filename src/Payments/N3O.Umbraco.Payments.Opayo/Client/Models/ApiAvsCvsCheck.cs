using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Client {
    public class ApiAvsCvsCheck {
        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
        
        [JsonProperty("securityCode")]
        public string SecurityCode { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}