using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.TotalProcessing.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Commands;

public class PrepareCredentialCheckoutCommand : PaymentsCommand<PrepareCheckoutReq, TotalProcessingCredential> {
    public PrepareCredentialCheckoutCommand(FlowId flowId) : base(flowId) { }
}
