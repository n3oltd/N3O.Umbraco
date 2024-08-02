using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingCreatePageViewModel {
    public IEnumerable<CrowdfundingCampaignContent> CrowdfundingCampaigns { get; set; }
}