﻿using System;

namespace N3O.Umbraco.Crowdfunding.Models;

public class SelectedGoalRes {
    public Guid CampaignGoalId { get; set; }
    public decimal Value { get; set; }
    public SelectedFeedbackGoalRes Feedback { get; set; }
}