using N3O.Umbraco.Extensions;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.TotalProcessing.Clients;
using N3O.Umbraco.Payments.TotalProcessing.Commands;
using N3O.Umbraco.Payments.TotalProcessing.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.TotalProcessing.Handlers;

public class CredentialProcessedHandler :
    PaymentsHandler<CredentialProcessedCommand, CheckoutCompletedReq, TotalProcessingCredential> {
    private readonly ITotalProcessingClient _checkoutClient;
    private readonly ITotalProcessingHelper _totalProcessingHelper;
    private readonly TotalProcessingApiSettings _totalProcessingApiSettings;
    
    public CredentialProcessedHandler(IPaymentsScope paymentsScope,
                                      ITotalProcessingClient checkoutClient,
                                      ITotalProcessingHelper totalProcessingHelper,
                                      TotalProcessingApiSettings totalProcessingApiSettings) 
        : base(paymentsScope) {
        _checkoutClient = checkoutClient;
        _totalProcessingHelper = totalProcessingHelper;
        _totalProcessingApiSettings = totalProcessingApiSettings;
    }

    protected override async Task HandleAsync(CredentialProcessedCommand req,
                                              TotalProcessingCredential credential,
                                              PaymentsParameters parameters,
                                              CancellationToken cancellationToken) {
        var apiTransaction = await _checkoutClient.GetPaymentAsync(_totalProcessingApiSettings.EntityId, req.Model.Id);

        await DoAsync<TotalProcessingPayment>(payment => {
            _totalProcessingHelper.ApplyApiTransaction(payment, apiTransaction);

            return Task.CompletedTask;
        }, cancellationToken);
        
        if (apiTransaction.RegistrationId.HasValue()) {
            credential.SetUp(apiTransaction.RegistrationId);
        } else {
            credential.Error(apiTransaction.Result.Code, apiTransaction.Result.Description);
        }
    }
}
