using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackAllocation {
    FeedbackScheme Scheme { get; }
    IEnumerable<IFeedbackCustomField> CustomFields { get; }
}