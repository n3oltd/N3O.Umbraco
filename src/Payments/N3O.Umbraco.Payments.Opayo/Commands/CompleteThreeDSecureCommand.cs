using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class CompleteThreeDSecureCommand : PaymentsCommand<CompleteThreeDSecureReq, OpayoPayment> {
        public CompleteThreeDSecureCommand(FlowId flowId) : base(flowId) { }
    }
}