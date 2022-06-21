using N3O.Umbraco.Content;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Checkout.Content;

public class GivingSettingsContent : UmbracoContent<GivingSettingsContent> {
    public IEnumerable<IPublishedContent> DonationPaymentMethods => GetPickedAs(x => x.DonationPaymentMethods);
    public IEnumerable<IPublishedContent> RegularGivingPaymentMethods => GetPickedAs(x => x.RegularGivingPaymentMethods);
}
