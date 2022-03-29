namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        public void SetUp(string customerCode) {
            BamboraCustomerCode = customerCode;
            
            ClearErrors();
            SetUp();
        }
    }
}