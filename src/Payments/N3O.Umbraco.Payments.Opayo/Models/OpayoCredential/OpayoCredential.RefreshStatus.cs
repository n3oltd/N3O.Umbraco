namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoCredential {
    public override void RefreshStatus() {
        if (AdvancePayment.IsComplete) {
            SetUp();
        } else if (AdvancePayment.HasError) {
            Error(AdvancePayment.ErrorMessage);
        }
    }
}
