using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.CrowdFunding.Services;
using N3O.Umbraco.Lookups;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Events;

public class PledgeUpdatedHandler : PledgeEventHandler<PledgeUpdatedEvent> {
    private readonly IOfflineContributionRepository _offlineContributionRepository;
    private readonly ILookups _lookups;

    public PledgeUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                IOfflineContributionRepository offlineContributionRepository,
                                ILookups lookups)
        : base(asyncKeyedLocker) {
        _offlineContributionRepository = offlineContributionRepository;
        _lookups = lookups;
    }

    protected override async Task HandleEventAsync(PledgeUpdatedEvent req, CancellationToken cancellationToken) {
        await _offlineContributionRepository.AddOrUpdateAsync(req.Model.Crowdfunder,
                                                              req.Model.BalanceSummary.ImportedDonationsTotal.ToMoney(_lookups),
                                                              req.Model.BalanceSummary.ImportedDonationsCount);
    }
}