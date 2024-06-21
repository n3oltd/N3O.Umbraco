using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class UpdatePagePropertyCommand : Request<PagePropertyReq, None> {
    public PageId PageId { get; }

    public UpdatePagePropertyCommand(PageId pageId) {
        PageId = pageId;
    }
}