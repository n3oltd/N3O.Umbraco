// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     [OutputCache(Duration = 0, NoStore = true, Location = OutputCacheLocation.None)]
//     public abstract class CheckoutPageController : DefaultController {
//         protected CheckoutPageController(IServiceProvider services) : base(services) {
//             Cart = services.GetRequiredService<ICart>();
//             Checkout = services.GetRequiredService<ICheckout>();
//             CheckoutStageUrl = services.GetRequiredService<ICheckoutStageUrl>();
//         }
//
//         protected override ActionResult Render(ContentModel model) {
//             var actionResult = Validate();
//
//             if (actionResult != null) {
//                 return actionResult;
//             }
//
//             return ExecuteAsync(model)
//                    .GetAwaiter()
//                    .GetResult();
//         }
//
//         protected virtual Task<ActionResult> ExecuteAsync(ContentModel model) {
//             return Task.FromResult(base.Render(model));
//         }
//
//         protected ActionResult Success() {
//             Checkout.MoveToNextStage();
//
//             var nextUrl = CheckoutStageUrl.Get(Checkout.CurrentStage);
//
//             Checkout.Save();
//
//             return new RedirectResult(nextUrl);
//         }
//
//         private ActionResult Validate() {
//             if (Stage != CheckoutStages.Complete) {
//                 if (Cart.IsEmpty()) {
//                     return new RedirectToUmbracoPageResult(ContentLocator.Single<IDonatePage>());
//                 }
//             }
//
//             if (Checkout.CurrentStage != Stage) {
//                 return new RedirectToUmbracoPageResult(ContentLocator.Single<ICheckoutStartPage>());
//             }
//
//             return null;
//         }
//
//         protected abstract CheckoutStage Stage { get; }
//
//         protected ICart Cart { get; }
//         protected ICheckout Checkout { get; }
//         protected ICheckoutStageUrl CheckoutStageUrl { get; }
//     }
// }