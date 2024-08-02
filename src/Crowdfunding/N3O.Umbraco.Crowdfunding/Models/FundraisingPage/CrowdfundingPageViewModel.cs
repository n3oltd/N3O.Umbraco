using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models.FundraisingPage;

public class CrowdfundingPageViewModel {
    public CrowdfundingPageContent Content { get; set; }
    public IEnumerable<CrowdfundingContributionViewModel> Contributions { get; set; }
}