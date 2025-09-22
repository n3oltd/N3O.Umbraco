using AsyncKeyedLock;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Events;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
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

                    using (var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true)) {
                        using (_ = scope.Notifications.Suppress()) {
                            _contentService.SaveAndPublish(content);
                        }
                    }
                    
                    var type = content.ContentType.Alias.ToCrowdfunderType();

                    if (type == CrowdfunderTypes.Campaign) {
                        _backgroundJob.EnqueueCommand<CampaignPublishedNotification>(p => {
                            p.Add<ContentId>(content.Key.ToString());
                        });
                    } else if (type == CrowdfunderTypes.Fundraiser) {
                        _backgroundJob.EnqueueCommand<FundraiserPublishedNotification>(p => {
                            p.Add<ContentId>(content.Key.ToString());
                        });
                    }
                } else {
                    SetError(content, req.Model);

                    content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
                    
                    using (var scope = _coreScopeProvider.CreateCoreScope(autoComplete: true)) {
                        using (_ = scope.Notifications.Suppress()) {
                            _contentService.Save(content);
                        }
                    }
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
        return _contentService.GetById(id);
    }

    private void SetError(IContent content, JobResult result) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, JsonConvert.SerializeObject(result.Error));
    }

    protected abstract Task HandleNotificationAsync(TJobNotification req, IContent content);
}