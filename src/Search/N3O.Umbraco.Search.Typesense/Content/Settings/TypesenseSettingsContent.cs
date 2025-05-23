using N3O.Umbraco.Content;

namespace N3O.Umbraco.Search.Typesense.Content.Settings;

public class TypesenseSettingsContent : UmbracoContent<TypesenseSettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
    public string Node => GetValue(x => x.Node);
}
