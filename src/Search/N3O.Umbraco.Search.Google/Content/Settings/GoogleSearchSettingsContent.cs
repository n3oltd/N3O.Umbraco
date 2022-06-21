using N3O.Umbraco.Content;

namespace N3O.Umbraco.Search.Google.Content;

public class GoogleSearchSettingsContent : UmbracoContent<GoogleSearchSettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
    public string SearchEngineId => GetValue(x => x.SearchEngineId);
}
