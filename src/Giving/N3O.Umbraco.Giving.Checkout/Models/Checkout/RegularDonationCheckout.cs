// using Newtonsoft.Json;
//
// namespace N3O.Umbraco.Giving.Checkout.Models {
//     public class RegularDonationCheckout : DonationCheckoutBase {
//         public BankAccount BankDetails { get; }
//         public PaymentCardToken CardTokenDetails { get; }
//         public RecurringPaymentMethod PaymentMethod { get; }
//         public int PaymentDayOfMonth { get; }
//
//         [JsonIgnore]
//         public override Money TotalIncome => new(Value.Amount * CheckoutConstants.RegularIncomeMultiplier,
//                                                  Value.Currency);
//     }
// }