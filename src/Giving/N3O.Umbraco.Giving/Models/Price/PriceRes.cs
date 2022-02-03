using N3O.Umbraco.Forex.Models;

namespace N3O.Umbraco.Giving.Models {
    public class PriceRes : IPrice {
        public decimal Amount { get; set; }
        public CurrencyValuesRes CurrencyValues { get; set; }
        public bool Locked { get; set; }
    }
}