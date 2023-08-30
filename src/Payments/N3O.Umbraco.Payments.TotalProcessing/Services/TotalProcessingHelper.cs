using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Clients;
using N3O.Umbraco.Payments.TotalProcessing.Extensions;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using Payments.TotalProcessing.Clients.Models;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing;

public class TotalProcessingHelper : ITotalProcessingHelper {
    private readonly Lazy<ITotalProcessingClient> _checkoutClient;
    private readonly TotalProcessingApiSettings _totalProcessingApiSettings;

    public TotalProcessingHelper(Lazy<ITotalProcessingClient> checkoutClient,
                                 TotalProcessingApiSettings totalProcessingApiSettings) {
        _checkoutClient = checkoutClient;
        _totalProcessingApiSettings = totalProcessingApiSettings;
    }

    public void ApplyApiPayment(TotalProcessingPayment payment, ApiTransactionRes apiPayment) {
        if (apiPayment.IsAuthorised()) {
            payment.Paid(apiPayment.Id,
                         apiPayment.Result.Code,
                         apiPayment.Result.Description,
                         apiPayment.ResultDetails?.ConnectorTxID1,
                         apiPayment.ResultDetails?.ConnectorTxID2,
                         apiPayment.ResultDetails?.ConnectorTxID3);
        } else if (apiPayment.IsDeclined()) {
            payment.Declined(apiPayment.Id,
                             apiPayment.Result.Code,
                             apiPayment.Result.Description);
        } else if (apiPayment.IsRejected()) {
            payment.Error(apiPayment.Id,
                          apiPayment.Result.Code,
                          apiPayment.Result.Description);
        } else {
            throw UnrecognisedValueException.For(apiPayment.Result.Code);
        }
    }

    public async Task PrepareCheckoutAsync(TotalProcessingPayment payment,
                                           PrepareCheckoutReq req,
                                           PaymentsParameters parameters,
                                           bool saveCard) {
        var checkoutReq = GetPaymentReq(req);

        var res = await _checkoutClient.Value.PrepareCheckoutAsync(checkoutReq);

        payment.CheckoutPrepared(req.ReturnUrl, res.Ndc, res.Id);
    }

    private PaymentReq GetPaymentReq(PrepareCheckoutReq req) {
        var paymentReq = new PaymentReq();
        paymentReq.Amount = req.Value.Amount?.ToString(CultureInfo.InvariantCulture);
        paymentReq.Currency = req.Value.Currency.Name;
        paymentReq.PaymentType = "DB";
        paymentReq.EntityId = _totalProcessingApiSettings.EntityId;

        paymentReq.StandingInstruction = new StandingInstruction();
        paymentReq.StandingInstruction.Source = "CIT";
        paymentReq.StandingInstruction.Mode = "INITIAL";
        paymentReq.StandingInstruction.Type = "UNSCHEDULED";

        return paymentReq;
    }
}
