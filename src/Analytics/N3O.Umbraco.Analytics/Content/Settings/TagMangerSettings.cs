using N3O.Umbraco.Content;

namespace N3O.Umbraco.Analytics.Content;

public class TagMangerSettings : UmbracoContent {
    public string Body => GetValue<TagMangerSettings, string>(x => x.Body);
    public string Head => GetValue<TagMangerSettings, string>(x => x.Head);
}
