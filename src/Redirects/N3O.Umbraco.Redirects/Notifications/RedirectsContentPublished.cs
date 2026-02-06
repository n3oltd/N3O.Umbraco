using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Redirects.Commands;
using N3O.Umbraco.Redirects.Content;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Redirects.Notifications;

public class RedirectsContentPublished : INotificationAsyncHandler<ContentPublishedNotification> {
    private static readonly string Alias = AliasHelper<RedirectContent>.ContentTypeAlias();
    
    private readonly IBackgroundJob _backgroundJob;

    public RedirectsContentPublished(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken) {
        foreach (var content in notification.PublishedEntities) {
            if (content.ContentType.Alias.EqualsInvariant(Alias)) {
                _backgroundJob.EnqueueCommand<PopulateUmbracoRedirectsCommand>();
            }
        }

        return Task.CompletedTask;
    }
}