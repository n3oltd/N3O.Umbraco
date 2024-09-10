using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class FundraiserGoalsRes {
    public IEnumerable<FundraiserGoalRes> AvailableGoals { get; set; }
    public IEnumerable<FundraiserSelectedGoalRes> SelectedGoals { get; set; }
}