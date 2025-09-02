using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Search.Typesense.Commands;

public class IndexContentsOfTypeCommand : Request<None, None> {
    public IndexContentsOfTypeCommand(ContentType contentType) {
        ContentType = contentType;
    }
    
    public ContentType ContentType { get; }
}