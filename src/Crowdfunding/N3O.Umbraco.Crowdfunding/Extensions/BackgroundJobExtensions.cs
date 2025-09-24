using Flurl;
using N3O.Umbraco.Cloud.Engage.Lookups;
using N3O.Umbraco.Crowdfunding.Commands;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Parameters;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Scheduler.Extensions;
using N3O.Umbraco.Webhooks;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Extensions;

public static class BackgroundJobExtensions {
    public static void EnqueueCampaignUrlWebhook(this IBackgroundJob backgroundJob,
                                                 Guid campaignId,
                                                 string campaignUrl,
                                                 string stagingBaseUrl) {
        var req = GetWebhookReq(campaignId, campaignUrl, stagingBaseUrl);
        var jobName = $"DWH Campaign Url Updated to {req.Url}";

        backgroundJob.Enqueue<DispatchWebhookCommand, DispatchWebhookReq>(jobName, req);
    }
    
    public static void EnqueueCrowdfunderUpdated(this IBackgroundJob backgroundJob,
                                                 Guid contentId,
                                                 CrowdfunderType crowdfunderType) {
        backgroundJob.EnqueueCommand<CrowdfunderUpdatedNotification>(p => {
            p.Add<ContentId>(contentId.ToString());
            p.Add<CrowdfunderTypeId>(crowdfunderType.Id);
        });
    }
    
    private static DispatchWebhookReq GetWebhookReq(Guid campaignId, string campaignUrl, string stagingBaseUrl) {
        var webhookUrl = new Url(stagingBaseUrl.TrimEnd('/'));
        webhookUrl.AppendPathSegment($"umbraco/api/{WebhooksConstants.ApiName}/{CrowdfundingConstants.Webhooks.HookIds.CampaignUrl}");
        webhookUrl.AppendPathSegment($"{campaignId}");

        var headers = new Dictionary<string, string>();
        headers.Add("N3O-Foreground-Job", "true");
            
        var dispatchReq = new DispatchWebhookReq();
        dispatchReq.Url = webhookUrl;
        dispatchReq.Body = campaignUrl;
        dispatchReq.Headers = headers;

        return dispatchReq;
    }
}