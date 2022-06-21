using NJsonSchema.Generation;
using System.Reflection;

namespace N3O.Umbraco.Hosting;

public class StringSchemaFilter : TypeTransformationFilter {
    protected override void DoProcess(SchemaProcessorContext context) {
        var type = context.ContextualType.Type;
        var attribute = type.GetCustomAttribute<StringSchemaAttribute>();

        if (attribute != null) {
            ModelAsString(null, attribute.ApiDescription);
        };
    }
}
