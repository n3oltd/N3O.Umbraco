using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Clients;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Handlers;

public class PaymentProcessedHandler :
    PaymentsHandler<PaymentProcessedCommand, PaymentProcessedReq, TotalProcessingPayment> {
    private readonly ITotalProcessingClient _checkoutClient;
    private readonly ITotalProcessingHelper _totalProcessingHelper;
    private readonly TotalProcessingApiSettings _totalProcessingApiSettings;
    
    public PaymentProcessedHandler(IPaymentsScope paymentsScope,
                                   ITotalProcessingClient checkoutClient,
                                   ITotalProcessingHelper totalProcessingHelper,
                                   TotalProcessingApiSettings totalProcessingApiSettings) 
        : base(paymentsScope) {
        _checkoutClient = checkoutClient;
        _totalProcessingHelper = totalProcessingHelper;
        _totalProcessingApiSettings = totalProcessingApiSettings;
    }

    protected override async Task HandleAsync(PaymentProcessedCommand req,
                                              TotalProcessingPayment payment,
                                              PaymentsParameters parameters,
                                              CancellationToken cancellationToken) {
        var apiPayment = await _checkoutClient.GetPaymentAsync(_totalProcessingApiSettings.EntityId, req.Model.Id);

        _totalProcessingHelper.ApplyApiPayment(payment, apiPayment);
    }
}
