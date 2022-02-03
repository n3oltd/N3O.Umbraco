using N3O.Umbraco.Forex.Models;

namespace N3O.Umbraco.Giving.Models {
    public class PriceHandleRes {
        public decimal Amount { get; set; }
        public CurrencyValuesRes CurrencyValues { get; set; }
        public string Description { get; set; }
    }
}