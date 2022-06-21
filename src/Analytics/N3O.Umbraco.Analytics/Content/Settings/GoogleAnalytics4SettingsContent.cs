using N3O.Umbraco.Content;

namespace N3O.Umbraco.Analytics.Content;

public class GoogleAnalytics4SettingsContent : UmbracoContent<GoogleAnalytics4SettingsContent> {
    public string MeasurementId => GetValue(x => x.MeasurementId);
}
