using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Typesense.Indexing;

public interface ISearchIndexer
{
    bool CanIndex(IPublishedContent content);

    void Index(ISearchDocumentBuilder builder, IPublishedContent content);

}