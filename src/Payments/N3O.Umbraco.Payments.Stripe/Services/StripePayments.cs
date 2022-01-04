// using N3O.Umbraco.Payments.Stripe.Models;
// using Stripe;
// using Address = Stripe.Address;
//
// namespace N3O.Umbraco.Payments.Stripe {
//     public class StripePayments : IStripePayments {
//         private readonly StripeKeys _stripeKeys;
//
//         public StripePayments(StripeKeys stripeKeys) {
//             _stripeKeys = stripeKeys;
//         }
//
//         public void CheckoutSinglePayment(ICheckout checkout, string paymentIntentId, string paymentMethodId) {
//             var stripeCard = GetCard(paymentMethodId);
//
//             checkout.SingleDonation.PaymentMethod = SinglePaymentMethod.WebPaymentCard;
//             checkout.SingleDonation.CardDetails = new WebPaymentCardDetails();
//             checkout.SingleDonation.CardDetails.CardType = GetCardType(stripeCard);
//             checkout.SingleDonation.CardDetails.Cardholder = "Stripe";
//             checkout.SingleDonation.CardDetails.ExpiryMonth = (int) stripeCard.ExpMonth;
//             checkout.SingleDonation.CardDetails.ExpiryYear = (int) stripeCard.ExpYear;
//             checkout.SingleDonation.CardDetails.LastFourDigits = stripeCard.Last4;
//             checkout.SingleDonation.CardDetails.AcquirerAuthorisationCode = paymentIntentId;
//         }
//
//         public void CheckoutRegularPayment(ICheckout checkout, string setupIntentId, string paymentMethodId) {
//             var stripeCard = GetCard(paymentMethodId);
//
//             checkout.RegularDonation.PaymentMethod = RecurringPaymentMethod.PaymentCardToken;
//             checkout.RegularDonation.CardTokenDetails = new PaymentCardToken();
//             checkout.RegularDonation.CardTokenDetails.CardType = GetCardType(stripeCard);
//             checkout.RegularDonation.CardTokenDetails.ExpiryMonth = (int) stripeCard.ExpMonth;
//             checkout.RegularDonation.CardTokenDetails.ExpiryYear = (int) stripeCard.ExpYear;
//             checkout.RegularDonation.CardTokenDetails.LastFourDigits = stripeCard.Last4;
//             checkout.RegularDonation.CardTokenDetails.Token = $"{paymentMethodId}|{setupIntentId}";
//         }
//
//         public PaymentIntent CreatePaymentIntent(Money value) {
//             var options = new PaymentIntentCreateOptions();
//             options.Amount = value.GetAmountInLowestDenomination();
//             options.Currency = value.Currency.Name;
//
//             var service = new PaymentIntentService();
//
//             var paymentIntent = service.Create(options, RequestOptions);
//
//             return paymentIntent;
//         }
//
//         public SetupIntent CreateSetupIntent() {
//             var options = new SetupIntentCreateOptions();
//
//             options.PaymentMethodTypes = "card".Yield()
//                                                .ToList();
//
//             options.Usage = "off_session";
//
//             var service = new SetupIntentService();
//
//             var setupIntent = service.Create(options, RequestOptions);
//
//             return setupIntent;
//         }
//
//         public PaymentMethodBillingDetails GetBillingDetails(ICheckout checkout) {
//             var billingDetails = new PaymentMethodBillingDetails();
//
//             billingDetails.Name = checkout.Account.Name.ToString();
//             billingDetails.Email = checkout.Account.Email.ToString();
//             billingDetails.Phone = checkout.Account.Telephone.ToString();
//
//             billingDetails.Address = new Address();
//             billingDetails.Address.Line1 = checkout.Account.Address.Line1;
//             billingDetails.Address.Line2 = checkout.Account.Address.Line2;
//             billingDetails.Address.City = checkout.Account.Address.Locality;
//             billingDetails.Address.State = checkout.Account.Address.AdministrativeArea;
//             billingDetails.Address.PostalCode = checkout.Account.Address.PostalCode;
//             billingDetails.Address.Country = checkout.Account.Address.Country.Iso2Code;
//
//             return billingDetails;
//         }
//
//         public string ClientKey => _stripeKeys.Client;
//
//         private RequestOptions RequestOptions {
//             get {
//                 var requestOptions = new RequestOptions();
//                 requestOptions.ApiKey = _stripeKeys.Secret;
//
//                 return requestOptions;
//             }
//         }
//     }
// }