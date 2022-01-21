using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using N3O.Umbraco.Payments.Testing;
using Newtonsoft.Json;
using Refit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, OpayoPaymentReq, OpayoPaymentRes> {
        private readonly IOpayoClient _opayoClient;
        private readonly TestPaymentsFlow _testPaymentsFlow;
        private readonly IUmbracoMapper _umbracoMapper;

        public ProcessPaymentHandler(IOpayoClient opayoClient,
                                     TestPaymentsFlow testPaymentsFlow,
                                     IUmbracoMapper umbracoMapper) {
            _opayoClient = opayoClient;
            _testPaymentsFlow = testPaymentsFlow;
            _umbracoMapper = umbracoMapper;
        }

        public async Task<OpayoPaymentRes> Handle(ProcessPaymentCommand req, CancellationToken cancellationToken) {
            var payment = _testPaymentsFlow.GetOrCreatePaymentObject<OpayoPayment>();
            var billingInfo = _testPaymentsFlow.GetBillingInfo();
            await ProcessAsync(payment, req.Model, billingInfo);

            return _umbracoMapper.Map<OpayoPayment, OpayoPaymentRes>(payment);
        }

        private async Task ProcessAsync(OpayoPayment payment, OpayoPaymentReq req, BillingInfo billingInfo) {
            var apiRequest = GetProcessPaymentRequest(payment, req, billingInfo);
            
            try {
                var transaction = await _opayoClient.TransactionAsync(apiRequest);

                if (transaction.IsAuthorised()) {
                    payment.Paid(transaction.TransactionId, transaction.BankAuthorisationCode, transaction.RetrievalReference.GetValueOrThrow());
                }

                if (transaction.IsDeclined()) {
                    payment.Declined(transaction.StatusDetail);
                }
                if (transaction.RequiresThreeDSecure()) {
                    payment.ThreeDSecureRequired(transaction.TransactionId, transaction.AcsTransId, transaction.AcsUrl, transaction.CReq);
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

        private ApiPaymentTransactionReq GetProcessPaymentRequest(OpayoPayment payment,
                                                                  OpayoPaymentReq req,
                                                                  IBillingInfo billingInfo) {
            var apiReq = new ApiPaymentTransactionReq();

            apiReq.Amount = (long) req.Value.Amount.GetValueOrThrow();
            apiReq.Currency = req.Value.Currency.Id;
            // TODO Both description and vendor TX code should come from checkout reference
            apiReq.Description = "This is a test payment";
            apiReq.VendorTxCode = payment.Id.ToString();

            apiReq.BillingAddress = GetBillingAddress(billingInfo.Address);
            apiReq.ApplyThreeDSecure = "Force";
            apiReq.CustomerFirstName = billingInfo.Name.FirstName;
            apiReq.CustomerLastName = billingInfo.Name.LastName;

            apiReq.PaymentMethod = new ApiPaymentMethodReq();

            apiReq.PaymentMethod = GetApiPaymentMethodReq(req);

            apiReq.EntryMethod = "Ecommerce";
            apiReq.TransactionType = "Payment";
            if (req.SaveCard.GetValueOrThrow()) {
                apiReq.CredentialType = new ApiCredentialTypeReq() {
                    CofUsage = "First", InitiatedType = "CIT", MitType = "Unscheduled"
                };
            }

            // TODO These should not be hard coded but should come from flow or accessors
            apiReq.StrongCustomerAuthentication = new ApiStrongCustomerAuthentication();
            apiReq.StrongCustomerAuthentication.NotificationUrl = "https://127.0.0.1:44389/callback3ds";
            apiReq.StrongCustomerAuthentication.BrowserIp = "127.0.0.1";
            apiReq.StrongCustomerAuthentication.BrowserAcceptHeader = "*";
            apiReq.StrongCustomerAuthentication.BrowserJavascriptEnabled = false;
            apiReq.StrongCustomerAuthentication.BrowserLanguage = "en-GB";
            apiReq.StrongCustomerAuthentication.BrowserUserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.14; rv:67.0) Gecko/20100101 Firefox/67.0";
            apiReq.StrongCustomerAuthentication.ChallengeWindowSize = "Small";
            apiReq.StrongCustomerAuthentication.TransactionType = "GoodsAndServicePurchase";

            return apiReq;
        }

        private ApiBillingAddressReq GetBillingAddress(IAddress address) {
            // TODO Below is missing address line 2, 3 etc. + trimming these + removing unacceptable characters in API
            var billingAddress = new ApiBillingAddressReq();
            billingAddress.Address1 = address.Line1;
            billingAddress.City = address.Locality;
            billingAddress.PostalCode = address.PostalCode;
            billingAddress.Country = address.Country.Iso2Code;

            return billingAddress;
        }

        private ApiPaymentMethodReq GetApiPaymentMethodReq(OpayoPaymentReq req) {
            var apiCard = new ApiCard();
            apiCard.CardIdentifier = req.CardIdentifier;
            apiCard.MerchantSessionKey = req.MerchantSessionKey;
            apiCard.Reusable = false;
            apiCard.Save = req.SaveCard.GetValueOrThrow();

            var apiPaymentMethodReq = new ApiPaymentMethodReq();
            apiPaymentMethodReq.Card = apiCard;

            return apiPaymentMethodReq;
        }
    }
}