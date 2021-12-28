using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace N3O.Umbraco.Json {
    public class JsonContractResolver : CamelCasePropertyNamesContractResolver {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
            var jProperty = base.CreateProperty(member, memberSerialization);

            jProperty.Writable = jProperty.Writable || (member as PropertyInfo)?.SetMethod != null;

            return jProperty;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
            var props = base.CreateProperties(type, memberSerialization);

            return props;
        }
    }
}
