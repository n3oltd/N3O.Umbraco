using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Bambora.Client;
using N3O.Umbraco.Payments.Bambora.Commands;
using N3O.Umbraco.Payments.Bambora.Extensions;
using N3O.Umbraco.Payments.Bambora.Models;
using N3O.Umbraco.Payments.Bambora.Models.ThreeDSecureChallenge;
using Newtonsoft.Json;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Bambora.Handlers {
    public class CompleteThreeDSecureChallengeHandler :
        PaymentsHandler<CompleteThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, BamboraPayment> {
        private readonly IBamboraPaymentsClient _paymentsClient;

        public CompleteThreeDSecureChallengeHandler(IPaymentsScope paymentsScope, IBamboraPaymentsClient paymentsClient)
            : base(paymentsScope) {
            _paymentsClient = paymentsClient;
        }

        protected override async Task HandleAsync(CompleteThreeDSecureChallengeCommand req,
                                                  BamboraPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var apiReq = new ThreeDSecureChallenge();
                apiReq.CardResponse = new ThreeDSecureCardResponse();
                apiReq.PaymentMethod = "token";
                apiReq.ThreeDSessionData = payment.Card.ThreeDSecureAcsTransId;
                apiReq.CardResponse.Cres = req.Model.CRes;

                var apiPayment = await _paymentsClient.CompleteThreeDSecureAsync(apiReq);

                payment.ThreeDSecureComplete(req.Model.CRes);

                if (apiPayment.IsAuthorised()) {
                    payment.Paid(apiPayment.Id, apiPayment.MessageId.GetValueOrThrow(), apiPayment.Message);
                } else if (apiPayment.IsDeclined()) {
                    payment.Declined(apiPayment.Id, apiPayment.MessageId.GetValueOrThrow(), apiPayment.Message);
                } else {
                    throw UnrecognisedValueException.For(apiPayment.Message);
                }
            } catch (ApiException apiException) {
                var apiPaymentError = apiException.Content.IfNotNull(JsonConvert.DeserializeObject<ApiPaymentError>);

                if (apiPaymentError.IsDeclined()) {
                    payment.Declined(apiPaymentError.TransactionId, apiPaymentError.Code, apiPaymentError.Message);
                } else {
                    payment.Error(apiPaymentError.TransactionId, apiPaymentError.Code, apiPaymentError.Message);
                }
            }
        }
    }
}