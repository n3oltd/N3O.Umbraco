// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     public class CheckoutStartPageController : CheckoutPageController {
//         public CheckoutStartPageController(IServiceProvider services) : base(services) { }
//
//         protected override ActionResult Render(ContentModel model) {
//             Checkout.Delete();
//
//             return base.Render(model);
//         }
//
//         protected override Task<ActionResult> ExecuteAsync(ContentModel model) {
//             return Task.FromResult(Redirect<ICheckoutAccountPage>());
//         }
//
//         protected override CheckoutStage Stage => CheckoutStages.Account;
//     }
// }