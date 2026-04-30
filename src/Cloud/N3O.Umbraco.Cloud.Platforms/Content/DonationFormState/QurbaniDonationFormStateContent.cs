using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.DonationFormState.Qurbani)]
public class QurbaniDonationFormStateContent : UmbracoContent<QurbaniDonationFormStateContent> {
    public QurbaniItem QurbaniItem => GetValue(x => x.QurbaniItem);
    public IEnumerable<QurbaniSeasonCategoryContent> Categories => GetPickedAs(x => x.Categories);
}