using N3O.Umbraco.Content;

namespace N3O.Umbraco.Forex.Currencylayer.Content {
    public class CurrencylayerSettings : UmbracoContent<CurrencylayerSettings> {
        public string ApiKey => GetValue(x => x.ApiKey);
    }
}
