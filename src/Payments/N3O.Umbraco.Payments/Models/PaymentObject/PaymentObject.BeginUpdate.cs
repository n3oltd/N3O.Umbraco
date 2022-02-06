using NodaTime;
using System;

namespace N3O.Umbraco.Payments.Models {
    public partial class PaymentObject {
        internal IDisposable BeginUpdate(IClock clock) {
            Clock = clock;

            return this;
        }
    }
}