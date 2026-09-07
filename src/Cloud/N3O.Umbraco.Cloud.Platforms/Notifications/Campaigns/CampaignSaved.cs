using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CampaignSaved : INotificationAsyncHandler<ContentSavedNotification> {
    private readonly Lazy<IContentEditor> _contentEditor;
    private readonly IContentHelper _contentHelper;
    private readonly IContentService _contentService;
    private readonly IContentTypeService _contentTypeService;

    public CampaignSaved(Lazy<IContentEditor> contentEditor,
                         IContentHelper contentHelper,
                         IContentService contentService,
                         IContentTypeService contentTypeService) {
        _contentEditor = contentEditor;
        _contentHelper = contentHelper;
        _contentService = contentService;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentSavedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.IsCampaign(_contentTypeService)) {
                SyncCrowdfunderNames(content.Key, content.Name);
            }
        }

        return Task.CompletedTask;
    }

    private void SyncCrowdfunderNames(Guid campaignKey, string campaignName) {
        foreach (var crowdfunder in _contentService.GetCrowdfunders(_contentTypeService)) {
            if (crowdfunder.GetCrowdfunderCampaignKey(_contentHelper) != campaignKey ||
                crowdfunder.Name.EqualsInvariant(campaignName)) {
                continue;
            }

            var contentPublisher = _contentEditor.Value.ForExisting(crowdfunder.Key);

            contentPublisher.SetName(campaignName);

            if (crowdfunder.Published) {
                contentPublisher.SaveAndPublish();
            } else {
                contentPublisher.SaveUnpublished();
            }
        }
    }
}
