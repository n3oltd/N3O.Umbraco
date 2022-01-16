using N3O.Umbraco.Content;

namespace N3O.Umbraco.Forex.Currencylayer.Content {
    public class CurrencylayerSettingsContent : UmbracoContent<CurrencylayerSettingsContent> {
        public string ApiKey => GetValue(x => x.ApiKey);
    }
}
