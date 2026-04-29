using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormStates.Qurbani)]
public class QurbaniDonationFormStateContent : UmbracoContent<QurbaniDonationFormStateContent> {
    public DonationItem DonationItem => GetValue(x => x.DonationItem);
}