// using N3O.Umbraco.Giving.Checkout.Lookups;
// using N3O.Umbraco.Giving.Checkout.Models;
// using System;
// using System.Collections.Generic;
//
// namespace N3O.Umbraco.Giving.Checkout {
//     public interface ICheckout {
//         Guid CartId { get; }
//         Account Account { get; }
//         string Reference { get; }
//         CheckoutStage CurrentStage { get; }
//         IReadOnlyList<CheckoutStage> RequiredStages { get; }
//         IReadOnlyList<CheckoutStage> CompletedStages { get; }
//         Currency Currency { get; }
//         RegularDonationCheckout RegularDonation { get; }
//         SingleDonationCheckout SingleDonation { get; }
//         string TotalText { get; }
//         Money TotalIncome { get; }
//         bool IsRequired(CheckoutStage stage);
//         bool IsCompleted(CheckoutStage stage);
//         CheckoutStage MoveToNextStage();
//         void Save();
//         void Delete();
//         bool HasSingleDonation();
//         bool HasRegularDonation();
//         void Start();
//     }
// }