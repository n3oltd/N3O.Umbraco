using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Handlers;

public class PrepareCheckoutHandler :
    PaymentsHandler<PrepareCheckoutCommand, PrepareCheckoutReq, TotalProcessingPayment> {
    private readonly ITotalProcessingHelper _totalProcessingHelper;

    public PrepareCheckoutHandler(IPaymentsScope paymentsScope, ITotalProcessingHelper totalProcessingHelper)
        : base(paymentsScope) {
        _totalProcessingHelper = totalProcessingHelper;
    }

    protected override async Task HandleAsync(PrepareCheckoutCommand req,
                                              TotalProcessingPayment payment,
                                              PaymentsParameters parameters,
                                              CancellationToken cancellationToken) {
        await _totalProcessingHelper.PrepareCheckoutAsync(payment, req.Model, parameters, false);
    }
}
