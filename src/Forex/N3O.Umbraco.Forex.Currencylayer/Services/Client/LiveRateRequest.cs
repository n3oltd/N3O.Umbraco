using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Forex.Currencylayer {
    public class LiveRateRequest : ApiRequest {
        public LiveRateRequest(Currency baseCurrency, Currency quoteCurrency) : base(baseCurrency, quoteCurrency) { }
    }
}