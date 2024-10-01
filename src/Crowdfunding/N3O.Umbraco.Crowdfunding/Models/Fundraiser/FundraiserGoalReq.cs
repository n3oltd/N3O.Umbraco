using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Models;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
    
    [Name("Goal Option ID")]
    public string GoalOptionId { get; set; }
    
    [Name("Fund Dimensions")]
    public FundDimensionValuesReq FundDimensions { get; set; }
    
    [Name("Feedback")]
    public FeedbackGoalReq Feedback { get; set; }
}