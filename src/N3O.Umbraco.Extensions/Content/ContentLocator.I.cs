using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IContentLocator {
    IReadOnlyList<PublishedContentModel> All(Func<PublishedContentModel, bool> predicate = null);
    IReadOnlyList<PublishedContentModel> All(string contentTypeAlias, Func<PublishedContentModel, bool> predicate = null);
    IReadOnlyList<T> All<T>(Func<T, bool> predicate = null);

    PublishedContentModel ById(int id);
    T ById<T>(int id);
    
    PublishedContentModel ById(Guid id);
    T ById<T>(Guid id);

    PublishedContentModel Single(string contentTypeAlias, Func<PublishedContentModel, bool> predicate = null);
    T Single<T>(Func<T, bool> predicate = null);
}
