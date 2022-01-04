// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.ViewModels {
//     public abstract class CheckoutSinglePaymentBlockViewModelBase<TBlock> :
//         CheckoutBlockViewModelBase<TBlock>,
//         ICheckoutPaymentBlockViewModel
//         where TBlock : ICheckoutSinglePaymentBlock {
//         protected CheckoutSinglePaymentBlockViewModelBase(TBlock block,
//                                                           IEnumerable<LiteCartAllocation> allocations,
//                                                           bool showRegularPaymentSuccessMessage,
//                                                           Money value,
//                                                           string valueText)
//             : base(block) {
//             Allocations = allocations.OrEmpty()
//                                      .ToList();
//
//             ShowRegularPaymentSuccessMessage = showRegularPaymentSuccessMessage;
//             Value = value;
//             ValueText = valueText;
//         }
//
//         public IReadOnlyList<LiteCartAllocation> Allocations { get; }
//         public bool ShowRegularPaymentSuccessMessage { get; }
//         public Money Value { get; }
//         public string ValueText { get; }
//     }
//
//     public abstract class CheckoutSinglePaymentBlockViewModelBaseFactory<TBlock>
//         : CheckoutBlockViewModelFactoryBase<TBlock>
//         where TBlock : ICheckoutSinglePaymentBlock {
//         private readonly ICheckout _checkout;
//         private readonly IFormatter _formatter;
//
//         protected CheckoutSinglePaymentBlockViewModelBaseFactory(ICheckout checkout, IFormatter formatter) {
//             _checkout = checkout;
//             _formatter = formatter;
//         }
//
//         public override async Task<object> CreateAsync(TBlock block, CancellationToken cancellationToken = default) {
//             var allocations = _checkout.SingleDonation.Cart.Allocations;
//             var showRegularPaymentSuccessMessage = _checkout.HasSingleDonation();
//             var value = _checkout.SingleDonation.Value;
//             var valueText = _formatter.Number.FormatMoney(value, true);
//
//             var res = await CreateAsync(allocations,
//                                         showRegularPaymentSuccessMessage,
//                                         value,
//                                         valueText,
//                                         block,
//                                         cancellationToken);
//
//             return res;
//         }
//
//         protected abstract Task<object> CreateAsync(IEnumerable<LiteCartAllocation> allocations,
//                                                     bool showRegularPaymentSuccessMessage,
//                                                     Money value,
//                                                     string valueText,
//                                                     TBlock block,
//                                                     CancellationToken cancellationToken = default);
//     }
// }