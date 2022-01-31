using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Opayo.Commands;
using N3O.Umbraco.Payments.Opayo.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Opayo.Handlers {
    public class CreateCredentialHandler : PaymentsHandler<CreateCredentialCommand, OpayoCredentialReq, OpayoCredential> {
        private readonly ITransactionsService _transactionsService;

        public CreateCredentialHandler(IPaymentsScope paymentsScope, ITransactionsService transactionsService)
            : base(paymentsScope) {
            _transactionsService = transactionsService;
        }

        protected override async Task HandleAsync(CreateCredentialCommand req,
                                                  OpayoCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            await _transactionsService.ProcessAsync(credential.AdvancePayment,
                                                    req.Model.AdvancePayment,
                                                    parameters,
                                                    true);
        }
    }
}