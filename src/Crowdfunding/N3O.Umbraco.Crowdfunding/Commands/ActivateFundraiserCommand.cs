using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class ActivateFundraiserCommand : Request<None, None> {
    public FundraiserId FundraiserId { get; set; }
    
    public ActivateFundraiserCommand(FundraiserId fundraiserId) {
        FundraiserId = fundraiserId;
    }
}