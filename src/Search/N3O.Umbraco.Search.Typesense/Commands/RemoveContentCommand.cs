using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Search.Typesense.Commands;

public class RemoveContentCommand : Request<None, None> {
    public RemoveContentCommand(ContentId contentId, ContentType contentType) {
        ContentId = contentId;
        ContentType = contentType;
    }

    public ContentId ContentId { get; }
    public ContentType ContentType { get; }
}
