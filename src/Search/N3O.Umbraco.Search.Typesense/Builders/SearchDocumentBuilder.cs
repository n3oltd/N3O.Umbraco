using N3O.Umbraco.Extensions;
using NodaTime;
using System;
using N3O.Umbraco.Search.Typesense.Models;

namespace N3O.Umbraco.Search.Typesense.Builders;

public class SearchDocumentBuilder : ISearchDocumentBuilder {
    private string _content;
    private string _description;
    private string _url;
    private string _title;
    private Instant _timestamp;

    public ISearchDocumentBuilder IsPrimaryFor(string[] keywords)
    {
        return this;
    }

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