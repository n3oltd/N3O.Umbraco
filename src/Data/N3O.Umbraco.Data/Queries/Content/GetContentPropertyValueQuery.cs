using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.Queries;

public class GetContentPropertyValueQuery : Request<None, ContentPropertyValueRes> {
    public ContentId ContentId { get; }
    public PropertyAlias PropertyAlias { get; }

    public GetContentPropertyValueQuery(ContentId contentId, PropertyAlias propertyAlias) {
        ContentId = contentId;
        PropertyAlias = propertyAlias;
    }
}