using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.AccountEntry.Consent)]
public class ConsentEntryContent : UmbracoContent<ConsentEntryContent> {
    public string ConsentText => GetValue(x => x.ConsentText);
    public string PrivacyText => GetValue(x => x.PrivacyText);
    public Link PrivacyUrl => GetValue(x => x.PrivacyUrl);
}