using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Extensions; 

public static class FeedbackSchemeExtensions {
    public static IEnumerable<FeedbackCustomFieldDefinition> ToFeedbackCustomFieldsDefinition(this FeedbackScheme scheme) {
        var customFields = scheme.CustomFields.Select(x => new FeedbackCustomFieldDefinition(x.Type,
                                                                                             x.Alias,
                                                                                             x.Name,
                                                                                             x.Required,
                                                                                             x.TextMaxLength.GetValueOrDefault()));

        return customFields;
    }
    
    public static FeedbackCustomFieldDefinitionElement GetFeedbackCustomFieldDefinition(this FeedbackScheme scheme,
                                                                                        string alias) {
        return scheme.CustomFields.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}