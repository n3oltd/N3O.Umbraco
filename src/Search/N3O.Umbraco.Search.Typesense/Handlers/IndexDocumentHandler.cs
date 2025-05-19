using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Search.Typesense.Commands;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class IndexDocumentHandler : IRequestHandler<IndexDocumentCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ISearchDocumentBuilder _searchDocumentBuilder;
    private readonly ITypesenseService _typesenseService;
    private readonly IReadOnlyList<ISearchIndexer> _searchIndexers;

    public IndexDocumentHandler(IContentLocator contentLocator,
                                ISearchDocumentBuilder searchDocumentBuilder,
                                ITypesenseService typesenseService,
                                IEnumerable<ISearchIndexer> searchIndexers) {
        _contentLocator = contentLocator;
        _searchDocumentBuilder = searchDocumentBuilder;
        _typesenseService = typesenseService;
        _searchIndexers = searchIndexers.ApplyAttributeOrdering();
    }
    
    public async Task<None> Handle(IndexDocumentCommand req, CancellationToken cancellationToken) {
        var publishedContent = req.ContentId.Run(id => _contentLocator.ById(id), true);

        var searchIndexer = _searchIndexers.FirstOrDefault(x => x.CanIndex(publishedContent));

        if (searchIndexer.HasValue()) {
            searchIndexer.Index(_searchDocumentBuilder, publishedContent);
            
            var searchDocument = _searchDocumentBuilder.Build();

            await _typesenseService.UpsertAsync(searchDocument, cancellationToken);
        }
        
        return None.Empty;
    }
}

public interface ISearchIndexer {
    bool CanIndex(IPublishedContent content);
    void Index(ISearchDocumentBuilder builder, IPublishedContent content);
}

public abstract class SearchIndexer<T> : ISearchIndexer where T : IPublishedContent {
    public bool CanIndex(IPublishedContent content) {
        return content is T;
    }
    
    public void Index(ISearchDocumentBuilder builder, IPublishedContent content) {
        Index(builder, (T) content);
    }

    protected abstract void Index(ISearchDocumentBuilder builder, T content);
}

public class SearchDocument : Value {
    public SearchDocument(string content, string description, string title, string url, Instant timestamp) {
        Content = content;
        Description = description;
        Title = title;
        Url = url;
        Timestamp = timestamp;
    }

    public string Content { get; }
    public string Description { get; }
    public string Title { get; }
    public string Url { get; }
    public Instant Timestamp { get; }
}

public interface ISearchDocumentBuilder {
    ISearchDocumentBuilder IsPrimaryFor(string[] keywords);
    
    ISearchDocumentBuilder WithContent(string content);
    ISearchDocumentBuilder WithDescription(string description);
    ISearchDocumentBuilder WithTitle(string title);
    ISearchDocumentBuilder WithUrl(string url);
    ISearchDocumentBuilder WithTimestamp(Instant timestamp);
    SearchDocument Build();
}

public class SearchDocumentBuilder : ISearchDocumentBuilder {
    private string _content;
    private string _description;
    private string _url;
    private string _title;
    private Instant _timestamp;

    public ISearchDocumentBuilder WithContent(string content) {
        _content = content;

        return this;
    }
    
    public ISearchDocumentBuilder WithDescription(string description) {
        _description = description;

        return this;
    }
    
    public ISearchDocumentBuilder WithTitle(string title) {
        _title = title;

        return this;
    }
    public ISearchDocumentBuilder WithUrl(string url) {
        _url = url;

        return this;
    }
    
    public ISearchDocumentBuilder WithTimestamp(Instant timestamp) {
        _timestamp = timestamp;

        return this;
    }
    
    public SearchDocument Build() {
        Validate();
        
        var searchDocument = new SearchDocument(_content,
                                                _description,
                                                _title,
                                                _url,
                                                _timestamp);

        return searchDocument;
    }

    private void Validate() {
        if (!_title.HasValue()) {
            throw new Exception("Title must be specified");
        }
    }
}