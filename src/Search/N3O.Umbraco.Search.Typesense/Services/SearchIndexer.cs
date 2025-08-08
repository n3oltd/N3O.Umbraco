using System.Collections.Generic;
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

    public async Task IndexAsync(IPublishedContent content) {
        await ProcessContentAsync(_searchDocumentBuilder, (TContent) content);

        var document = _searchDocumentBuilder.Build();
        var collection = TypesenseHelper.GetCollectionName<TDocument>();

        var fields = new List<Field>();
        fields.Add(new Field("timestamp", FieldType.Auto));
        fields.Add(new Field("content", FieldType.Auto));
        fields.Add(new Field("description", FieldType.Auto));
        fields.Add(new Field("title", FieldType.Auto));
        fields.Add(new Field("url", FieldType.Auto));
        
        var schema = new Schema(collection, fields);

        await _typesenseClient.DeleteCollection(collection);

        await _typesenseClient.CreateCollection(schema);
        
        await _typesenseClient.UpsertDocument(collection, document);
    }

    protected abstract Task ProcessContentAsync(ISearchDocumentBuilder<TDocument> builder, TContent content);
}