namespace N3O.Umbraco.Payments.GoCardless.Models {
    public partial class GoCardlessCredential {
        public void CompleteRedirectFlow(string customerId, string mandateId) {
            CustomerId = customerId;
            MandateId = mandateId;
        }
    }
}