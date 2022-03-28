using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Bambora.Models.ThreeDSecureChallenge;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.Bambora.Commands {
    public class CompleteThreeDSecureChallengeCommand : PaymentsCommand<ThreeDSecureChallengeReq, BamboraPayment> {
        public CompleteThreeDSecureChallengeCommand(FlowId flowId) : base(flowId) { }
    }
}