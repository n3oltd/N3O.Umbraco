using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using Newtonsoft.Json;
using Refit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CompleteThreeDSecureHandler : PaymentsHandler<CompleteThreeDSecureCommand, CompleteThreeDSecureReq, OpayoPayment> {
        private readonly IOpayoClient _opayoClient;

        public CompleteThreeDSecureHandler(IPaymentsScope paymentsScope, IOpayoClient opayoClient)
            : base(paymentsScope) {
            _opayoClient = opayoClient;
        }

        protected override async Task HandleAsync(CompleteThreeDSecureCommand req,
                                                  OpayoPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                ApiTransactionRes transaction;
                if (req.Model.PaRes.HasValue()) {
                    transaction = await CompleteV1Async(req.Model, payment);
                } else {
                    transaction = await CompleteV2Async(req.Model, payment);
                }

                if (transaction.IsAuthorised()) {
                    payment.Paid(payment.VendorTxCode,
                                 transaction.TransactionId,
                                 transaction.StatusCode,
                                 transaction.StatusDetail,
                                 transaction.BankAuthorisationCode,
                                 transaction.RetrievalReference.GetValueOrThrow());
                } else if (transaction.IsDeclined() || transaction.IsRejected()) {
                    payment.Declined(payment.VendorTxCode, transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
                } else if (transaction.IsRejected()) {
                    payment.Error(payment.VendorTxCode, transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
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

                payment.Error(payment.VendorTxCode, transactionId, errorCode, errorMessage);
            }
        }

        private async Task<ApiTransactionRes> CompleteV2Async(CompleteThreeDSecureReq req,
                                                              OpayoPayment payment) {
            var apiReq = new ApiThreeDSecureChallengeResponse();
            apiReq.CRes = req.CRes;
            apiReq.TransactionId = payment.OpayoTransactionId;

            var transaction = await _opayoClient.CompleteThreeDSecureChallengeResponseAsync(apiReq);

            payment.ThreeDSecureComplete(req.CRes);

            return transaction;
        }

        private async Task<ApiTransactionRes> CompleteV1Async(CompleteThreeDSecureReq req, OpayoPayment payment) {
            var apiReq = new ApiThreeDSecureFallbackResponse();
            apiReq.PaRes = req.PaRes;
            apiReq.TransactionId = payment.OpayoTransactionId;

            await _opayoClient.CompleteThreeDSecureFallbackResponseAsync(apiReq);

            payment.ThreeDSecureComplete(req.PaRes);

            var transaction = await _opayoClient.GetTransactionByIdAsync(payment.OpayoTransactionId);

            return transaction;
        }
    }
}