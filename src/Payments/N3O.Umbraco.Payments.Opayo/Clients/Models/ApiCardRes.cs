using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients {
    public class ApiCardRes : ApiCard {
        [JsonProperty("expiryDate")]
        public string ExpiryDate { get; set; }
        
        [JsonProperty("lastFourDigits")]
        public string LastFourDigits { get; set; }
        
        [JsonProperty("cardType")]
        public string CardType { get; set; }
    }
}