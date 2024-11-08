using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class PaymentMethod {
    [JsonProperty("payer_selected")]
    public string PayerSelected { get; set; }
    
    [JsonProperty("payee_preferred")]
    public string PayeePreferred { get; set; }
}