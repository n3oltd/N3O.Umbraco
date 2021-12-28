using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Content;

public interface IContentCache {
    IReadOnlyList<T> All<T>(Func<T, bool> predicate = null);
    void Flush(IEnumerable<string> contentTypeAliases);
    T Single<T>(Func<T, bool> predicate = null);
}