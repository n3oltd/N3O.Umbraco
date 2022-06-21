namespace N3O.Umbraco.Payments.Opayo.Models;

public class OpayoViewModel : IPaymentMethodViewModel<OpayoPaymentMethod> {
    public OpayoViewModel(OpayoApiSettings apiSettings) {
        BaseUrl = apiSettings.BaseUrl;
        IntegrationKey = apiSettings.IntegrationKey;
    }

    public string BaseUrl { get; }
    public string IntegrationKey { get; }
}
