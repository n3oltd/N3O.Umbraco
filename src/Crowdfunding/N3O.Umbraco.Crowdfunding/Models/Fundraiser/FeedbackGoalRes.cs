﻿using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeedbackGoalRes {
    public IEnumerable<FeedbackCustomFieldRes> Feedback { get; set; }
}