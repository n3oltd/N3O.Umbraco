using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    public PledgeUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker) : base(asyncKeyedLocker) { }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        // TODO
        throw new NotImplementedException();
    }
}