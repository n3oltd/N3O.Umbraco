using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.CrowdFunding.Services;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeTransactionUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    private readonly IOfflineContributionsRepository _offlineContributionsRepository;
    
    public PledgeTransactionUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                           IOfflineContributionsRepository offlineContributionsRepository) : base(asyncKeyedLocker) {
        _offlineContributionsRepository = offlineContributionsRepository;
    }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        await _offlineContributionsRepository.AddOrUpdateAsync(req.Model);
    }
}