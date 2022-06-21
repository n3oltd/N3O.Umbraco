using NodaTime;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class MerchantSessionKeyRes {
    public string Key { get; set; }
    public Instant ExpiresAt { get; set; }
}
