using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace N3O.Umbraco.Json;

public class TypeOnlyContractResolver : CamelCasePropertyNamesContractResolver {
    private readonly Type _type;

    public TypeOnlyContractResolver(Type type) {
        _type = type;
    }
    
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        var property = base.CreateProperty(member, memberSerialization);
        
        property.ShouldSerialize = _ => property.DeclaringType == _type;
        
        return property;
    }
}