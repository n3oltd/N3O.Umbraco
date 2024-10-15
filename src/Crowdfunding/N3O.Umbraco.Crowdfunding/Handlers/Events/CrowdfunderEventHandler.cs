using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public abstract class CrowdfunderEventHandler<TEvent> : IRequestHandler<TEvent, WebhookCrowdfunderInfo, None>
    where TEvent : CrowdfunderEvent {
    private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRevisionRepository _crowdfunderRevisionRepository;
    private readonly ILookups _lookups;

    protected CrowdfunderEventHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                      IContentService contentService,
                                      IContentLocator contentLocator,
                                      ICrowdfunderRevisionRepository crowdfunderRevisionRepository,
                                      ILookups lookups) {
        _asyncKeyedLocker = asyncKeyedLocker;
        _contentService = contentService;
        _contentLocator = contentLocator;
        _crowdfunderRevisionRepository = crowdfunderRevisionRepository;
        _lookups = lookups;
    }

    public async Task<None> Handle(TEvent req, CancellationToken cancellationToken) {
        using (await _asyncKeyedLocker.LockAsync(req.Model.Id.ToString(), cancellationToken)) {
            await HandleEventAsync(req, cancellationToken);
        }

        return None.Empty;
    }

    protected void UpdateAndPublishStatus(IContent content, string status) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, status);
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);

        _contentService.SaveAndPublish(content);
    }

    protected async Task AddOrUpdateCrowdfunderRevisionAsync(IContent content,
                                                             CrowdfunderType crowdfunderType,
                                                             string status) {
        var crowdfunderContent = content.Key.ToCrowdfunderContent(_contentLocator, crowdfunderType);

        var wasActive = crowdfunderContent.Status == CrowdfunderStatuses.Active;
        var isActive = _lookups.FindByName<CrowdfunderStatus>(status) == CrowdfunderStatuses.Active;

        if (!wasActive && isActive) {
            await _crowdfunderRevisionRepository.AddCrowdfunderRevisionAsync(crowdfunderContent, content.VersionId);
        } else if(wasActive && !isActive) {
            await _crowdfunderRevisionRepository.CloseCrowdfunderRevisionAsync(crowdfunderContent.Key);
        }
    }

    protected IContent GetContent(Guid id) {
        return _contentService.GetById(id);
    }

    protected abstract Task HandleEventAsync(TEvent req, CancellationToken cancellationToken);
}