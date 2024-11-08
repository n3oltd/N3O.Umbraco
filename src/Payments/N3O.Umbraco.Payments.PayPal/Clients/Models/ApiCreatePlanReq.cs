using N3O.Umbraco.Payments.PayPal.Clients.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.PayPal.Clients;

public class ApiCreatePlanReq {
    [JsonProperty("product_id")]
    public string ProductId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; }
    
    [JsonProperty("billing_cycles")]
    public List<BillingCycle> BillingCycles { get; set; }
    
    [JsonProperty("payment_preferences")]
    public PaymentPreferences PaymentPreferences { get; set; }
}