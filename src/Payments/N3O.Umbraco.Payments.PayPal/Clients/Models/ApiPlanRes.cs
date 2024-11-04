using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public class ApiPlanRes {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    
}