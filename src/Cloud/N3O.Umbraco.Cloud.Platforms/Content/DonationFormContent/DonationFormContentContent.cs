using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormContent.CompositionAlias)]
public class DonationFormContentContent : UmbracoContent<DonationFormContentContent> {
    public MediaWithCrops Image => GetValue(x => x.Image);
    public MediaWithCrops Icon => GetValue(x => x.Icon);
    public IHtmlEncodedString Description => GetValue(x => x.Description);
    public string Summary => GetValue(x => x.Summary);
}
