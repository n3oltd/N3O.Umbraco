using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class CompleteThreeDSecureChallengeCommand : PaymentsCommand<ThreeDSecureChallengeReq, OpayoPayment> {
        public CompleteThreeDSecureChallengeCommand(FlowId flowId) : base(flowId) { }
    }
}