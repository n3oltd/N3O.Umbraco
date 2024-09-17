using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crm.Models;

public interface IFeedbackCrowdfunderGoal {
    FeedbackScheme Scheme { get; }
    IEnumerable<IFeedbackCustomField> CustomFields { get; }
}