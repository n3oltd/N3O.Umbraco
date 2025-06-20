using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiCardGooglePay {
    [JsonProperty("clientIpAddress")]
    public string ClientIpAddress { get; set; }

    [JsonProperty("merchantSessionKey")]
    public string MerchantSessionKey { get; set; }

    //base 64 encoded payment token from Google
    [JsonProperty("payload")]
    public string Payload { get; set; }
}
