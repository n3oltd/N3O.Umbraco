using Flurl;
using N3O.Umbraco.Scheduler;
using N3O.Umbraco.Webhooks;
using N3O.Umbraco.Webhooks.Commands;
using N3O.Umbraco.Webhooks.Models;
using System;

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
    
    
    private static DispatchWebhookReq GetWebhookReq(Guid campaignId, string campaignUrl, string stagingBaseUrl) {
        var webhookUrl = new Url(stagingBaseUrl);
        webhookUrl.AppendPathSegment($"umbraco/api/{WebhooksConstants.ApiName}/{CrowdfundingConstants.Webhooks.HookIds.CampaignUrl}");
        webhookUrl.AppendPathSegment($"{campaignId}");
            
        var dispatchReq = new DispatchWebhookReq();
        dispatchReq.Url = webhookUrl;
        dispatchReq.Body = campaignUrl;

        return dispatchReq;
    }
}