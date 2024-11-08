using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class ApplicationContext {
    [JsonProperty("return_url")]
    public string ReturnUrl { get; set; }
    
    [JsonProperty("cancel_url")]
    public string CancelUrl { get; set; }
    
    [JsonProperty("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }
}