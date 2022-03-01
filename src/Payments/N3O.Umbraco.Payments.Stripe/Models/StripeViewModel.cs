namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeViewModel : IPaymentMethodViewModel<StripePaymentMethod> {
        public StripeViewModel(StripeApiSettings apiSettings) {
            ClientKey = apiSettings.ClientKey;
        }

        public string ClientKey { get; }
    }
}