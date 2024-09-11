using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfunderData : Value, ICrowdfunderData {
    [JsonConstructor]
    public CrowdfunderData(Guid campaignId,
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

    public CrowdfunderData(ICrowdfunderData crowdfunderData)
        : this(crowdfunderData.CampaignId,
               crowdfunderData.TeamId,
               crowdfunderData.FundraiserId,
               crowdfunderData.FundraiserUrl,
               crowdfunderData.Comment,
               crowdfunderData.Anonymous) { }

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