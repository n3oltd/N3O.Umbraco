using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.DonateButton)]
public class DonateButtonElementContent : UmbracoContent<DonateButtonElementContent> {
    public string Label => GetValue(x => x.Label);
}