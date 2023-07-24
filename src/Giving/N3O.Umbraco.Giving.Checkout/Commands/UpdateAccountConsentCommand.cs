using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Giving.Checkout.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Checkout.Commands;

public class UpdateAccountConsentCommand : Request<ConsentReq, CheckoutRes> {
    public UpdateAccountConsentCommand(CheckoutRevisionId checkoutRevisionId) {
        CheckoutRevisionId = checkoutRevisionId;
    }
    
    public CheckoutRevisionId CheckoutRevisionId { get; }
}
