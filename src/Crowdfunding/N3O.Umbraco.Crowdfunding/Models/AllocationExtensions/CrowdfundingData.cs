using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingData : Value, ICrowdfundingData {
    // TeamId, CampaignId, PageId, comment, should also probably store here (and in
    // the SQL DB which price handle they clicked if any (for stats purposes)

    [JsonConstructor]
    public CrowdfundingData(Guid? campaignId,
                            Guid? teamId,
                            string teamName,
                            Guid? pageId,
                            string pageUrl,
                            string comment,
                            bool anonymous) {
        CampaignId = campaignId;
        TeamId = teamId;
        TeamName = teamName;
        PageId = pageId;
        Comment = comment;
        PageUrl = pageUrl;
        Anonymous = anonymous;
    }

    public CrowdfundingData(ICrowdfundingData crowdfundingData)
        : this(crowdfundingData.CampaignId,
               crowdfundingData.TeamId,
               crowdfundingData.TeamName,
               crowdfundingData.PageId,
               crowdfundingData.PageUrl,
               crowdfundingData.Comment,
               crowdfundingData.Anonymous) { }

    public Guid? CampaignId { get; }
    public Guid? TeamId { get; }
    public string TeamName { get; }
    public Guid? PageId { get; }
    public string PageUrl { get; }
    public string Comment { get; }
    public bool Anonymous { get; }
}