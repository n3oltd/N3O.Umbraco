namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserNotificationViewModel {
    public FundraiserNotificationViewModel(FundraiserContentViewModel fundraiser,
                                           FundraiserGoalsTotalViewModel goalsTotal) {
        Fundraiser = fundraiser;
        GoalsTotal = goalsTotal;
    }
    
    public FundraiserContentViewModel Fundraiser { get; set; }
    public FundraiserGoalsTotalViewModel GoalsTotal { get; set; }
}