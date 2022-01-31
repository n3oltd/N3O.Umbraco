using N3O.Umbraco.Financial;
using N3O.Umbraco.Forex;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class PriceHandleMapping : IMapDefinition {
        private readonly IForexConverter _forexConverter;

        public PriceHandleMapping(IForexConverter forexConverter) {
            _forexConverter = forexConverter;
        }
        
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<PriceHandleElement, PriceHandleRes>((_, _) => new PriceHandleRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(PriceHandleElement src, PriceHandleRes dest, MapperContext ctx) {
            dest.Amount = ctx.Map<Money, MoneyRes>(_forexConverter.BaseToQuote().Convert(src.Amount).Quote);
            dest.Description = src.Description;
        }
    }
}