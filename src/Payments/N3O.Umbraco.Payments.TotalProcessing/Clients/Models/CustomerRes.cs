using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class CustomerRes {
    [JsonProperty("ip")]
    public string Ip { get; set; }
}
