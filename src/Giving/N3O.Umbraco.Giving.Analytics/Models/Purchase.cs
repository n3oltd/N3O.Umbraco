using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Analytics.Models;

public class Purchase {
    [JsonProperty("transaction_id")]
    public string Id { get; set; }
    
    [JsonProperty("affiliation")]
    public string Affiliation { get; set; }
    
    [JsonProperty("value")]
    public decimal Value { get; set; }
    
    [JsonProperty("tax")]
    public decimal Tax { get; set; }
    
    [JsonProperty("shipping")]
    public decimal Shipping { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("coupon")]
    public string Coupon { get; set; }
    
    [JsonProperty("items")]
    public IEnumerable<Item> Items { get; set; }
}
