using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalRes {
    public string OptionId { get; set; }
    public decimal Value { get; set; }
    public FeedbackGoalRes Feedback { get; set; }
}