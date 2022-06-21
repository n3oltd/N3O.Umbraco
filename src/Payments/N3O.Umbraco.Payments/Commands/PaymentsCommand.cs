using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.NamedParameters;

namespace N3O.Umbraco.Payments.Commands;

public abstract class PaymentsCommand<TReq, TRes> : Request<TReq, PaymentFlowRes<TRes>> {
    protected PaymentsCommand(FlowId flowId) {
        FlowId = flowId;
    }
    
    public FlowId FlowId { get; }
}
