namespace N3O.Umbraco.Payments.Opayo.Models;

public interface IApplePaySession {
    string Status { get; }
    string StatusCode { get; }
    string StatusDetail { get; }
    long EpochTimeStamp { get; }
    long ExpiresAt { get; }
    string MerchantSessionIdentifier { get; }
    string Nonce { get; }
    string MerchantIdentifier { get; }
    string DomainName { get; }
    string DisplayName { get; }
    string Signature { get; }
    string SessionValidationToken { get; }
}
