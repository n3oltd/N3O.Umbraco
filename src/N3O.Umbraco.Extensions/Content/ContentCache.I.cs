using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IContentCache {
    IReadOnlyList<T> All<T>(Func<T, bool> predicate = null);
    IReadOnlyList<IPublishedContent> All(string contentTypeAlias, Func<IPublishedContent, bool> predicate = null);
    void Flush(IEnumerable<string> contentTypeAliases);
    T Single<T>(Func<T, bool> predicate = null);
    IPublishedContent Single(string contentTypeAlias, Func<IPublishedContent, bool> predicate = null);
}
