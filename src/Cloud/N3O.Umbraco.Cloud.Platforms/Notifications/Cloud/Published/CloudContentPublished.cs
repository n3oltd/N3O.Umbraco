using Microsoft.Extensions.Logging;
using N3O.Umbraco.Scheduler;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public abstract class CloudContentPublished : CloudContentNotification, INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly ILogger<CloudContentPublished> _logger;

    protected CloudContentPublished(ISubscriptionAccessor subscriptionAccessor,
                                    ICloudUrl cloudUrl,
                                    IBackgroundJob backgroundJob,
                                    ILogger<CloudContentPublished> logger) 
        : base(subscriptionAccessor, cloudUrl, backgroundJob) {
        _logger = logger;
    }

    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (CanProcess(content)) {
                try {
                    var body = GetBody(content);

                    Enqueue(body);
                } catch (Exception e) {
                    _logger.LogError(e, "Error mapping content");
                    
                    throw;
                }
            }
        }
        
        return Task.CompletedTask;
    }

    protected abstract object GetBody(IContent content);
    protected abstract bool CanProcess(IContent content);
    protected abstract override string HookId { get; }
}