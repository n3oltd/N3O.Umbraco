using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class Frequency {
    [JsonProperty("interval_unit")]
    public string IntervalUnit { get; set; }
    
    [JsonProperty("interval_count")]
    public int IntervalCount { get; set; }
}