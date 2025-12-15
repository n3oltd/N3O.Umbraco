using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Search.Commands;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Notifications;

public class SitemapContentPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public SitemapContentPublished(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        _backgroundJob.EnqueueCommand<GenerateSitemapCommand>();
        
        return Task.CompletedTask;
    }
}