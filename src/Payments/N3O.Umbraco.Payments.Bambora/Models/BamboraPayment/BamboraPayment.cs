using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Bambora.Models;

public partial class BamboraPayment : Payment {
    public int? BamboraErrorCode { get; private set; }
    public string BamboraErrorMessage { get; private set; }
    public string BamboraPaymentId { get; private set; }
    public int? BamboraStatusCode { get; private set; }
    public string BamboraStatusDetail { get; private set; }
    public string BamboraToken { get; private set; }
    public string ReturnUrl { get; private set; }

    public override PaymentMethod Method => BamboraConstants.PaymentMethod;
}
