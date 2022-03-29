namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        public new void ThreeDSecureComplete(string cRes) {
            ClearErrors();
            
            base.ThreeDSecureComplete(cRes);
        }
    }
}