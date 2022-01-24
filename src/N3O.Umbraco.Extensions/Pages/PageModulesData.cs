using System.Collections;
using System.Collections.Generic;

namespace N3O.Umbraco.Pages {
    public class PageModulesData : IEnumerable<KeyValuePair<string, object>> {
        private readonly Dictionary<string, object> _dict = new();

        public void Add(string key, object value) {
            _dict[key] = value;
        }

        public T Get<T>(string key) {
            return _dict.ContainsKey(key) ? (T) _dict[key] : default;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>_dict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}