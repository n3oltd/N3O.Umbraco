using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.DonateButton)]
public class DonateButtonElementContent : DesignatableElementContent<DonateButtonElementContent> {
    public string Text => GetValue(x => x.Text);
    public decimal Amount => GetValue(x => x.Amount);
    public DonateButtonAction Action => GetValue(x => x.Action);
}