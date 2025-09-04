using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.Commands;

public class UpdateContentPropertyCommand : Request<ContentPropertyReq, None> {
    public ContentId ContentId { get; }

    public UpdateContentPropertyCommand(ContentId contentId) {
        ContentId = contentId;
    }
}