// using N3O.Umbraco.Content;
// using N3O.Umbraco.Lookups;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace N3O.Umbraco.Giving.Checkout.ViewModels {
//     public abstract class CheckoutAccountBlockViewModelBase<TBlock> : CheckoutBlockViewModelBase<TBlock>
//         where TBlock : ICheckoutAccountBlock {
//         protected CheckoutAccountBlockViewModelBase(TBlock block,
//                                                     IEnumerable<Country> countries,
//                                                     Country defaultCountry,
//                                                     IEnumerable<ITitle> titles,
//                                                     ITermsPage termsPage)
//             : base(block) {
//             Countries = countries.OrEmpty()
//                                  .ToList();
//
//             DefaultCountry = defaultCountry;
//             Titles = titles.OrEmpty()
//                            .ToList();
//
//             TermsPage = termsPage;
//         }
//
//         public IReadOnlyList<Country> Countries { get; }
//         public Country DefaultCountry { get; }
//         public IReadOnlyList<ITitle> Titles { get; }
//         public ITermsPage TermsPage { get; }
//     }
//
//     public abstract class CheckoutAccountBlockViewModelBaseFactory<TBlock>
//         : CheckoutBlockViewModelFactoryBase<TBlock>
//         where TBlock : ICheckoutAccountBlock {
//         private readonly ILookups _lookups;
//         private readonly IContentLocator _contentLocator;
//
//         protected CheckoutAccountBlockViewModelBaseFactory(ILookups lookups, IContentLocator contentLocator) {
//             _lookups = lookups;
//             _contentLocator = contentLocator;
//         }
//
//         public override async Task<object> CreateAsync(TBlock block, CancellationToken cancellationToken = default) {
//             var countries = _lookups.GetAll<Country>();
//             var defaultCountry = _contentLocator.Single<IAddressValidationSettings>()
//                                                 ?.DefaultCountry
//                                  ?? countries.First();
//
//             var titles = _lookups.GetAll<ITitle>();
//             var termsPage = _contentLocator.Single<ITermsPage>();
//
//             var res = await CreateAsync(block, countries, defaultCountry, titles, termsPage, cancellationToken);
//
//             return res;
//         }
//
//         protected abstract Task<object> CreateAsync(TBlock block,
//                                                     IEnumerable<Country> countries,
//                                                     Country defaultCountry,
//                                                     IEnumerable<ITitle> titles,
//                                                     ITermsPage termsPage,
//                                                     CancellationToken cancellationToken = default);
//     }
// }