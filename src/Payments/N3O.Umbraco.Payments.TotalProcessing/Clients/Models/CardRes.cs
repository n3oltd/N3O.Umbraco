using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class CardRes {
    [JsonProperty("bin")]
    public string Bin { get; set; }

    [JsonProperty("last4Digits")]
    public string Last4Digits { get; set; }

    [JsonProperty("holder")]
    public string Holder { get; set; }

    [JsonProperty("expiryMonth")]
    public string ExpiryMonth { get; set; }

    [JsonProperty("expiryYear")]
    public string ExpiryYear { get; set; }
}
