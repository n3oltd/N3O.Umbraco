namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        private void ClearErrors() {
            BamboraErrorCode = null;
            BamboraErrorMessage = null;
        }
    }
}