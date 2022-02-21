using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Context;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
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
    public class ChargeService : IChargeService {
        private readonly IOpayoClient _opayoClient;
        private readonly IContentCache _contentCache;
        private readonly IActionLinkGenerator _actionLinkGenerator;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IBrowserInfoAccessor _browserInfoAccessor;

        public ChargeService(IOpayoClient opayoClient,
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
        
        public async Task ChargeAsync(OpayoPayment payment,
                                      ChargeCardReq req,
                                      PaymentsParameters parameters,
                                      bool saveCard) {
            try {
                var apiRequest = GetApiPaymentTransactionReq(req, parameters, saveCard);
                
                var transaction = await _opayoClient.TransactionAsync(apiRequest);

                if (transaction.IsAuthorised()) {
                    payment.Paid(transaction.TransactionId,
                                 transaction.StatusCode,
                                 transaction.StatusDetail,
                                 transaction.BankAuthorisationCode,
                                 transaction.RetrievalReference.GetValueOrThrow());
                } else if (transaction.IsDeclined()) {
                    payment.Declined(transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
                } else if (transaction.RequiresThreeDSecure()) {
                    payment.RequireThreeDSecure(req.ReturnUrl,
                                                transaction.TransactionId,
                                                transaction.AcsUrl,
                                                transaction.AcsTransId,
                                                transaction.CReq);
                } else {
                    throw UnrecognisedValueException.For(transaction.Status);
                }
            } catch (ApiException apiException) {
                var opayoErrors = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<OpayoErrors>);
                var opayoError = opayoErrors?.Errors.OrEmpty().FirstOrDefault() ??
                                 apiException.Content.IfNotNull(JsonConvert.DeserializeObject<OpayoError>);

                var errorMessage = opayoError?.ClientMessage ?? opayoError?.Description ?? opayoError?.StatusDetail;
                var errorCode = opayoError?.Code ?? opayoError?.StatusCode;
                var transactionId = opayoError?.TransactionId;

                payment.Error(transactionId, errorCode, errorMessage);
            }
        }

        private ApiPaymentTransactionReq GetApiPaymentTransactionReq(ChargeCardReq req,
                                                                     PaymentsParameters parameters,
                                                                     bool saveCard) {
            var settings = _contentCache.Single<OpayoSettingsContent>();
            var apiReq = new ApiPaymentTransactionReq();

            apiReq.Amount = ((Money) req.Value).GetAmountInLowestDenomination();
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

            apiReq.StrongCustomerAuthentication = new ApiStrongCustomerAuthentication();
            apiReq.StrongCustomerAuthentication.BrowserIp = _remoteIpAddressAccessor.GetRemoteIpAddress().ToString();
            apiReq.StrongCustomerAuthentication.BrowserAcceptHeader = _browserInfoAccessor.GetAccept();
            apiReq.StrongCustomerAuthentication.BrowserJavascriptEnabled = req.BrowserParameters.JavaScriptEnabled.GetValueOrThrow();
            apiReq.StrongCustomerAuthentication.BrowserJavaEnabled = req.BrowserParameters.JavaEnabled.GetValueOrThrow();
            apiReq.StrongCustomerAuthentication.BrowserLanguage = _browserInfoAccessor.GetLanguage();
            apiReq.StrongCustomerAuthentication.BrowserScreenHeight = req.BrowserParameters.ScreenHeight.ToString();
            apiReq.StrongCustomerAuthentication.BrowserScreenWidth = req.BrowserParameters.ScreenWidth.ToString();
            apiReq.StrongCustomerAuthentication.BrowserUserAgent = _browserInfoAccessor.GetUserAgent();
            apiReq.StrongCustomerAuthentication.ChallengeWindowSize = req.ChallengeWindowSize.Name;
            apiReq.StrongCustomerAuthentication.BrowserColorDepth = req.BrowserParameters.GetColourDepth().ToString();
            apiReq.StrongCustomerAuthentication.BrowserTimezone = req.BrowserParameters.UtcOffsetMinutes.ToString();
            apiReq.StrongCustomerAuthentication.NotificationUrl = GetNotificationUrl(parameters.FlowId);
            apiReq.StrongCustomerAuthentication.TransactionType = "GoodsAndServicePurchase";

            return apiReq;
        }

        private string GetNotificationUrl(EntityId flowId) {
            return _actionLinkGenerator.GetUrl<OpayoController>(x => x.CompleteThreeDSecureChallenge(null),
                                                                new { flowId = flowId.ToString() });
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

        private ApiPaymentMethodReq GetApiPaymentMethodReq(ChargeCardReq req, bool saveCard) {
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