using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Extensions;

public static class AllocationExtensionDataExtensions {
    public static IDictionary<string, JToken> BindAll(this IDictionary<string, JToken> req,
                                                      IEnumerable<IAllocationExtensionRequestBinder> binders) {
        var extensionData = new Dictionary<string, JToken>();

        foreach (var binder in binders.Where(req.CanBind)) {
            binder.Bind(req, extensionData);
        }

        return extensionData;
    }
    
    public static bool CanBind(this IDictionary<string, JToken> req, IAllocationExtensionRequestBinder binder) {
        return req.ContainsKey(binder.Key);
    }
    
    public static bool CanValidate(this IDictionary<string, JToken> req,
                                   IAllocationExtensionRequestValidator validator) {
        return req?.ContainsKey(validator.Key) == true;
    }

    public static bool HasFor(this IDictionary<string, JToken> extensionData, string key) {
        return extensionData.ContainsKey(key);
    }
    
    public static T Get<T>(this IDictionary<string, JToken> extensionData, IJsonProvider jsonProvider, string key) {
        var serializerSettings = jsonProvider.GetSettings();
        var jsonSerializer = JsonSerializer.Create(serializerSettings);
        
        return extensionData[key].ToObject<T>(jsonSerializer);
    }
    
    public static void Set<T>(this IDictionary<string, JToken> extensionData,
                              IJsonProvider jsonProvider,
                              string key,
                              T value) {
        var serializerSettings = jsonProvider.GetSettings();
        var jsonSerializer = JsonSerializer.Create(serializerSettings);
        
        extensionData[key] = JObject.FromObject(value, jsonSerializer);
    }
}