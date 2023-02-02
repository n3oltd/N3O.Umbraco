using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Giving.Cart;

public class UpsellService {
    private readonly Lazy<ICartAccessor> _cartAccessor;
    private readonly Lazy<ICurrencyAccessor> _currencyAccessor;
    private readonly IContentLocator _contentLocator;

    public UpsellService(Lazy<ICartAccessor> cartAccessor,
                         Lazy<ICurrencyAccessor> currencyAccessor,
                         IContentLocator contentLocator) {
        _cartAccessor = cartAccessor;
        _contentLocator = contentLocator;
        _currencyAccessor = currencyAccessor;
    }
    
    public async Task<Entities.Cart> AddUpsellItem(Guid id) {
        var upsellItem = _contentLocator.ById<UpsellItemContent>(id);
        
        var cart = await _cartAccessor.Value.GetAsync();

        cart.Add(upsellItem.GetGivingType(),
                 upsellItem.GetAllocation(_currencyAccessor.Value.GetCurrency()));

        return cart;
    }
    
    public async Task<bool> IsUpsellItemAdded() {
        var cart = await _cartAccessor.Value.GetAsync();

        return cart.Donation.Allocations.HasAny(x => x.IsUpsellItem);
    }
}