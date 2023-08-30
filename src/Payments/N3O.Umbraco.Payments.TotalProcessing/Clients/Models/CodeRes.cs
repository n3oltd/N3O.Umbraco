using Newtonsoft.Json;

namespace Payments.TotalProcessing.Clients.Models;

public class CodeRes {
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}
