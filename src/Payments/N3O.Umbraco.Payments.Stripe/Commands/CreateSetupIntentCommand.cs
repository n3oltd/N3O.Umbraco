using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Stripe.Models;

namespace N3O.Umbraco.Payments.Stripe.Commands {
    public class CreateSetupIntentCommand : PaymentsCommand<SetupIntentReq, StripeCredential> {
        public CreateSetupIntentCommand(FlowId flowId) : base(flowId) { }
    }
}