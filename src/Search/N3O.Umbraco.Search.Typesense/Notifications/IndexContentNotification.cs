using Microsoft.Extensions.Logging;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Commands;
using N3O.Umbraco.Search.Typesense.NamedParameters;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Typesense.Notifications;

public class IndexContentNotification : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public IndexContentNotification(IBackgroundJob backgroundJob, ILogger<IndexContentNotification> logger) {
        _backgroundJob = backgroundJob;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            _backgroundJob.EnqueueCommand<IndexContentCommand>(m => m.Add<ContentId>(content.Key.ToString()));
        }

        return Task.CompletedTask;
    }
}