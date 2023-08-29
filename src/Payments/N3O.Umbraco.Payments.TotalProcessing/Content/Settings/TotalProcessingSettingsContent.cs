using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.TotalProcessing.Content;

public class TotalProcessingSettingsContent : PaymentMethodSettingsContent<TotalProcessingSettingsContent> {
    public string ProductionAccessToken => GetValue(x => x.ProductionAccessToken);
    public string ProductionEntityId => GetValue(x => x.ProductionEntityId);

    public string StagingAccessToken => GetValue(x => x.StagingAccessToken);
    public string StagingEntityId => GetValue(x => x.StagingEntityId);
}
