using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Extensions;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Content;
using N3O.Umbraco.Payments.Opayo.Controllers;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using Newtonsoft.Json;
using Refit;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo {
    public class TransactionsService : ITransactionsService {
        private readonly IOpayoClient _opayoClient;
        private readonly IContentCache _contentCache;
        private readonly IActionLinkGenerator _actionLinkGenerator;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IBrowserInfoAccessor _browserInfoAccessor;

        public TransactionsService(IOpayoClient opayoClient,
                                   IContentCache contentCache,
                                   IActionLinkGenerator actionLinkGenerator,
                                   IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                   IBrowserInfoAccessor browserInfoAccessor) {
            _opayoClient = opayoClient;
            _contentCache = contentCache;
            _actionLinkGenerator = actionLinkGenerator;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _browserInfoAccessor = browserInfoAccessor;
        }
        
        public async Task ProcessAsync(OpayoPayment payment,
                                       OpayoPaymentReq req,
                                       PaymentsParameters parameters,
                                       bool saveCard) {
            try {
                var apiRequest = GetProcessPaymentRequest(req, parameters, saveCard);
                
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

        private ApiPaymentTransactionReq GetProcessPaymentRequest(OpayoPaymentReq req,
                                                                  PaymentsParameters parameters,
                                                                  bool saveCard) {
            var settings = _contentCache.Single<OpayoSettingsContent>();
            
            var apiReq = new ApiPaymentTransactionReq();

            apiReq.Amount = (long) req.Value.Amount.GetValueOrThrow();
            apiReq.Currency = req.Value.Currency.Id;
            
            apiReq.Description = settings.GetTransactionDescription(parameters.Reference);
            apiReq.VendorTxCode = settings.GetTransactionId(parameters.Reference);

            var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();
            
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
            var url = _actionLinkGenerator.GetUrl<OpayoController>(x => x.Authorize(null), new { flowId = parameters.FlowId });
            apiReq.StrongCustomerAuthentication = new ApiStrongCustomerAuthentication();
            apiReq.StrongCustomerAuthentication.NotificationUrl =  url;
            apiReq.StrongCustomerAuthentication.BrowserIp = _remoteIpAddressAccessor.GetRemoteIpAddress().ToString();
            apiReq.StrongCustomerAuthentication.BrowserAcceptHeader = _browserInfoAccessor.GetAccept();
            apiReq.StrongCustomerAuthentication.BrowserJavascriptEnabled = req.BrowserParameters.JavaScriptEnabled.GetValueOrThrow();
            apiReq.StrongCustomerAuthentication.BrowserJavaEnabled = req.BrowserParameters.JavaEnabled.GetValueOrThrow();
            if (_browserInfoAccessor.GetLanguage().Split(",").HasAny()) {
                apiReq.StrongCustomerAuthentication.BrowserLanguage = _browserInfoAccessor.GetLanguage().Split(",")[0];
            } else {
                apiReq.StrongCustomerAuthentication.BrowserLanguage = _browserInfoAccessor.GetLanguage();
            }
            apiReq.StrongCustomerAuthentication.BrowserScreenHeight = req.BrowserParameters.ScreenHeight.ToString();
            apiReq.StrongCustomerAuthentication.BrowserScreenWidth = req.BrowserParameters.ScreenWidth.ToString();
            apiReq.StrongCustomerAuthentication.BrowserUserAgent = _browserInfoAccessor.GetUserAgent();
            apiReq.StrongCustomerAuthentication.ChallengeWindowSize = req.ChallengeWindowSize.Name;
            apiReq.StrongCustomerAuthentication.BrowserColorDepth = req.BrowserParameters.GetColourDepth().ToString();
            apiReq.StrongCustomerAuthentication.BrowserTimezone = req.BrowserParameters.UtcOffsetMinutes.ToString();
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