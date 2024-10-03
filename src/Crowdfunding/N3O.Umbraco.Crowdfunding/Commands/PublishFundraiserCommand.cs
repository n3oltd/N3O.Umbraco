using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class PublishFundraiserCommand : Request<None, None> {
    public FundraiserId FundraiserId { get; set; }
    
    public PublishFundraiserCommand(FundraiserId fundraiserId) {
        FundraiserId = fundraiserId;
    }
}