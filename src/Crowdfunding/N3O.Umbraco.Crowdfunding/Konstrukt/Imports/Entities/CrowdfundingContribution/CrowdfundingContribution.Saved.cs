namespace N3O.Umbraco.Crowdfunding.Konstrukt;

public partial class CrowdfundingContribution {
    public void Saved() {
        Status = CrowdfundingContributionStatuses.Visible;
    }
}
