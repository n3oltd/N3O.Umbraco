// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.ViewModels {
//     public abstract class CheckoutRegularPaymentBlockViewModelBase<TBlock> :
//         CheckoutBlockViewModelBase<TBlock>,
//         ICheckoutPaymentBlockViewModel
//         where TBlock : ICheckoutRegularPaymentBlock {
//         protected CheckoutRegularPaymentBlockViewModelBase(TBlock block,
//                                                            IEnumerable<LiteCartAllocation> allocations,
//                                                            bool showSinglePaymentNextMessage,
//                                                            Money value,
//                                                            string valueText)
//             : base(block) {
//             Allocations = allocations.OrEmpty()
//                                      .ToList();
//
//             ShowSinglePaymentNextMessage = showSinglePaymentNextMessage;
//             Value = value;
//             ValueText = valueText;
//         }
//
//         public IReadOnlyList<LiteCartAllocation> Allocations { get; }
//         public bool ShowSinglePaymentNextMessage { get; }
//         public Money Value { get; }
//         public string ValueText { get; }
//     }
//
//     public abstract class CheckoutRegularPaymentBlockViewModelBaseFactory<TBlock>
//         : CheckoutBlockViewModelFactoryBase<TBlock>
//         where TBlock : ICheckoutRegularPaymentBlock {
//         private readonly ICheckout _checkout;
//         private readonly IFormatter _formatter;
//
//         protected CheckoutRegularPaymentBlockViewModelBaseFactory(ICheckout checkout, IFormatter formatter) {
//             _checkout = checkout;
//             _formatter = formatter;
//         }
//
//         public override async Task<object> CreateAsync(TBlock block, CancellationToken cancellationToken = default) {
//             var allocations = _checkout.RegularDonation.Cart.Allocations;
//             var showSinglePaymentNextMessage = _checkout.HasSingleDonation();
//             var value = _checkout.RegularDonation.Value;
//             var valueText = _formatter.Number.FormatMoney(value, true);
//
//             var res = await CreateAsync(allocations,
//                                         showSinglePaymentNextMessage,
//                                         value,
//                                         valueText,
//                                         block,
//                                         cancellationToken);
//
//             return res;
//         }
//
//         protected abstract Task<object> CreateAsync(IEnumerable<LiteCartAllocation> allocations,
//                                                     bool showSinglePaymentNextMessage,
//                                                     Money value,
//                                                     string valueText,
//                                                     TBlock block,
//                                                     CancellationToken cancellationToken = default);
//     }
// }