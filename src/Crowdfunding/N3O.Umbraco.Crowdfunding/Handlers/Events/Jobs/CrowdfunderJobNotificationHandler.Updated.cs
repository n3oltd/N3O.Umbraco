using AsyncKeyedLock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Lookups;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Handlers;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Utilities;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Events;

public class CrowdfunderUpdatedHandler : CrowdfunderJobNotificationHandler<CrowdfunderUpdatedJobNotification> {
    private readonly IBackgroundJob _backgroundJob;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;

    public CrowdfunderUpdatedHandler(AsyncKeyedLocker<string> asyncKeyedLocker,
                                     IContentService contentService,
                                     IBackgroundJob backgroundJob,
                                     ICoreScopeProvider coreScopeProvider,
                                     IWebHostEnvironment webHostEnvironment,
                                     IContentLocator contentLocator,
                                     ICrowdfundingUrlBuilder crowdfundingUrlBuilder) 
        : base(asyncKeyedLocker, contentService, backgroundJob, coreScopeProvider) {
        _backgroundJob = backgroundJob;
        _webHostEnvironment = webHostEnvironment;
        _contentLocator = contentLocator;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    protected override Task HandleNotificationAsync(CrowdfunderUpdatedJobNotification req, IContent content) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, req.Model.CrowdfunderInfo.Status.Name);
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
        
        var type = content.ContentType.Alias.ToCrowdfunderType();
        
        if (type == CrowdfunderTypes.Campaign) {
            EnqueueCampaignWebhook(content);
        }

        return Task.CompletedTask;
    }
    
    private void EnqueueCampaignWebhook(IContent content) {
        var status = content.GetValue<string>(CrowdfundingConstants.Crowdfunder.Properties.Status);
        
        if(status.HasValue() && !status.EqualsInvariant(CrowdfunderStatuses.Draft.Name) && _webHostEnvironment.IsProduction()) {
            var campaign = _contentLocator.ById<CampaignContent>(content.Key);
            var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();
        
            var stagingBaseUrl = urlSettingsContent.StagingBaseUrl;
            var campaignUrl = campaign.Url(_crowdfundingUrlBuilder);

            _backgroundJob.EnqueueCampaignUrlWebhook(campaign.Key, campaignUrl, stagingBaseUrl);
        }
    }
}