using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Typesense;

public interface ISearchIndexer {
    bool CanIndex(IPublishedContent content);
    Task DeleteAsync(string id);
    Task IndexAsync(IPublishedContent content);
}