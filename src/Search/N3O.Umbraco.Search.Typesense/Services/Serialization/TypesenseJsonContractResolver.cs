using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseJsonContractResolver : JsonContractResolver {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        var jsonProperty = base.CreateProperty(member, memberSerialization);

        var typesenseFieldName = TypesenseField.ToSearchFieldName(member);

        if (typesenseFieldName.HasValue()) {
            jsonProperty.PropertyName = typesenseFieldName;
        }

        var converter = TypesenseConverterRegistry.GetConverter(jsonProperty.PropertyType);

        if (converter != null) {
            var valueProvider = jsonProperty.ValueProvider;

            // A converter can produce null from a non-null value, which NullValueHandling cannot see
            jsonProperty.Converter = TypesenseDispatchJsonConverter.Instance;
            jsonProperty.ShouldSerialize = instance => converter.ToTypesenseValue(valueProvider?.GetValue(instance)) != null;
        }

        return jsonProperty;
    }
}
