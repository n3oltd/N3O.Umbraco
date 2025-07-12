using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedLookup : Value {
    public string Id { get; set; }
    
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Id;
    }
}