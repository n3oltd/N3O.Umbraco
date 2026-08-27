using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Collections;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public class ContentCache : IContentCache {
    private readonly IContentLocator _contentLocator;
    private readonly ConcurrentDictionary<string, object> _typedStore = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly ConcurrentDictionary<string, IReadOnlyList<IPublishedContent>> _untypedStore = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly ConcurrentDictionary<string, ConcurrentHashSet<string>> _cacheKeysByContentType = new(StringComparer.InvariantCultureIgnoreCase);

    public ContentCache(IContentLocator contentLocator) {
        _contentLocator = contentLocator;
    }

    public IReadOnlyList<T> All<T>(Func<T, bool> predicate = null) {
        var cacheKey = GetCacheKey<T>();

        var all = (IReadOnlyList<T>) _typedStore.GetOrAdd(cacheKey, _ => _contentLocator.All<T>());

        Hold(cacheKey, AliasHelper<T>.ContentTypeAlias());
        Hold(cacheKey, all.Select(GetContentTypeAlias));

        if (predicate == null) {
            return all;
        } else {
            return all.Where(predicate).ToList();
        }
    }
    
    public IReadOnlyList<IPublishedContent> All(string contentTypeAlias,
                                                Func<IPublishedContent, bool> predicate = null) {
        var cacheKey = GetCacheKey(contentTypeAlias);

        var all = _untypedStore.GetOrAdd(cacheKey, _ => _contentLocator.All(contentTypeAlias));

        Hold(cacheKey, contentTypeAlias);
        Hold(cacheKey, all.Select(x => x.ContentType.Alias));

        if (predicate == null) {
            return all;
        } else {
            return all.Where(predicate).ToList();
        }
    }

    public void Flush() {
        _cacheKeysByContentType.Clear();
        _typedStore.Clear();
        _untypedStore.Clear();
        
        Flushed?.Invoke(this, EventArgs.Empty);
    }

    public void Flush(string contentTypeAlias) {
        if (_cacheKeysByContentType.TryRemove(contentTypeAlias, out var cacheKeys)) {
            foreach (var cacheKey in cacheKeys) {
                _typedStore.TryRemove(cacheKey, out _);
                _untypedStore.TryRemove(cacheKey, out _);
            }

            Flushed?.Invoke(this, EventArgs.Empty);
        }
    }

    public T Single<T>(Func<T, bool> predicate = null) {
        return All(predicate).SingleOrDefault();
    }

    public IPublishedContent Single(string contentTypeAlias, Func<IPublishedContent, bool> predicate = null) {
        return All(contentTypeAlias, predicate).SingleOrDefault();
    }

    public event EventHandler Flushed;

    private string GetCacheKey<T>() {
        // Not AliasHelper<T>.ContentTypeAlias() as need to distinguish T and TContent : UmbracoContent<TContent>
        return GetCacheKey(typeof(T).FullName);
    }
    
    private string GetCacheKey(string value) {
        var cacheKey = CacheKey.Generate<ContentCache>(value);
        
        return cacheKey;
    }

    private string GetContentTypeAlias<T>(T item) {
        if (item is IPublishedContent publishedContent) {
            return publishedContent.ContentType.Alias;
        } else if (item is IUmbracoContent umbracoContent) {
            return umbracoContent.Content()?.ContentType.Alias;
        } else {
            return null;
        }
    }

    private void Hold(string cacheKey, string contentTypeAlias) {
        if (contentTypeAlias.HasValue()) {
            _cacheKeysByContentType.GetOrAdd(contentTypeAlias, _ => []).AddIfNotExists(cacheKey);
        }
    }

    private void Hold(string cacheKey, IEnumerable<string> contentTypeAliases) {
        foreach (var contentTypeAlias in contentTypeAliases) {
            Hold(cacheKey, contentTypeAlias);
        }
    }
}
