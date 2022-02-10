using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using Stripe;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Payments.Stripe.Handlers {
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
            
                var setupIntentOptions = GetSetupIntentOptions(credential);
                
                var setupIntent = await service.ConfirmAsync(credential.StripeSetupIntentId,
                                                             setupIntentOptions,
                                                             cancellationToken: cancellationToken);
                
                credential.IntentConfirmed(setupIntent);
            } catch (StripeException ex) {
                credential.Error(ex);
            }
        }
        
        private SetupIntentConfirmOptions GetSetupIntentOptions(StripeCredential credential) {
            var options = new SetupIntentConfirmOptions();
            options.ClientSecret = credential.StripeSetupIntentClientSecret;
            options.PaymentMethod = credential.StripePaymentMethodId;

            return options;
        }
    }
}