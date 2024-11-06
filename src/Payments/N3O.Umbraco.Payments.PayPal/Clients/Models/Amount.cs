using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class Amount {
    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }

    [JsonProperty("value")]
    public decimal Value { get; set; }
}