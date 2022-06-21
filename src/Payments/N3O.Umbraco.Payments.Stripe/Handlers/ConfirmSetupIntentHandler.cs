using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using Stripe;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe.Handlers;

public class ConfirmSetupIntentHandler : PaymentsHandler<ConfirmSetupIntentCommand, None, StripeCredential> {
    private readonly StripeClient _stripeClient;
    
    public ConfirmSetupIntentHandler(IPaymentsScope paymentsScope, StripeClient stripeClient)
        : base(paymentsScope) {
        _stripeClient = stripeClient;
    }

    protected override async Task HandleAsync(ConfirmSetupIntentCommand req,
                                              StripeCredential credential,
                                              PaymentsParameters parameters,
                                              CancellationToken cancellationToken) {
        try {
            var service = new SetupIntentService(_stripeClient);
        
            var setupIntent = await service.GetAsync(credential.StripeSetupIntentId,
                                                     cancellationToken: cancellationToken);
            
            credential.IntentConfirmed(setupIntent);
        } catch (StripeException ex) {
            credential.Error(ex);
        }
    }
}
