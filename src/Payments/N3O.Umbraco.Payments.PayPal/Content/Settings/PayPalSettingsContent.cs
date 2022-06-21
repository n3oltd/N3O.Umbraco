using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.PayPal.Content;

public class PayPalSettingsContent : PaymentMethodSettingsContent<PayPalSettingsContent> {
    public string ProductionAccessToken => GetValue(x => x.ProductionAccessToken);
    public string ProductionClientId => GetValue(x => x.ProductionClientId);

    public string StagingAccessToken => GetValue(x => x.StagingAccessToken);
    public string StagingClientId => GetValue(x => x.StagingClientId);
}
