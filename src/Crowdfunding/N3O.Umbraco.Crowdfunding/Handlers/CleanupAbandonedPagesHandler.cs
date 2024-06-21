using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

[RecurringJob("Cleanup Abandoned Pages", "0 0 * * 0")]
public class CleanupAbandonedPagesHandler : IRequestHandler<CleanupAbandonedPagesCommand, None, None> {
    // TODO We should be searching here for any pages that have been created but the user did not complete the
    // process of publishing the page. This is so we aren't left with lots of orphaned pages in the system. We
    // will be storing a property "SetupComplete" to indicate if the page has ever completed the set-up process
    // and will be removing pages where this is false and the page was last saved
    // more than 30 days ago.
    public async Task<None> Handle(CleanupAbandonedPagesCommand req, CancellationToken cancellationToken) {
        return None.Empty;
    }
}