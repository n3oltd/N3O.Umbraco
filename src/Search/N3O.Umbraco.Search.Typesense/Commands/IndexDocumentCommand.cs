using N3O.Umbraco.Mediator;
using N3O.Umbraco.Search.Typesense.NamedParameters;

namespace N3O.Umbraco.Search.Typesense.Commands;

public class IndexDocumentCommand : Request<None, None> {
    public IndexDocumentCommand(ContentId contentId) {
        ContentId = contentId;
    }
    
    public ContentId ContentId { get; }
}