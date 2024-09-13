using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class UpdateFundraiserGoalsCommand : Request<UpdateFundraiserGoalsReq, None> {
    public ContentId ContentId { get; }

    public UpdateFundraiserGoalsCommand(ContentId contentId) {
        ContentId = contentId;
    }
}