using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class DeactivateFundraiserCommand : Request<None, None> {
    public FundraiserId FundraiserId { get; set; }
    
    public DeactivateFundraiserCommand(FundraiserId fundraiserId) {
        FundraiserId = fundraiserId;
    }
}