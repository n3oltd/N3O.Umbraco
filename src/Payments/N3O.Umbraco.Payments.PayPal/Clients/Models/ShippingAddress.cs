using N3O.Umbraco.Accounts.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ShippingAddress {
    [JsonProperty("name")]
    public Name Name { get; set; }

    [JsonProperty("address")]
    public Address Address { get; set; }
}