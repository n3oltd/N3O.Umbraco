using N3O.Umbraco.Content;

namespace N3O.Umbraco.Elements.Content;

public class PreferencesDataEntrySettingsContent : UmbracoContent<PreferencesDataEntrySettingsContent> {
    public string PrivacyText => GetValue(x => x.PrivacyText);
    public string PreferenceText => GetValue(x => x.PreferenceText);
    public bool SkipNoPreferences => GetValue(x => x.SkipNoPreferences);
}