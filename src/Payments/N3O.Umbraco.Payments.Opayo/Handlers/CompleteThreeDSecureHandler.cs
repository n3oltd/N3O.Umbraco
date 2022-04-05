using N3O.Umbraco.Mediator;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CompleteThreeDSecureHandler : PaymentsHandler<CompleteThreeDSecureCommand, CompleteThreeDSecureReq, OpayoPayment> {
        private readonly IOpayoClient _opayoClient;

        public CompleteThreeDSecureHandler(IPaymentsScope paymentsScope, IOpayoClient opayoClient)
            : base(paymentsScope) {
            _opayoClient = opayoClient;
        }
        
        protected override Task HandleAsync(CompleteThreeDSecureCommand req,
                                            OpayoPayment paymentObject,
                                            PaymentsParameters parameters,
                                            CancellationToken cancellationToken) {
            if (req.Model.PaRes) {
                // v1
            } else {
                // v2
            }
            
            var transaction = await ProcessThreeDSecureAsync(req.Model, payment, parameters);

            if (transaction.IsAuthorised()) {
                payment.Paid(transaction.TransactionId,
                             transaction.StatusCode,
                             transaction.StatusDetail,
                             transaction.BankAuthorisationCode,
                             transaction.RetrievalReference.GetValueOrThrow());
            } else if (transaction.IsDeclined() || transaction.IsRejected()) {
                payment.Declined(transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
            } else if (transaction.IsRejected()) {
                payment.Error(transaction.TransactionId, transaction.StatusCode, transaction.StatusDetail);
            } else {
                throw UnrecognisedValueException.For(transaction.Status);
            }
        }

        protected override async Task<ApiTransactionRes> CompleteV2Async(CompleteThreeDSecureReq req,
                                                                                  OpayoPayment payment,
                                                                                  PaymentsParameters parameters) {
            var apiReq = new ApiThreeDSecureChallengeResponse();
            apiReq.CRes = req.CRes;
            apiReq.TransactionId = payment.OpayoTransactionId;

            var transaction = await _opayoClient.CompleteThreeDSecureChallengeResponseAsync(apiReq);

            payment.ThreeDSecureComplete(req.CRes);

            return transaction;
        }
        
        protected override async Task<ApiTransactionRes> CompleteV1Async(ThreeDSecureFallbackReq req, OpayoPayment payment, PaymentsParameters parameters) {
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