using AsyncKeyedLock;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Scheduler;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedHandler : CrowdfunderJobNotificationHandler<CrowdfunderUpdatedJobNotification> {
    public CrowdfunderUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IBackgroundJob backgroundJob,
                                     ICoreScopeProvider coreScopeProvider) 
        : base(asyncKeyedLocker, contentService, backgroundJob, coreScopeProvider) { }

    protected override Task HandleNotificationAsync(CrowdfunderUpdatedJobNotification req, IContent content) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, req.Model.CrowdfunderInfo.Status.Name);
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);

        return Task.CompletedTask;
    }
}