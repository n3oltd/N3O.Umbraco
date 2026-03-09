using N3O.Umbraco.Attributes;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Typesense.Commands;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Typesense.Notifications;

[SkipDuringSync]
public class IndexContentOnPublish : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public IndexContentOnPublish(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }
    
    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            _backgroundJob.EnqueueCommand<IndexContentCommand>(m => m.Add<ContentId>(content.Key.ToString()),
                                                               content.Key);
        }

        return Task.CompletedTask;
    }
}