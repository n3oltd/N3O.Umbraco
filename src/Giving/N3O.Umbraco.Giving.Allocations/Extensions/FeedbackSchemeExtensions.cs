﻿using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;

namespace N3O.Umbraco.Giving.Allocations.Extensions; 

public static class FeedbackSchemeExtensions {
    public static FeedbackCustomFieldDefinition GetFeedbackCustomFieldDefinition(this FeedbackScheme scheme,
                                                                                        string alias) {
        return scheme.CustomFields.SingleOrDefault(x => x.Alias.EqualsInvariant(alias));
    }
}