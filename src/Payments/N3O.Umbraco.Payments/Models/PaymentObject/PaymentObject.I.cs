using N3O.Umbraco.Entities;
using N3O.Umbraco.Payments.Lookups;
using System;

namespace N3O.Umbraco.Payments.Models {
    public interface IPaymentObject {
        EntityId Id { get; }
        PaymentObjectType Type { get; }
        PaymentMethod Method { get; }
        PaymentObjectStatus Status { get; }

        void UnhandledError(Exception ex);
    }
}