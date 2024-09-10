namespace N3O.Umbraco.Crowdfunding.Content;

public class FundraiserGoalElement : CrowdfunderGoalElement<FundraiserGoalElement> {
    public string CampaignGoalId => GetValue(x => x.CampaignGoalId);
}