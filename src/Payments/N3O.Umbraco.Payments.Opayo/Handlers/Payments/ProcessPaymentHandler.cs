using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class ProcessPaymentHandler : PaymentsHandler<ProcessPaymentCommand, OpayoPaymentReq, OpayoPayment> {
        private readonly ITransactionsService _transactionsService;

        public ProcessPaymentHandler(IPaymentsScope paymentsScope, ITransactionsService transactionsService)
            : base(paymentsScope) {
            _transactionsService = transactionsService;
        }

        protected override async Task HandleAsync(ProcessPaymentCommand req,
                                                  OpayoPayment payment,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            await _transactionsService.ProcessAsync(payment, req.Model, parameters, false);
        }
    }
}