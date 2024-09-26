using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Handlers;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedUpdatedHandler : CrowdfunderEventHandler<CrowdfunderUpdatedEvent> {
    public CrowdfunderUpdatedUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                            IContentService contentService) 
        : base(asyncKeyedLocker, contentService) { }

    protected override Task HandleEventAsync(CrowdfunderUpdatedEvent req, CancellationToken cancellationToken) {
        UpdateAndPublishStatus(req.Model.Id, req.Model.Status.Name);
        
        return Task.FromResult(None.Empty);
    }
}