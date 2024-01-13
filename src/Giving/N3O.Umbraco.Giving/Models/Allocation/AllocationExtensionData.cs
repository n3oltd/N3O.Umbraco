using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class AllocationExtensionData  {

    public AllocationExtensionData(IDictionary<string, JToken> extensions) {
        JsonData = extensions.ToDictionary(x => x.Key, x=> x.Value.Value<object>());
    }
    
    public AllocationExtensionData(IDictionary<string, object> extensions) {
        JsonData = extensions;
    }

    public AllocationExtensionData() { }
    /*[JsonExtensionData]
    private readonly Dictionary<string, object> _dict = new();*/
    
    [JsonExtensionData]
    public IDictionary<string, object> JsonData { get; set; }
    
    [JsonIgnore]
    public IReadOnlyDictionary<string, string> Fields => JsonData.OrEmpty()
                                                                 .ToDictionary(jsonData => jsonData.Key,
                                                                               jsonData => (string) jsonData.Value);
    public void Add(string key, object value) {
        JsonData = new Dictionary<string, object>();
        
        JsonData[key] = value;
    }

    /*public T Get<T>(string key) {
        return JsonConvert.DeserializeObject<T>(JsonData.TryGetValue(key, out var val).ToString());
    }*/
    
    public T Get<T>(string key) {
        return (T) JsonData[key];
    }
}

/*public class AllocationExtensionData : IEnumerable<KeyValuePair<string, object>> {
    private readonly Dictionary<string, object> _dict = new();

    public void Add(string key, object value) {
        _dict[key] = value;
    }
    
    public bool ContainsKey(string key) {
        return _dict.ContainsKey(key);
    }

    public T Get<T>(string key) {
        return (T) _dict.GetValueOrDefault(key, default);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>_dict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}*/