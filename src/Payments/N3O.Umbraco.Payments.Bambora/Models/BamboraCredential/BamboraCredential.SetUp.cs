namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        public void SetUp(string customerCode, string status, int code) {
            BamboraCustomerCode = customerCode;
            BamboraStatusCode = code;
            BamboraStatusDetail = status;
            ClearErrors();
            SetUp();
        }
    }
}