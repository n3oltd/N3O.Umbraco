using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.Stripe.Models;

public class StripeSettingsContent : PaymentMethodSettingsContent<StripeSettingsContent> {
    public string ProductionClientKey => GetValue(x => x.ProductionClientKey);
    public string ProductionSecretKey => GetValue(x => x.ProductionSecretKey);
    public string StagingClientKey => GetValue(x => x.StagingClientKey);
    public string StagingSecretKey => GetValue(x => x.StagingSecretKey);
}
