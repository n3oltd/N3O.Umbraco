using N3O.Umbraco.Content;
using N3O.Umbraco.TaxRelief.Content;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.TaxRelief {
    public class TaxReliefSchemeAccessor : ITaxReliefSchemeAccessor {
        private readonly IContentCache _contentCache;

        public TaxReliefSchemeAccessor(IContentCache contentCache) {
            _contentCache = contentCache;
        }

        public TaxReliefScheme GetScheme() {
            var settings = _contentCache.Single<TaxReliefSettingsContent>();
            
            return settings?.Scheme;
        }
    }
}