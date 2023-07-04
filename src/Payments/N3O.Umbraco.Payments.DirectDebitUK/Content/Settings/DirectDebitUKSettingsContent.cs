using N3O.Umbraco.Payments.Content;

namespace N3O.Umbraco.Payments.DirectDebitUK.Content;

public class DirectDebitUKSettingsContent : PaymentMethodSettingsContent<DirectDebitUKSettingsContent> {
    public string LoqateApiKey => GetValue(x => x.LoqateApiKey);
}