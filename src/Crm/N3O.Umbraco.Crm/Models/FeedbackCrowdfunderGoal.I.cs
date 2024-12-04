﻿using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crm.Models;

public interface IFeedbackCrowdfunderGoal {
    FeedbackScheme Scheme { get; }
    IEnumerable<IFeedbackCustomField> CustomFields { get; }
}