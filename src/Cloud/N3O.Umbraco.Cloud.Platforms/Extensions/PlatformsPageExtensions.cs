using Humanizer;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Models;
using N3O.Umbraco.Exceptions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class PlatformsPageExtensions {
    public static string GetCampaignId(this PlatformsPage page) {
        if (page.Kind == PublishedFileKinds.CampaignPage) {
            return page.Content[nameof(PublishedCampaignPage.Campaign).Camelize()][nameof(PublishedCampaignPage.Campaign.Id).Camelize()].ToString();
        } else if (page.Kind == PublishedFileKinds.OfferingPage) {
            return page.Content[nameof(PublishedOfferingPage.Offering).Camelize()][nameof(PublishedOfferingPage.Offering.CampaignId).Camelize()].ToString();
        } else if (page.Kind == PublishedFileKinds.CrowdfunderPage) {
            return page.Content[nameof(PublishedCrowdfunderPage.Crowdfunder).Camelize()][nameof(PublishedCrowdfunderPage.Crowdfunder.CampaignId).Camelize()].ToString();
        } else {
            throw UnrecognisedValueException.For(page.Kind);
        }
    }
    
    public static string GetOfferingId(this PlatformsPage page) {
        if (page.Kind == PublishedFileKinds.OfferingPage) {
            return page.Content[nameof(PublishedOfferingPage.Offering).Camelize()][nameof(PublishedOfferingPage.Offering.Id).Camelize()].ToString();
        } else {
            throw UnrecognisedValueException.For(page.Kind);
        }
    }
    
    public static string GetTitle(this PlatformsPage page) {
        if (page.Kind == PublishedFileKinds.CampaignPage) {
            return page.Content[nameof(PublishedCampaignPage.Campaign).Camelize()][nameof(PublishedCampaignPage.Campaign.Name).Camelize()].ToString();
        } else if (page.Kind == PublishedFileKinds.OfferingPage) {
            return page.Content[nameof(PublishedOfferingPage.Offering).Camelize()][nameof(PublishedOfferingPage.Offering.Name).Camelize()].ToString();
        } else if (page.Kind == PublishedFileKinds.CrowdfunderPage) {
            return page.Content[nameof(PublishedCrowdfunderPage.Crowdfunder).Camelize()][nameof(PublishedCrowdfunderPage.Crowdfunder.Name).Camelize()].ToString();
        } else {
            throw UnrecognisedValueException.For(page.Kind);
        }
    }
}