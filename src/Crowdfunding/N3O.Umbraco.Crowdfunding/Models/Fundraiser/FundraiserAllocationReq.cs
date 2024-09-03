﻿using N3O.Umbraco.Attributes;
using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserAllocationReq {
    [Name("Amount")]
    public decimal? Amount { get; set; }
    
    [Name("Goal Id")]
    public Guid? GoalId { get; set; }
    
    [Name("Feedback")]
    public FeedbackGoalReq Feedback { get; set; }
}