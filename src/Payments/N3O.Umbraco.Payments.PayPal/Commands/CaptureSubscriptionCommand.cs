using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.NamedParameters;
using N3O.Umbraco.Payments.PayPal.Models;
using N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;

namespace N3O.Umbraco.Payments.PayPal.Commands;

public class CaptureSubscriptionCommand : PaymentsCommand<PayPalSubscriptionReq, PayPalCredential> {
    public CaptureSubscriptionCommand(FlowId flowId) : base(flowId) { }
}