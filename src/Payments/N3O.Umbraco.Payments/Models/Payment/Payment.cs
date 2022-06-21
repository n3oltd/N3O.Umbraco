using N3O.Umbraco.Payments.Lookups;
using NodaTime;

namespace N3O.Umbraco.Payments.Models;

public abstract partial class Payment : PaymentObject {
    public CardPayment Card { get; private set; }
    public Instant? PaidAt { get; private set; }
    public Instant? DeclinedAt { get; private set; }
    public string DeclinedReason { get; private set; }
    public bool IsDeclined { get; private set; }
    
    public bool IsPaid { get; private set; }
    
    public override PaymentObjectType Type => PaymentObjectTypes.Payment;
}
