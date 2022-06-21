using N3O.Umbraco.Payments.Lookups;
using NodaTime;

namespace N3O.Umbraco.Payments.Models;

public abstract partial class Credential : PaymentObject {
    public Payment AdvancePayment { get; private set; }
    public Instant? SetupAt { get; private set; }
    
    public bool IsSetUp { get; private set; }

    public override PaymentObjectType Type => PaymentObjectTypes.Credential;
}
