using Newtonsoft.Json;

namespace N3O.Umbraco.Cdn.Cloudflare.Clients;

public class ApiPlayback {
    [JsonProperty("hls")]
    public string Hls { get; set; }
}