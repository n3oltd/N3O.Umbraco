using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models; 

public class FundraiserGoalRes {
    public string GoalId { get; set; }
    public string GoalName { get; set; }
    public AllocationType Type { get; set; }
    public FeedbackGoalRes FeedbackGoal { get; set; }
    public IEnumerable<string> Tags { get; set; }
}