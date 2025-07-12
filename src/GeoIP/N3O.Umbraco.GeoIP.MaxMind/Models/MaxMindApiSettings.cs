namespace N3O.Umbraco.GeoIP.MaxMind.Models;

public class MaxMindApiSettings : Value {
    public MaxMindApiSettings(int accountId, string licenseKey) {
        AccountId = accountId;
        LicenseKey = licenseKey;
    }

    public int AccountId { get; }
    public string LicenseKey { get; }
}
