using N3O.Umbraco.Entities;
using NJsonSchema.Generation;

namespace N3O.Umbraco.Hosting {
    public class RevisionIdFilter : TypeTransformationFilter {
        protected override void DoProcess(SchemaProcessorContext context) {
            var type = context.ContextualType.Type;

            if (type == typeof(RevisionId)) {
                ModelAsString($"{new RevisionId(EntityId.New(), 1)}", "A well formed revision ID string");
            }
        }
    }
}