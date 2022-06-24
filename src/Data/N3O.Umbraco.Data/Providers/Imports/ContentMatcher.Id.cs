using N3O.Umbraco.Data.Providers;
using System;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Handlers; 

public class IdContentMatcher : IContentMatcher {
    public bool IsMatcher(string contentTypeAlias) {
        return true;
    }

    public bool IsMatch(IContent content, string criteria) {
        if (int.TryParse(criteria, out var id)) {
            return content.Id == id;
        } else if (Guid.TryParse(criteria, out var key)) {
            return content.Key == key;
        } else {
            return false;
        }
    }
}