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

    public void ApplyApiTransaction(TotalProcessingPayment payment, ApiTransactionRes apiTransaction) {
        if (apiTransaction.IsAuthorised()) {
            payment.Paid(apiTransaction.Id,
                         apiTransaction.Result.Code,
                         apiTransaction.Result.Description,
                         apiTransaction.ResultDetails?.ConnectorTxID1,
                         apiTransaction.ResultDetails?.ConnectorTxID2,
                         apiTransaction.ResultDetails?.ConnectorTxID3);
        } else if (apiTransaction.IsDeclined()) {
            payment.Declined(apiTransaction.Id,
                             apiTransaction.Result.Code,
                             apiTransaction.Result.Description);
        } else if (apiTransaction.IsRejected() || apiTransaction.HasError()) {
            payment.Error(apiTransaction.Id,
                          apiTransaction.Result.Code,
                          apiTransaction.Result.Description);
        } else {
            throw UnrecognisedValueException.For(apiTransaction.Result.Code);
        }
    }

    public async Task PrepareCredentialCheckoutAsync(TotalProcessingCredential credential,
                                                     PaymentsParameters parameters,
                                                     PrepareCheckoutReq req) {
        var paymentReq = GetPaymentReq(req.Value, parameters);
        
        var res = await _checkoutClient.Value.PrepareCheckoutAsync(paymentReq);

        credential.CheckoutPrepared(req.ReturnUrl, res.Ndc, res.Id);
    }

    public async Task PreparePaymentCheckoutAsync(TotalProcessingPayment payment,
                                                  PaymentsParameters parameters,
                                                  PrepareCheckoutReq req) {
        var paymentReq = GetPaymentReq(req.Value, parameters);

        var res = await _checkoutClient.Value.PrepareCheckoutAsync(paymentReq);

        payment.CheckoutPrepared(req.ReturnUrl, res.Ndc, res.Id);
    }
    
    private PaymentReq GetPaymentReq(MoneyReq value, PaymentsParameters parameters) {
        var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
        
        var paymentReq = new PaymentReq();
        paymentReq.Amount = value.Amount?.ToString(CultureInfo.InvariantCulture);
        paymentReq.Currency = value.Currency.Name;
        paymentReq.PaymentType = "DB";
        paymentReq.EntityId = _totalProcessingApiSettings.EntityId;
        paymentReq.Billing = GetBillingAddress(billingInfo);
        paymentReq.StandingInstruction = new StandingInstruction();
        paymentReq.StandingInstruction.Source = "CIT";
        paymentReq.StandingInstruction.Mode = "INITIAL";
        paymentReq.StandingInstruction.Type = "UNSCHEDULED";
        paymentReq.CreateRegistration = true;

        return paymentReq;
    }
    
    private ApiBillingReq GetBillingAddress(IBillingInfo billing) {
        var billingAddress = new ApiBillingReq();
        billingAddress.Street1 = GetText(billing.Address.Line1, 50, true);
        billingAddress.City = GetText(billing.Address.Locality, 40, true);
        billingAddress.Postcode = GetText(billing.Address.PostalCode, 10, true, "0000");
        billingAddress.Country = billing.Address.Country.Iso2Code;

        return billingAddress;
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
