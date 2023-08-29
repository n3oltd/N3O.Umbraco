using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Clients;
using N3O.Umbraco.Payments.TotalProcessing.Extensions;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using Payments.TotalProcessing.Clients.Models;
using Refit;
using System;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing;

public class TotalProcessingHelper : ITotalProcessingHelper {
    private readonly ITotalProcessingClient _checkoutClient;
    private readonly TotalProcessingApiSettings _totalProcessingApiSettings;

    public TotalProcessingHelper(ITotalProcessingClient checkoutClient,
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

    public async Task PrepareCheckout(TotalProcessingPayment payment,
                                      PrepareCheckoutReq req,
                                      PaymentsParameters parameters,
                                      bool saveCard) {
        var checkoutReq = GetPaymentReq(req);

        var res = await _checkoutClient.PrepareCheckoutAsync(checkoutReq);

        payment.UpdateCheckoutId(req.ReturnUrl, res.Ndc, res.Id);
    }

    private PaymentReq GetPaymentReq(PrepareCheckoutReq req) {
        var paymentReq = new PaymentReq();
        paymentReq.Amount = req.Value.Amount?.ToString();
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
