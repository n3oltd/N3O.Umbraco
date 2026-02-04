using Flurl;
using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Slugify;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class ContentLocatorExtensions {
    public static string GetCampaignPath(this IContentCache contentCache,
                                         ISlugHelper slugHelper,
                                         string name) {
        var campaignPage = contentCache.Special(PlatformsSpecialPages.Campaign);

        if (campaignPage.HasValue()) {
            var campaignSlug = slugHelper.GenerateSlug(name);
            var campaignUrl = new Url(campaignPage.RelativeUrl());

            campaignUrl.RemovePathSegment();
            campaignUrl.AppendPathSegment(campaignSlug);

            return campaignUrl;
        } else {
            return null;
        }
    }
    
    public static string GetCampaignUrl(this IContentCache contentCache,
                                        ISlugHelper slugHelper,
                                        IWebHostEnvironment webHostEnvironment,
                                        string name) {
        var campaignPath = GetCampaignPath(contentCache, slugHelper, name);
        var baseUrl = contentCache.Single<UrlSettingsContent>().BaseUrl(webHostEnvironment);

        if (campaignPath.HasValue()) {
            var campaignUrl = new Url(baseUrl);

            campaignUrl.AppendPathSegment(campaignPath);

            return campaignUrl;
        } else {
            return null;
        }
    }
    
    public static string GetOfferingPath(this IContentCache contentCache,
                                         ISlugHelper slugHelper,
                                         string campaignName,
                                         string offeringName) {
        var campaignUrl = GetCampaignPath(contentCache, slugHelper, campaignName);

        if (campaignUrl.HasValue()) {
            var offeringSlug = slugHelper.GenerateSlug(offeringName);
            var offeringUrl = new Url(campaignUrl);

            offeringUrl.AppendPathSegment(offeringSlug);

            return offeringUrl;
        } else {
            return null;
        }
    }
    
    public static string GetOfferingUrl(this IContentCache contentCache,
                                        ISlugHelper slugHelper,
                                        IWebHostEnvironment webHostEnvironment,
                                        string campaignName,
                                        string offeringName) {
        var offeringPath = GetOfferingPath(contentCache, slugHelper, campaignName, offeringName);
        var baseUrl = contentCache.Single<UrlSettingsContent>().BaseUrl(webHostEnvironment);

        if (offeringPath.HasValue()) {
            var offeringUrl = new Url(baseUrl);

            offeringUrl.AppendPathSegment(offeringPath);

            return offeringUrl;
        } else {
            return null;
        }
    }
}
