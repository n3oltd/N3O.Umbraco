using System;

namespace N3O.Umbraco.Payments.Models {
    public partial class PaymentObject {
        public void UnhandledError(Exception ex, string message) {
            ExceptionDetails = ex.ToString();
            
            Error(message);
        }
    }
}