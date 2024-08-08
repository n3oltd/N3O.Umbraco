using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class FundraisingContentPageViewModel {
    public CrowdfundingPageContent Content { get; set; }
    public IEnumerable<FundraisingContentPageAllocation> Allocations { get; set; }
    public FundraisingContentPageProgress Progress { get; set; }
    public FundraisingContentPageOwner OwnerInfo { get; set; }
    public IEnumerable<FundraisingContentPageDonations> PageDonations { get; set; }
    public IEnumerable<FundraisingContentPageCampaignFundraisers> CampaignFundraisers { get; set; }
}