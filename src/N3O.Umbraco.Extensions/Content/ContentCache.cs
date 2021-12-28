using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Content {
    public class ContentCache : IContentCache {
        private readonly IContentLocator _contentLocator;
        private readonly ConcurrentDictionary<string, object> _store = new();

        public ContentCache(IContentLocator contentLocator) {
            _contentLocator = contentLocator;
        }

        public IReadOnlyList<T> All<T>(Func<T, bool> predicate = null) {
            var cacheKey = GetCacheKey<T>();

            var all = (IReadOnlyList<T>) _store.GetOrAdd(cacheKey, _ => _contentLocator.All<T>());

            if (predicate == null) {
                return all;
            } else {
                return all.Where(predicate).ToList();
            }
        }

        public void Flush(IEnumerable<string> contentTypeAliases) {
            var prefixes = contentTypeAliases.Select(x => x + "_").ToList();
        
            _store.RemoveWhereKey(x => prefixes.Any(p => x.StartsWith(p, StringComparison.InvariantCultureIgnoreCase)));
        }

        public T Single<T>(Func<T, bool> predicate = null) {
            return All(predicate).SingleOrDefault();
        }

        private string GetCacheKey<T>() {
            var alias = AliasHelper.ForContentType<T>();
            var cacheKey = $"{alias}_{typeof(T).FullName}";

            return cacheKey;
        }
    }
}