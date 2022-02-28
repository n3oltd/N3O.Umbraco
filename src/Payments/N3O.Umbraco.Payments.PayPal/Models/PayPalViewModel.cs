namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalViewModel : IPaymentMethodViewModel<PayPalPaymentMethod> {
        public PayPalViewModel(PayPalApiSettings apiSettings) {
            ClientId = apiSettings.ClientId;
        }

        public string ClientId { get; }
    }
}