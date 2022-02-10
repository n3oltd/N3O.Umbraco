namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment {
        private void ClearErrors() {
            OpayoErrorCode = null;
            OpayoErrorMessage = null;
        }
    }
}