using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Allocations {
    public class FundStructure : IFundStructure {
        private readonly IContentCache _contentCache;
        private readonly ILookups _lookups;

        public FundStructure(IContentCache contentCache, ILookups lookups) {
            _contentCache = contentCache;
            _lookups = lookups;
        }
    
        public FundDimension1Content Dimension1 => _contentCache.Single<FundDimension1Content>();
        public FundDimension2Content Dimension2 => _contentCache.Single<FundDimension2Content>();
        public FundDimension3Content Dimension3 => _contentCache.Single<FundDimension3Content>();
        public FundDimension4Content Dimension4 => _contentCache.Single<FundDimension4Content>();

        public IEnumerable<FundDimensionContent> Dimensions {
            get {
                yield return Dimension1;
                yield return Dimension2;
                yield return Dimension3;
                yield return Dimension4;
            }
        }

        public IReadOnlyList<T> GetOptions<T>() where T : FundDimensionOption<T> {
            return _lookups.GetAll<T>();
        }
    }
}
