using N3O.Umbraco.Entities;
using NJsonSchema.Generation;

namespace N3O.Umbraco.Hosting {
    public class EntityIdFilter : TypeTransformationFilter {
        protected override void DoProcess(SchemaProcessorContext context) {
            var type = context.Type;

            if (type == typeof(EntityId)) {
                ModelAsString(EntityId.New().ToString(), "A well formed guid");
            }
        }
    }
}