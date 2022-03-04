using NodaTime;
using System;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public void BeginPaymentFlow(IClock clock) {
            Donation?.Payment?.BeginUpdate(clock);
            RegularGiving?.Credential?.BeginUpdate(clock);
            RegularGiving?.Credential?.AdvancePayment?.BeginUpdate(clock);
        }
    }
}