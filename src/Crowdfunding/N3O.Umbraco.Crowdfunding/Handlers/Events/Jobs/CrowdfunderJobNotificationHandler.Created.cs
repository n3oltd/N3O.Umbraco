using AsyncKeyedLock;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedHandler : CrowdfunderJobNotificationHandler<CrowdfunderCreatedJobNotification> {
    private readonly IBackgroundJob _backgroundJob;

    public CrowdfunderCreatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IBackgroundJob backgroundJob,
                                     ICoreScopeProvider coreScopeProvider,
                                     IContentLocator contentLocator,
                                     ICrowdfundingNotifications crowdfundingNotifications,
                                     ICrowdfundingUrlBuilder crowdfundingUrlBuilder) 
        : base(asyncKeyedLocker, contentService, backgroundJob, coreScopeProvider) {
        _backgroundJob = backgroundJob;
    }

    protected override Task HandleNotificationAsync(CrowdfunderCreatedJobNotification req, IContent content) {
        var type = content.ContentType.Alias.ToCrowdfunderType();

        UpdateStatus(content, type, req.Model.CrowdfunderInfo.Status.Name);

        if (type == CrowdfunderTypes.Fundraiser) {
            _backgroundJob.EnqueueCommand<FundraiserCreatedNotification>(p => {
                p.Add<ContentId>(content.Key.ToString());
            });
        }
        
        return Task.CompletedTask;
    }

    private void UpdateStatus(IContent content, CrowdfunderType type, string statusName) {
        var status = StaticLookups.GetAll<CrowdfunderStatus>()
                                  .Single(x => x.Name == statusName);

        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, status.Name);

        if (type == CrowdfunderTypes.Campaign && status == CrowdfunderStatuses.Draft) {
            content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);
        } else {
            content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
        }
    }
}