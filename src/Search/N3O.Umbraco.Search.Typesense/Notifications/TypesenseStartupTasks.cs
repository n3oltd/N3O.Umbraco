using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Search.Typesense.Commands;
using System.Threading;
using System.Threading.Tasks;
using Typesense;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace N3O.Umbraco.Search.Typesense.Notifications;

public class TypesenseStartupTasks : INotificationAsyncHandler<UmbracoApplicationStartedNotification> {
    private const string JobName = "MigrateCollections";

    private readonly ITypesenseClient _typesenseClient;
    private readonly IBackgroundJob _backgroundJob;

    public TypesenseStartupTasks(ITypesenseClient typesenseClient, IBackgroundJob backgroundJob) {
        _typesenseClient = typesenseClient;
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken) {
        if (_typesenseClient.HasValue()) {
            _backgroundJob.Enqueue<MigrateCollectionsCommand>(JobName);
        }

        return Task.CompletedTask;
    }
}
