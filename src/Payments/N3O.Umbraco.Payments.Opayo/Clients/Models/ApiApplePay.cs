using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiApplePay {
    [JsonProperty("merchantSessionKey")]
    public string MerchantSessionKey { get; set; }
    
    [JsonProperty("clientIpAddress")]
    public string ClientIpAddress { get; set; }
    
    [JsonProperty("sessionValidationToken")]
    public string SessionValidationToken { get; set; }
    
    [JsonProperty("paymentData")]
    public string PaymentData { get; set; }
    
    [JsonProperty("applicationData")]
    public string ApplicationData { get; set; }
    
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
}