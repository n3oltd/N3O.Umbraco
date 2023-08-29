using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class Risk {
    [JsonProperty("score")]
    public string Score { get; set; }
}
