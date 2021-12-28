using N3O.Umbraco.Content;

namespace N3O.Umbraco.Forex.Currencylayer.Content;

public class CurrencylayerSettings : UmbracoContent {
    public string ApiKey => GetValue<CurrencylayerSettings, string>(x => x.ApiKey);
}
