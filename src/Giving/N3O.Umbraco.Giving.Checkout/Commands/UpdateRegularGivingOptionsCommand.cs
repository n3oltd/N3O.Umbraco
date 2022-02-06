using N3O.Umbraco.Giving.Checkout.Models;
using N3O.Umbraco.Giving.Checkout.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Giving.Checkout.Commands {
    public class UpdateRegularGivingOptionsCommand : Request<RegularGivingOptionsReq, CheckoutRes> {
        public UpdateRegularGivingOptionsCommand(CheckoutRevisionId checkoutRevisionId) {
            CheckoutRevisionId = checkoutRevisionId;
        }
        
        public CheckoutRevisionId CheckoutRevisionId { get; }
    }
}