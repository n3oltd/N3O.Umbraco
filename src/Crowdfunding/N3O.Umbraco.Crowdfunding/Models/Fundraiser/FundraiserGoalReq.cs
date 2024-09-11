using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserGoalReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
    
    [Name("Goal ID")]
    public Guid? GoalId { get; set; }
    
    [Name("Feedback")]
    public FeedbackGoalReq Feedback { get; set; }
}