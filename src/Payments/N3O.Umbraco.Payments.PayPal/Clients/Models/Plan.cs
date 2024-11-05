using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public class Plan {
    [JsonProperty("product_id")]
    public string ProductId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
}