using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CrowdfundingCampaignSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;
    private readonly IContentHelper _contentHelper;
    private readonly IContentTypeService _contentTypeService;
    private readonly ILookups _lookups;

    public CrowdfundingCampaignSaving(IContentService contentService,
                                      IContentLocator contentLocator,
                                      IContentTypeService contentTypeService,
                                      ILookups lookups,
                                      IContentHelper contentHelper) {
        _contentService = contentService;
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
        _lookups = lookups;
        _contentHelper = contentHelper;
    }

    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (!content.IsCrowdfundingCampaign()) {
                continue;
            }

            var campaignKey = content.GetCampaignKey(_contentHelper);

            if (campaignKey == null) {
                continue;
            }

            var campaignId = campaignKey.Value.ToString();
            var offeringIds = _lookups.GetAll<Offering>()
                                      .Where(x => x.CampaignId == campaignId)
                                      .Select(x => x.Id)
                                      .ToList();

            if (_contentLocator.All(x => x.IsOffering(AliasHelper<OfferingContent>.ContentTypeAlias()))
                               .As<OfferingContent>()
                               .Where(x => offeringIds.Contains(x.Key.ToString()))
                               .None(x => x.AllowCrowdfunding)) {
                notification.CancelWithError("No offering allows crowdfunding for the selected campaign");

                continue;
            }

            if (AnotherCrowdfundingCampaignExistsFor(content, campaignKey.Value)) {
                notification.CancelWithError("This campaign already has a crowdfunding campaign");

                continue;
            }

            var campaign = _contentService.GetById(campaignKey.Value);

            if (campaign != null) {
                content.Name = campaign.Name;
            }
        }

        return Task.CompletedTask;
    }

    private bool AnotherCrowdfundingCampaignExistsFor(IContent crowdfundingCampaign, Guid campaignKey) {
        return _contentService.GetCrowdfundingCampaigns(_contentTypeService)
                              .Any(x => x.Key != crowdfundingCampaign.Key &&
                                        x.GetCampaignKey(_contentHelper) == campaignKey);
    }
}