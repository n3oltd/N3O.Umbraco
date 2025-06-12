using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Search.Typesense;

public class SearchDocumentBuilder<T> : ISearchDocumentBuilder<T> where T : class, new() {
    private readonly List<Action<T>> _actions = new();
    
    public ISearchDocumentBuilder<T> Set(Action<T> action) {
        _actions.Add(action);

        return this;
    }

    public T Build() {
        var searchDocument = new T();
        
        _actions.Do(x => x(searchDocument));

        return searchDocument;
    }
}