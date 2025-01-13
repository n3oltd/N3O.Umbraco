using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.GoCardless.Commands;

public class CompleteRedirectFlowCommand : PaymentsCommand<None, GoCardlessCredential> {
    public FlowId FlowId { get; }

    public CompleteRedirectFlowCommand(FlowId flowId) : base(flowId) {
        FlowId = flowId;
    }
}
