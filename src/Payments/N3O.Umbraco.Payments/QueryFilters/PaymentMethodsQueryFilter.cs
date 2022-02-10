using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Criteria;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.QueryFilters;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Payments.QueryFilters {
    public class PaymentMethodsQueryFilter : QueryFilter<PaymentMethod, PaymentMethodCriteria> {
        private readonly IContentCache _contentCache;

        public PaymentMethodsQueryFilter(IContentCache contentCache) {
            _contentCache = contentCache;
        }
        
        public override IEnumerable<PaymentMethod> Apply(IEnumerable<PaymentMethod> q, PaymentMethodCriteria criteria) {
            q = q.Where(x => x.IsAvailable(_contentCache, criteria.Country, criteria.Currency));

            return q;
        }
    }
}