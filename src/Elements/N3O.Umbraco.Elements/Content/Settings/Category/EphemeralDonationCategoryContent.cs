using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Elements.Content;

[UmbracoContent(ElementsConstants.DonationCategory.Ephemeral.Alias)]
public class EphemeralDonationCategoryContent : UmbracoContent<EphemeralDonationCategoryContent> {
    public DateTime StartOn => GetValue(x => x.StartOn);
    public DateTime EndOn => GetValue(x => x.EndOn);
}