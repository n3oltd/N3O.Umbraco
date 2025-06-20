using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.AddressEntry.Alias)]
public class AddressEntryContent : UmbracoContent<AddressEntryContent> {
    public AddressLayout Layout => GetValue(x => x.Layout);
    public virtual string LookupApiKey => GetValue(x => x.LookupApiKey);
}