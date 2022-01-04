// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     public abstract class CheckoutSinglePaymentController<TReq> : CheckoutController<TReq>
//         where TReq : CheckoutSinglePaymentReq {
//         protected CheckoutSinglePaymentController(IServiceProvider services) : base(services) { }
//
//         protected override void OnExecuting(TReq req) {
//             Checkout.SingleDonation.Reference = $"SD-{Checkout.Reference}";
//         }
//
//         protected override CheckoutStage Stage => CheckoutStages.SinglePayment;
//     }
// }