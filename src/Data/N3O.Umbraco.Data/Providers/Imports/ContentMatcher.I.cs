using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Providers;

public interface IContentMatcher {
    bool IsMatcher(string contentTypeAlias);
    bool IsMatch(IContent content, string criteria);
}
