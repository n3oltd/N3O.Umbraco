using System.Collections.Concurrent;

namespace N3O.Umbraco.Extensions;

public static class ConcurrentDictionaryExtensions {
    public static TValue TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> source, TKey key) {
        if (source.TryRemove(key, out var result)) {
            return result;
        }

        return default;
    }
}
