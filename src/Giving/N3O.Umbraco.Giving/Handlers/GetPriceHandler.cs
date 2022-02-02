using N3O.Umbraco.Giving.Models;
using N3O.Umbraco.Giving.Queries;
using N3O.Umbraco.Mediator;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Handlers {
    public class GetPriceHandler : IRequestHandler<GetPriceQuery, PriceCriteria, PriceRes> {
        private readonly IPriceCalculator _priceCalculator;
        private readonly IUmbracoMapper _mapper;

        public GetPriceHandler(IPriceCalculator priceCalculator, IUmbracoMapper mapper) {
            _priceCalculator = priceCalculator;
            _mapper = mapper;
        }
        
        public async Task<PriceRes> Handle(GetPriceQuery req, CancellationToken cancellationToken) {
            var pricing = (IPricing) req.Model.DonationItem ?? req.Model.SponsorshipComponent;

            PriceRes res = null;

            if (pricing != null) {
                var price = await _priceCalculator.InCurrentCurrencyAsync(pricing,
                                                                          req.Model.FundDimensions,
                                                                          cancellationToken);
                
                res = _mapper.Map<Price, PriceRes>(price);
            }

            return res;
        }
    }
}