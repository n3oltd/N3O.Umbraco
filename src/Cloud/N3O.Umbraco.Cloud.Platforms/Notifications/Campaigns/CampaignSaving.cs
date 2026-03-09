using N3O.Umbraco.Cloud.Platforms.Content;
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

public class CampaignSaving : INotificationAsyncHandler<ContentSavingNotification> {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IContentTypeService _contentTypeService;
    
    public CampaignSaving(Lazy<IContentLocator> contentLocator, IContentTypeService contentTypeService) {
        _contentLocator = contentLocator;
        _contentTypeService = contentTypeService;
    }

    public Task HandleAsync(ContentSavingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.SavedEntities) {
            if (content.IsCampaign(_contentTypeService)) {
                var crowdfundingEnabled = content.GetValue<bool>(AliasHelper<CrowdfundingCampaignContent>.PropertyAlias(x => x.CrowdfundingEnabled));

                if (crowdfundingEnabled) {
                    var offerings = _contentLocator.Value.All(x => x.Parent?.Id == content.Id).As<OfferingContent>();

                    if (offerings.None(x => x.AllowCrowdfunding)) {
                        notification.CancelWithError("Crowdfunding cannot be enabled as no offering available for crowdfunding");
                    }
                }
            }
        }
        
        return Task.CompletedTask;
    }
}
