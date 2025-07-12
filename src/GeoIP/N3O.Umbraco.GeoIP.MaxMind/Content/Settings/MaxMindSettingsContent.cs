using N3O.Umbraco.Content;

namespace N3O.Umbraco.GeoIP.MaxMind.Content;

public class MaxMindSettingsContent : UmbracoContent<MaxMindSettingsContent> {
    public int AccountId => GetValue(x => x.AccountId);
    public string LicenseKey => GetValue(x => x.LicenseKey);
}
