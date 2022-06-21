using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data;

public interface IContentSummariser {
    string GetSummary(IContent content);
    bool IsSummariser(string contentTypeAlias);
}
