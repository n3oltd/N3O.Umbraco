using N3O.Umbraco.Redirects.Commands;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Redirects.Notifications;

public class RedirectsApplicationStarted : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public RedirectsApplicationStarted(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken) {
        _backgroundJob.EnqueueCommand<PopulateUmbracoRedirectsCommand>();

        return Task.CompletedTask;
    }
}