using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Elements.Content;

public class EphemeralDonationCategoryContent : UmbracoContent<EphemeralDonationCategoryContent> {
    public DateTime StartOn => GetValue(x => x.StartOn);
    public DateTime EndOn => GetValue(x => x.EndOn);
}