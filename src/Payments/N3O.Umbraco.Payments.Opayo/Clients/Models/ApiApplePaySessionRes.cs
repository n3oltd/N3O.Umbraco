using N3O.Umbraco.Payments.Opayo.Models;
using Newtonsoft.Json;

namespace N3O.Umbraco.Payments.Opayo.Clients;

public class ApiApplePaySessionRes : IApplePaySession {
    [JsonIgnore]
    public string Status { get; set; }
    
    [JsonProperty("statusCode")]
    public string StatusCode { get; set; }
    
    [JsonIgnore]
    public string StatusDetail { get; set; }
    
    [JsonProperty("epochTimeStamp")]
    public long EpochTimeStamp { get; set; }
    
    [JsonProperty("expiresAt")]
    public long ExpiresAt { get; set; }
    
    [JsonProperty("merchantSessionIdentifier")]
    public string MerchantSessionIdentifier { get; set; }
    
    [JsonProperty("nonce")]
    public string Nonce { get; set; }
    
    [JsonProperty("merchantIdentifier")]
    public string MerchantIdentifier { get; set; }
    
    [JsonProperty("domainName")]
    public string DomainName { get; set; }
    
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }
    
    [JsonProperty("signature")]
    public string Signature { get; set; }
    
    [JsonProperty("sessionValidationToken")]
    public string SessionValidationToken { get; set; }
}
