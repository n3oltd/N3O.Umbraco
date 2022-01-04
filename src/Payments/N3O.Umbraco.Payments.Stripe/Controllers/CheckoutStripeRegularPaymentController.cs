// using N3O.Umbraco.Payments.Stripe.Models;
// using System;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Payments.Stripe.Controllers {
//     public class CheckoutStripeRegularPaymentController : CheckoutRegularPaymentController<StripeRegularPaymentReq> {
//         private readonly IStripePayments _stripePayments;
//
//         public CheckoutStripeRegularPaymentController(IServiceProvider services, IStripePayments stripePayments)
//             : base(services) {
//             _stripePayments = stripePayments;
//         }
//
//         protected override Task<CheckoutStageRes> ExecuteAsync(StripeRegularPaymentReq req) {
//             _stripePayments.CheckoutRegularPayment(Checkout, req.SetupIntentId, req.PaymentMethodId);
//
//             return Task.FromResult(Success());
//         }
//     }
// }