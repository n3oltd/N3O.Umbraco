using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Search.Typesense.Commands;

public class IndexContentCommand : Request<None, None> {
    public IndexContentCommand(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}