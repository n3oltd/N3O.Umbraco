using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class CompleteThreeDSecureFallbackCommand : PaymentsCommand<ThreeDSecureFallbackReq, OpayoPayment> {
        public CompleteThreeDSecureFallbackCommand(FlowId flowId) : base(flowId) { }
    }
}