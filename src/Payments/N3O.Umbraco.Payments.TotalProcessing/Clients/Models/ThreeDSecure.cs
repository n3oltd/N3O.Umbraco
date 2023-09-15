using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class ThreeDSecure {
    [JsonProperty("eci")]
    public string Eci { get; set; }
}
