using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Cart.Content;
using N3O.Umbraco.Giving.Cart.Extensions;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Cart.Modules;

public class CartBlockModule : IBlockModule {
    private static readonly string BlockAlias = AliasHelper<CartBlockContent>.ContentTypeAlias();

    private readonly Lazy<ICartAccessor> _cartAccessor;
    private readonly Lazy<IFormatter> _formatter;
    private readonly Lazy<IContentCache> _contentCache;
    private readonly Lazy<ICurrencyAccessor> _currencyAccessor;
    private readonly Lazy<IForexConverter> _forexConverter;
    private readonly Lazy<IPriceCalculator> _priceCalculator;
    private readonly Lazy<IQueryStringAccessor> _queryStringAccessor;
    private readonly Lazy<ILookups> _lookups;

    public CartBlockModule(Lazy<ICartAccessor> cartAccessor,
                           Lazy<IFormatter> formatter,
                           Lazy<IContentCache> contentCache,
                           Lazy<ICurrencyAccessor> currencyAccessor,
                           Lazy<IForexConverter> forexConverter,
                           Lazy<IPriceCalculator> priceCalculator,
                           Lazy<IQueryStringAccessor> queryStringAccessor,
                           Lazy<ILookups> lookups) {
        _cartAccessor = cartAccessor;
        _formatter = formatter;
        _contentCache = contentCache;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _queryStringAccessor = queryStringAccessor;
        _lookups = lookups;
    }
    
    public bool ShouldExecute(IPublishedElement block) {
        return block.ContentType.Alias.EqualsInvariant(BlockAlias);
    }

    public async Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
        var checkoutView = _queryStringAccessor.Value.GetValue(CartConstants.QueryString.CheckoutView);
        var currency = _currencyAccessor.Value.GetCurrency();
        var cart = await _cartAccessor.Value.GetAsync(cancellationToken);
        var upsellOffers = await GetUpsellOffersAsync(block, cart, currency);
        
        var cartModel = new CartModel(_formatter.Value,
                                      _contentCache.Value,
                                      currency,
                                      cart.Donation,
                                      cart.RegularGiving,
                                      upsellOffers,
                                      checkoutView.HasValue());
        
        return cartModel;
    }

    private async Task<IReadOnlyList<UpsellOffer>> GetUpsellOffersAsync(IPublishedElement block,
                                                                        Entities.Cart cart,
                                                                        Currency currency) {
        // TODO Fix this
        var upsellOffersContents = GetUpsellOffers(block, cart);

        var upsellOffers = new List<UpsellOffer>();

        foreach (var upsellOffersContent in upsellOffersContents.OrEmpty()) {
            upsellOffers.Add(await upsellOffersContent.ToUpsellOfferAsync(_forexConverter.Value,
                                                                          _priceCalculator.Value,
                                                                          _lookups.Value,
                                                                          currency,
                                                                          upsellOffersContent.GivingType,
                                                                          cart.GetTotalExcludingUpsells(upsellOffersContent.GivingType)));
        }

        return upsellOffers;
    }

    private IReadOnlyList<UpsellOfferContent> GetUpsellOffers(IPublishedElement block, Entities.Cart cart) {
        var upsellOffersValue = block.GetProperty("upsellOffers")?.GetValue();

        if (!upsellOffersValue.HasValue()) {
            return null;
        }

        var upsellOffers = new List<UpsellOfferContent>();

        if (upsellOffersValue is IEnumerable<IPublishedContent>) {
            var upsellOffersContent = (upsellOffersValue as IEnumerable<IPublishedContent>).As<UpsellOfferContent>();
            
            upsellOffers.AddRange(upsellOffersContent);
        } else if (upsellOffersValue is IPublishedContent) {
            var upsellOfferContent = (upsellOffersValue as IPublishedContent).As<UpsellOfferContent>();
            
            upsellOffers.Add(upsellOfferContent);
        } else {
            throw UnrecognisedValueException.For(upsellOffersValue?.GetType());
        }
        
        if (cart.Donation.IsEmpty()) {
            upsellOffers = upsellOffers?.ExceptWhere(x => x.OfferedFor.HasAny(x => x == GivingTypes.Donation) &&
                                                          x.OfferedFor.Count() == 1)
                                       .ToList();
        }

        if (cart.RegularGiving.IsEmpty()) {
            upsellOffers = upsellOffers?.ExceptWhere(x => x.OfferedFor.HasAny(x => x == GivingTypes.RegularGiving) && 
                                                          x.OfferedFor.Count() == 1)
                                       .ToList();
        }
        
        return upsellOffers;
    }
    
    public string Key => CartConstants.BlockModuleKeys.Cart;
}
