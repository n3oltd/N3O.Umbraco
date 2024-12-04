using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Crm.Models;

public interface ICrowdfunderGoal {
    AllocationType Type { get; }
    IFundDimensionValues FundDimensions { get; }
    decimal Amount { get; }
    IFundCrowdfunderGoal Fund { get; }
    IFeedbackCrowdfunderGoal Feedback { get; }
}