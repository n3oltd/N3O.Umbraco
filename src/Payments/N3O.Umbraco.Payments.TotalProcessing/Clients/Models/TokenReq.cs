using Newtonsoft.Json;
using Refit;

namespace Payments.TotalProcessing.Clients.Models;

public class TokenReq {
    [JsonProperty("entityId")]
    public string EntityId { get; set; }

    [JsonProperty("createRegistration")]
    public bool? CreateRegistration { get; set; }

    [AliasAs("standingInstruction.source")]
    public string Source { get; set; }

    [AliasAs("standingInstruction.mode")]
    public string Mode { get; set; }
    
    [JsonProperty("paymentType")]
    public string PaymentType { get; set; }
}
