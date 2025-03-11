using Newtonsoft.Json;

namespace N3O.Umbraco.Giving.Analytics.Models;

public class Item {
    [JsonProperty("item_id")]
    public string Id { get; set; }
    
    [JsonProperty("item_name")]
    public string Name { get; set; }
    
    [JsonProperty("affiliation")]
    public string Affiliation { get; set; }
    
    [JsonProperty("coupon")]
    public string Coupon { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("discount")]
    public decimal Discount { get; set; }
    
    [JsonProperty("index")]
    public int Index { get; set; }
    
    [JsonProperty("item_brand")]
    public string Brand { get; set; }
    
    [JsonProperty("item_category")]
    public string Category { get; set; }
    
    [JsonProperty("item_category2")]
    public string Category2 { get; set; }
    
    [JsonProperty("item_category3")]
    public string Category3 { get; set; }
    
    [JsonProperty("item_category4")]
    public string Category4 { get; set; }
    
    [JsonProperty("item_category5")]
    public string Category5 { get; set; }
    
    [JsonProperty("item_list_id")]
    public string ListId { get; set; }
    
    [JsonProperty("item_list_name")]
    public string ListName { get; set; }
    
    [JsonProperty("item_variant")]
    public string Variant { get; set; }
    
    [JsonProperty("location_id")]
    public string LocationId { get; set; }
    
    [JsonProperty("price")]
    public decimal Price { get; set; }
    
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
