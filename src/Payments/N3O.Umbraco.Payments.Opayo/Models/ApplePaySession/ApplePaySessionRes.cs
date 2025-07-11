namespace N3O.Umbraco.Payments.Opayo.Models;

public class ApplePaySessionRes {
    public string Status { get; set; }
    public string StatusCode { get; set; }
    public string StatusDetail { get; set; }
    public long EpochTimeStamp { get; set; }
    public long ExpiresAt { get; set; }
    public string MerchantSessionIdentifier { get; set; }
    public string Nonce { get; set; }
    public string MerchantIdentifier { get; set; }
    public string DomainName { get; set; }
    public string DisplayName { get; set; }
    public string Signature { get; set; }
    public string SessionValidationToken { get; set; }
}