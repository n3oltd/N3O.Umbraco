// using N3O.Umbraco.Financial;
// using Stripe;
//
// namespace N3O.Umbraco.Payments.Stripe {
//     public interface IStripePayments {
//         void CheckoutSinglePayment(ICheckout checkout, string paymentIntentId, string paymentMethodId);
//         void CheckoutRegularPayment(ICheckout checkout, string setupIntentId, string paymentMethodId);
//         PaymentIntent CreatePaymentIntent(Money value);
//         SetupIntent CreateSetupIntent();
//         PaymentMethodBillingDetails GetBillingDetails(ICheckout checkout);
//
//         string ClientKey { get; }
//     }
// }