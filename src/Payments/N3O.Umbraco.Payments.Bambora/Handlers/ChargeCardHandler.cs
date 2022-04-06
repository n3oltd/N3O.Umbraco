using N3O.Umbraco.Context;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Bambora.Commands;
using N3O.Umbraco.Payments.Bambora.Extensions;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using Newtonsoft.Json;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Handlers {
    public class ChargeCardHandler : PaymentsHandler<ChargeCardCommand, ChargeCardReq, BamboraPayment> {
        private readonly IBamboraPaymentsClient _paymentsClient;
        private readonly IBrowserInfoAccessor _browserInfoAccessor;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IActionLinkGenerator _actionLinkGenerator;

        public ChargeCardHandler(IPaymentsScope paymentsScope,
                                 IBamboraPaymentsClient paymentsClient,
                                 IBrowserInfoAccessor browserInfoAccessor,
                                 IRemoteIpAddressAccessor remoteIpAddressAccessor,
                                 IActionLinkGenerator actionLinkGenerator) : base(paymentsScope) {
            _paymentsClient = paymentsClient;
            _browserInfoAccessor = browserInfoAccessor;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _actionLinkGenerator = actionLinkGenerator;
        }

        protected override async Task HandleAsync(ChargeCardCommand req,
                                                  BamboraPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var apiRequest = GetRequest(req.Model, parameters);

                var apiPayment = await _paymentsClient.CreatePaymentAsync(apiRequest);

                payment.UpdateToken(req.Model.Token);

                if (apiPayment.IsAuthorised()) {
                    payment.Paid(apiPayment.Id, apiPayment.MessageId.GetValueOrThrow(), apiPayment.Message);
                } else if (apiPayment.IsDeclined()) {
                    payment.Declined(apiPayment.Id, apiPayment.MessageId.GetValueOrThrow(), apiPayment.Message);
                } else {
                    throw UnrecognisedValueException.For(apiPayment.Message);
                }
            } catch (ApiException apiException) {
                if (apiException.RequiresThreeDSecure()) {
                    var threeDSecure = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<ThreeDRes>);
                    
                    payment.RequireThreeDSecureV2(req.Model.ReturnUrl,
                                                  threeDSecure?.ChallengeUrl,
                                                  threeDSecure?.ThreeDSessionData,
                                                  threeDSecure?.DecodedContents);
                } else {
                    var apiPaymentError = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<ApiPaymentError>);

                    if (apiPaymentError.IsDeclined()) {
                        payment.Declined(apiPaymentError.TransactionId, apiPaymentError.Code, apiPaymentError.Message);
                    } else {
                        payment.Error(apiPaymentError.TransactionId, apiPaymentError.Code, apiPaymentError.Message);
                    }
                }
            }
        }

        private ApiPaymentReq GetRequest(ChargeCardReq req, PaymentsParameters parameters) {
            var billingInfo = parameters.BillingInfoAccessor.GetBillingInfo();

            var apiReq = new ApiPaymentReq();
            apiReq.Amount = req.Value.Amount.GetValueOrThrow();
            apiReq.BillingAddress = billingInfo.GetApiBillingAddress();
            apiReq.PaymentMethod = "token";
            apiReq.CustomerIp = _remoteIpAddressAccessor.GetRemoteIpAddress().ToString();
            apiReq.Token = GetRequestToken(req, billingInfo);
            apiReq.ReturnUrl = _actionLinkGenerator.GetPaymentThreeDSecureUrl(parameters.FlowId);
            
            return apiReq;
        }

        private Token GetRequestToken(ChargeCardReq req, BillingInfo billingInfo) {
            var token = new Token();
            token.Code = req.Token;
            token.Complete = true;
            token.Name = billingInfo.Name.FirstName;
            token.ThreeDSecure = new ThreeDSecure();
            token.ThreeDSecure.Enabled = true;
            token.ThreeDSecure.Version = 2;
            token.ThreeDSecure.AuthRequired = false;
            token.ThreeDSecure.Browser = _browserInfoAccessor.GetBrowserReq(req.BrowserParameters);

            return token;
        }
    }
}