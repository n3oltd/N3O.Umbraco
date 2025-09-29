using N3O.Umbraco.Context;
using N3O.Umbraco.Giving.Allocations;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Giving.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Handlers;

public class GetPriceHandler : IRequestHandler<GetPriceQuery, PriceCriteria, PriceRes> {
    private readonly IPriceCalculator _priceCalculator;
    private readonly ICurrencyAccessor _currencyAccessor;
    private readonly IUmbracoMapper _mapper;

    public GetPriceHandler(IPriceCalculator priceCalculator,
                           ICurrencyAccessor currencyAccessor,
                           IUmbracoMapper mapper) {
        _priceCalculator = priceCalculator;
        _currencyAccessor = currencyAccessor;
        _mapper = mapper;
    }
    
    public async Task<PriceRes> Handle(GetPriceQuery req, CancellationToken cancellationToken) {
        var currency = _currencyAccessor.GetCurrency();
        var pricing = (IHoldPricing) req.Model.DonationItem ??
                      (IHoldPricing) req.Model.SponsorshipComponent ??
                      (IHoldPricing) req.Model.FeedbackScheme;

        PriceRes res = null;

        if (pricing != null) {
            var price = await _priceCalculator.InCurrencyAsync(pricing.Pricing,
                                                               req.Model.FundDimensions,
                                                               currency,
                                                               cancellationToken);
            
            res = _mapper.Map<Price, PriceRes>(price);
        }

        return res;
    }
}
