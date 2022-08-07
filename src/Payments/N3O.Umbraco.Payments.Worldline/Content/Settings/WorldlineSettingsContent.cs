using N3O.Umbraco.Payments.Content;
using N3O.Umbraco.Payments.Worldline.Lookups;

namespace N3O.Umbraco.Payments.Worldline.Content;

public class WorldlineSettingsContent : PaymentMethodSettingsContent<WorldlineSettingsContent> {
    public WorldlinePlatform Platform => GetValue(x => x.Platform);
    
    public string ProductionApiKey => GetValue(x => x.ProductionApiKey);
    public string ProductionApiSecret => GetValue(x => x.ProductionApiSecret);
    public string ProductionMerchantId => GetValue(x => x.ProductionMerchantId);

    public string StagingApiKey => GetValue(x => x.StagingApiKey);
    public string StagingApiSecret => GetValue(x => x.StagingApiSecret);
    public string StagingMerchantId => GetValue(x => x.StagingMerchantId);
}
