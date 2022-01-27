using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.Entities;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using Newtonsoft.Json;
using Refit;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public abstract class PaymentHandler<TCommand, TReq> : PaymentsHandler<TCommand, TReq, OpayoPayment> 
        where TCommand : PaymentsCommand<TReq, OpayoPayment> {
        private readonly IPaymentsFlow _paymentsFlow;
        private readonly IOpayoClient _opayoClient;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IBrowserInfoAccessor _userAgentAccessor;

        protected PaymentHandler(IPaymentsFlow paymentsFlow,
                                 IPaymentsScope paymentsScope,
                                 IOpayoClient opayoClient,
                                 IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                 IBrowserInfoAccessor userAgentAccessor) : base(paymentsScope) {
            _paymentsFlow = paymentsFlow;
            _opayoClient = opayoClient;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _userAgentAccessor = userAgentAccessor;
        }

        protected async Task ProcessPaymentAsync(OpayoPayment payment, OpayoPaymentReq req, BillingInfo billingInfo, bool saveCard) {
            var transactionInfo = _paymentsFlow.GetTransctionInfo();
            var apiRequest = GetProcessPaymentRequest(transactionInfo, payment, req, billingInfo, saveCard);
            
            try {
                var transaction = await _opayoClient.TransactionAsync(apiRequest);

                if (transaction.IsAuthorised()) {
                    payment.Paid(transaction.TransactionId, transaction.BankAuthorisationCode, transaction.RetrievalReference.GetValueOrThrow());
                }

                if (transaction.IsDeclined()) {
                    payment.Declined(transaction.StatusDetail);
                }
                
                if (transaction.RequiresThreeDSecure()) {
                    payment.ThreeDSecureRequired(req.CallbackUrl, transaction.TransactionId, transaction.AcsTransId, transaction.AcsUrl, transaction.CReq);
                }
            } catch (ApiException apiException) {
                var opayoErrors = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<OpayoErrors>);
                var opayoError = opayoErrors?.Errors.OrEmpty().FirstOrDefault() ??
                                 apiException.Content.IfNotNull(JsonConvert.DeserializeObject<OpayoError>);

                var errorMessage = opayoError?.ClientMessage ?? opayoError?.Description ?? opayoError?.StatusDetail;
                var errorCode = opayoError?.Code ?? opayoError?.StatusCode;
                var transactionId = opayoError?.TransactionId;

                payment.Failed(transactionId, errorCode, errorMessage);
            }
        }

        private ApiPaymentTransactionReq GetProcessPaymentRequest(TransactionInfo transactionInfo,
                                                                  OpayoPayment payment,
                                                                  OpayoPaymentReq req,
                                                                  IBillingInfo billingInfo,
                                                                  bool saveCard) {
            var apiReq = new ApiPaymentTransactionReq();

            apiReq.Amount = (long) req.Value.Amount.GetValueOrThrow();
            apiReq.Currency = req.Value.Currency.Id;
            
            apiReq.Description = transactionInfo.Description;
            apiReq.VendorTxCode = transactionInfo.Reference.Text;

            apiReq.BillingAddress = GetBillingAddress(billingInfo.Address);
            apiReq.ApplyThreeDSecure = "Force"; // TODO use UseMSPSetting
            apiReq.CustomerFirstName = billingInfo.Name.FirstName;
            apiReq.CustomerLastName = billingInfo.Name.LastName;

            apiReq.PaymentMethod = new ApiPaymentMethodReq();

            apiReq.PaymentMethod = GetApiPaymentMethodReq(req, saveCard);

            apiReq.EntryMethod = "Ecommerce";
            apiReq.TransactionType = "Payment";
            if (saveCard) {
               var apiCredentialTypeReq =  new ApiCredentialTypeReq();
               apiCredentialTypeReq.MitType = "Unscheduled";
               apiCredentialTypeReq.InitiatedType = "CIT";
               apiCredentialTypeReq.CofUsage = "First";
               apiReq.CredentialType = apiCredentialTypeReq;
            }

            apiReq.StrongCustomerAuthentication = new ApiStrongCustomerAuthentication();
            apiReq.StrongCustomerAuthentication.NotificationUrl = $"https://127.0.0.1:5001/umbraco/api/Opayo/{_paymentsFlow.Id}/authorize";
            apiReq.StrongCustomerAuthentication.BrowserIp = _remoteIpAddressAccessor.GetRemoteIpAddress().ToString();
            apiReq.StrongCustomerAuthentication.BrowserAcceptHeader = _userAgentAccessor.GetAccept();
            apiReq.StrongCustomerAuthentication.BrowserJavascriptEnabled = req.BrowserParameters.JavaScriptEnabled.GetValueOrThrow();
            apiReq.StrongCustomerAuthentication.BrowserJavaEnabled = req.BrowserParameters.JavaEnabled.GetValueOrThrow();
            if (_userAgentAccessor.GetLanguage().Split(",").HasAny()) {
                apiReq.StrongCustomerAuthentication.BrowserLanguage = _userAgentAccessor.GetLanguage().Split(",")[0];
            } else {
                apiReq.StrongCustomerAuthentication.BrowserLanguage = _userAgentAccessor.GetLanguage();
            }
            apiReq.StrongCustomerAuthentication.BrowserScreenHeight = req.BrowserParameters.ScreenHeight.ToString();
            apiReq.StrongCustomerAuthentication.BrowserScreenWidth = req.BrowserParameters.ScreenWidth.ToString();
            apiReq.StrongCustomerAuthentication.BrowserUserAgent = _userAgentAccessor.GetUserAgent();
            apiReq.StrongCustomerAuthentication.ChallengeWindowSize = req.ChallengeWindowSize.Name;
            apiReq.StrongCustomerAuthentication.BrowserColorDepth = req.BrowserParameters.ColorDepth.GetColorDepth()?.ToString();
            apiReq.StrongCustomerAuthentication.BrowserTimezone = req.BrowserParameters.Timezone.UtcOffset.ToString();
            apiReq.StrongCustomerAuthentication.TransactionType = "GoodsAndServicePurchase";

            return apiReq;
        }

        private ApiBillingAddressReq GetBillingAddress(IAddress address) {
            var billingAddress = new ApiBillingAddressReq();
            billingAddress.Address1 = address.Line1;
            billingAddress.Address2 = address.Line2;
            billingAddress.Address3 = address.Line3;
            billingAddress.City = address.Locality;
            billingAddress.PostalCode = address.PostalCode;
            billingAddress.Country = address.Country.Iso2Code;

            return billingAddress;
        }

        private ApiPaymentMethodReq GetApiPaymentMethodReq(OpayoPaymentReq req, bool saveCard) {
            var apiCard = new ApiCard();
            apiCard.CardIdentifier = req.CardIdentifier;
            apiCard.MerchantSessionKey = req.MerchantSessionKey;
            apiCard.Reusable = false;
            apiCard.Save = saveCard;

            var apiPaymentMethodReq = new ApiPaymentMethodReq();
            apiPaymentMethodReq.Card = apiCard;

            return apiPaymentMethodReq;
        }
    }
}