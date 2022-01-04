// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     public abstract class CheckoutRegularPaymentController<TReq> : CheckoutController<TReq>
//         where TReq : CheckoutRegularPaymentReq {
//         protected CheckoutRegularPaymentController(IServiceProvider services) : base(services) { }
//
//         protected override void OnExecuting(TReq req) {
//             Checkout.RegularDonation.Reference = $"RD-{Checkout.Reference}";
//             Checkout.RegularDonation.PaymentDayOfMonth = req.PaymentDay ?? 1;
//         }
//
//         protected override CheckoutStage Stage => CheckoutStages.RegularPayment;
//     }
// }