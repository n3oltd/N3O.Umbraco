using N3O.Umbraco.Blocks;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using N3O.Umbraco.Giving.Cart.Content;
using N3O.Umbraco.Giving.Cart.Models;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Localization;
using System;
using System.Collections.Generic;
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

    public CartBlockModule(Lazy<ICartAccessor> cartAccessor,
                           Lazy<IFormatter> formatter,
                           Lazy<IContentCache> contentCache,
                           Lazy<ICurrencyAccessor> currencyAccessor,
                           Lazy<IForexConverter> forexConverter,
                           Lazy<IPriceCalculator> priceCalculator,
                           Lazy<IQueryStringAccessor> queryStringAccessor) {
        _cartAccessor = cartAccessor;
        _formatter = formatter;
        _contentCache = contentCache;
        _currencyAccessor = currencyAccessor;
        _forexConverter = forexConverter;
        _priceCalculator = priceCalculator;
        _queryStringAccessor = queryStringAccessor;
    }
    
    public bool ShouldExecute(IPublishedElement block) {
        return block.ContentType.Alias.EqualsInvariant(BlockAlias);
    }

    public async Task<object> ExecuteAsync(IPublishedElement block, CancellationToken cancellationToken) {
        var checkoutView = _queryStringAccessor.Value.GetValue(CartConstants.QueryString.CheckoutView);
        var currency = _currencyAccessor.Value.GetCurrency();
        var cart = await _cartAccessor.Value.GetAsync(cancellationToken);
        // TODO Fix this
        var upsellContents = (block.GetProperty("upsells")?.GetValue() as IEnumerable<IPublishedContent>)?.As<UpsellContent>();
        var upsells = await GetUpsellsAsync(upsellContents, currency, cart.RegularGiving.Total, cart.Donation.Total);
        
        var cartModel = new CartModel(_formatter.Value,
                                      _contentCache.Value,
                                      currency,
                                      cart.Donation,
                                      cart.RegularGiving,
                                      upsells,
                                      checkoutView.HasValue());
        
        return cartModel;
    }

    private async Task<IReadOnlyList<UpsellModel>> GetUpsellsAsync(IEnumerable<UpsellContent> upsellContents,
                                                                   Currency currency,
                                                                   Money regularTotal,
                                                                   Money donationTotal) {
        if (upsellContents.None()) {
            return null;
        }

        var upsells = new List<UpsellModel>();

        foreach (var upsellContent in upsellContents) {
            var priceOrAmount = await upsellContent.GetPriceOrAmountInCurrencyAsync(_forexConverter.Value,
                                                                                    _priceCalculator.Value,
                                                                                    currency,
                                                                                    regularTotal,
                                                                                    donationTotal);

            upsells.Add(new UpsellModel(upsellContent.Content().Key,
                                        upsellContent.Content().Name,
                                        upsellContent.Description,
                                        priceOrAmount,
                                        upsellContent.PriceHandles));
        }

        return upsells;
    }

    public string Key => CartConstants.BlockModuleKeys.Cart;
}
