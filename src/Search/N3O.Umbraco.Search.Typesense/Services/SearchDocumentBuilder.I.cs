using System;

namespace N3O.Umbraco.Search.Typesense;

public interface ISearchDocumentBuilder<TDocument> where TDocument : class, new() {
    ISearchDocumentBuilder<TDocument> Set(Action<TDocument> action);
    TDocument Build();
}
