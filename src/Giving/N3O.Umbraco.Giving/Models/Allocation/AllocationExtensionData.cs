using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class AllocationExtensionData  {
    [JsonExtensionData]
    public IDictionary<string, object> JsonData { get; set; }
    
    [JsonIgnore]
    public IReadOnlyDictionary<string, string> Fields => JsonData.OrEmpty()
                                                                 .ToDictionary(jsonData => jsonData.Key,
                                                                               jsonData => (string) jsonData.Value);
    public void Add(string key, object value) {
        JsonData = JsonData ?? new Dictionary<string, object>();
        
        JsonData[key] = value;
    }

    /*public T Get<T>(string key) {
        return JsonConvert.DeserializeObject<T>(JsonData.TryGetValue(key, out var val).ToString());
    }*/
    
    public T Get<T>(string key) {
        return (T) JsonData[key];
    }
}