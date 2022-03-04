namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutErrorModel {
        public CheckoutErrorModel(Entities.Checkout checkout) {
            Checkout = checkout;
        }

        public Entities.Checkout Checkout { get; }
    }
}