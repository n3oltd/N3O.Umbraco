using NodaTime;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public interface IMerchantSessionKey {
        string Key { get; }
        Instant ExpiresAt { get; }
    }
}