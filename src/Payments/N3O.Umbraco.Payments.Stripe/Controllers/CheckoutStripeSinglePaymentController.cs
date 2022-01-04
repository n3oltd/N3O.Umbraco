// using N3O.Umbraco.Payments.Stripe.Models;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Payments.Stripe.Controllers {
//     public class CheckoutStripeSinglePaymentController : CheckoutSinglePaymentController<StripeSinglePaymentReq> {
//         private readonly IStripePayments _stripePayments;
//
//         public CheckoutStripeSinglePaymentController(IServiceProvider services, IStripePayments stripePayments)
//             : base(services) {
//             _stripePayments = stripePayments;
//         }
//
//         protected override Task<CheckoutStageRes> ExecuteAsync(StripeSinglePaymentReq req) {
//             _stripePayments.CheckoutSinglePayment(Checkout, req.PaymentIntentId, req.PaymentMethodId);
//
//             return Task.FromResult(Success());
//         }
//     }
// }