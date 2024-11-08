using N3O.Umbraco.Accounts.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class Subscriber {
    [JsonProperty("shipping_address")]
    public ShippingAddress ShippingAddress { get; set; }

    [JsonProperty("name")]
    public Name Name { get; set; }

    [JsonProperty("email_address")]
    public string EmailAddress { get; set; }

    [JsonProperty("payer_id")]
    public string PayerId { get; set; }
}