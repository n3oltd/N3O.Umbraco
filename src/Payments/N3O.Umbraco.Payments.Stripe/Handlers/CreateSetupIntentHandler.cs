using N3O.Umbraco.Content;
using N3O.Umbraco.Payments.Handlers;
using N3O.Umbraco.Payments.Models;
using N3O.Umbraco.Payments.Stripe.Commands;
using N3O.Umbraco.Payments.Stripe.Models;
using Stripe;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Stripe.Handlers {
    public class CreateSetupIntentHandler :
        PaymentsHandler<CreateSetupIntentCommand, SetupIntentReq, StripeCredential> {
        private readonly IContentCache _contentCache;
        private readonly StripeClient _stripeClient;
        
        public CreateSetupIntentHandler(IPaymentsScope paymentsScope,
                                        IContentCache contentCache,
                                        StripeClient stripeClient)
            : base(paymentsScope) {
            _contentCache = contentCache;
            _stripeClient = stripeClient;
        }

        protected override async Task HandleAsync(CreateSetupIntentCommand req,
                                                  StripeCredential credential,
                                                  PaymentsParameters parameters,
                                                  CancellationToken cancellationToken) {
            try {
                var service = new SetupIntentService(_stripeClient);
            
                var setupIntentOptions = GetSetupIntentOptions(parameters, req.Model);
                
                var setupIntent = await service.CreateAsync(setupIntentOptions, cancellationToken: cancellationToken);
                
                credential.IntentCreated(setupIntent);
            } catch (StripeException ex) {
                credential.Error(ex);
            }
        }
        
        private SetupIntentCreateOptions GetSetupIntentOptions(PaymentsParameters parameters, SetupIntentReq req) {
            var settings = _contentCache.Single<StripeSettingsContent>();
            
            var options = new SetupIntentCreateOptions();

            options.PaymentMethodTypes = "card".Yield().ToList();
            options.Customer = req.CustomerId;
            options.Usage = "off_session";
            options.Confirm = true;
            options.Description = parameters.GetTransactionDescription(settings);
            options.PaymentMethod = req.PaymentMethodId;
            
            options.PaymentMethodOptions = new SetupIntentPaymentMethodOptionsOptions();
            options.PaymentMethodOptions.Card = new SetupIntentPaymentMethodOptionsCardOptions();
            options.PaymentMethodOptions.Card.Moto = false;

            return options;
        }
    }
}