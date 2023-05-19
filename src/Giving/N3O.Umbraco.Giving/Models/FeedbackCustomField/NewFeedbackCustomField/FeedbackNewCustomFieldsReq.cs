using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackNewCustomFieldsReq {
    [Name("Entries")]
    public IEnumerable<FeedbackNewCustomFieldReq> Entries { get; set; }
}