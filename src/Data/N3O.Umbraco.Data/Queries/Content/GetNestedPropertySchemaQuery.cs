using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Queries;

public class GetNestedPropertySchemaQuery : Request<None, NestedSchemaRes> {
    public ContentId ContentId { get; }
    public PropertyAlias PropertyAlias { get; }

    public GetNestedPropertySchemaQuery(ContentId contentId, PropertyAlias propertyAlias) {
        ContentId = contentId;
        PropertyAlias = propertyAlias;
    }
}