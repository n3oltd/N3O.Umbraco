using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Extensions;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Utilities;
using Newtonsoft.Json;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfunderJob {
    private readonly IContentService _contentService;
    private readonly IBackgroundJob _backgroundJob;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IContentLocator _contentLocator;
    private readonly ICrowdfundingUrlBuilder _crowdfundingUrlBuilder;
    
    public CrowdfunderJob(IContentService contentService,
                          IBackgroundJob backgroundJob,
                          IWebHostEnvironment webHostEnvironment,
                          IContentLocator contentLocator,
                          ICrowdfundingUrlBuilder crowdfundingUrlBuilder) {
        _contentService = contentService;
        _backgroundJob = backgroundJob;
        _webHostEnvironment = webHostEnvironment;
        _contentLocator = contentLocator;
        _crowdfundingUrlBuilder = crowdfundingUrlBuilder;
    }

    public void CrowdfunderCreated(JobResult result, Guid contentId) {
        CrowdfunderCreatedOrUpdated(result, contentId, content => CrowdfunderCreated(result, content));
    } 
    
    public void CrowdfunderUpdated(JobResult result, Guid contentId) {
        CrowdfunderCreatedOrUpdated(result, contentId, content => CrowdfunderUpdated(result, content));
    }

    private void CrowdfunderCreatedOrUpdated(JobResult result, Guid contentId, Action<IContent> action) {
        var content = _contentService.GetById(contentId);

        if (content.HasValue()) {
            if (result.Success) {
                action(content);
                
                content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, null);

                _contentService.SaveAndPublish(content);
            } else {
                content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Error, JsonConvert.SerializeObject(result.Error));

                content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);

                _contentService.Save(content);
            }

            _backgroundJob.EnqueueCrowdfunderUpdated(content.Key, content.ContentType.Alias.ToCrowdfunderType());
        }
    }
    
    private void CrowdfunderCreated(JobResult crowdfunderCreated, IContent content) {
        var type = content.ContentType.Alias.ToCrowdfunderType();

        UpdateStatus(content, type, crowdfunderCreated.CrowdfunderInfo.Status.Name);

        if (type == CrowdfunderTypes.Fundraiser) {
            _backgroundJob.EnqueueCommand<FundraiserCreatedNotification>(p => {
                p.Add<ContentId>(content.Key.ToString());
            });
        }
    }
    
    private void CrowdfunderUpdated(JobResult crowdfunderUpdated, IContent content) {
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, crowdfunderUpdated.CrowdfunderInfo.Status.Name);
        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
        
        var type = content.ContentType.Alias.ToCrowdfunderType();
        
        if (type == CrowdfunderTypes.Campaign) {
            EnqueueCampaignWebhook(content, crowdfunderUpdated.CrowdfunderInfo.Status.Name);
        }
    }
    
    private void EnqueueCampaignWebhook(IContent content, string status) {
        if (status.HasValue() && !status.EqualsInvariant(CrowdfunderStatuses.Draft.Name) && _webHostEnvironment.IsProduction()) {
            var campaign = _contentLocator.ById<CampaignContent>(content.Key);
            var urlSettingsContent = _contentLocator.Single<UrlSettingsContent>();
        
            var stagingBaseUrl = urlSettingsContent.StagingBaseUrl;
            var campaignUrl = campaign.Url(_crowdfundingUrlBuilder);

            _backgroundJob.EnqueueCampaignUrlWebhook(campaign.Key, campaignUrl, stagingBaseUrl);
        }
    }
    
    private void UpdateStatus(IContent content, CrowdfunderType type, string statusName) {
        var status = StaticLookups.GetAll<CrowdfunderStatus>().Single(x => x.Name == statusName);

        content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.Status, status.Name);

        if (type == CrowdfunderTypes.Campaign && status == CrowdfunderStatuses.Draft) {
            content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, true);
        } else {
            content.SetValue(CrowdfundingConstants.Crowdfunder.Properties.ToggleStatus, false);
        }
    }
}
