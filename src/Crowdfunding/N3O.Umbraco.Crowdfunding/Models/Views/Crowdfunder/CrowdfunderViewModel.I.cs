using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Financial;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfunderViewModel : ICrowdfundingViewModel {
    ICrowdfunderContent Content { get; }
    CrowdfunderType CrowdfunderType { get; }
    Currency SiteCurrency { get; }
    IReadOnlyList<CrowdfunderGoalViewModel> Goals { get; }
    IReadOnlyList<Currency> Currencies { get; }
    CrowdfunderProgressViewModel CrowdfunderProgress { get; }
    IReadOnlyList<CrowdfunderContributionViewModel> Contributions { get; }
    CrowdfunderOwnerViewModel OwnerInfo { get; }
    IReadOnlyList<CrowdfunderTagViewModel> Tags { get; }

    bool EditMode();
}