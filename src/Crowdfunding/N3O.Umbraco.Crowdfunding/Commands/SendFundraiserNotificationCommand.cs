using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class SendFundraiserNotificationCommand : Request<FundraiserNotificationReq, None> {
    public FundraiserId FundraiserId { get; }

    public SendFundraiserNotificationCommand(FundraiserId fundraiserId) {
        FundraiserId = fundraiserId;
    }
}