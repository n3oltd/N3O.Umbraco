using AsyncKeyedLock;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Scheduler;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderCreatedHandler : CrowdfunderJobNotificationHandler<CrowdfunderCreatedJobNotification> {
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingNotifications _crowdfundingNotifications;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public CrowdfunderCreatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IBackgroundJob backgroundJob,
                                     ICoreScopeProvider coreScopeProvider,
                                     IContentLocator contentLocator,
                                     ICrowdfundingNotifications crowdfundingNotifications,
                                     ICrowdfundingUrlBuilder crowdfundingUrlBuilder) 
        : base(asyncKeyedLocker, contentService, backgroundJob, coreScopeProvider) {
        _contentLocator = contentLocator;
        _crowdfundingNotifications = crowdfundingNotifications;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    protected override Task HandleNotificationAsync(CrowdfunderCreatedJobNotification req, IContent content) {
        var type = content.ContentType.Alias.ToCrowdfunderType();

        UpdateStatus(content, type, req.Model.CrowdfunderInfo.Status.Name);

        if (type == CrowdfunderTypes.Fundraiser) {
            SendFundraiserCreatedEmail(content);
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

    private void SendFundraiserCreatedEmail(IContent content) {
        var fundraiser = _contentLocator.ById<FundraiserContent>(content.Key);
        var fundraiserContentViewModel = new FundraiserContentViewModel(_crowdfundingUrlBuilder, fundraiser);
        var model = new FundraiserNotificationViewModel(fundraiserContentViewModel, null);

        _crowdfundingNotifications.Enqueue(FundraiserNotificationTypes.FundraiserCreated, model, fundraiser.Key);
    }
}