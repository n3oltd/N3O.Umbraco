using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations;

public interface IAllocationExtensionRequestBinder {
    void Bind(IDictionary<string, JToken> src, IDictionary<string, JToken> dest);
    
    string Key { get; }
}