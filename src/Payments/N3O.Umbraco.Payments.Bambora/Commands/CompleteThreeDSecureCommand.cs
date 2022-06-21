using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.Bambora.Commands;

public class CompleteThreeDSecureCommand : PaymentsCommand<CompleteThreeDSecureReq, BamboraPayment> {
    public CompleteThreeDSecureCommand(FlowId flowId) : base(flowId) { }
}
