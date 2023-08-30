using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment : Payment {
    public string ReturnUrl { get; private set; }
    
    public string TotalProcessingCheckoutId { get; private set; }
    public string TotalProcessingTransactionId { get; private set; }

    public string TotalProcessingStatusCode { get; private set; }
    public string TotalProcessingStatusDetail { get; private set; }

    public string TotalProcessingErrorCode { get; private set; }
    public string TotalProcessingErrorMessage { get; private set; }

    public string TotalProcessingConnectorTxId1 { get; private set; }
    public string TotalProcessingConnectorTxId2 { get; private set; }
    public string TotalProcessingConnectorTxId3 { get; private set; }

    public string TotalProcessingUniqueReference { get; private set; }

    public override PaymentMethod Method => TotalProcessingConstants.PaymentMethod;
}
