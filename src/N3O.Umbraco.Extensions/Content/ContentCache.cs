using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public class ContentCache : IContentCache {
        private readonly IContentLocator _contentLocator;
        private readonly ConcurrentDictionary<string, object> _typedStore = new(StringComparer.InvariantCultureIgnoreCase);
        private readonly ConcurrentDictionary<string, IReadOnlyList<IPublishedContent>> _untypedStore = new(StringComparer.InvariantCultureIgnoreCase);

        public ContentCache(IContentLocator contentLocator) {
            _contentLocator = contentLocator;
        }

        public IReadOnlyList<T> All<T>(Func<T, bool> predicate = null) {
            var cacheKey = GetCacheKey<T>();

            var all = (IReadOnlyList<T>) _typedStore.GetOrAdd(cacheKey, _ => _contentLocator.All<T>());

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

            if (predicate == null) {
                return all;
            } else {
                return all.Where(predicate).ToList();
            }
        }

        public void Flush(IEnumerable<string> contentTypeAliases) {
            var prefixes = contentTypeAliases.Select(x => x + "_").ToList();
        
            _typedStore.RemoveWhereKey(x => prefixes.Any(p => x.StartsWith(p, StringComparison.InvariantCultureIgnoreCase)));
            _untypedStore.RemoveWhereKey(x => prefixes.Any(p => x.StartsWith(p, StringComparison.InvariantCultureIgnoreCase)));
        }

        public T Single<T>(Func<T, bool> predicate = null) {
            return All(predicate).SingleOrDefault();
        }
        
        public IPublishedContent Single(string contentTypeAlias, Func<IPublishedContent, bool> predicate = null) {
            return All(contentTypeAlias, predicate).SingleOrDefault();
        }

        private string GetCacheKey<T>() {
            var contentTypeAlias = AliasHelper<T>.ContentTypeAlias();
            var cacheKey = $"{contentTypeAlias}_{typeof(T).FullName}";
            
            return cacheKey;
        }

        private string GetCacheKey(string contentTypeAlias) {
            return contentTypeAlias;
        }
    }
}