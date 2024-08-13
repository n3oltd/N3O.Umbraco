using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingData : Value, ICrowdfundingData {
    [JsonConstructor]
    public CrowdfundingData(Guid campaignId,
                            Guid? teamId,
                            Guid fundraiserId,
                            string fundraiserUrl,
                            string comment,
                            bool anonymous) {
        CampaignId = campaignId;
        TeamId = teamId;
        FundraiserId = fundraiserId;
        FundraiserUrl = fundraiserUrl;
        Comment = comment;
        Anonymous = anonymous;
    }

    public CrowdfundingData(ICrowdfundingData crowdfundingData)
        : this(crowdfundingData.CampaignId,
               crowdfundingData.TeamId,
               crowdfundingData.FundraiserId,
               crowdfundingData.FundraiserUrl,
               crowdfundingData.Comment,
               crowdfundingData.Anonymous) { }

    public Guid CampaignId { get; }
    public Guid? TeamId { get; }
    public Guid FundraiserId { get; }
    public string FundraiserUrl { get; }
    public string Comment { get; }
    public bool Anonymous { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CampaignId;
        yield return TeamId;
        yield return FundraiserId;
        yield return FundraiserUrl;
        yield return Comment;
        yield return Anonymous;
    }
}