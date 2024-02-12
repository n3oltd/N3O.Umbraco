using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Models;

public class AllocationExtensionData  {
    [JsonExtensionData]
    public IDictionary<string, object> JsonData { get; set; }
    
    public void Add(string key, object value) {
        JsonData ??= new Dictionary<string, object>();
        
        JsonData[key] = value;
    }

    public bool ContainsKey(string key) {
        return JsonData.ContainsKey(key);
    }

    public T Get<T>(IJsonProvider jsonProvider, string key) {
        return jsonProvider.DeserializeObject<T>(JsonData[key].ToString());
    }
}