using N3O.Umbraco.Content;

namespace N3O.Umbraco.Search.Google.Content {
    public class GoogleSearchSettings : UmbracoContent {
        public string ApiKey => GetValue<GoogleSearchSettings, string>(x => x.ApiKey);
        public string SearchEngineId => GetValue<GoogleSearchSettings, string>(x => x.SearchEngineId);
    }
}
