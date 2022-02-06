// using N3O.Umbraco.Entities;
// using N3O.Umbraco.Giving.Checkout.Commands;
// using NodaTime;
//
// namespace N3O.Umbraco.Giving.Checkout.ChangeFeeds {
//     public partial class CheckoutJobsFeed {
//         private void ProcessCompleteJobs(EntityChange<Entities.Checkout> entityChange) {
//             if (entityChange.Operation == EntityOperations.Update) {
//                 var isComplete = entityChange.SessionEntity.IsComplete;
//                 var wasComplete = entityChange.DatabaseEntity.IsComplete;
//
//                 if (isComplete && !wasComplete) {
//                     var checkout = entityChange.SessionEntity;
//                     
//                     ScheduleJob<DeleteCheckoutCommand>(checkout, Duration.FromDays(90));
//                 }
//             }
//         }
//     }
// }
