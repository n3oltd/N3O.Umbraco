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

[RecurringJob("Cleanup Abandoned Pages", "0 0 * * 0")]
public class CleanupAbandonedPagesHandler : IRequestHandler<CleanupAbandonedPagesCommand, None, None> {
    private readonly IContentService _contentService;
    private readonly IFundraisingPages _fundraisingPages;
    private readonly ILocalClock _localClock;

    public CleanupAbandonedPagesHandler(IContentService contentService,
                                        IFundraisingPages fundraisingPages,
                                        ILocalClock localClock) {
        _contentService = contentService;
        _fundraisingPages = fundraisingPages;
        _localClock = localClock;
    }

    public Task<None> Handle(CleanupAbandonedPagesCommand req, CancellationToken cancellationToken) {
        var pages = _fundraisingPages.GetAllFundraisingPages();
        var thirtyDaysAgo = _localClock.GetLocalNow().Minus(Period.FromDays(30)).ToDateTimeUnspecified();
        
        var toDelete = pages.Where(x => x.PageStatus == CrowdfundingPageStatuses.Pending &&
                                        x.Content().UpdateDate < thirtyDaysAgo);
        
        toDelete.Do(DeleteExpiredPage);

        return Task.FromResult(None.Empty);
    }
    
    private void DeleteExpiredPage(CrowdfundingPageContent page) {
        var pageContent = _contentService.GetById(page.Content().Id);
        
        _contentService.Delete(pageContent);
    }
}