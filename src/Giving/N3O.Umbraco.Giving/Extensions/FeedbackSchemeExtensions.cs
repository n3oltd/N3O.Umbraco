using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using System.Linq;

namespace N3O.Umbraco.Giving.Extensions; 

public static class FeedbackSchemeExtensions {
    public static FeedbackCustomFieldDefinitionElement GetFeedbackCustomFieldDefinition(this FeedbackScheme scheme,
                                                                                        string alias) {
        return scheme.CustomFields.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}