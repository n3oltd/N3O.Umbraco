using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiCreatePlanRes {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
}