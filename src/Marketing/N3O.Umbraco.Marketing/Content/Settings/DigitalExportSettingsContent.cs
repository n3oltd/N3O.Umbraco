using N3O.Umbraco.Content;

namespace N3O.Umbraco.Marketing.Content;

public class DigitalExportSettingsContent : UmbracoContent<DigitalExportSettingsContent> {
    public string ExportKey => GetValue(x => x.ExportKey);
}
