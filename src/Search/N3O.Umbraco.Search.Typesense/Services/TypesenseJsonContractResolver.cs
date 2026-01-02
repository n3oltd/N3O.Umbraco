using N3O.Umbraco.Json;
using N3O.Umbraco.Search.Typesense.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseJsonContractResolver : JsonContractResolver {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        var jsonProperty = base.CreateProperty(member, memberSerialization);
        
        var attribute = (member as PropertyInfo)?.GetCustomAttribute<FieldAttribute>();

        if (attribute != null) {
            jsonProperty.PropertyName = attribute.Name;
        }

        return jsonProperty;
    }
}