using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using N3O.Umbraco.Sync.Extensions.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Sync.Extensions.Handlers;

[RecurringJob("Trigger Data Sync", "*/5 * * * *")]
public class TriggerDataSyncHandler : IRequestHandler<TriggerDataSyncCommand, None, None> {
    public async Task<None> Handle(TriggerDataSyncCommand req, CancellationToken cancellationToken) {
        // TODO Check if any syncs are due, if so run their producers and then call the API
        
        throw new System.NotImplementedException();
    }
}