using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class UpdateContentPropertyCommand : Request<ContentPropertyReq, None> {
    public ContentId ContentId { get; }

    public UpdateContentPropertyCommand(ContentId contentId) {
        ContentId = contentId;
    }
}