using System.Collections;
using System.Collections.Generic;

namespace N3O.Umbraco.Pages;

public class PageExtensionData : IEnumerable<KeyValuePair<string, object>> {
    private readonly Dictionary<string, object> _dict = new();

    public void Add(string key, object value) {
        _dict[key] = value;
    }

    public T Get<T>(string key) {
        return (T) _dict[key];
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>_dict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}