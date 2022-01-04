// using System;
// using System.Collections.Generic;
//
// namespace N3O.Umbraco.Giving.Checkout.Models {
//     public class CheckoutState {
//         public CheckoutState() {
//             RequiredStages = new List<CheckoutStage>();
//             CompletedStages = new List<CheckoutStage>();
//         }
//
//         public Guid CartId { get; }
//         public string Reference { get; }
//         public CheckoutStage CurrentStage { get; }
//         public List<CheckoutStage> RequiredStages { get; }
//         public List<CheckoutStage> CompletedStages { get; }
//         public int CurrencyId { get; }
//         public Account Account { get; }
//         public RegularDonationCheckout RegularDonation { get; }
//         public SingleDonationCheckout SingleDonation { get; }
//     }
// }