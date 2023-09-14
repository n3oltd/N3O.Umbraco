using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class StandingInstruction {
    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("mode")]
    public string Mode { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("initialTransactionId ")]
    public string InitialTransactionId  { get; set; }
}
