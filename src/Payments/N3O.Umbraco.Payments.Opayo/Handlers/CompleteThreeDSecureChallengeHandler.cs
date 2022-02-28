using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Client;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CompleteThreeDSecureChallengeHandler :
        PaymentsHandler<CompleteThreeDSecureChallengeCommand, ThreeDSecureChallengeReq, OpayoPayment> {
        private readonly IOpayoClient _opayoClient;

        public CompleteThreeDSecureChallengeHandler(IPaymentsScope paymentsScope, IOpayoClient opayoClient)
            : base(paymentsScope) {
            _opayoClient = opayoClient;
        }

        protected override async Task HandleAsync(CompleteThreeDSecureChallengeCommand req,
                                                  OpayoPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            var apiReq = new ApiThreeDSecureChallengeResponse();
            apiReq.CRes = req.Model.CRes;
            apiReq.TransactionId = payment.OpayoTransactionId;

            var transaction = await _opayoClient.CompleteThreeDSecureChallengeResponseAsync(apiReq);

            payment.ThreeDSecureComplete(req.Model.CRes);

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
    }
}