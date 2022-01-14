using N3O.Umbraco.Content;

namespace N3O.Umbraco.Analytics.Content {
    public class TagManagerSettings : UmbracoContent<TagManagerSettings> {
        public string Body => GetValue(x => x.Body);
        public string Head => GetValue(x => x.Head);
    }
}
