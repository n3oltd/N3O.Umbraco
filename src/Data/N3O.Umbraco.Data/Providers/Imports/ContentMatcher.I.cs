using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Providers {
    public interface IContentMatcher {
        public bool IsMatcher(string contentTypeAlias);
        public bool IsMatch(IContent content, string criteria);
    }
}