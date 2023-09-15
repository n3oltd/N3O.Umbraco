namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class TotalProcessingViewModel : IPaymentMethodViewModel<TotalProcessingPaymentMethod> {
    public TotalProcessingViewModel(TotalProcessingApiSettings apiSettings) {
        BaseUrl = apiSettings.BaseUrl;
    }

    public string BaseUrl { get; }
}
