using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingContentPageViewModel {
    public CrowdfundingPageContent Content { get; set; }
    public IEnumerable<CrowdfundingPageAllocation> Allocations { get; set; }
    public CrowdfundingPageProgress Progress { get; set; }
    public CrowdfundingPageOwner OwnerInfo { get; set; }
    public IEnumerable<FundraisingContentPageDonations> PageDonations { get; set; }
    public IEnumerable<CrowdfundingPageCampaignFundraisers> CampaignFundraisers { get; set; }
}