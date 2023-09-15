using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Handlers;

public class PrepareCredentialCheckoutHandler : PaymentsHandler<PrepareCredentialCheckoutCommand, PrepareCheckoutReq, TotalProcessingCredential> {
    private readonly ITotalProcessingHelper _totalProcessingHelper;

    public PrepareCredentialCheckoutHandler(IPaymentsScope paymentsScope,
                                     ITotalProcessingHelper totalProcessingHelper)
        : base(paymentsScope) {
        _totalProcessingHelper = totalProcessingHelper;
    }

    protected override Task HandleAsync(PrepareCredentialCheckoutCommand req,
                                        TotalProcessingCredential credential,
                                        PaymentsParameters parameters,
                                        CancellationToken cancellationToken) {
        return _totalProcessingHelper.PrepareCredentialCheckoutAsync(credential, parameters, req.Model);
    }
}
