using N3O.Umbraco.Payments.Models;
using System;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public T GetOrCreatePaymentObject<T>() where T : PaymentObject, new() {
            throw new NotImplementedException();
        }
    }
}