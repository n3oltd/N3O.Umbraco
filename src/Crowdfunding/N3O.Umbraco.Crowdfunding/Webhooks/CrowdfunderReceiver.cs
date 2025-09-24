using AsyncKeyedLock;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using N3O.Umbraco.Webhooks.Attributes;
using N3O.Umbraco.Webhooks.Extensions;
using N3O.Umbraco.Webhooks.Models;
using N3O.Umbraco.Webhooks.Receivers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Crowdfunding.Webhooks;

[WebhookReceiver(CrowdfundingConstants.Webhooks.HookIds.Crowdfunder)]
public class CrowdfunderReceiver : WebhookReceiver {
    private readonly IJsonProvider _jsonProvider;
    private readonly AsyncKeyedLocker<string> _locker;
    private readonly CrowdfunderJob _crowdfunderJob;

    public CrowdfunderReceiver(IJsonProvider jsonProvider,
                               AsyncKeyedLocker<string> locker,
                               CrowdfunderJob crowdfunderJob) {
        _jsonProvider = jsonProvider;
        _locker = locker;
        _crowdfunderJob = crowdfunderJob;
    }
    
    protected override async Task ProcessAsync(WebhookPayload payload, CancellationToken cancellationToken) {
        var contentIdStr = payload.GetHeader(CrowdfundingConstants.Webhooks.Headers.CrowdfunderId);
        var webhookCrowdfunder = payload.GetBody<JobResult>(_jsonProvider);

        if (!contentIdStr.HasValue() && !webhookCrowdfunder.CrowdfunderInfo.HasValue()) {
            return;
        }
        
        using (await _locker.LockAsync(contentIdStr, cancellationToken)) {
            var jobType = payload.GetHeader(CrowdfundingConstants.Webhooks.Headers.JobType);
            
            var contentId = Guid.Parse(contentIdStr);

            switch (jobType) {
                case CrowdfundingConstants.Webhooks.JobTypes.Crowdfunder.CampaignCreated:
                case CrowdfundingConstants.Webhooks.JobTypes.Crowdfunder.FundraiserCreated:
                    _crowdfunderJob.CrowdfunderCreated(webhookCrowdfunder, contentId);
                    
                    break;
                
                case CrowdfundingConstants.Webhooks.JobTypes.Crowdfunder.CrowdfunderSynced:
                    _crowdfunderJob.CrowdfunderUpdated(webhookCrowdfunder, contentId);
                    
                    break;
            }
        }
    }
}