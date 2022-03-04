namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutCompleteModel {
        public CheckoutCompleteModel(Entities.Checkout checkout) {
            Checkout = checkout;
        }

        public Entities.Checkout Checkout { get; }
    }
}