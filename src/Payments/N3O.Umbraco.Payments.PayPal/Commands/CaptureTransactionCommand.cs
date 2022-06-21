using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.PayPal.Models;

namespace N3O.Umbraco.Payments.PayPal.Commands;

public class CaptureTransactionCommand : PaymentsCommand<PayPalTransactionReq, PayPalPayment> {
    public CaptureTransactionCommand(FlowId flowId) : base(flowId) { }
}
