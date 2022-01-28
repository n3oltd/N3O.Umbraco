using N3O.Umbraco.Payments.Entities;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public TransactionInfo GetTransctionInfo() {
            return new TransactionInfo(Reference, $"Donation {Reference.Text}");
        }
    }
}