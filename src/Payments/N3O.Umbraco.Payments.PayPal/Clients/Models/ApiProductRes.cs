using J2N.Collections.Generic;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiProductRes {
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

    [JsonProperty("image_url")]
    public string ImageUrl { get; set; }
    
    [JsonProperty("home_url")]
    public string HomeUrl { get; set; }
    
    [JsonProperty("create_time")]
    public string CreateTime { get; set; }
    
    [JsonProperty("update_time")]
    public string UpdateTime { get; set; }
    
    [JsonProperty("links")]
    public List<Link> Links { get; set; }
}