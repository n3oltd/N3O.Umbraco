namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public void ThreeDSecureProcessCompleted() {
            ThreeDSecureCompleted = true;
        }
    }
}