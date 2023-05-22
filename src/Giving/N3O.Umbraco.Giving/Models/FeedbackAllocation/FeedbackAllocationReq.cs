using N3O.Umbraco.Attributes;
using N3O.Umbraco.Giving.Extensions;
using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackAllocationReq : IFeedbackAllocation {
    [Name("Scheme")]
    public FeedbackScheme Scheme { get; set; }

    [Name("Custom Fields")]
    public FeedbackNewCustomFieldsReq CustomFields { get; set; }

    [JsonIgnore]
    IEnumerable<IFeedbackCustomField> IFeedbackAllocation.CustomFields =>
        CustomFields.Entries.Select(x => x.ToFeedbackCustomField(Scheme));
}