using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.DonationButton)]
public class DonationButtonElementContent : DonationElementContent<DonationButtonElementContent> {
    public string Text => GetValue(x => x.Text);
    public decimal Amount => GetValue(x => x.Amount);
    public DonationButtonAction Action => GetValue(x => x.Action);
}