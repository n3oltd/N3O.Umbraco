using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookCrowdfundingInfo : Value {
    public WebhookCrowdfundingInfo(Guid campaignId, Guid? teamId, Guid? fundraiserId) {
        CampaignId = campaignId;
        TeamId = teamId;
        FundraiserId = fundraiserId;
    }

    public Guid CampaignId { get; }
    public Guid? TeamId { get; }
    public Guid? FundraiserId { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CampaignId;
        yield return TeamId;
        yield return FundraiserId;
    }
}