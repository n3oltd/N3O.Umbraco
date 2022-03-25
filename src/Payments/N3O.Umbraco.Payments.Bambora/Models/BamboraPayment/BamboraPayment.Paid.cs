namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void Paid(string paymentId,
                         int statusCode,
                         string statusDetail) {
            ClearErrors();
            
            BamboraPaymentId = paymentId;
            BamboraStatusCode = statusCode;
            BamboraStatusDetail = statusDetail;

            Paid();
        }
    }
}