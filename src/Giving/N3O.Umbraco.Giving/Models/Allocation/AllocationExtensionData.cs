using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class AllocationExtensionData {
    [JsonExtensionData]
    public IDictionary<string, object> JsonData { get; set; }
    
    [JsonIgnore]
    public IReadOnlyDictionary<string, object> Fields => JsonData.OrEmpty()
                                                                 .ToDictionary(jsonData => jsonData.Key,
                                                                               jsonData => (object) jsonData.Value);
    public T Get<T>(string key) {
        return Fields.ContainsKey(key) ? (T) Fields[key] : default;
    }
}