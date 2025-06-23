using N3O.Umbraco.Accounts.Lookups;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.DataEntry.Name)]
public class NameEntryContent : UmbracoContent<NameEntryContent> {
    public NameLayout Layout => GetValue(x => x.Layout);
}