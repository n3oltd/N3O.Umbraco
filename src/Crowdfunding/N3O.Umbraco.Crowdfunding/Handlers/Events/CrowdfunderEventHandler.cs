using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public abstract class CrowdfunderEventHandler<TEvent> : IRequestHandler<TEvent, JobResult, None>
    where TEvent : CrowdfunderEvent {
    private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;
    private readonly IContentService _contentService;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfunderRevisionRepository _crowdfunderRevisionRepository;

    protected CrowdfunderEventHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                      IContentService contentService,
                                      IContentLocator contentLocator,
                                      ICrowdfunderRevisionRepository crowdfunderRevisionRepository) {
        _asyncKeyedLocker = asyncKeyedLocker;
        _contentService = contentService;
        _contentLocator = contentLocator;
        _crowdfunderRevisionRepository = crowdfunderRevisionRepository;
    }

    public async Task<None> Handle(TEvent req, CancellationToken cancellationToken) {
        using (await _asyncKeyedLocker.LockAsync(req.ContentId.Value.ToString(), cancellationToken)) {
            var content = GetContent(req.ContentId.Value);
            
            if (req.Model.Success) {
                await HandleEventAsync(req, content, cancellationToken);
                
                ClearError(content);
            } else {
                SetError(content, req.Model);
            }
            
            _contentService.SaveAndPublish(content);
        }

        return None.Empty;
    }

    protected async Task AddOrUpdateRevisionAsync(Guid contentId, int contentVersionId, CrowdfunderType type) {
        var content = _contentLocator.GetCrowdfunderContent(contentId, type);
        
        await _crowdfunderRevisionRepository.AddOrUpdateAsync(content, contentVersionId); 
    }

    private void ClearError(IContent content) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, null);
    }
    
    private IContent GetContent(Guid id) {
        return _contentService.GetById(id);
    }

    private void SetError(IContent content, JobResult result) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, JsonConvert.SerializeObject(result.Error));
    }

    protected abstract Task HandleEventAsync(TEvent req, IContent content, CancellationToken cancellationToken);
}