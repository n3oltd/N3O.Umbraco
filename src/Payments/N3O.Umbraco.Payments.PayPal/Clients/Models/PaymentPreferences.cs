using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.PayPal.Clients.Models;

public class PaymentPreferences {
    [JsonProperty("auto_bill_outstanding")]
    public bool AutoBillOutstanding { get; set; }
    
    [JsonProperty("setup_fee")]
    public SetupFee SetupFee { get; set; }
    
    [JsonProperty("setup_fee_failure_action")]
    public string SetupFeeFailureAction { get; set; }
    
    [JsonProperty("payment_failure_threshold")]
    public int PaymentFailureThreshold { get; set; }
}