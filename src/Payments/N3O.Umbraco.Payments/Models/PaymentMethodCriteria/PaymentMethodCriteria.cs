using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Payments.Criteria {
    public class PaymentMethodCriteria {
        [Name("Country")]
        public Country Country { get; set; }
        
        [Name("Currency")]
        public Currency Currency { get; set; }
    }
}