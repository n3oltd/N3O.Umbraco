using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class CustomParametersRes {
    [JsonProperty("StandingInstructionAPI")]
    public string StandingInstructionAPI { get; set; }

    [JsonProperty("SHOPPER_EndToEndIdentity")]
    public string SHOPPEREndToEndIdentity { get; set; }

    [JsonProperty("StoredCredentialType")]
    public string StoredCredentialType { get; set; }
}
