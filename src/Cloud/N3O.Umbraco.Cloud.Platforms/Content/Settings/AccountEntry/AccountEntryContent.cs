using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.AccountEntry.Alias)]
public class AccountEntryContent : UmbracoContent<AccountEntryContent> {
    public AddressEntryContent Address => Content().GetSingleChildOfTypeAs<AddressEntryContent>();
    public ConsentEntryContent Consent => Content().GetSingleChildOfTypeAs<ConsentEntryContent>();
}