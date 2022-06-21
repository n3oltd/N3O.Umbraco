using N3O.Umbraco.Content;

namespace N3O.Umbraco.Search.Algolia.Content;

public class AlgoliaSettingsContent : UmbracoContent<AlgoliaSettingsContent> {
    public string AdminApiKey => GetValue(x => x.AdminApiKey);
    public string ApplicationId => GetValue(x => x.ApplicationId);
    public string SearchOnlyApiKey => GetValue(x => x.SearchOnlyApiKey);
}
