using NodaTime;

namespace N3O.Umbraco.Payments.Models;

public partial class PaymentObject {
    public void BeginUpdate(IClock clock) {
        Clock = clock;
    }
}
