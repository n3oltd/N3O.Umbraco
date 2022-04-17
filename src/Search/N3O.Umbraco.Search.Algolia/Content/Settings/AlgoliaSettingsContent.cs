using N3O.Umbraco.Content;

namespace N3O.Umbraco.Search.Algolia.Content {
    public class AlgoliaSettingsContent : UmbracoContent<AlgoliaSettingsContent> {
        public string ApiKey => GetValue(x => x.ApiKey);
        public string ApplicationId => GetValue(x => x.ApplicationId);
    }
}
