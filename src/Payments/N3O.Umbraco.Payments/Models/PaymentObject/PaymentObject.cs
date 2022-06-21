using N3O.Umbraco.Payments.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Payments.Models;

public abstract partial class PaymentObject : Value {
    protected PaymentObject() {
        Status = PaymentObjectStatuses.InProgress;
    }
    
    public Instant? CompleteAt { get; private set; }
    public Instant? ErrorAt { get; private set; }
    public string ErrorMessage { get; private set; }
    public string ExceptionDetails { get; private set; }
    public PaymentObjectStatus Status { get; private set; }

    [JsonIgnore]
    public bool HasError => Status == PaymentObjectStatuses.Error;
    
    [JsonIgnore]
    public bool IsComplete => Status == PaymentObjectStatuses.Complete;

    [JsonIgnore]
    public bool IsInProgress => Status == PaymentObjectStatuses.InProgress;

    public abstract PaymentObjectType Type { get; }
    public abstract PaymentMethod Method { get; }
    
    [JsonIgnore]
    protected IClock Clock { get; private set; }
}
