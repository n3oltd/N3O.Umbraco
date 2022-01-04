// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.Controllers {
//     public class CheckoutAccountController : CheckoutController<CheckoutAccountReq> {
//         private readonly ICheckoutReference _checkoutReference;
//
//         public CheckoutAccountController(IServiceProvider services, ICheckoutReference checkoutReference) :
//             base(services) {
//             _checkoutReference = checkoutReference;
//         }
//
//         protected override async Task<CheckoutStageRes> ExecuteAsync(CheckoutAccountReq req) {
//             Checkout.Start();
//
//             Checkout.Reference = await _checkoutReference.GenerateAsync();
//             Checkout.Account = Mapper.Map<Account>(req.Account);
//
//             return Success();
//         }
//
//         protected override CheckoutStage Stage => CheckoutStages.Account;
//     }
// }