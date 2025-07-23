using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Engage.Models;

public interface ICrowdfunderGoal {
    AllocationType Type { get; }
    decimal Amount { get; }
    IFundCrowdfunderGoal Fund { get; }
    IFeedbackCrowdfunderGoal Feedback { get; }
    
    IFundDimensionValues GetFundDimensionValues(ILookups lookups);
    
}