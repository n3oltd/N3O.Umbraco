using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    public PledgeUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker) : base(asyncKeyedLocker) { }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        // TODO Talha
        // Update the webhook donation model to include the email, reference and name
        // Insert these into the contributions table via repository (remember to delete existing entries)
        // Use allocation reference for transaction reference
        // If the donation is anonymous then put name as _formatter("Anonymous") and email as _formatter("anonymous") (lowercase)
        // We also need to update the crowdfunders table to update the non donations balance.
        throw new NotImplementedException();
    }
}