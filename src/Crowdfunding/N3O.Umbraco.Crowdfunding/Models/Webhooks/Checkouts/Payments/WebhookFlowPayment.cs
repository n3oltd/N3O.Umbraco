using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Models;

public class WebhookFlowPayment : Value {
    public WebhookFlowPayment(WebhookPaymentInfo info, string declinedReason, bool isDeclined, bool isPaid) {
        IsPaid = isPaid;
        Info = info;
        DeclinedReason = declinedReason;
        IsDeclined = isDeclined;
    }
    
    public WebhookPaymentInfo Info { get; }
    public string DeclinedReason { get; }
    public bool IsDeclined { get; }
    public bool IsPaid { get; }

    protected override IEnumerable<object> GetAtomicValues() {
        yield return Info;
        yield return DeclinedReason;
        yield return IsDeclined;
        yield return IsPaid;
    }
}