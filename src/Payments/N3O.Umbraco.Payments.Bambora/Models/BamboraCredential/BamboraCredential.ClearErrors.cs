namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        protected override void ClearErrors() {
            base.ClearErrors();
            
            BamboraErrorCode = null;
            BamboraErrorMessage = null;
        }
    }
}