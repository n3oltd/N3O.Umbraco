using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.PayPalErrors;

public class ApiCreateProductReq {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("category")]
    public string Category { get; set; }
}