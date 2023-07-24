using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Giving.Checkout.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Checkout.Commands;

public class UpdateAccountInformationCommand : Request<AccountInformationReq, CheckoutRes> {
    public UpdateAccountInformationCommand(CheckoutRevisionId checkoutRevisionId) {
        CheckoutRevisionId = checkoutRevisionId;
    }
    
    public CheckoutRevisionId CheckoutRevisionId { get; }
}
