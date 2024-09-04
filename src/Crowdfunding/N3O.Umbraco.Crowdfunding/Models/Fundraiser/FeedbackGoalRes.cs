using N3O.Umbraco.Giving.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FeedbackGoalRes {
    public IEnumerable<FeedbackCustomFieldDefinitionElement> CustomFieldDefinitions { get; set; }
}