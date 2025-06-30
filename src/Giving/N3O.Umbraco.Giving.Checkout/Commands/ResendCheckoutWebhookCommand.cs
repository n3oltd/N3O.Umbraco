using N3O.Umbraco.Giving.Checkout.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Checkout.Commands;

public class ResendCheckoutWebhookCommand : Request<None, None> {
    public ResendCheckoutWebhookCommand(CheckoutRevisionId checkoutRevisionId) {
        CheckoutRevisionId = checkoutRevisionId;
    }
    
    public CheckoutRevisionId CheckoutRevisionId { get; }
}