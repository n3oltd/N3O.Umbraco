using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class DictionaryExtensions {
    public static void AddIfKeyNotExists<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                       TKey key,
                                                       TValue value) {
        if (!source.ContainsKey(key)) {
            source[key] = value;
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                 TKey key,
                                                 Func<TValue> addValue,
                                                 Func<TValue, TValue> updateValue) {
        if (source.ContainsKey(key)) {
            var currentValue = source[key];
            var newValue = updateValue(currentValue);

            source[key] = newValue;
        } else {
            var value = addValue();

            source[key] = value;
        }
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                TKey key,
                                                Func<TValue> addValue) {
        if (source.ContainsKey(key)) {
            return source[key];
        }

        var value = addValue();

        source[key] = value;

        return value;
    }

    public static TValue GetOrAddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                        TKey key,
                                                        Func<TValue> addValue,
                                                        Func<TValue, TValue> updateValue) {
        AddOrUpdate(source, key, addValue, updateValue);

        return source[key];
    }

    public static bool LacksKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) {
        return !source.ContainsKey(key);
    }

    public static void RemoveWhere<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                 Func<KeyValuePair<TKey, TValue>, bool> predicate) {
        lock (source) {
            var toRemove = source.Where(predicate);

            foreach (var item in toRemove) {
                source.Remove(item.Key);
            }
        }
    }

    public static void RemoveWhereKey<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                    Func<TKey, bool> predicate) {
        lock (source) {
            var toRemove = source.Where(x => predicate(x.Key));

            foreach (var item in toRemove) {
                source.Remove(item.Key);
            }
        }
    }

    public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) {
        if (source.TryGetValue(key, out var result)) {
            return result;
        }

        return default;
    }
}
