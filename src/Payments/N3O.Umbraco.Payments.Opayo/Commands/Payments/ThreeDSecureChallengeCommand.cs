using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.Opayo.Models;

namespace N3O.Umbraco.Payments.Opayo.Commands {
    public class ThreeDSecureChallengeCommand : PaymentsCommand<ThreeDSecureChallengeReq, OpayoPayment> {
        public ThreeDSecureChallengeCommand(FlowId flowId) : base(flowId) { }
    }
}