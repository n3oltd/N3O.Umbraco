using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Stripe.Models;

namespace N3O.Umbraco.Payments.Stripe.Commands {
    public class ConfirmSetupIntentCommand : PaymentsCommand<None, StripeCredential> {
        public ConfirmSetupIntentCommand(FlowId flowId) : base(flowId) { }
    }
}