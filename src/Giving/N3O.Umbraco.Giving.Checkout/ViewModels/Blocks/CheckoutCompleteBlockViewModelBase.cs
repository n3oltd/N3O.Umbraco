// using System.Threading;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.ViewModels {
//     public abstract class CheckoutCompleteBlockViewModelBase<TBlock> : CheckoutBlockViewModelBase<TBlock>
//         where TBlock : ICheckoutCompleteBlock {
//         protected CheckoutCompleteBlockViewModelBase(TBlock block, string email, string dataLayer) : base(block) {
//             Email = email;
//             DataLayer = dataLayer;
//         }
//
//         public string Email { get; }
//         public string DataLayer { get; }
//     }
//
//     public abstract class CheckoutCompleteBlockViewModelBaseFactory<TBlock> : CheckoutBlockViewModelFactoryBase<TBlock>
//         where TBlock : ICheckoutCompleteBlock {
//         private readonly ICheckout _checkout;
//         private readonly IDataLayer _dataLayer;
//
//         protected CheckoutCompleteBlockViewModelBaseFactory(ICheckout checkout, IDataLayer dataLayer) {
//             _checkout = checkout;
//             _dataLayer = dataLayer;
//         }
//
//         public override async Task<object> CreateAsync(TBlock block, CancellationToken cancellationToken = default) {
//             var email = _checkout.Account.Email.Address;
//             var dataLayer = _dataLayer.Get();
//
//             var res = await CreateAsync(email, dataLayer, block, cancellationToken);
//
//             return res;
//         }
//
//         protected abstract Task<object> CreateAsync(string email,
//                                                     string dataLayer,
//                                                     TBlock block,
//                                                     CancellationToken cancellationToken = default);
//     }
// }