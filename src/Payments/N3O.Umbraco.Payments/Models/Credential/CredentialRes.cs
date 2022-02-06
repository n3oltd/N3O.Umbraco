namespace N3O.Umbraco.Payments.Models {
    public class CredentialRes : PaymentObjectRes {
        public PaymentRes AdvancePayment { get; set; }
        public bool IsSetUp { get; set; }
    }
}