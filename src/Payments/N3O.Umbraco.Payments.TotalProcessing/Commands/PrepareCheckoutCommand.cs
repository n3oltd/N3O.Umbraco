using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.TotalProcessing.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Commands;

public class PrepareCheckoutCommand : PaymentsCommand<PrepareCheckoutReq, TotalProcessingPayment> {
    public PrepareCheckoutCommand(FlowId flowId) : base(flowId) { }
}
