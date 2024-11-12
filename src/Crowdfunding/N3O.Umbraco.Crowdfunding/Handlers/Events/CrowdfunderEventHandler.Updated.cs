using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedUpdatedHandler : CrowdfunderEventHandler<CrowdfunderUpdatedEvent> {
    public CrowdfunderUpdatedUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                            IContentService contentService,
                                            IContentLocator contentLocator,
                                            ICrowdfunderRevisionRepository crowdfunderRevisionRepository)
        : base(asyncKeyedLocker, contentService, contentLocator, crowdfunderRevisionRepository) { }

    protected override async Task HandleEventAsync(CrowdfunderUpdatedEvent req, CancellationToken cancellationToken) {
        var content = GetContent(req.Model.Id);
        var type = content.ContentType.Alias.ToCrowdfunderType();

        await AddOrUpdateRevisionAsync(content.Key, content.VersionId, type);

        UpdateAndPublishStatus(content, req.Model.Status.Name);
    }
}