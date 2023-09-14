using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class PaymentReq {
    [JsonProperty("entityId")]
    public string EntityId { get; set; }

    [JsonProperty("amount")]
    public string Amount { get; set; }

    [JsonProperty("billing")]
    public ApiBillingReq Billing { get; set; }
    
    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("paymentType")]
    public string PaymentType { get; set; }
    
    [JsonProperty("createRegistration")]
    public bool? CreateRegistration { get; set; }

    [JsonProperty("standingInstruction")]
    public StandingInstruction StandingInstruction { get; set; }
}