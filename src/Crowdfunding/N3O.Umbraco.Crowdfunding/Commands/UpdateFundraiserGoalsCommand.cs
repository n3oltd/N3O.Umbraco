using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class UpdateFundraiserGoalsCommand : Request<FundraiserGoalsReq, None> {
    public FundraiserId FundraiserId { get; set; }

    public UpdateFundraiserGoalsCommand(FundraiserId fundraiserId) {
        FundraiserId = fundraiserId;
    }
}