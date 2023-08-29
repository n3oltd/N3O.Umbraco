using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class TokenRes {
    [JsonProperty("result")]
    public CodeRes Result { get; set; }

    [JsonProperty("buildNumber")]
    public string BuildNumber { get; set; }

    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }

    [JsonProperty("ndc")]
    public string Ndc { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
}
