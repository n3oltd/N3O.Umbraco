using System.Threading.Tasks;
using Typesense;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Typesense;

public abstract class SearchIndexer<TContent, TDocument> : ISearchIndexer
    where TContent : IPublishedContent
    where TDocument : class, new() {
    private readonly ITypesenseClient _typesenseClient;
    private readonly ISearchDocumentBuilder<TDocument> _searchDocumentBuilder;

    protected SearchIndexer(ITypesenseClient typesenseClient, ISearchDocumentBuilder<TDocument> searchDocumentBuilder) {
        _typesenseClient = typesenseClient;
        _searchDocumentBuilder = searchDocumentBuilder;
    }
    
    public bool CanIndex(IPublishedContent content) {
        return content is TContent;
    }

    public async Task DeleteAsync(string id) {
        var collectionInfo = TypesenseHelper.GetCollection<TDocument>();
        
        await _typesenseClient.DeleteDocument<TDocument>(collectionInfo.Name.Resolve(), id);
    }

    public async Task IndexAsync(IPublishedContent content) {
        await ProcessContentAsync(_searchDocumentBuilder, (TContent) content);

        var document = _searchDocumentBuilder.Build();
        var collectionInfo = TypesenseHelper.GetCollection<TDocument>();
        
        await _typesenseClient.UpsertDocument(collectionInfo.Name.Resolve(), document);
    }

    protected abstract Task ProcessContentAsync(ISearchDocumentBuilder<TDocument> builder, TContent content);
}