using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class CampaignPublished : INotificationAsyncHandler<ContentPublishingNotification> {
    private readonly IContentTypeService _contentTypeService;
    private readonly Lazy<IBackgroundJob> _backgroundJob;

    public CampaignPublished(IContentTypeService contentTypeService, Lazy<IBackgroundJob> backgroundJob) {
        _contentTypeService = contentTypeService;
        _backgroundJob = backgroundJob;
    }
    
    public Task HandleAsync(ContentPublishingNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.IsCampaign(_contentTypeService)) {
                _backgroundJob.Value.EnqueueCommand<CreateDefaultElementForCampaignCommand>(m => m.Add<ContentId>(content.Key.ToString()));
            }
        }
        
        return Task.CompletedTask;
    }
    
   
}