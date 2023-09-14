using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
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

    public void ApplyApiTransaction(TotalProcessingPayment payment, ApiPaymentRes apiPayment) {
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
        } else if (apiPayment.IsRejected() || apiPayment.HasError()) {
            payment.Error(apiPayment.Id,
                          apiPayment.Result.Code,
                          apiPayment.Result.Description);
        } else {
            throw UnrecognisedValueException.For(apiPayment.Result.Code);
        }
    }

    public async Task PrepareCredentialCheckoutAsync(TotalProcessingCredential credential,
                                                     PaymentsParameters parameters,
                                                     PrepareCheckoutReq req) {
        var paymentReq = GetPaymentReq(req.Value, parameters, true);
        
        var res = await _checkoutClient.Value.PrepareCheckoutAsync(paymentReq);

        credential.CheckoutPrepared(req.ReturnUrl, res.Ndc, res.Id);
    }

    public async Task PreparePaymentCheckoutAsync(TotalProcessingPayment payment,
                                                  PaymentsParameters parameters,
                                                  PrepareCheckoutReq req) {
        var paymentReq = GetPaymentReq(req.Value, parameters, false);

        var res = await _checkoutClient.Value.PrepareCheckoutAsync(paymentReq);

        payment.CheckoutPrepared(req.ReturnUrl, res.Ndc, res.Id);
    }
    
    private PaymentReq GetPaymentReq(MoneyReq value, PaymentsParameters parameters, bool store) {
        var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
        
        var req = new PaymentReq();
        req.Amount = value.Amount?.ToString(CultureInfo.InvariantCulture);
        req.Currency = value.Currency.Name;
        req.PaymentType = "DB";
        req.EntityId = _totalProcessingApiSettings.EntityId;
        req.Billing = GetBillingAddress(billingInfo);

        if (store) {
            req.CreateRegistration = true;
            req.StandingInstruction = new StandingInstruction();
            req.StandingInstruction.Source = "CIT";
            req.StandingInstruction.Mode = "INITIAL";
            req.StandingInstruction.Type = "UNSCHEDULED";
        }

        return req;
    }
    
    private ApiBillingReq GetBillingAddress(IBillingInfo billing) {
        var req = new ApiBillingReq();
        req.Street1 = GetText(billing.Address.Line1, 50, true);
        req.City = GetText(billing.Address.Locality, 40, true);
        req.Postcode = GetText(billing.Address.PostalCode, 10, true, "0000");
        req.Country = billing.Address.Country.Iso2Code;

        return req;
    }
    
    private string GetText(string value, int maxLength, bool required, string defaultValue = ".") {
        if (!value.HasValue() && required) {
            value = defaultValue;
        }

        if (value == null) {
            return null;
        }

        return value.RemoveNonAscii().Trim().Left(maxLength);
    }
}
