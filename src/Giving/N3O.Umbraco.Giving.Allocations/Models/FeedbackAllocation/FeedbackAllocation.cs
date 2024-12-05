using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackAllocation : Value, IFeedbackAllocation {
    [JsonConstructor]
    public FeedbackAllocation(FeedbackScheme scheme, IEnumerable<FeedbackCustomField> customFields) {
        Scheme = scheme;
        CustomFields = customFields;
    }

    public FeedbackAllocation(IFeedbackAllocation feedback)
        : this(feedback.Scheme, feedback.CustomFields.OrEmpty().Select(x => new FeedbackCustomField(x))) { }

    public FeedbackScheme Scheme { get; }
    public IEnumerable<FeedbackCustomField> CustomFields { get; }

    public string Summary => Scheme?.Name;

    [JsonIgnore]
    IEnumerable<IFeedbackCustomField> IFeedbackAllocation.CustomFields => CustomFields;
}