using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ElementContentExtensions {
    public static OfferingContent GetOffering(this ElementContent element, IContentLocator contentLocator) {
        if (element.Campaign.HasValue()) {
            return element.Campaign.DefaultOffering;
        } else if (element.DonationButton.HasValue(x => x.Offering) || element.DonationForm.HasValue(x => x.Offering)) {
            return element.DonationButton?.Offering ?? element.DonationForm.Offering;
        } else {
            return contentLocator.Single<PlatformsContent>().Campaigns.First().DefaultOffering;
        }
    }
}