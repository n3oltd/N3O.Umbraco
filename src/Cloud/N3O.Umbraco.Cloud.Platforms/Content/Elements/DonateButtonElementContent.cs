using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.DonateButton)]
public class DonateButtonElementContent : UmbracoContent<DonateButtonElementContent> {
    public string Text => GetValue(x => x.Text);
    public decimal Amount => GetValue(x => x.Amount);
    public DonateButtonAction Action => GetValue(x => x.Action);
}