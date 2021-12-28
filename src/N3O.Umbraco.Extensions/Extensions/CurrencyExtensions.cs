using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Extensions {
    public static class CurrencyExtensions {
        public static Money Zero(this Currency currency) {
            return new Money(0, currency);
        }
    
        public static ForexMoney ForexZero(this Currency currency) {
            return new ForexMoney(Zero(currency), Zero(currency), 1);
        }
    }
}
