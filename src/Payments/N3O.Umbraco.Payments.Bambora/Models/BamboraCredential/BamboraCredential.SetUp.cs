namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        public void SetUp(string customerCode, string status, int code) {
            ClearErrors();
            
            BamboraCustomerCode = customerCode;
            BamboraStatusCode = code;
            BamboraStatusDetail = status;
            
            SetUp();
        }
    }
}