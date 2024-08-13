using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetNestedPropertySchemaQuery : Request<None, NestedPropertySchemaRes> {
    public ContentId ContentId { get; }
    public PropertyAlias PropertyAlias { get; }

    public GetNestedPropertySchemaQuery(ContentId contentId, PropertyAlias propertyAlias) {
        ContentId = contentId;
        PropertyAlias = propertyAlias;
    }
}