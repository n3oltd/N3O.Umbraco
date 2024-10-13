using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public abstract class PledgeEventHandler<TEvent> : IRequestHandler<TEvent, WebhookPledge, None> 
    where TEvent : PledgeEvent {
    private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;
    private readonly IContentLocator _contentLocator;
    
    protected PledgeEventHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                 IContentLocator contentLocator) {
        _contentLocator = contentLocator;
        _asyncKeyedLocker = asyncKeyedLocker;
    }

    public async Task<None> Handle(TEvent req, CancellationToken cancellationToken) {
        using (await _asyncKeyedLocker.LockAsync(req.Model.Revision.Id.ToString(), cancellationToken)) {
            await HandleEventAsync(req, cancellationToken);
        }

        return None.Empty;
    }
    
    protected abstract Task HandleEventAsync(TEvent req, CancellationToken cancellationToken);
}