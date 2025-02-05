using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Scheduler;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Handlers;

public abstract class CrowdfunderJobNotificationHandler<TJobNotification> :
    IRequestHandler<TJobNotification, JobResult, None>
    where TJobNotification : CrowdfunderJobNotification {
    private readonly AsyncKeyedLocker<string> _asyncKeyedLocker;
    private readonly IContentService _contentService;
    private readonly IBackgroundJob _backgroundJob;
    private readonly ICoreScopeProvider _coreScopeProvider;

    protected CrowdfunderJobNotificationHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                                IContentService contentService,
                                                IBackgroundJob backgroundJob,
                                                ICoreScopeProvider coreScopeProvider) {
        _asyncKeyedLocker = asyncKeyedLocker;
        _contentService = contentService;
        _backgroundJob = backgroundJob;
        _coreScopeProvider = coreScopeProvider;
    }

    public async Task<None> Handle(TJobNotification req, CancellationToken cancellationToken) {
        using (await _asyncKeyedLocker.LockAsync(req.ContentId.Value.ToString(), cancellationToken)) {
            var content = GetContent(req.ContentId.Value);

            if (content.HasValue()) {
                if (req.Model.Success) {
                    await HandleNotificationAsync(req, content);
                
                    ClearError(content);
                } else {
                    SetError(content, req.Model);
                    
                    content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
                }
                
                if (typeof(TJobNotification) == typeof(CrowdfunderUpdatedJobNotification)) {
                    using var scope = _coreScopeProvider.CreateCoreScope(autoComplete:true);
                    
                    using (_ = scope.Notifications.Suppress()) {
                        _contentService.SaveAndPublish(content);

                        scope.Complete();
                    }
                } else {
                    _contentService.SaveAndPublish(content);
                }
                
                _backgroundJob.EnqueueCrowdfunderUpdated(content.Key, content.ContentType.Alias.ToCrowdfunderType());
            }
        }

        return None.Empty;
    }

    private void ClearError(IContent content) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, null);
    }
    
    private IContent GetContent(Guid id) {
        //Workaround as getting content by guid returns an old cached version of the content
        var content = _contentService.GetById(id);
        
        return _contentService.GetById(content.Id);
    }

    private void SetError(IContent content, JobResult result) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, JsonConvert.SerializeObject(result.Error));
    }

    protected abstract Task HandleNotificationAsync(TJobNotification req, IContent content);
}