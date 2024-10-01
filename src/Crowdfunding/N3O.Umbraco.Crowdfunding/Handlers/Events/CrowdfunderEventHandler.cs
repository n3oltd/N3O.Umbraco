using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Models;
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
    
    protected CrowdfunderEventHandler(AsyncKeyedLocker<string> asyncKeyedLocker, IContentService contentService) {
        _asyncKeyedLocker = asyncKeyedLocker;
        _contentService = contentService;
    }

    public async Task<None> Handle(TEvent req, CancellationToken cancellationToken) {
        using (await _asyncKeyedLocker.LockAsync(req.Model.Id.ToString(), cancellationToken)) {
            await HandleEventAsync(req, cancellationToken);
        }

        return None.Empty;
    }

    protected void UpdateAndPublishStatus(Guid id, string status) {
        var content = GetContent(id);
        
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, status);
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);

        _contentService.SaveAndPublish(content);
    }

    private IContent GetContent(Guid id) {
        return _contentService.GetById(id);
    }
    
    protected abstract Task HandleEventAsync(TEvent req, CancellationToken cancellationToken);
}