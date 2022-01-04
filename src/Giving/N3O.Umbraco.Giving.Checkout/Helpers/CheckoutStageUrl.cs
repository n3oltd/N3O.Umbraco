// using N3O.Umbraco.Content;
// using N3O.Umbraco.Giving.Checkout.Lookups;
// using System;
//
// namespace N3O.Umbraco.Giving.Checkout {
//     public class CheckoutStageUrl : ICheckoutStageUrl {
//         private readonly IContentLocator _contentLocator;
//
//         public CheckoutStageUrl(IContentLocator contentLocator) {
//             _contentLocator = contentLocator;
//         }
//
//         public string Get(CheckoutStage stage) {
//             if (stage == CheckoutStages.Account) {
//                 return _contentLocator.Single<ICheckoutAccountPage>()
//                                       .Url();
//             } else if (stage == CheckoutStages.SinglePayment) {
//                 return _contentLocator.Single<ICheckoutSinglePaymentPage>()
//                                       .Url();
//             } else if (stage == CheckoutStages.RegularPayment) {
//                 return _contentLocator.Single<ICheckoutRegularPaymentPage>()
//                                       .Url();
//             } else if (stage == CheckoutStages.Complete) {
//                 return _contentLocator.Single<ICheckoutCompletePage>()
//                                       .Url();
//             } else {
//                 throw new InvalidOperationException($"Unrecognised value {stage} for {nameof(stage)}");
//             }
//         }
//     }
// }