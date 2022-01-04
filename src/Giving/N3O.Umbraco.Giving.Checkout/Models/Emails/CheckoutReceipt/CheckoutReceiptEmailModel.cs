// using N3O.Umbraco.Localization;
// using N3O.Umbraco.TaxRelief.Lookups;
// using System;
// using System.Collections.Generic;
// using System.Linq;
//
// namespace N3O.Umbraco.Giving.Checkout.Models {
//     public class CheckoutReceiptEmailModel {
//         private readonly IFormatter _formatter;
//
//         public CheckoutReceiptEmailModel(ILocalClock localClock, IFormatter formatter, Checkout checkout) {
//             _formatter = formatter;
//             var today = localClock.GetLocalToday();
//
//             DateText = formatter.DateTime.FormatDate(today);
//             Reference = checkout.Reference;
//             Account = checkout.Account;
//
//             TaxPayer = checkout.Account.TaxStatus == TaxStatuses.Payer;
//             TaxStatusUnspecified = checkout.Account.TaxStatus == TaxStatuses.NotSpecified;
//
//             TotalText = checkout.TotalText;
//
//             if (checkout.HasRegularDonation()) {
//                 RegularDonation = GetRegularDonation(checkout);
//             }
//
//             if (checkout.HasSingleDonation()) {
//                 SingleDonation = GetSingleDonation(checkout);
//             }
//         }
//
//         private CheckoutReceiptEmailDonation GetRegularDonation(Checkout checkout) {
//             var title = _formatter.Text.Format<Strings>(s => s.RegularDonation);
//             var totalText = checkout.RegularDonation.Cart.TotalText;
//
//             string paymentMethod;
//             switch (checkout.RegularDonation.PaymentMethod) {
//                 case RecurringPaymentMethod.GoCardless:
//                     paymentMethod = _formatter.Text.Format<Strings>(s => s.DirectDebit);
//                     break;
//
//                 case RecurringPaymentMethod.PaymentCardToken:
//                     paymentMethod = _formatter.Text.Format<Strings>(s => s.Card);
//                     break;
//
//                 default:
//                     throw new NotImplementedException();
//             }
//
//             var allocations = GetAllocations(checkout.RegularDonation.Cart, title);
//
//             var donation = new CheckoutReceiptEmailDonation(title, totalText, paymentMethod, allocations);
//
//             return donation;
//         }
//
//         private CheckoutReceiptEmailDonation GetSingleDonation(Checkout checkout) {
//             var title = _formatter.Text.Format<Strings>(s => s.SingleDonation);
//             var totalText = checkout.SingleDonation.Cart.TotalText;
//
//             string paymentMethod;
//             switch (checkout.SingleDonation.PaymentMethod) {
//                 case SinglePaymentMethod.Free:
//                     paymentMethod = _formatter.Text.Format<Strings>(s => s.Free);
//                     break;
//
//                 case SinglePaymentMethod.Paypal:
//                     paymentMethod = _formatter.Text.Format<Strings>(s => s.PayPal);
//                     break;
//
//                 case SinglePaymentMethod.WebPaymentCard:
//                     paymentMethod = _formatter.Text.Format<Strings>(s => s.Card);
//                     break;
//
//                 default:
//                     throw new NotImplementedException();
//             }
//
//             var allocations = GetAllocations(checkout.SingleDonation.Cart, title);
//
//             var donation = new CheckoutReceiptEmailDonation(title, totalText, paymentMethod, allocations);
//
//             return donation;
//         }
//
//         private IReadOnlyList<CheckoutReceiptEmailAllocation> GetAllocations(LiteCart cart, string donationType) {
//             var allocations = new List<CheckoutReceiptEmailAllocation>();
//
//             foreach (var cartAllocation in cart.Allocations) {
//                 var allocation = new CheckoutReceiptEmailAllocation(cartAllocation.Summary,
//                                                                     donationType,
//                                                                     cartAllocation.FundDimension1,
//                                                                     cartAllocation.FundDimension2,
//                                                                     cartAllocation.FundDimension3,
//                                                                     cartAllocation.FundDimension4,
//                                                                     cartAllocation.Value,
//                                                                     cartAllocation.ValueText);
//
//                 allocations.Add(allocation);
//             }
//
//             return allocations;
//         }
//
//         public string DateText { get; }
//         public string Reference { get; }
//         public string TotalText { get; }
//         public Account Account { get; }
//         public bool TaxPayer { get; }
//         public bool TaxStatusUnspecified { get; }
//
//         public CheckoutReceiptEmailDonation SingleDonation { get; }
//         public CheckoutReceiptEmailDonation RegularDonation { get; }
//
//         public IEnumerable<CheckoutReceiptEmailDonation> Donations {
//             get {
//                 if (SingleDonation != null) {
//                     yield return SingleDonation;
//                 }
//
//                 if (RegularDonation != null) {
//                     yield return RegularDonation;
//                 }
//             }
//         }
//
//         public IEnumerable<CheckoutReceiptEmailAllocation> Allocations =>
//             Donations.SelectMany(x => x.Allocations);
//
//         public class Strings : CodeStrings {
//             public string Card => "Card";
//             public string DirectDebit => "Direct Debit";
//             public string Free => "Free";
//             public string PayPal => "PayPal";
//             public string RegularDonation => "Regular Donation";
//             public string SingleDonation => "Single Donation";
//         }
//     }
// }