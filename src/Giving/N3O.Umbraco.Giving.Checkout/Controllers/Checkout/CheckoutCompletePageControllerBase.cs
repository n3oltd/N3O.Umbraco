// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     public abstract class CheckoutCompletePageControllerBase : CheckoutPageController {
//         private readonly IServiceProvider _services;
//
//         protected CheckoutCompletePageControllerBase(IServiceProvider services) : base(services) {
//             _services = services;
//         }
//
//         protected override Task<ActionResult> ExecuteAsync(ContentModel model) {
//             var viewModel = GetViewModel(_factory, model);
//
//             return Task.FromResult(CurrentTemplate(viewModel));
//         }
//
//         protected abstract object GetViewModel(IServiceProvider services, ContentModel model);
//
//         protected override CheckoutStage Stage => CheckoutStages.Complete;
//     }
// }