using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Engage.Models;

public interface IFeedbackCrowdfunderGoal {
    IEnumerable<IFeedbackCustomField> CustomFields { get; }
    
    FeedbackScheme GetScheme(ILookups lookups);
}