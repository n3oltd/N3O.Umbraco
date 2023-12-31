using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CrowdfundingData : Value, ICrowdfundingData {
    // TeamId, CampaignId, PageId, comment, should also probably store here (and in
    // the SQL DB which price handle they clicked if any (for stats purposes)

    [JsonConstructor]
    public CrowdfundingData(Guid? campaignId,
                            Guid? teamId,
                            Guid? pageId,
                            string pageUrl,
                            string comment) {
        CampaignId = campaignId;
        TeamId = teamId;
        PageId = pageId;
        Comment = comment;
        PageUrl = pageUrl;
    }

    public CrowdfundingData(ICrowdfundingData crowdfundingData)
        : this(crowdfundingData.CampaignId,
               crowdfundingData.TeamId,
               crowdfundingData.PageId,
               crowdfundingData.PageUrl,
               crowdfundingData.Comment) { }

    public Guid? CampaignId { get; }
    public Guid? TeamId { get; }
    public Guid? PageId { get; }
    public string PageUrl { get; }
    public string Comment { get; }
}