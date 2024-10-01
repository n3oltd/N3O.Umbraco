using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
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
    private readonly IContentLocator _contentLocator;
    private readonly ILocalClock _localClock;

    public CleanupAbandonedFundraisersHandler(IContentService contentService,
                                              IContentLocator contentLocator,
                                              ILocalClock localClock) {
        _contentService = contentService;
        _contentLocator = contentLocator;
        _localClock = localClock;
    }

    public Task<None> Handle(CleanupAbandonedFundraisersCommand req, CancellationToken cancellationToken) {
        var allFundraisers = _contentLocator.All<FundraiserContent>();
        var thirtyDaysAgo = _localClock.GetLocalNow().Minus(Period.FromDays(30)).ToDateTimeUnspecified();
        
        var toDelete = allFundraisers.Where(x => (!x.Status.HasValue() || x.Status == CrowdfunderStatuses.Draft) &&
                                                 x.Content().UpdateDate < thirtyDaysAgo);
        
        toDelete.Do(DeleteExpiredFundraiser);

        return Task.FromResult(None.Empty);
    }
    
    private void DeleteExpiredFundraiser(FundraiserContent fundraiser) {
        var content = _contentService.GetById(fundraiser.Content().Id);
        
        _contentService.Delete(content);
    }
}