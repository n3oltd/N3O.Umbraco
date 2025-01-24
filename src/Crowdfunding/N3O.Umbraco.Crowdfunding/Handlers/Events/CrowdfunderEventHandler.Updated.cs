using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedUpdatedHandler : CrowdfunderEventHandler<CrowdfunderUpdatedEvent> {
    private readonly IContentService _contentService;

    public CrowdfunderUpdatedUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                            IContentService contentService,
                                            IContentLocator contentLocator,
                                            ICrowdfunderRevisionRepository crowdfunderRevisionRepository)
        : base(asyncKeyedLocker, contentService, contentLocator, crowdfunderRevisionRepository) {
        _contentService = contentService;
    }

    protected override async Task HandleEventAsync(CrowdfunderUpdatedEvent req,
                                                   IContent content,
                                                   CancellationToken cancellationToken) {
        var type = content.ContentType.Alias.ToCrowdfunderType();

        await AddOrUpdateRevisionAsync(content.Key, content.VersionId, type);

        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, req.Model.CrowdfunderInfo.Status.Name);
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
    }
}