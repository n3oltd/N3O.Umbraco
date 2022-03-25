namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void UpdateToken(string token) {
            BamboraToken = token;
        }
    }
}