using N3O.Umbraco.Search.Typesense.Models;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Search.Typesense;

public interface ISearchIndexer {
    bool CanIndex(IPublishedContent content);
    bool CanIndex(string contentTypeAlias);
    bool IsIndexerFor(CollectionName collectionName);
    Task DeleteAsync(Guid contentKey);
    Task IndexAsync(IPublishedContent content, string culture = null, string targetCollectionName = null);
}
