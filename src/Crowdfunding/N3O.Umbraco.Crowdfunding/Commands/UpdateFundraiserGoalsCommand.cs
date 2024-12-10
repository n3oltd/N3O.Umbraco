using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class UpdateFundraiserGoalsCommand : Request<FundraiserGoalsReq, None> {
    public ContentId ContentId { get; set; }

    public UpdateFundraiserGoalsCommand(ContentId contentId) {
        ContentId = contentId;
    }
}