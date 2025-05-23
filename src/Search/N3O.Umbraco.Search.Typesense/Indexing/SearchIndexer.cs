using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Typesense.Indexing;

public abstract class SearchIndexer<T> : ISearchIndexer where T : IPublishedContent {
    public bool CanIndex(IPublishedContent content) {
        return content is T;
    }

    public void Index(ISearchDocumentBuilder builder, IPublishedContent content) {
        Index(builder, (T) content);
    }

    protected abstract void Index(ISearchDocumentBuilder builder, T content);
}