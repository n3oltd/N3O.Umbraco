using Humanizer;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Hosting {
    public class ExcludedPropertiesFilter : ISchemaProcessor {
        private readonly Type[] _excludedTypes = {
            typeof(JToken),
            typeof(IDictionary<string, JToken>),
            typeof(Dictionary<string, JToken>)
        };

        public void Process(SchemaProcessorContext context) {
            var type = context.Type;

            foreach (var property in type.GetProperties()) {
                if (property.HasAttribute<ExcludeFromSchemaAttribute>() ||
                    property.HasAttribute<JsonIgnoreAttribute>() ||
                    _excludedTypes.Contains(property.PropertyType)) {
                    context.Schema.Properties.Remove(property.Name.Camelize());
                }
            }
        }
    }
}
