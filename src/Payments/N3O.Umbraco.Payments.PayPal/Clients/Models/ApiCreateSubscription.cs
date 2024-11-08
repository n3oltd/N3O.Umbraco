using N3O.Umbraco.Payments.PayPal.Clients.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApiCreateSubscription {
    [JsonProperty("plan_id")]
    public string PlanId { get; set; }
    
    [JsonProperty("application_context")]
    public ApplicationContext ApplicationContext { get; set; }
}