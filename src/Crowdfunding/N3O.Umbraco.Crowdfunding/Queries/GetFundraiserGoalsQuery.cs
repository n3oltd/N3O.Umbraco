using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetFundraiserGoalsQuery : Request<None, FundraiserGoalsRes> {
    public FundraiserId FundraiserId { get; }

    public GetFundraiserGoalsQuery(FundraiserId fundraiserId) {
        FundraiserId = fundraiserId;
    }
}