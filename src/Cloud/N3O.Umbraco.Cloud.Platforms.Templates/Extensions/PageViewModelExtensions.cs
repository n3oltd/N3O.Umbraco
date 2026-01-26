using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Templates.Models;
using N3O.Umbraco.Pages;
using N3O.Umbraco.Templates.Extensions;
using System;

namespace N3O.Umbraco.Cloud.Platforms.Templates.Extensions;

public static class PageViewModelExtensions {
    public static NisabMergeModel Nisab(this IPageViewModel pageViewModel) {
        return pageViewModel.MergeModel<NisabMergeModel>(PlatformsTemplateConstants.ModelKeys.Nisab);
    }
    
    public static PublishedCampaign Campaign(this IPageViewModel pageViewModel) {
        return PublishedFile<PublishedCampaign, PlatformsPublishedFileKind>(pageViewModel,
                                                                            PlatformsPublishedFileKind.Campaign);
    }

    public static PublishedCampaignPage CampaignPage(this IPageViewModel pageViewModel) {
        return PublishedFile<PublishedCampaignPage, PlatformsPublishedFileKind>(pageViewModel,
                                                                            PlatformsPublishedFileKind.CampaignPage);
    }
    
    public static PublishedCampaigns Campaigns(this IPageViewModel pageViewModel) {
        return pageViewModel.MergeModel<PublishedCampaigns>(PlatformsTemplateConstants.ModelKeys.Campaigns);
    }
    
    public static PublishedCrowdfunderPage CrowdfunderPage(this IPageViewModel pageViewModel) {
        return PublishedFile<PublishedCrowdfunderPage, CrowdfundingPublishedFileKind>(pageViewModel,
                                                                                      CrowdfundingPublishedFileKind.CrowdfunderPage);
    }
    
    public static PublishedOffering Offering(this IPageViewModel pageViewModel) {
        return PublishedFile<PublishedOffering, PlatformsPublishedFileKind>(pageViewModel,
                                                                            PlatformsPublishedFileKind.Offering);
    }
    
    public static PublishedOfferingPage OfferingPage(this IPageViewModel pageViewModel) {
        return PublishedFile<PublishedOfferingPage, PlatformsPublishedFileKind>(pageViewModel,
                                                                                PlatformsPublishedFileKind.OfferingPage);
    }
    
    public static T PublishedFile<T, U>(this IPageViewModel pageViewModel, U kind) where U : struct, Enum {
        return pageViewModel.MergeModel<T>(kind.ToEnumString());
    }
    
    public static PlatformsUserRes User(this IPageViewModel pageViewModel) {
        return pageViewModel.MergeModel<PlatformsUserRes>(PlatformsTemplateConstants.ModelKeys.User);
    }
}
