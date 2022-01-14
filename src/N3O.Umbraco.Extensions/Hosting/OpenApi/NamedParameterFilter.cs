using N3O.Umbraco.Extensions;
using N3O.Umbraco.Parameters;
using NJsonSchema.Generation;

namespace N3O.Umbraco.Hosting {
    public class NamedParameterFilter : TypeTransformationFilter {
        protected override void DoProcess(SchemaProcessorContext context) {
            var type = context.Type;

            if (type.ImplementsInterface<INamedParameter>()) {
                ModelAsString();
            }
        }
    }
}