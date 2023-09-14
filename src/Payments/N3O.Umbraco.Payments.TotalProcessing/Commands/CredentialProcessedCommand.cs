using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.TotalProcessing.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Commands;

public class CredentialProcessedCommand : PaymentsCommand<CheckoutCompletedReq, TotalProcessingCredential> {
    public CredentialProcessedCommand(FlowId flowId) : base(flowId) { }
}
