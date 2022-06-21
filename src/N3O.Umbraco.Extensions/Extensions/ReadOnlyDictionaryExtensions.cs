using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class ReadOnlyDictionaryExtensions {
    public static IReadOnlyList<TKey> GetKeys<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source) {
        return source.Select(i => i.Key).ToList();
    }

    public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key) {
        return GetOrOther(source, key, default);
    }

    public static TValue GetOrOther<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source,
                                                  TKey key,
                                                  TValue other) {
        if (source.ContainsKey(key)) {
            return source[key];
        }

        return other;
    }

    public static IReadOnlyList<TValue> GetValues<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source) {
        return source.Select(i => i.Value).ToList();
    }
}
