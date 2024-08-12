using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using NodaTime;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

[RecurringJob("Cleanup Abandoned Fundraisers", "0 0 * * 0")]
public class CleanupAbandonedFundraisersHandler : IRequestHandler<CleanupAbandonedFundraisersCommand, None, None> {
    private readonly IContentService _contentService;
    private readonly ICrowdfundingHelper _crowdfundingHelper;
    private readonly ILocalClock _localClock;

    public CleanupAbandonedFundraisersHandler(IContentService contentService,
                                              ICrowdfundingHelper crowdfundingHelper,
                                              ILocalClock localClock) {
        _contentService = contentService;
        _crowdfundingHelper = crowdfundingHelper;
        _localClock = localClock;
    }

    public Task<None> Handle(CleanupAbandonedFundraisersCommand req, CancellationToken cancellationToken) {
        var fundraisers = _crowdfundingHelper.GetAllFundraisers();
        var thirtyDaysAgo = _localClock.GetLocalNow().Minus(Period.FromDays(30)).ToDateTimeUnspecified();
        
        var toDelete = fundraisers.Where(x => x.Status == FundraiserStatuses.Pending &&
                                              x.Content().UpdateDate < thirtyDaysAgo);
        
        toDelete.Do(DeleteExpiredFundraiser);

        return Task.FromResult(None.Empty);
    }
    
    private void DeleteExpiredFundraiser(FundraiserContent fundraiser) {
        var content = _contentService.GetById(fundraiser.Content().Id);
        
        _contentService.Delete(content);
    }
}