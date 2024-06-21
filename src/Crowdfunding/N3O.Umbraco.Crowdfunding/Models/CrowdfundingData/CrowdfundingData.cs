using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingData : Value, ICrowdfundingData {
    [JsonConstructor]
    public CrowdfundingData(Guid campaignId,
                            Guid? teamId,
                            Guid pageId,
                            string pageUrl,
                            string comment,
                            bool anonymous) {
        CampaignId = campaignId;
        TeamId = teamId;
        PageId = pageId;
        PageUrl = pageUrl;
        Comment = comment;
        Anonymous = anonymous;
    }

    public CrowdfundingData(ICrowdfundingData crowdfundingData)
        : this(crowdfundingData.CampaignId,
               crowdfundingData.TeamId,
               crowdfundingData.PageId,
               crowdfundingData.PageUrl,
               crowdfundingData.Comment,
               crowdfundingData.Anonymous) { }

    public Guid CampaignId { get; }
    public Guid? TeamId { get; }
    public Guid PageId { get; }
    public string PageUrl { get; }
    public string Comment { get; }
    public bool Anonymous { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return CampaignId;
        yield return TeamId;
        yield return PageId;
        yield return PageUrl;
        yield return Comment;
        yield return Anonymous;
    }
}