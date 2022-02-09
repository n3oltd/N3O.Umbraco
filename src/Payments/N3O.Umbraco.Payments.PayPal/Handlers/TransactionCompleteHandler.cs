using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.PayPal.Commands;
using N3O.Umbraco.Payments.PayPal.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.PayPal.Handlers {
    public class TransactionCompleteHandler : PaymentsHandler<TransactionCompleteCommand, PayPalTransactionReq, PayPalPayment> {
        public TransactionCompleteHandler(IPaymentsScope paymentsScope) : base(paymentsScope) { }

        protected override Task HandleAsync(TransactionCompleteCommand req,
                                            PayPalPayment payment,
                                            PaymentsParameters parameters,
                                            CancellationToken cancellationToken) {
            payment.Paid(req.Model.Email, req.Model.TransactionId);
            
            return Task.CompletedTask;
        }
    }
}