using N3O.Umbraco.Cloud.Platforms.Commands;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using NodaTime;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class PublishedCompositionsApplicationStarted
    : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public PublishedCompositionsApplicationStarted(IBackgroundJob backgroundJob) {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken) {
        _backgroundJob.Schedule<GeneratePublishedCompositionsCommand>($"{nameof(GeneratePublishedCompositionsCommand)}".Replace("Command", ""),
                                                                      Duration.FromSeconds(30));

        return Task.CompletedTask;
    }
}