using N3O.Umbraco.Payments.PayPal.Clients.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class Plan {
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("product_id")]
    public string ProductId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("usage_type")]
    public string UsageType { get; set; }
    
    [JsonProperty("billing_cycles")]
    public List<BillingCycle> BillingCycles { get; set; }
    
    [JsonProperty("payment_preferences")]
    public PaymentPreferences PaymentPreferences { get; set; }
    
    [JsonProperty("quantity_supported")]
    public bool QuantitySupported { get; set; }
}