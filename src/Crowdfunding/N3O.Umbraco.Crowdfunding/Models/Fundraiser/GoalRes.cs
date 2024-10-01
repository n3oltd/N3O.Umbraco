using N3O.Umbraco.Giving.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class GoalRes {
    public string OptionId { get; set; }
    public decimal Value { get; set; }
    public FundDimensionValuesRes FundDimensions { get; set; }
    public FeedbackGoalRes Feedback { get; set; }
    public IEnumerable<TagRes> Tags { get; set; }
}