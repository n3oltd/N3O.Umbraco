using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Providers;

public class NameContentMatcher : IContentMatcher {
    public bool IsMatcher(string contentTypeAlias) => true;

    public bool IsMatch(IContent content, string criteria) {
        return content.Name.EqualsInvariant(criteria);
    }
}
