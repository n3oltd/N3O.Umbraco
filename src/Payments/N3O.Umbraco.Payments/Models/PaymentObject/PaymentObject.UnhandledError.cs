using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Payments.Models {
    public partial class PaymentObject {
        public void UnhandledError(IFormatter formatter, Exception ex) {
            ExceptionId = Guid.NewGuid();
            ExceptionDetails = ex.ToString();
            
            Error(formatter.Text.Format<UnhandledErrorStrings>(s => s.Message_1, ExceptionId));
        }
    }
}