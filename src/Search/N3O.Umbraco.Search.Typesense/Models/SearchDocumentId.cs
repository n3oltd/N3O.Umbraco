using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Search.Typesense.Models;

public static class SearchDocumentId {
    public static string Create(Guid contentKey, string culture) {
        return culture.HasValue() ? $"{contentKey}_{culture}" : contentKey.ToString();
    }
}
