using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiStrongCustomerAuthentication {
    [JsonProperty("browserAcceptHeader")]
    public string BrowserAcceptHeader { get; set; }

    [JsonProperty("browserColorDepth")]
    public string BrowserColorDepth { get; set; }

    [JsonProperty("browserIP")]
    public string BrowserIp { get; set; }

    [JsonProperty("browserJavascriptEnabled")]
    public bool BrowserJavascriptEnabled { get; set; }

    [JsonProperty("browserJavaEnabled")]
    public bool? BrowserJavaEnabled { get; set; }

    [JsonProperty("browserLanguage")]
    public string BrowserLanguage { get; set; }

    [JsonProperty("browserScreenHeight")]
    public string BrowserScreenHeight { get; set; }
    
    [JsonProperty("browserScreenWidth")]
    public string BrowserScreenWidth { get; set; }

    [JsonProperty("transType")]
    public string TransactionType { get; set; }

    [JsonProperty("browserTZ")]
    public string BrowserTimezone { get; set; }

    [JsonProperty("browserUserAgent")]
    public string BrowserUserAgent { get; set; }

    [JsonProperty("challengeWindowSize")]
    public string ChallengeWindowSize { get; set; }

    [JsonProperty("notificationURL")]
    public string NotificationUrl { get; set; }
}
