using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.TotalProcessing.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Commands;

public class PaymentProcessedCommand : PaymentsCommand<CheckoutCompletedReq, TotalProcessingPayment> {
    public PaymentProcessedCommand(FlowId flowId) : base(flowId) { }
}