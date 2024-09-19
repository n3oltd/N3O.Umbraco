using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalRes {
    public Guid CampaignGoalId { get; set; }
    public decimal Value { get; set; }
    public FeedbackGoalRes Feedback { get; set; }
}