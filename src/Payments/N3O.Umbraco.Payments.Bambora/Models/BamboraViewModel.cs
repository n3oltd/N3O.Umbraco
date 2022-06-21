namespace N3O.Umbraco.Payments.Bambora.Models;

public class BamboraViewModel : IPaymentMethodViewModel<BamboraPaymentMethod> {
    public BamboraViewModel(BamboraApiSettings apiSettings) {
        MerchantId = apiSettings.MerchantId;
    }

    public string MerchantId { get; }
}
