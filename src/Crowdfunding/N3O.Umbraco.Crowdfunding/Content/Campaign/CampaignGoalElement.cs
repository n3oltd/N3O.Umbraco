namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CampaignGoalElement : CrowdfunderGoalElement<CampaignGoalElement> {
    public string GetGoalId() => $"{Parent().Key}_{Content().Key}";
}