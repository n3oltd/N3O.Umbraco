using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.GoCardless.Models;
using N3O.Umbraco.Payments.GoCardless.NamedParameters;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.GoCardless.Commands;

public class CompleteRedirectFlowCommand : PaymentsCommand<None, GoCardlessCredential> {
    public RedirectFlowId RedirectFlowId { get; }

    public CompleteRedirectFlowCommand(FlowId flowId, RedirectFlowId redirectFlowId) : base(flowId) {
        RedirectFlowId = redirectFlowId;
    }
}
