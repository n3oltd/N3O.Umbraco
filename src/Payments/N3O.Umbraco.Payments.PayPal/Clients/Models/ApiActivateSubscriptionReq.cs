using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiActivateSubscriptionReq {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("reason")]
    public string Reason { get; set; }
}