using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DataEntry.Alias)]
public class DataEntryContent : UmbracoContent<DataEntryContent> {
    public AddressEntryContent Address => Content().GetSingleChildOfTypeAs<AddressEntryContent>();
    public ConsentEntryContent Consent => Content().GetSingleChildOfTypeAs<ConsentEntryContent>();
    public EmailEntryContent Email => Content().GetSingleChildOfTypeAs<EmailEntryContent>();
    public NameEntryContent Name => Content().GetSingleChildOfTypeAs<NameEntryContent>();
    public TelephoneEntryContent Telephone => Content().GetSingleChildOfTypeAs<TelephoneEntryContent>();
}