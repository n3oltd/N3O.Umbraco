using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Commands;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Clients;
using N3O.Umbraco.Payments.Opayo.Extensions;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public abstract class CompleteThreeDSecureHandlerBase<TCommand, TRequest> : PaymentsHandler<TCommand, TRequest, OpayoPayment>
        where TCommand : PaymentsCommand<TRequest, OpayoPayment> {
        protected CompleteThreeDSecureHandlerBase(IPaymentsScope paymentsScope) : base(paymentsScope) { }

        protected override async Task HandleAsync(TCommand req, OpayoPayment payment, PaymentsParameters parameters, CancellationToken cancellationToken) {
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

        protected abstract Task<ApiTransactionRes> ProcessThreeDSecureAsync(TRequest req, OpayoPayment payment, PaymentsParameters parameters);
    }
}