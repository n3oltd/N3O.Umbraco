using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Lookups;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedHandler : CrowdfunderEventHandler<CrowdfunderCreatedEvent> {
    public CrowdfunderCreatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IContentLocator contentLocator,
                                     ICrowdfunderRevisionRepository crowdfunderRevisionRepository,
                                     ILookups lookups)
        : base(asyncKeyedLocker, contentService, contentLocator, crowdfunderRevisionRepository, lookups) { }

    protected override async Task HandleEventAsync(CrowdfunderCreatedEvent req, CancellationToken cancellationToken) {
        var content = GetContent(req.Model.Id);
        var crowdfunderType = content.ContentType.Alias.ToCrowdfunderType();

        await AddOrUpdateCrowdfunderRevisionAsync(content, crowdfunderType, req.Model.Status.Name);

        UpdateAndPublishStatus(content, req.Model.Status.Name);
    }
}