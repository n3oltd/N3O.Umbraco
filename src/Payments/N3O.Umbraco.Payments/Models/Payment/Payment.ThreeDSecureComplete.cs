namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        public void ThreeDSecureComplete(string CRes) {
            Card = Card.ThreeDSecureComplete(CRes);
        }
    }
}