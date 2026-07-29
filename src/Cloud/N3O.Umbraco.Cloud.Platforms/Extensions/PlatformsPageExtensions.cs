using Flurl;
using Humanizer;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class PlatformsPageExtensions {
    public static string AbsoluteUrl(this PlatformsPage page, IUrlBuilder urlBuilder) {
        if (!page.HasValue(x => x.Url)) {
            return null;
        }

        var rootUrl = urlBuilder.Root();
        var url = new Url(page.Url.AbsolutePath);

        url.Scheme = rootUrl.Scheme;
        url.Host = rootUrl.Host;
        url.Port = rootUrl.Port;

        return url;
    }

    public static string GetCampaignId(this PlatformsPage page) {
        if (page.Kind == PublishedFileKinds.CampaignPage) {
            return page.Content[nameof(PublishedCampaignPage.Campaign).Camelize()]?[nameof(PublishedCampaignPage.Campaign.Id).Camelize()]?.ToString();
        } else if (page.Kind == PublishedFileKinds.OfferingPage) {
            return page.Content[nameof(PublishedOfferingPage.Offering).Camelize()]?[nameof(PublishedOfferingPage.Offering.Campaign).Camelize()]?[nameof(PublishedOfferingPage.Offering.Campaign.Id).Camelize()]?.ToString();
        } else if (page.Kind == PublishedFileKinds.CrowdfunderPage) {
            return page.Content[nameof(PublishedCrowdfunderPage.Crowdfunder).Camelize()]?[nameof(PublishedCrowdfunderPage.Crowdfunder.CampaignId).Camelize()]?.ToString();
        } else {
            throw UnrecognisedValueException.For(page.Kind);
        }
    }

    public static string GetOfferingId(this PlatformsPage page) {
        if (page.Kind == PublishedFileKinds.OfferingPage) {
            return page.Content[nameof(PublishedOfferingPage.Offering).Camelize()]?[nameof(PublishedOfferingPage.Offering.Id).Camelize()]?.ToString();
        } else {
            throw UnrecognisedValueException.For(page.Kind);
        }
    }
}