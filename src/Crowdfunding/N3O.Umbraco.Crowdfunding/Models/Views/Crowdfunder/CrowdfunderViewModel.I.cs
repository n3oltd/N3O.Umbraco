using N3O.Umbraco.Crowdfunding.Content;
using N3O.Umbraco.Crowdfunding.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public interface ICrowdfunderViewModel : ICrowdfundingViewModel {
    ICrowdfunderContent Content { get; }
    CrowdfunderType CrowdfunderType { get; }
    IReadOnlyList<CrowdfunderGoalViewModel> Goals { get; }
    CrowdfunderProgressViewModel CrowdfunderProgress { get; }
    IReadOnlyList<CrowdfunderContributionViewModel> Contributions { get; }
    CrowdfunderOwnerViewModel OwnerInfo { get; }
    IReadOnlyList<string> Tags { get; }

    bool EditMode();
}