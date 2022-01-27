using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.GoCardless.Commands {
    public class BeginRedirectFlowCommand : PaymentsCommand<None, GoCardlessCredential> {
        public BeginRedirectFlowCommand(FlowId flowId) : base(flowId) { }
    }
}