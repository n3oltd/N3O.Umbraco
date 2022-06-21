using System.Collections;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Models;

public class MultiDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, List<TValue>> {
    private readonly Dictionary<TKey, List<TValue>> _dict = new();

    public void Add(TKey key, TValue value) {
        if (_dict.TryGetValue(key, out var valueList)) {
            valueList.Add(value);
        } else {
            valueList = new List<TValue>();
            valueList.Add(value);
            _dict.Add(key, valueList);
        }
    }

    public bool Remove(TKey key, TValue value) {
        if (_dict.TryGetValue(key, out var valueList)) {
            if (valueList.Remove(value)) {
                if (valueList.Count == 0) {
                    _dict.Remove(key);
                }

                return true;
            }
        }

        return false;
    }

    public int RemoveAll(TKey key, TValue value) {
        var n = 0;

        if (_dict.TryGetValue(key, out var valueList)) {
            while (valueList.Remove(value)) {
                n++;
            }

            if (valueList.Count == 0) {
                _dict.Remove(key);
            }
        }

        return n;
    }

    public int CountAll {
        get {
            int n = 0;

            foreach (var valueList in _dict.Values) {
                n += valueList.Count;
            }

            return n;
        }
    }

    public bool Contains(TKey key, TValue value) {
        if (_dict.TryGetValue(key, out var valueList)) {
            return valueList.Contains(value);
        }

        return false;
    }

    public bool Contains(TValue value) {
        foreach (var valueList in _dict.Values) {
            if (valueList.Contains(value)) {
                return true;
            }
        }

        return false;
    }

    public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator() => _dict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _dict.Count;

    public bool ContainsKey(TKey key) => _dict.ContainsKey(key);

    public bool TryGetValue(TKey key, out List<TValue> value) => _dict.TryGetValue(key, out value);

    public List<TValue> this[TKey key] => _dict[key];

    public IEnumerable<TKey> Keys => _dict.Keys;

    public IEnumerable<List<TValue>> Values => _dict.Values;
}
