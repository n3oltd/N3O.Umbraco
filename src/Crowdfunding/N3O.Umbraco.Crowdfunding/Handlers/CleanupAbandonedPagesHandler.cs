using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Crowdfunding.Lookups;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler.Attributes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

[RecurringJob("Cleanup Abandoned Pages", "0 0 * * 0")]
public class CleanupAbandonedPagesHandler : IRequestHandler<CleanupAbandonedPagesCommand, None, None> {
    private readonly IContentService _contentService;
    private readonly IFundraisingPages _fundraisingPages;
    
    public CleanupAbandonedPagesHandler(IContentService contentService, IFundraisingPages fundraisingPages) {
        _contentService = contentService;
        _fundraisingPages = fundraisingPages;
    }

    public Task<None> Handle(CleanupAbandonedPagesCommand req, CancellationToken cancellationToken) {
        var pages = _fundraisingPages.GetAllFundraisingPages();
        
        var toDelete = pages.Where(x => x.PageStatus == CrowdfundingPageStatuses.Pending &&
                                        x.Content().UpdateDate < DateTime.Now.AddDays(-30));
        
        toDelete.Do(RemoveExpiredPage);

        return Task.FromResult(None.Empty);
    }
    
    private void RemoveExpiredPage(CrowdfundingPageContent page) {
        var pageContent = _contentService.GetById(page.Content().Id);
        
        _contentService.Delete(pageContent);
    }
}