using N3O.Umbraco.Content;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.Queries;

public class GetContentByIdQuery : Request<None, ContentRes> {
    public GetContentByIdQuery(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}
