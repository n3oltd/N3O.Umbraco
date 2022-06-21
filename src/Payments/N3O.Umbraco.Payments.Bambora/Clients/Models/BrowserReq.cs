using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Bambora.Client;

public class BrowserReq {
    [JsonProperty("accept_header")]
    public string AcceptHeader { get; set; }

    [JsonProperty("java_enabled")]
    public bool JavaEnabled { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("color_depth")]
    public string ColorDepth { get; set; }

    [JsonProperty("screen_height")]
    public int ScreenHeight { get; set; }

    [JsonProperty("screen_width")]
    public int ScreenWidth { get; set; }

    [JsonProperty("time_zone")]
    public int TimeZone { get; set; }

    [JsonProperty("user_agent")]
    public string UserAgent { get; set; }

    [JsonProperty("javascript_enabled")]
    public bool JavascriptEnabled { get; set; }
}
