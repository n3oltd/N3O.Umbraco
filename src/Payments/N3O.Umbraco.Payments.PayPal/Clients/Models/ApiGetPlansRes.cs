using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiGetPlansRes {
    [JsonProperty("plans")]
    public List<Plan> Plans { get; set; }
    
    [JsonProperty("total_items")]
    public int TotalItems { get; set; }
    
    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }
    
    [JsonProperty("links")]
    public List<Link> Links { get; set; }
}