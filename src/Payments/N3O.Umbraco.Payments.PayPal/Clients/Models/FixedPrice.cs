using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class FixedPrice {
    [JsonProperty("value")]
    public string Value { get; set; }
    
    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }
}