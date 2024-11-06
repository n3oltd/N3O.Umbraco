using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class PricingScheme {
    [JsonProperty("fixed_price")]
    public FixedPrice FixedPrice { get; set; }
}