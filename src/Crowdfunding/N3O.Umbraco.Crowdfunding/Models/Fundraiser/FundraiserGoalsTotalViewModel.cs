namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalsTotalViewModel {
    public FundraiserGoalsTotalViewModel(decimal goalsTotal, decimal raisedTotal) {
        GoalsTotal = goalsTotal;
        RaisedTotal = raisedTotal;
    }
    
    public decimal GoalsTotal { get; set; }
    public decimal RaisedTotal { get; set; }
}