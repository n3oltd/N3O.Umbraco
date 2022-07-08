using N3O.Umbraco.Content;
using N3O.Umbraco.Data.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Data.Queries;

public class GetContentByIdQuery : Request<None, ContentRes> {
    public GetContentByIdQuery(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}
