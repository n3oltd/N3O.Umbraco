using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class TokenReq {
    [JsonProperty("entityId")]
    public string EntityId { get; set; }

    [JsonProperty("createRegistration")]
    public bool? CreateRegistration { get; set; }

    [JsonProperty("standingInstruction")]
    public StandingInstruction StandingInstruction { get; set; }
    
    [JsonProperty("paymentType")]
    public string PaymentType { get; set; }
}
