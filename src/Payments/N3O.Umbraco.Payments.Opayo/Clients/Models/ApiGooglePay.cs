using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiGooglePay {
    [JsonProperty("clientIpAddress")]
    public string ClientIpAddress { get; set; }

    [JsonProperty("merchantSessionKey")]
    public string MerchantSessionKey { get; set; }

    [JsonProperty("payload")]
    public string Token { get; set; }
}
