using N3O.Umbraco.Giving.Allocations.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFeedbackAllocation {
    FeedbackScheme Scheme { get; }
    IEnumerable<IFeedbackCustomField> CustomFields { get; }
}