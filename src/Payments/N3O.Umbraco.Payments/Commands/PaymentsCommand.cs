using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.Commands {
    public abstract class PaymentsCommand<TReq> : Request<TReq, None> {
        public FlowId FlowId { get; }

        protected PaymentsCommand(FlowId flowId) {
            FlowId = flowId;
        }
    }
}