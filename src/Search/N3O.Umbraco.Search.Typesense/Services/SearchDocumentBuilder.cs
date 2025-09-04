using N3O.Umbraco.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Search.Typesense;

public class SearchDocumentBuilder<T> : ISearchDocumentBuilder<T> where T : class, new() {
    private readonly List<Action<T>> _actions = [];
    
    private readonly ITypesenseJsonProvider _typesenseJsonProvider;

    public SearchDocumentBuilder(ITypesenseJsonProvider typesenseJsonProvider) {
        _typesenseJsonProvider = typesenseJsonProvider;
    }
    
    public ISearchDocumentBuilder<T> Set(Action<T> action) {
        _actions.Add(action);

        return this;
    }

    public object Build() {
        var searchDocument = new T();
        
        _actions.Do(x => x(searchDocument));

        return TransformObject(searchDocument);
    }

    private object TransformObject(T document) {
        return JObject.Parse(_typesenseJsonProvider.SerializeObject(document)).ConvertToObject();
    }
}