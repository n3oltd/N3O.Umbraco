using N3O.Umbraco.Crowdfunding.Content;

namespace N3O.Umbraco.CrowdFunding.Models;

public interface ICrowdfundingViewModel {
    public ICrowdfunderContent Content { get; }
}