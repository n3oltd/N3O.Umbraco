// using Microsoft.AspNetCore.Mvc;
// using System.Net;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     public abstract class CheckoutController<TReq> : ApiController {
//         protected CheckoutController(IServiceProvider services) {
//             Checkout = services.GetRequiredService<ICheckout>();
//             CheckoutStageUrl = services.GetRequiredService<ICheckoutStageUrl>();
//         }
//
//         [HttpPost]
//
//         public async Task<CheckoutStageRes> Submit(TReq req) {
//             if (Checkout.CurrentStage != Stage) {
//                 throw new HttpResponseException(HttpStatusCode.Unauthorized);
//             }
//
//             OnExecuting(req);
//
//             var res = await ExecuteAsync(req);
//
//             Checkout.Save();
//
//             return res;
//         }
//
//         protected CheckoutStageRes Success() {
//             Checkout.MoveToNextStage();
//
//             var nextUrl = CheckoutStageUrl.Get(Checkout.CurrentStage);
//
//             return CheckoutStageRes.Success(nextUrl);
//         }
//
//         protected virtual void OnExecuting(TReq req) { }
//
//         protected abstract Task<CheckoutStageRes> ExecuteAsync(TReq req);
//         protected abstract CheckoutStage Stage { get; }
//
//         protected ICheckout Checkout { get; }
//         protected ICheckoutStageUrl CheckoutStageUrl { get; }
//     }
// }