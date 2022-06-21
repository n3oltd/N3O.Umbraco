namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public void EndPaymentFlow() {
        Donation?.Payment?.RefreshStatus();
        RegularGiving?.Credential?.AdvancePayment?.RefreshStatus();
        RegularGiving?.Credential?.RefreshStatus();
    }
}
