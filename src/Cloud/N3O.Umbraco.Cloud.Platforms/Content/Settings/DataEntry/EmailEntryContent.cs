using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.EmailEntry.Alias)]
public class EmailEntryContent : UmbracoContent<EmailEntryContent> {
    public bool Required => GetValue(x => x.Required);
    public bool Validate => GetValue(x => x.Validate);
}