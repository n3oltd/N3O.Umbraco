using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Search.Commands;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Notifications;

public class SitemapApplicationStarted : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public SitemapApplicationStarted(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken) {
        _backgroundJob.Schedule<GenerateSitemapCommand>($"{nameof(GenerateSitemapCommand)}".Replace("Command", ""),
                                                        Duration.FromSeconds(20));

        return Task.CompletedTask;
    }
}