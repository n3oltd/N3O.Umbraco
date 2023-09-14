using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Handlers;

public class PreparePaymentCheckoutHandler 
    : PaymentsHandler<PreparePaymentCheckoutCommand, PrepareCheckoutReq, TotalProcessingPayment> {
    private readonly ITotalProcessingHelper _totalProcessingHelper;

    public PreparePaymentCheckoutHandler(IPaymentsScope paymentsScope, ITotalProcessingHelper totalProcessingHelper)
        : base(paymentsScope) {
        _totalProcessingHelper = totalProcessingHelper;
    }

    protected override async Task HandleAsync(PreparePaymentCheckoutCommand req,
                                              TotalProcessingPayment payment,
                                              PaymentsParameters parameters,
                                              CancellationToken cancellationToken) {
        await _totalProcessingHelper.PreparePaymentCheckoutAsync(payment, parameters, req.Model);
    }
}
