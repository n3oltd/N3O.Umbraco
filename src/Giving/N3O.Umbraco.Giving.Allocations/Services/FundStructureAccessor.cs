using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;

namespace N3O.Umbraco.Giving.Allocations {
    public class FundStructureAccessor : IFundStructureAccessor {
        private readonly IContentCache _contentCache;

        public FundStructureAccessor(IContentCache contentCache) {
            _contentCache = contentCache;
        }

        public FundStructure GetFundStructure() {
            var dimension1 = _contentCache.Single<FundDimension1>();
            var dimension2 = _contentCache.Single<FundDimension2>();
            var dimension3 = _contentCache.Single<FundDimension3>();
            var dimension4 = _contentCache.Single<FundDimension4>();
            
            return new FundStructure(dimension1, dimension2, dimension3, dimension4);
        }
    }
}
