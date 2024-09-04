using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookFundraiserInfo : Value, IFundraiserInfo {
    public WebhookFundraiserInfo(Guid campaignId, Guid? teamId) {
        CampaignId = campaignId;
        TeamId = teamId;
    }
    
    public Guid CampaignId { get; }
    public Guid? TeamId { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CampaignId;
        yield return TeamId;
    }
}