// using System.Collections.Generic;
//
// namespace N3O.Umbraco.Giving.Checkout.Models {
//     public class CheckoutStageRes : ValidatedRes {
//         private CheckoutStageRes() { }
//
//         private CheckoutStageRes(IEnumerable<ValidationErrorRes> errors) : base(errors) { }
//
//         public string NextUrl { get; private set; }
//
//         public CheckoutStageRes Success(string nextUrl) => new CheckoutStageRes {
//             NextUrl = nextUrl
//         };
//
//         public CheckoutStageRes Failure(params ValidationErrorRes[] errors) => new CheckoutStageRes(errors);
//     }
// }